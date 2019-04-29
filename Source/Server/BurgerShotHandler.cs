using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.BurgerShotService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class BurgerShotHandler 
		: Script
	{

        public BurgerShotHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventNameBurgerShot, params object[] arguments) 
		{
			BurgerShot BurgerShot = null;
			Player player = null;
            
            switch (eventNameBurgerShot)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start BurgerShot");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					BurgerShot = BurgerShotService.BurgerShotList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (BurgerShot == null)
						return;
					if(BurgerShot.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, wir verkaufen im Moment nichts.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + BurgerShot.Id, "Burger Shot");
						return;
					}
					BurgerShotService.OpenBurgerShotMenu(client, BurgerShot);
                    break;

				case "BurgerShotBuyItem":                      
                    if (!client.hasData("player"))
						return;
					player = client.getData("player");
					BurgerShot = BurgerShotService.BurgerShotList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(BurgerShot == null)
					{
						// No BurgerShot nearby
						API.sendNotificationToPlayer(client, "~r~Kein Geschäft in der Nähe!");
						BurgerShotService.CloseBurgerShotMenu(client);
						return;
					}

					BurgerShotItem item = BurgerShot.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						BurgerShotService.CloseBurgerShotMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						BurgerShotService.CloseBurgerShotMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel ist ausverkauft!");
						BurgerShotService.CloseBurgerShotMenu(client);
						return;
					}

					Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						BurgerShotService.CloseBurgerShotMenu(client);
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
                        item.Count--;
                        MoneyService.RemovePlayerCash(client, item.BuyPrice);
                        BurgerShot.MoneyStorage += item.BuyPrice;
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
                    BurgerShotService.CloseBurgerShotMenu(client);
                    
                    break;

                case "BurgerShotSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					BurgerShot = BurgerShotService.BurgerShotList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (BurgerShot == null)
					{
						// No BurgerShot nearby
						API.sendNotificationToPlayer(client, "~r~Kein Geschäft in der Nähe!");
						BurgerShotService.CloseBurgerShotMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Gegenstand nicht!");
						BurgerShotService.CloseBurgerShotMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						BurgerShotService.CloseBurgerShotMenu(client);
						return;
					}
					if(invItem.Count <= 0)
					{
						// You does not own this item!
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Gegenstand nicht!");
						player.Character.Inventory.Remove(invItem);
					}
					invItem.Count--;
					if (invItem.Count <= 0)
						player.Character.Inventory.Remove(invItem);
					item = BurgerShot.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Verkauft ~w~1x " + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $");
					BurgerShotService.RefreshBurgerShotBuyMenu(client, BurgerShot);
                    BurgerShotService.CloseBurgerShotMenu(client);
                    break;
			}
		}

	}
}
