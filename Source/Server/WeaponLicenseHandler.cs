using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.WeaponLicenseService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class WeaponLicenseHandler 
		: Script
	{

        public WeaponLicenseHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			WeaponLicense WeaponLicense = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start WeaponLicense");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					WeaponLicense = WeaponLicenseService.WeaponLicenseList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (WeaponLicense == null)
						return;
					if(WeaponLicense.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein Lehrer da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + WeaponLicense.Id, "State Center");
						return;
					}
					WeaponLicenseService.OpenWeaponLicenseMenu(client, WeaponLicense);
                    API.delay(3000, true, () =>
                    {
                        API.sendNotificationToPlayer(client, "~r~Du musst 5 Sekunden warten bis du wieder was machen kannst.");
                    });
                    break;

				case "WeaponLicenseBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					WeaponLicense = WeaponLicenseService.WeaponLicenseList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(WeaponLicense == null)
					{
						// No WeaponLicense nearby
						API.sendNotificationToPlayer(client, "~r~Kein State Center in der Nähe!!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
						return;
					}

					WeaponLicenseItem item = WeaponLicense.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
						return;
					}

					Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
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
					WeaponLicense.MoneyStorage += item.BuyPrice;
					API.sendNotificationToPlayer(client, "~g~Erfolgreich beantragt: ~w~" + itemInfo.Name + " ~g~Die Gebür von ~w~" + item.BuyPrice + " $ hast du bereits bezahlt!");
					WeaponLicenseService.RefreshWeaponLicenseBuyMenu(client, WeaponLicense);
					WeaponLicenseService.CloseWeaponLicenseMenu(client);
                    break;

                case "WeaponLicenseSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					WeaponLicense = WeaponLicenseService.WeaponLicenseList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (WeaponLicense == null)
					{
						// No WeaponLicense nearby
						API.sendNotificationToPlayer(client, "~r~Kein State Center in der Nähe!!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diese Lizenz nicht!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						WeaponLicenseService.CloseWeaponLicenseMenu(client);
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
					item = WeaponLicense.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Erfolgreich beantragt: ~w~" + itemInfo.Name + " ~g~Die Gebür von ~w~" + itemInfo.DefaultSellPrice + " $ hast du bereits bezahlt!");
					WeaponLicenseService.RefreshWeaponLicenseBuyMenu(client, WeaponLicense);
                    WeaponLicenseService.CloseWeaponLicenseMenu(client);
                    break;
			}
		}

	}
}
