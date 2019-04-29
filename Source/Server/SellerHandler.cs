using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.SellerService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class SellerHandler 
		: Script
	{
        public SellerHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			Seller Seller = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start Seller");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					Seller = SellerService.SellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Seller == null)
						return;
					if(Seller.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein Seller da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + Seller.Id, "Seller");
						return;
					}
					SellerService.OpenSellerMenu(client, Seller);
                    break;

				case "SellerBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Seller = SellerService.SellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(Seller == null)
					{
						// No Seller nearby
						API.sendNotificationToPlayer(client, "~r~Kein Seller in der Nähe!!");
						SellerService.CloseSellerMenu(client);
						return;
					}

					SellerItem item = Seller.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						SellerService.CloseSellerMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						SellerService.CloseSellerMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						SellerService.CloseSellerMenu(client);
						return;
					}

					Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						SellerService.CloseSellerMenu(client);
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
					Seller.MoneyStorage += item.BuyPrice;
                    API.shared.sendPictureNotificationToPlayer(client, "~g~Gekauft ~w~1x" + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $!", "CHAR_NIGEL", 0, 2, "Seller", "Gekauft");
					SellerService.RefreshSellerBuyMenu(client, Seller);
					SellerService.CloseSellerMenu(client);
                    break;

                case "SellerSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Seller = SellerService.SellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Seller == null)
					{
						// No Seller nearby
						API.sendNotificationToPlayer(client, "~r~Kein Seller in der Nähe!!");
						SellerService.CloseSellerMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						SellerService.CloseSellerMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						SellerService.CloseSellerMenu(client);
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
					item = Seller.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
                    API.shared.sendPictureNotificationToPlayer(client, "Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ erhalten!", "CHAR_NIGEL", 0, 2, "Seller", "Verkauft");
					SellerService.RefreshSellerBuyMenu(client, Seller);
                    SellerService.CloseSellerMenu(client);
                    break;
			}
		}

	}
}
