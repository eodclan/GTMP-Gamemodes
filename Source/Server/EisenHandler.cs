using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.EisenService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class EisenHandler 
		: Script
	{
        public EisenHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			Eisen Eisen = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start Eisen");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					Eisen = EisenService.EisenList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Eisen == null)
						return;
					if(Eisen.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein Eisen da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + Eisen.Id, "Eisen");
						return;
					}
					EisenService.OpenEisenMenu(client, Eisen);
                    break;

				case "EisenBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Eisen = EisenService.EisenList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(Eisen == null)
					{
						// No Eisen nearby
						API.sendNotificationToPlayer(client, "~r~Kein Eisen in der Nähe!!");
						EisenService.CloseEisenMenu(client);
						return;
					}

					EisenItem item = Eisen.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						EisenService.CloseEisenMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						EisenService.CloseEisenMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						EisenService.CloseEisenMenu(client);
						return;
					}

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						EisenService.CloseEisenMenu(client);
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
                        if (player.Character.Inventory.Count >= ItemService.MaxInventorySlots)
                        {
                            API.sendNotificationToPlayer(client, "Deine Tasche ist leider zu voll...");
                        }
                        invitem.Count++;
                    }

                    item.Count--;
					MoneyService.RemovePlayerCash(client, item.BuyPrice);
					Eisen.MoneyStorage += item.BuyPrice;
                    API.shared.sendPictureNotificationToPlayer(client, "~g~Erfolgreich ~w~" + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $ abgebaut!", "CHAR_ARTHUR", 0, 2, "Erhalten", "Eisen");
					EisenService.RefreshEisenBuyMenu(client, Eisen);
					EisenService.CloseEisenMenu(client);
                    break;

                case "EisenSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Eisen = EisenService.EisenList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Eisen == null)
					{
						// No Eisen nearby
						API.sendNotificationToPlayer(client, "~r~Kein Eisen in der Nähe!!");
						EisenService.CloseEisenMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						EisenService.CloseEisenMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						EisenService.CloseEisenMenu(client);
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
					item = Eisen.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
                    API.shared.sendPictureNotificationToPlayer(client, "~g~Erfolgreich ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ abgebaut!", "CHAR_ARTHUR", 0, 2, "Erhalten", "Eisen");
					EisenService.RefreshEisenBuyMenu(client, Eisen);
                    EisenService.CloseEisenMenu(client);
                    break;
			}
		}

	}
}
