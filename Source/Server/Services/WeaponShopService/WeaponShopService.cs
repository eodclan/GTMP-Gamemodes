using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace FactionLife.Server.Services.WeaponShopService
{
	public class WeaponShopService 
		: Script
	{
        public static readonly List<WeaponShop> WeaponShopList = new List<WeaponShop>();

		public WeaponShopService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			WeaponShopList.ForEach(weaponshop => 
			{
				if (weaponshop.Blip != null)
					API.deleteEntity(weaponshop.Blip);
				if (weaponshop.Ped != null)
					API.deleteEntity(weaponshop.Ped);
				SaveWeaponShop(weaponshop);
			});
			WeaponShopList.Clear();
		}

		public static void LoadWeaponShopsFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM weaponshops", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					WeaponShop weaponshop = new WeaponShop
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<WeaponShopItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

					weaponshop.Blip.sprite = 110;
					weaponshop.Blip.scale = 0.94f;
					weaponshop.Blip.color = 5;
					weaponshop.Blip.name = "Ammu-Nation";
					weaponshop.Blip.shortRange = true;
					BlipService.BlipService.BlipList.Add(weaponshop.Blip);

					weaponshop.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, weaponshop.PedPosition, weaponshop.PedRotation.Z);

					WeaponShopList.Add(weaponshop);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Weapon Shops Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Geschäfte geladen ..");
			}
		}

		public static WeaponShop LoadWeaponShopFromDB(int id)
		{
			WeaponShop weaponshop = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM weaponshops WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					weaponshop = new WeaponShop
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<WeaponShopItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return weaponshop;
		}

		public static void SaveWeaponShop(WeaponShop weaponshop)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", weaponshop.Id.ToString() },
				{ "@Owner", weaponshop.Owner },
				{ "@Storage", JsonConvert.SerializeObject(weaponshop.Storage)},
				{ "@MoneyStorage", weaponshop.MoneyStorage.ToString() },
				{ "@PosX", weaponshop.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", weaponshop.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", weaponshop.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", weaponshop.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", weaponshop.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", weaponshop.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", weaponshop.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE weaponshops SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static WeaponShop AddWeaponShop(Vector3 position)
		{
			WeaponShop weaponshop = new WeaponShop();
            weaponshop.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(weaponshop.Storage)},
				{ "@MoneyStorage", weaponshop.MoneyStorage.ToString() },
				{ "@PosX", weaponshop.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", weaponshop.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", weaponshop.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", weaponshop.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", weaponshop.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", weaponshop.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", weaponshop.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO weaponshops (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			weaponshop.Blip = API.shared.createBlip(weaponshop.Position);
			weaponshop.Blip.sprite = 110;
			weaponshop.Blip.scale = 0.94f;
			weaponshop.Blip.color = 5;
			weaponshop.Blip.name = "Ammu-Nation";
			weaponshop.Blip.shortRange = true;

			BlipService.BlipService.BlipList.Add(weaponshop.Blip);
			WeaponShopList.Add(weaponshop);
			return weaponshop;
		}

		public static void OpenWeaponShopMenu(Client client, WeaponShop weaponshop)
		{
            List<WeaponShopMenuItem> weaponshopItems = new List<WeaponShopMenuItem>();
			weaponshop.Storage.ForEach(item => 
			{
				weaponshopItems.Add(new WeaponShopMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

            List<InventoryMenuItem> weaponshopInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					weaponshopInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});
            API.shared.sendPictureNotificationToPlayer(client, "Es ist nicht immer der Richtige Weg eine Waffe zu kaufen!", "CHAR_AMMUNATION", 0, 2, "Waffen Gesetz", "Ammu-Nation");
            SaveWeaponShop(weaponshop);
            API.shared.triggerClientEvent(client, "WeaponShop_OpenMenu", JsonConvert.SerializeObject(weaponshopItems), JsonConvert.SerializeObject(weaponshopInventoryItems), weaponshop.MenuImage);
		}

		public static void RefreshWeaponShopBuyMenu(Client client, WeaponShop weaponshop)
		{
			List<WeaponShopMenuItem> weaponshopItems = new List<WeaponShopMenuItem>();
			weaponshop.Storage.ForEach(item =>
			{
				weaponshopItems.Add(new WeaponShopMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> weaponshopInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					weaponshopInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "WeaponShop_RefreshShopMenu", JsonConvert.SerializeObject(weaponshopItems), JsonConvert.SerializeObject(weaponshopInventoryItems));
		}

		public static void CloseWeaponShopMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "WeaponShop_CloseMenu");
		}
	}
}
