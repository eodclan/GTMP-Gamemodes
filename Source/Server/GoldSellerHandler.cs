using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.GoldSellerService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class GoldSellerHandler 
		: Script
	{
        public GoldSellerHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			GoldSeller GoldSeller = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start Gold");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					GoldSeller = GoldSellerService.GoldSellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (GoldSeller == null)
						return;
					if(GoldSeller.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist keine Spedition da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + GoldSeller.Id, "Spedition");
						return;
					}
					GoldSellerService.OpenGoldSellerMenu(client, GoldSeller);
                    break;

				case "GoldSellerBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					GoldSeller = GoldSellerService.GoldSellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(GoldSeller == null)
					{
						// No GoldSeller nearby
						API.sendNotificationToPlayer(client, "~r~Keine Spedition in der Nähe!!");
						GoldSellerService.CloseGoldSellerMenu(client);
						return;
					}

					GoldSellerItem item = GoldSeller.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						GoldSellerService.CloseGoldSellerMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						GoldSellerService.CloseGoldSellerMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Die Spedition meinte gerade, dass du nicht genug Gold hast.");
						GoldSellerService.CloseGoldSellerMenu(client);
						return;
					}

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieses Gold existiert nicht!");
						GoldSellerService.CloseGoldSellerMenu(client);
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
					GoldSeller.MoneyStorage += item.BuyPrice;
					API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $ erhalten!");
					GoldSellerService.RefreshGoldSellerBuyMenu(client, GoldSeller);
					GoldSellerService.CloseGoldSellerMenu(client);
                    break;

                case "GoldSellerSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					GoldSeller = GoldSellerService.GoldSellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (GoldSeller == null)
					{
						// No GoldSeller nearby
						API.sendNotificationToPlayer(client, "~r~Keine Spedition in der Nähe!!");
						GoldSellerService.CloseGoldSellerMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						GoldSellerService.CloseGoldSellerMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						GoldSellerService.CloseGoldSellerMenu(client);
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
					item = GoldSeller.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ erhalten!");
					GoldSellerService.RefreshGoldSellerBuyMenu(client, GoldSeller);
                    GoldSellerService.CloseGoldSellerMenu(client);
                    break;
			}
		}

	}
}
