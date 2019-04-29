using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.HolzSellerService
{
	public class HolzSellerService 
		: Script
	{
		public static readonly List<HolzSeller> HolzSellerList = new List<HolzSeller>();

		public HolzSellerService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			HolzSellerList.ForEach(HolzSeller => 
			{
				if (HolzSeller.Blip != null)
					API.deleteEntity(HolzSeller.Blip);
				if (HolzSeller.Ped != null)
					API.deleteEntity(HolzSeller.Ped);
				SaveHolzSeller(HolzSeller);
			});
			HolzSellerList.Clear();
		}

		public static void LoadHolzSellerFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM holz_job_seller", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					HolzSeller HolzSeller = new HolzSeller
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<HolzSellerItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

                    HolzSeller.Blip.sprite = 486;
                    HolzSeller.Blip.name = "Wood forwarding";
                    HolzSeller.Blip.shortRange = true;
                    BlipService.BlipService.BlipList.Add(HolzSeller.Blip);

                    HolzSeller.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, HolzSeller.PedPosition, HolzSeller.PedRotation.Z);

					HolzSellerList.Add(HolzSeller);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " HolzSellers Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine HolzSeller geladen ..");
			}
		}

		public static HolzSeller LoadHolzSellerFromDB(int id)
		{
			HolzSeller HolzSeller = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM holz_job_seller WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					HolzSeller = new HolzSeller
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<HolzSellerItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return HolzSeller;
		}

		public static void SaveHolzSeller(HolzSeller HolzSeller)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", HolzSeller.Id.ToString() },
				{ "@Owner", HolzSeller.Owner },
				{ "@Storage", JsonConvert.SerializeObject(HolzSeller.Storage)},
				{ "@MoneyStorage", HolzSeller.MoneyStorage.ToString() },
				{ "@PosX", HolzSeller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", HolzSeller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", HolzSeller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", HolzSeller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", HolzSeller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", HolzSeller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", HolzSeller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE holz_job_seller SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static HolzSeller AddHolzSeller(Vector3 position)
		{
			HolzSeller HolzSeller = new HolzSeller();
			HolzSeller.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(HolzSeller.Storage)},
				{ "@MoneyStorage", HolzSeller.MoneyStorage.ToString() },
				{ "@PosX", HolzSeller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", HolzSeller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", HolzSeller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", HolzSeller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", HolzSeller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", HolzSeller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", HolzSeller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO holz_job_seller (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			HolzSeller.Blip = API.shared.createBlip(HolzSeller.Position);
			HolzSeller.Blip.sprite = 486;
            HolzSeller.Blip.scale = 0.8f;
            HolzSeller.Blip.name = "Wood forwarding";
			HolzSeller.Blip.shortRange = true;

            BlipService.BlipService.BlipList.Add(HolzSeller.Blip);
			HolzSellerList.Add(HolzSeller);
			return HolzSeller;
		}

		public static void OpenHolzSellerMenu(Client client, HolzSeller HolzSeller)
		{
			List<HolzSellerMenuItem> HolzSellerItems = new List<HolzSellerMenuItem>();
			HolzSeller.Storage.ForEach(item => 
			{
				HolzSellerItems.Add(new HolzSellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> HolzSellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					HolzSellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveHolzSeller(HolzSeller);

			API.shared.triggerClientEvent(client, "HolzSeller_OpenMenu", JsonConvert.SerializeObject(HolzSellerItems), JsonConvert.SerializeObject(HolzSellerInventoryItems), HolzSeller.MenuImage);
		}

		public static void RefreshHolzSellerBuyMenu(Client client, HolzSeller HolzSeller)
		{
			List<HolzSellerMenuItem> HolzSellerItems = new List<HolzSellerMenuItem>();
			HolzSeller.Storage.ForEach(item =>
			{
				HolzSellerItems.Add(new HolzSellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> HolzSellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					HolzSellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "HolzSeller_RefreshHolzSellerMenu", JsonConvert.SerializeObject(HolzSellerItems), JsonConvert.SerializeObject(HolzSellerInventoryItems));
		}

		public static void CloseHolzSellerMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "HolzSeller_CloseMenu");
		}
	}
}
