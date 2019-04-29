using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.EisenSellerService;
using System.Linq;
using System;

namespace FactionLife.Server
{
	class EisenSellerHandler 
		: Script
	{
        public EisenSellerHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			EisenSeller EisenSeller = null;
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
					EisenSeller = EisenSellerService.EisenSellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (EisenSeller == null)
						return;
					if(EisenSeller.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist keine Spedition da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + EisenSeller.Id, "Spedition");
						return;
					}
					EisenSellerService.OpenEisenSellerMenu(client, EisenSeller);
                    break;

				case "EisenSellerBuyItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					EisenSeller = EisenSellerService.EisenSellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(EisenSeller == null)
					{
						// No EisenSeller nearby
						API.sendNotificationToPlayer(client, "~r~Keine Spedition in der Nähe!!");
						EisenSellerService.CloseEisenSellerMenu(client);
						return;
					}

					EisenSellerItem item = EisenSeller.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						EisenSellerService.CloseEisenSellerMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						EisenSellerService.CloseEisenSellerMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Die Spedition meinte gerade, dass du nicht genug Eisen hast.");
						EisenSellerService.CloseEisenSellerMenu(client);
						return;
					}

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieses Eisen existiert nicht!");
						EisenSellerService.CloseEisenSellerMenu(client);
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
					EisenSeller.MoneyStorage += item.BuyPrice;
                    API.shared.sendPictureNotificationToPlayer(client, "~g~Erfolgreich ~w~" + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $ abgebaut!", "CHAR_ARTHUR", 0, 2, "Erhalten", "Eisen");
                    EisenSellerService.RefreshEisenSellerBuyMenu(client, EisenSeller);
					EisenSellerService.CloseEisenSellerMenu(client);
                    break;

                case "EisenSellerSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					EisenSeller = EisenSellerService.EisenSellerList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (EisenSeller == null)
					{
						// No EisenSeller nearby
						API.sendNotificationToPlayer(client, "~r~Keine Spedition in der Nähe!!");
						EisenSellerService.CloseEisenSellerMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
						EisenSellerService.CloseEisenSellerMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						EisenSellerService.CloseEisenSellerMenu(client);
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
					item = EisenSeller.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);

                    API.shared.sendPictureNotificationToPlayer(client, "~g~Erfolgreich ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ abgebaut!", "CHAR_ARTHUR", 0, 2, "Erhalten", "Eisen");
                    EisenSellerService.RefreshEisenSellerBuyMenu(client, EisenSeller);
                    EisenSellerService.CloseEisenSellerMenu(client);
                    break;
			}
		}

	}
}
