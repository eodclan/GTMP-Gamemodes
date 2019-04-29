using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.GoldSellerService
{
	public class GoldSellerService 
		: Script
	{
		public static readonly List<GoldSeller> GoldSellerList = new List<GoldSeller>();

		public GoldSellerService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			GoldSellerList.ForEach(GoldSeller => 
			{
				if (GoldSeller.Blip != null)
					API.deleteEntity(GoldSeller.Blip);
				if (GoldSeller.Ped != null)
					API.deleteEntity(GoldSeller.Ped);
				SaveGoldSeller(GoldSeller);
			});
			GoldSellerList.Clear();
		}

		public static void LoadGoldSellerFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM golds_seller", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					GoldSeller GoldSeller = new GoldSeller
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<GoldSellerItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

                    GoldSeller.Blip.sprite = 514;
                    GoldSeller.Blip.name = "Gold forwarding";
                    GoldSeller.Blip.shortRange = true;
                    BlipService.BlipService.BlipList.Add(GoldSeller.Blip);

                    GoldSeller.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, GoldSeller.PedPosition, GoldSeller.PedRotation.Z);

					GoldSellerList.Add(GoldSeller);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " GoldSellers Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine GoldSeller geladen ..");
			}
		}

		public static GoldSeller LoadGoldSellerFromDB(int id)
		{
			GoldSeller GoldSeller = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM golds_seller WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					GoldSeller = new GoldSeller
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<GoldSellerItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return GoldSeller;
		}

		public static void SaveGoldSeller(GoldSeller GoldSeller)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", GoldSeller.Id.ToString() },
				{ "@Owner", GoldSeller.Owner },
				{ "@Storage", JsonConvert.SerializeObject(GoldSeller.Storage)},
				{ "@MoneyStorage", GoldSeller.MoneyStorage.ToString() },
				{ "@PosX", GoldSeller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", GoldSeller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", GoldSeller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", GoldSeller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", GoldSeller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", GoldSeller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", GoldSeller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE golds_seller SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static GoldSeller AddGoldSeller(Vector3 position)
		{
			GoldSeller GoldSeller = new GoldSeller();
			GoldSeller.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(GoldSeller.Storage)},
				{ "@MoneyStorage", GoldSeller.MoneyStorage.ToString() },
				{ "@PosX", GoldSeller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", GoldSeller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", GoldSeller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", GoldSeller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", GoldSeller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", GoldSeller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", GoldSeller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO golds_seller (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			GoldSeller.Blip = API.shared.createBlip(GoldSeller.Position);
			GoldSeller.Blip.sprite = 514;
            GoldSeller.Blip.scale = 0.8f;
            GoldSeller.Blip.name = "Gold forwarding";
			GoldSeller.Blip.shortRange = true;

            BlipService.BlipService.BlipList.Add(GoldSeller.Blip);
			GoldSellerList.Add(GoldSeller);
			return GoldSeller;
		}

		public static void OpenGoldSellerMenu(Client client, GoldSeller GoldSeller)
		{
			List<GoldSellerMenuItem> GoldSellerItems = new List<GoldSellerMenuItem>();
			GoldSeller.Storage.ForEach(item => 
			{
				GoldSellerItems.Add(new GoldSellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> GoldSellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					GoldSellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveGoldSeller(GoldSeller);

			API.shared.triggerClientEvent(client, "GoldSeller_OpenMenu", JsonConvert.SerializeObject(GoldSellerItems), JsonConvert.SerializeObject(GoldSellerInventoryItems), GoldSeller.MenuImage);
		}

		public static void RefreshGoldSellerBuyMenu(Client client, GoldSeller GoldSeller)
		{
			List<GoldSellerMenuItem> GoldSellerItems = new List<GoldSellerMenuItem>();
			GoldSeller.Storage.ForEach(item =>
			{
				GoldSellerItems.Add(new GoldSellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> GoldSellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					GoldSellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "GoldSeller_RefreshGoldSellerMenu", JsonConvert.SerializeObject(GoldSellerItems), JsonConvert.SerializeObject(GoldSellerInventoryItems));
		}

		public static void CloseGoldSellerMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "GoldSeller_CloseMenu");
		}
	}
}
