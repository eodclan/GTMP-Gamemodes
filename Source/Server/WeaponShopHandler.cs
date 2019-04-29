using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.WeaponShopService;
using System.Linq;
using System;
using FactionLife.Server.Services.ShopRob;

namespace FactionLife.Server
{
	class WeaponShopHandler 
		: Script
	{
        public Main Main { get; set; }

        public WeaponShopHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) 
		{
			WeaponShop weaponshop = null;
			Player player = null;
            
            switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start Shop");
                    });
                    if (client.isInVehicle)
						return;
					if (!client.hasData("player"))
						return;
					weaponshop = WeaponShopService.WeaponShopList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (weaponshop == null)
						return;
					if(weaponshop.Storage.Count == 0)
					{
						API.sendPictureNotificationToPlayer(client, "Entschuldigung, wir verkaufen im Moment nichts.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + weaponshop.Id, "Shop");
						return;
					}
					WeaponShopService.OpenWeaponShopMenu(client, weaponshop);
                    break;

				case "WeaponShopBuyItem":                      
                    if (!client.hasData("player"))
						return;
					player = client.getData("player");
					weaponshop = WeaponShopService.WeaponShopList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if(weaponshop == null)
					{
						// No shop nearby
						API.sendNotificationToPlayer(client, "~r~Kein Geschäft in der Nähe!");
						WeaponShopService.CloseWeaponShopMenu(client);
						return;
					}

					WeaponShopItem item = weaponshop.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
					if(item == null)
					{
						// Not in the assortment
						API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
						WeaponShopService.CloseWeaponShopMenu(client);
						return;
					}

					if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
					{
						// Not enough Cash
						API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
						WeaponShopService.CloseWeaponShopMenu(client);
						return;
					}

					if(item.Count <= 0)
					{
						// Sold Out
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel ist ausverkauft!");
						WeaponShopService.CloseWeaponShopMenu(client);
						return;
					}

					Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
					if(itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						WeaponShopService.CloseWeaponShopMenu(client);
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
					weaponshop.MoneyStorage += item.BuyPrice;
                    WeaponShopService.CloseWeaponShopMenu(client);
                    API.shared.sendPictureNotificationToPlayer(client, "~g~Gekauft ~w~1x " + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $", "CHAR_AMMUNATION", 0, 2, "Waffe gekauft", "Ammu-Nation");
                    break;

                case "ShopSellItem":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					weaponshop = WeaponShopService.WeaponShopList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
					if (weaponshop == null)
					{
						// No shop nearby
						API.sendNotificationToPlayer(client, "~r~Kein Geschäft in der Nähe!");
						WeaponShopService.CloseWeaponShopMenu(client);
						return;
					}
					InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
					if(invItem == null)
					{
						// Doesn't own this item
						API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Gegenstand nicht!");
						WeaponShopService.CloseWeaponShopMenu(client);
					}
					itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (itemInfo == null)
					{
						// Item Doen't exist
						API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
						WeaponShopService.CloseWeaponShopMenu(client);
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
					item = weaponshop.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
					if (item != null)
						item.Count++;
					MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
                    API.shared.sendPictureNotificationToPlayer(client, "~g~Verkauft ~w~1x " + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $", "CHAR_AMMUNATION", 0, 2, "Waffe gekauft", "Ammu-Nation");
					WeaponShopService.RefreshWeaponShopBuyMenu(client, weaponshop);
                    WeaponShopService.CloseWeaponShopMenu(client);
                    break;
            }
		}

	}
}
