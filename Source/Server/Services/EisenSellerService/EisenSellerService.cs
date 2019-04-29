using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.EisenSellerService
{
	public class EisenSellerService 
		: Script
	{
		public static readonly List<EisenSeller> EisenSellerList = new List<EisenSeller>();

		public EisenSellerService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			EisenSellerList.ForEach(EisenSeller => 
			{
				if (EisenSeller.Blip != null)
					API.deleteEntity(EisenSeller.Blip);
				if (EisenSeller.Ped != null)
					API.deleteEntity(EisenSeller.Ped);
				SaveEisenSeller(EisenSeller);
			});
			EisenSellerList.Clear();
		}

		public static void LoadEisenSellerFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM eisens_seller", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					EisenSeller EisenSeller = new EisenSeller
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<EisenSellerItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

                    EisenSeller.Blip.sprite = 467;
                    EisenSeller.Blip.name = "Iron forwarding";
                    EisenSeller.Blip.shortRange = true;
                    BlipService.BlipService.BlipList.Add(EisenSeller.Blip);

                    EisenSeller.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, EisenSeller.PedPosition, EisenSeller.PedRotation.Z);

					EisenSellerList.Add(EisenSeller);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " EisenSellers Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine EisenSeller geladen ..");
			}
		}

		public static EisenSeller LoadEisenSellerFromDB(int id)
		{
			EisenSeller EisenSeller = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM eisens_seller WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					EisenSeller = new EisenSeller
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<EisenSellerItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return EisenSeller;
		}

		public static void SaveEisenSeller(EisenSeller EisenSeller)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", EisenSeller.Id.ToString() },
				{ "@Owner", EisenSeller.Owner },
				{ "@Storage", JsonConvert.SerializeObject(EisenSeller.Storage)},
				{ "@MoneyStorage", EisenSeller.MoneyStorage.ToString() },
				{ "@PosX", EisenSeller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", EisenSeller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", EisenSeller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", EisenSeller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", EisenSeller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", EisenSeller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", EisenSeller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE eisens_seller SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static EisenSeller AddEisenSeller(Vector3 position)
		{
			EisenSeller EisenSeller = new EisenSeller();
			EisenSeller.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(EisenSeller.Storage)},
				{ "@MoneyStorage", EisenSeller.MoneyStorage.ToString() },
				{ "@PosX", EisenSeller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", EisenSeller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", EisenSeller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", EisenSeller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", EisenSeller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", EisenSeller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", EisenSeller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO eisens_seller (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			EisenSeller.Blip = API.shared.createBlip(EisenSeller.Position);
			EisenSeller.Blip.sprite = 467;
            EisenSeller.Blip.scale = 0.8f;
            EisenSeller.Blip.name = "Iron forwarding";
			EisenSeller.Blip.shortRange = true;

            BlipService.BlipService.BlipList.Add(EisenSeller.Blip);
			EisenSellerList.Add(EisenSeller);
			return EisenSeller;
		}

		public static void OpenEisenSellerMenu(Client client, EisenSeller EisenSeller)
		{
			List<EisenSellerMenuItem> EisenSellerItems = new List<EisenSellerMenuItem>();
			EisenSeller.Storage.ForEach(item => 
			{
				EisenSellerItems.Add(new EisenSellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> EisenSellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					EisenSellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveEisenSeller(EisenSeller);

			API.shared.triggerClientEvent(client, "EisenSeller_OpenMenu", JsonConvert.SerializeObject(EisenSellerItems), JsonConvert.SerializeObject(EisenSellerInventoryItems), EisenSeller.MenuImage);
		}

		public static void RefreshEisenSellerBuyMenu(Client client, EisenSeller EisenSeller)
		{
			List<EisenSellerMenuItem> EisenSellerItems = new List<EisenSellerMenuItem>();
			EisenSeller.Storage.ForEach(item =>
			{
				EisenSellerItems.Add(new EisenSellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> EisenSellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					EisenSellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "EisenSeller_RefreshEisenSellerMenu", JsonConvert.SerializeObject(EisenSellerItems), JsonConvert.SerializeObject(EisenSellerInventoryItems));
        }

		public static void CloseEisenSellerMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "EisenSeller_CloseMenu");
		}
	}
}
