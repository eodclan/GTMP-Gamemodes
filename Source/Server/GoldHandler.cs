using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.GoldService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class GoldHandler 
		: Script
	{

        public GoldHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			Gold Gold = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					Gold = GoldService.GoldList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Gold == null)
						return;
					if(Gold.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein Gold da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + Gold.Id, "Gold Mine");
						return;
					}
					GoldService.OpenGoldMenu(client, Gold);
                    break;

				case "GoldBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Gold = GoldService.GoldList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(Gold == null)
					{
						// No Gold nearby
						API.sendNotificationToPlayer(client, "~r~Kein Gold in der Nähe!!");
						GoldService.CloseGoldMenu(client);
						return;
					}

					GoldItem item = Gold.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						GoldService.CloseGoldMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						GoldService.CloseGoldMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						GoldService.CloseGoldMenu(client);
						return;
					}

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						GoldService.CloseGoldMenu(client);
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
					Gold.MoneyStorage += item.BuyPrice;
					API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $ abgebaut!");
					GoldService.RefreshGoldBuyMenu(client, Gold);
					GoldService.CloseGoldMenu(client);
                    break;
                    //test

                case "GoldSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Gold = GoldService.GoldList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Gold == null)
					{
						// No Gold nearby
						API.sendNotificationToPlayer(client, "~r~Kein Gold in der Nähe!!");
						GoldService.CloseGoldMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						GoldService.CloseGoldMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						GoldService.CloseGoldMenu(client);
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
					item = Gold.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ abgebaut!");
					GoldService.RefreshGoldBuyMenu(client, Gold);
                    GoldService.CloseGoldMenu(client);
                    break;
			}
		}

	}
}
