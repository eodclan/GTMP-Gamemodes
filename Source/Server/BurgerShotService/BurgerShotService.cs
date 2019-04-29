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

namespace FactionLife.Server.Services.BurgerShotService
{
	public class BurgerShotService 
		: Script
	{
        public static readonly List<BurgerShot> BurgerShotList = new List<BurgerShot>();

		public BurgerShotService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			BurgerShotList.ForEach(BurgerShot => 
			{
				if (BurgerShot.Blip != null)
					API.deleteEntity(BurgerShot.Blip);
				if (BurgerShot.Ped != null)
					API.deleteEntity(BurgerShot.Ped);
				SaveBurgerShot(BurgerShot);
			});
			BurgerShotList.Clear();
		}

		public static void LoadBurgerShotsFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM burgershots", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					BurgerShot BurgerShot = new BurgerShot
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<BurgerShotItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

					BurgerShot.Blip.sprite = 52;
					BurgerShot.Blip.scale = 0.5f;
					BurgerShot.Blip.name = "BurgerShot";
					BurgerShot.Blip.shortRange = true;
					BlipService.BlipService.BlipList.Add(BurgerShot.Blip);

					BurgerShot.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, BurgerShot.PedPosition, BurgerShot.PedRotation.Z);

					BurgerShotList.Add(BurgerShot);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " BurgerShots Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Geschäfte geladen ..");
			}
		}

		public static BurgerShot LoadBurgerShotFromDB(int id)
		{
			BurgerShot BurgerShot = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM burgershots WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					BurgerShot = new BurgerShot
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<BurgerShotItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return BurgerShot;
		}

		public static void SaveBurgerShot(BurgerShot BurgerShot)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", BurgerShot.Id.ToString() },
				{ "@Owner", BurgerShot.Owner },
				{ "@Storage", JsonConvert.SerializeObject(BurgerShot.Storage)},
				{ "@MoneyStorage", BurgerShot.MoneyStorage.ToString() },
				{ "@PosX", BurgerShot.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", BurgerShot.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", BurgerShot.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", BurgerShot.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", BurgerShot.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", BurgerShot.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", BurgerShot.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE burgershots SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static BurgerShot AddBurgerShot(Vector3 position)
		{
			BurgerShot BurgerShot = new BurgerShot();
			BurgerShot.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(BurgerShot.Storage)},
				{ "@MoneyStorage", BurgerShot.MoneyStorage.ToString() },
				{ "@PosX", BurgerShot.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", BurgerShot.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", BurgerShot.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", BurgerShot.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", BurgerShot.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", BurgerShot.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", BurgerShot.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO burgershots (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			BurgerShot.Blip = API.shared.createBlip(BurgerShot.Position);
			BurgerShot.Blip.sprite = 52;
			BurgerShot.Blip.name = "BurgerShot";
			BurgerShot.Blip.shortRange = true;

			BlipService.BlipService.BlipList.Add(BurgerShot.Blip);
			BurgerShotList.Add(BurgerShot);
			return BurgerShot;
		}

		public static void OpenBurgerShotMenu(Client client, BurgerShot BurgerShot)
		{
            List<BurgerShotMenuItem> BurgerShotItems = new List<BurgerShotMenuItem>();
			BurgerShot.Storage.ForEach(item => 
			{
				BurgerShotItems.Add(new BurgerShotMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> BurgerShotInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					BurgerShotInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveBurgerShot(BurgerShot);
            API.shared.triggerClientEvent(client, "BurgerShot_OpenMenu", JsonConvert.SerializeObject(BurgerShotItems), JsonConvert.SerializeObject(BurgerShotInventoryItems), BurgerShot.MenuImage);
		}

		public static void RefreshBuyMenu(Client client, BurgerShot BurgerShot)
		{
			List<BurgerShotMenuItem> BurgerShotItems = new List<BurgerShotMenuItem>();
			BurgerShot.Storage.ForEach(item =>
			{
				BurgerShotItems.Add(new BurgerShotMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> BurgerShotInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					BurgerShotInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "BurgerShot_RefreshBurgerShotMenu", JsonConvert.SerializeObject(BurgerShotItems), JsonConvert.SerializeObject(BurgerShotInventoryItems));
		}

		public static void CloseBurgerShotMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "BurgerShot_CloseMenu");
		}
	}
}
