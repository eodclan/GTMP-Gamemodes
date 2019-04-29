using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.FisherSellService
{
	public class FisherSellService 
		: Script
	{
		public static readonly List<FisherSell> FisherSellList = new List<FisherSell>();

		public FisherSellService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			FisherSellList.ForEach(FisherSell => 
			{
				if (FisherSell.Blip != null)
					API.deleteEntity(FisherSell.Blip);
				if (FisherSell.Ped != null)
					API.deleteEntity(FisherSell.Ped);
				SaveFisherSell(FisherSell);
			});
			FisherSellList.Clear();
		}

		public static void LoadFisherSellFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM fisher_restaurant", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					FisherSell FisherSell = new FisherSell
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<FisherSellItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

                    FisherSell.Blip.sprite = 604;
                    FisherSell.Blip.scale = 0.9f;
                    FisherSell.Blip.name = "Sushi Garden";
                    FisherSell.Blip.shortRange = true;
                    BlipService.BlipService.BlipList.Add(FisherSell.Blip);

                    FisherSell.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, FisherSell.PedPosition, FisherSell.PedRotation.Z);

					FisherSellList.Add(FisherSell);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " FisherSells Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine FisherSell geladen ..");
			}
		}

		public static FisherSell LoadFisherSellFromDB(int id)
		{
			FisherSell FisherSell = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM fisher_restaurant WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					FisherSell = new FisherSell
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<FisherSellItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return FisherSell;
		}

		public static void SaveFisherSell(FisherSell FisherSell)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", FisherSell.Id.ToString() },
				{ "@Owner", FisherSell.Owner },
				{ "@Storage", JsonConvert.SerializeObject(FisherSell.Storage)},
				{ "@MoneyStorage", FisherSell.MoneyStorage.ToString() },
				{ "@PosX", FisherSell.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", FisherSell.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", FisherSell.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", FisherSell.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", FisherSell.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", FisherSell.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", FisherSell.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE fisher_restaurant SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static FisherSell AddFisherSell(Vector3 position)
		{
			FisherSell FisherSell = new FisherSell();
			FisherSell.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(FisherSell.Storage)},
				{ "@MoneyStorage", FisherSell.MoneyStorage.ToString() },
				{ "@PosX", FisherSell.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", FisherSell.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", FisherSell.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", FisherSell.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", FisherSell.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", FisherSell.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", FisherSell.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO fisher_restaurant (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			FisherSell.Blip = API.shared.createBlip(FisherSell.Position);
			FisherSell.Blip.sprite = 604;
            FisherSell.Blip.scale = 0.9f;
            FisherSell.Blip.name = "FisherSell";
			FisherSell.Blip.shortRange = true;

            BlipService.BlipService.BlipList.Add(FisherSell.Blip);
			FisherSellList.Add(FisherSell);
			return FisherSell;
		}

		public static void OpenFisherSellMenu(Client client, FisherSell FisherSell)
		{
			List<FisherSellMenuItem> FisherSellItems = new List<FisherSellMenuItem>();
			FisherSell.Storage.ForEach(item => 
			{
				FisherSellItems.Add(new FisherSellMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> FisherSellInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					FisherSellInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveFisherSell(FisherSell);

			API.shared.triggerClientEvent(client, "FisherSell_OpenMenu", JsonConvert.SerializeObject(FisherSellItems), JsonConvert.SerializeObject(FisherSellInventoryItems), FisherSell.MenuImage);
		}

		public static void RefreshFisherSellBuyMenu(Client client, FisherSell FisherSell)
		{
			List<FisherSellMenuItem> FisherSellItems = new List<FisherSellMenuItem>();
			FisherSell.Storage.ForEach(item =>
			{
				FisherSellItems.Add(new FisherSellMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> FisherSellInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					FisherSellInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "FisherSell_RefreshFisherSellMenu", JsonConvert.SerializeObject(FisherSellItems), JsonConvert.SerializeObject(FisherSellInventoryItems));
		}

		public static void CloseFisherSellMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "FisherSell_CloseMenu");
		}
	}
}
