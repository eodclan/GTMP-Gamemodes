using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.FisherSellService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class FisherSellHandler 
		: Script
	{
        public FisherSellHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			FisherSell FisherSell = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start FisherSell");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					FisherSell = FisherSellService.FisherSellList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (FisherSell == null)
						return;
					if(FisherSell.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, wir haben momentan geschlossen.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + FisherSell.Id, "Sushi Garden");
						return;
					}
					FisherSellService.OpenFisherSellMenu(client, FisherSell);
                    break;

				case "FisherSellBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					FisherSell = FisherSellService.FisherSellList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(FisherSell == null)
					{
						// No FisherSell nearby
						API.sendNotificationToPlayer(client, "~r~Kein FisherSell in der Nähe!!");
						FisherSellService.CloseFisherSellMenu(client);
						return;
					}

					FisherSellItem item = FisherSell.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						FisherSellService.CloseFisherSellMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						FisherSellService.CloseFisherSellMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						FisherSellService.CloseFisherSellMenu(client);
						return;
					}

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						FisherSellService.CloseFisherSellMenu(client);
						return;
					}

                    InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == item.Id);
                    if (invitem == null)
                    {
                        player.Character.Inventory.Add(new InventoryItem
                        {
                            ItemID = item.Id,
                            Count = 1
                        });
                        item.Count--;
                        MoneyService.RemovePlayerCash(client, item.BuyPrice);
                        FisherSell.MoneyStorage += item.BuyPrice;
                        API.sendNotificationToPlayer(client, "~g~Gekauft ~w~1x " + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $");
                    }
                    else
                    {
                        if (invitem.Count >= ItemService.MaxInventorySlots)
                        {
                            invitem.Count++;
                        }
                        if (invitem.Count == ItemService.MaxInventorySlots)
                        {
                            invitem.Count++;
                        }
                        API.sendNotificationToPlayer(client, "Du kannst nicht so viel tragen oder bist du Hulk?");
                    }
                    FisherSellService.RefreshFisherSellBuyMenu(client, FisherSell);
                    FisherSellService.CloseFisherSellMenu(client);
                    break;

                case "FisherSellSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					FisherSell = FisherSellService.FisherSellList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (FisherSell == null)
					{
						// No FisherSell nearby
						API.sendNotificationToPlayer(client, "~r~Kein FisherSell in der Nähe!!");
						FisherSellService.CloseFisherSellMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						FisherSellService.CloseFisherSellMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						FisherSellService.CloseFisherSellMenu(client);
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
					item = FisherSell.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ erhalten!");
					FisherSellService.RefreshFisherSellBuyMenu(client, FisherSell);
                    FisherSellService.CloseFisherSellMenu(client);
                    break;
			}
		}

	}
}
