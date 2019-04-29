using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.HolzService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class HolzHandler 
		: Script
	{
        public HolzHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			Holz Holz = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start Holz");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					Holz = HolzService.HolzList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Holz == null)
						return;
					if(Holz.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein Holz da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + Holz.Id, "Holz Mine");
						return;
					}
					HolzService.OpenHolzMenu(client, Holz);
                    break;

				case "HolzBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Holz = HolzService.HolzList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(Holz == null)
					{
						// No Holz nearby
						API.sendNotificationToPlayer(client, "~r~Kein Holz in der Nähe!!");
						HolzService.CloseHolzMenu(client);
						return;
					}

					HolzItem item = Holz.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						HolzService.CloseHolzMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						HolzService.CloseHolzMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						HolzService.CloseHolzMenu(client);
						return;
					}

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						HolzService.CloseHolzMenu(client);
                        API.delay(20000, true, () =>
                        {
                            API.consoleOutput("Server: Holz");
                        });
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
					Holz.MoneyStorage += item.BuyPrice;
					API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $ abgebaut!");
					HolzService.RefreshHolzBuyMenu(client, Holz);
					HolzService.CloseHolzMenu(client);
                    break;

                case "HolzSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Holz = HolzService.HolzList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Holz == null)
					{
						// No Holz nearby
						API.sendNotificationToPlayer(client, "~r~Kein Holz in der Nähe!!");
						HolzService.CloseHolzMenu(client);
                        return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						HolzService.CloseHolzMenu(client);
                        return;
                    }
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						HolzService.CloseHolzMenu(client);
                        return;
					}
					if(invItem.Count <= 0)
					{
						// You does not own this item!
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						player.Character.Inventory.Remove(invItem);
					}
                    invItem.Count--;
					if (invItem.Count <= 0)
						player.Character.Inventory.Remove(invItem);
					item = Holz.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ abgebaut!");
					HolzService.RefreshHolzBuyMenu(client, Holz);
                    HolzService.CloseHolzMenu(client);
                    break;
			}
		}

	}
}
