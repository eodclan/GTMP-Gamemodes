using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.DealerService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class DealerHandler 
		: Script
	{

        public DealerHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			Dealer Dealer = null;
			Player player = null;
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
					if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					Dealer = DealerService.DealerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Dealer == null)
						return;
					if(Dealer.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein Dealer da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + Dealer.Id, "Dealer");
						return;
					}
					DealerService.OpenDealerMenu(client, Dealer);
                    break;

				case "DealerBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Dealer = DealerService.DealerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(Dealer == null)
					{
						// No Dealer nearby
						API.sendNotificationToPlayer(client, "~r~Kein Dealer in der Nähe!!");
						DealerService.CloseDealerMenu(client);
						return;
					}

					DealerItem item = Dealer.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						DealerService.CloseDealerMenu(client);
						return;
					}

                    if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						DealerService.CloseDealerMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
						DealerService.CloseDealerMenu(client);
						return;
					}

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
						DealerService.CloseDealerMenu(client);
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
                        Dealer.MoneyStorage += item.BuyPrice;
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
                    DealerService.RefreshDealerBuyMenu(client, Dealer);
                    DealerService.CloseDealerMenu(client);
                    break;

				case "DealerSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					Dealer = DealerService.DealerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (Dealer == null)
					{
						// No Dealer nearby
						API.sendNotificationToPlayer(client, "~r~Kein Dealer in der Nähe!!");
						DealerService.CloseDealerMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						DealerService.CloseDealerMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						DealerService.CloseDealerMenu(client);
						return;
					}
					if(invItem.Count <= 0)
					{
						// You does not own this item!
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						player.Character.Inventory.Remove(invItem);
					}
                    if (invItem.Count <= 35)
                    {
                        // You does not own this item!
                        API.sendNotificationToPlayer(client, "~r~Du kannst nicht mehr tragen!");
                        player.Character.Inventory.Remove(invItem);
                    }
                    invItem.Count--;
					if (invItem.Count <= 0)
						player.Character.Inventory.Remove(invItem);
					item = Dealer.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
                    API.shared.sendPictureNotificationToPlayer(client, "~g~Erfolgreich ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ abgebaut!", "CHAR_ARTHUR", 0, 2, "Dealer", "Erhalten");
					DealerService.RefreshDealerBuyMenu(client, Dealer);
                    DealerService.CloseDealerMenu(client);
                    break;
			}
		}

	}
}
