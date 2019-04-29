using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.TequiLaLaService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class TequiLaLaHandler 
		: Script
	{

        public TequiLaLaHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			TequiLaLa tequilala = null;
			Player player = null;
            
            switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start TequiLaLa");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
                    tequilala = TequiLaLaService.TequiLaLaList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (tequilala == null)
						return;
					if(tequilala.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, wir verkaufen im Moment nichts.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + tequilala.Id, "Burger Shot");
						return;
					}
					TequiLaLaService.OpenTequiLaLaMenu(client, tequilala);
                    break;

				case "TequiLaLaBuyItem":                      
                    if (!client.hasData("player"))
						return;
					player = client.getData("player");
                    tequilala = TequiLaLaService.TequiLaLaList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(tequilala == null)
					{
						// No TequiLaLa nearby
						API.sendNotificationToPlayer(client, "~r~Kein Geschäft in der Nähe!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
						return;
					}

					TequiLaLaItem item = tequilala.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel ist ausverkauft!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
						return;
					}

					Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
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
                        tequilala.MoneyStorage += item.BuyPrice;
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
                    TequiLaLaService.CloseTequiLaLaMenu(client);
                    break;

                case "TequiLaLaSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
                    tequilala = TequiLaLaService.TequiLaLaList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (tequilala == null)
					{
						// No TequiLaLa nearby
						API.sendNotificationToPlayer(client, "~r~Kein Geschäft in der Nähe!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Gegenstand nicht!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						TequiLaLaService.CloseTequiLaLaMenu(client);
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
					item = tequilala.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
					API.sendNotificationToPlayer(client, "~g~Verkauft ~w~1x " + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $");
					TequiLaLaService.RefreshTequiLaLaBuyMenu(client, tequilala);
                    TequiLaLaService.CloseTequiLaLaMenu(client);
                    break;
			}
		}

	}
}
