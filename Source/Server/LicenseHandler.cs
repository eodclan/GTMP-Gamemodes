using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.LicenseService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class LicenseHandler 
		: Script
	{

        public LicenseHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			License License = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start License");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					License = LicenseService.LicenseList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (License == null)
						return;
					if(License.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein Lehrer da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + License.Id, "State Center");
						return;
					}
					LicenseService.OpenLicenseMenu(client, License);
                    API.delay(3000, true, () =>
                    {
                        API.sendNotificationToPlayer(client, "~r~Du musst 5 Sekunden warten bis du wieder was machen kannst.");
                    });
                    break;

				case "LicenseBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					License = LicenseService.LicenseList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(License == null)
					{
						// No License nearby
						API.sendNotificationToPlayer(client, "~r~Kein State Center in der Nähe!!");
						LicenseService.CloseLicenseMenu(client);
						return;
					}

					LicenseItem item = License.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						LicenseService.CloseLicenseMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						LicenseService.CloseLicenseMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						LicenseService.CloseLicenseMenu(client);
						return;
					}

					Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						LicenseService.CloseLicenseMenu(client);
						return;
					}

                    InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == item.Id);
					if(invitem == null)
					{
						player.Character.Inventory.Add(new InventoryItem
						{
							ItemID = item.Id,
							Count = 1
						});
					}
					else
					{
						invitem.Count++;
					}
					
					item.Count--;
					MoneyService.RemovePlayerCash(client, item.BuyPrice);
					License.MoneyStorage += item.BuyPrice;
					API.sendNotificationToPlayer(client, "~g~Erfolgreich beantragt: ~w~" + itemInfo.Name + " ~g~Die Gebür von ~w~" + item.BuyPrice + " $ hast du bereits bezahlt!");
					LicenseService.RefreshLicenseBuyMenu(client, License);
					LicenseService.CloseLicenseMenu(client);
                    break;

                case "LicenseSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					License = LicenseService.LicenseList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (License == null)
					{
						// No License nearby
						API.sendNotificationToPlayer(client, "~r~Kein State Center in der Nähe!!");
						LicenseService.CloseLicenseMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diese Lizenz nicht!");
						LicenseService.CloseLicenseMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						LicenseService.CloseLicenseMenu(client);
						return;
					}
					if(invItem.Count <= 0)
					{
						// You does not own this item!
						API.sendNotificationToPlayer(client, "~r~Du besitzt diese Lizenz nicht!");
						player.Character.Inventory.Remove(invItem);
					}
					invItem.Count--;
					if (invItem.Count <= 0)
						player.Character.Inventory.Remove(invItem);
					item = License.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Erfolgreich beantragt: ~w~" + itemInfo.Name + " ~g~Die Gebür von ~w~" + itemInfo.DefaultSellPrice + " $ hast du bereits bezahlt!");
					LicenseService.RefreshLicenseBuyMenu(client, License);
                    LicenseService.CloseLicenseMenu(client);
                    break;
			}
		}

	}
}
