using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.HolzService
{
	public class HolzService 
		: Script
	{
		public static readonly List<Holz> HolzList = new List<Holz>();

		public HolzService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			HolzList.ForEach(Holz => 
			{
				if (Holz.Blip != null)
					API.deleteEntity(Holz.Blip);
				if (Holz.Ped != null)
					API.deleteEntity(Holz.Ped);
				SaveHolz(Holz);
			});
			HolzList.Clear();
		}

		public static void LoadHolzFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM holz_job", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Holz Holz = new Holz
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<HolzItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

					Holz.Blip.sprite = 592;
					Holz.Blip.name = "Motion of Wood";
					Holz.Blip.shortRange = true;
					BlipService.BlipService.BlipList.Add(Holz.Blip);

					Holz.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, Holz.PedPosition, Holz.PedRotation.Z);

					HolzList.Add(Holz);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Holzs Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Holz geladen ..");
			}
		}

		public static Holz LoadHolzFromDB(int id)
		{
			Holz Holz = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM holz_job WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Holz = new Holz
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<HolzItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return Holz;
		}

		public static void SaveHolz(Holz Holz)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", Holz.Id.ToString() },
				{ "@Owner", Holz.Owner },
				{ "@Storage", JsonConvert.SerializeObject(Holz.Storage)},
				{ "@MoneyStorage", Holz.MoneyStorage.ToString() },
				{ "@PosX", Holz.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Holz.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Holz.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Holz.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Holz.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Holz.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Holz.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE holz_job SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static Holz AddHolz(Vector3 position)
		{
			Holz Holz = new Holz();
			Holz.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(Holz.Storage)},
				{ "@MoneyStorage", Holz.MoneyStorage.ToString() },
				{ "@PosX", Holz.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Holz.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Holz.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Holz.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Holz.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Holz.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Holz.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO holz_job (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			Holz.Blip = API.shared.createBlip(Holz.Position);
			Holz.Blip.sprite = 592;
            Holz.Blip.name = "Motion of Wood";
			Holz.Blip.shortRange = true;

			BlipService.BlipService.BlipList.Add(Holz.Blip);
			HolzList.Add(Holz);
			return Holz;
		}

		public static void OpenHolzMenu(Client client, Holz Holz)
		{
			List<HolzMenuItem> HolzItems = new List<HolzMenuItem>();
			Holz.Storage.ForEach(item => 
			{
				HolzItems.Add(new HolzMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> HolzInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					HolzInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveHolz(Holz);

			API.shared.triggerClientEvent(client, "Holz_OpenMenu", JsonConvert.SerializeObject(HolzItems), JsonConvert.SerializeObject(HolzInventoryItems), Holz.MenuImage);
		}

		public static void RefreshHolzBuyMenu(Client client, Holz Holz)
		{
			List<HolzMenuItem> HolzItems = new List<HolzMenuItem>();
			Holz.Storage.ForEach(item =>
			{
				HolzItems.Add(new HolzMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> HolzInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					HolzInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "Holz_RefreshHolzMenu", JsonConvert.SerializeObject(HolzItems), JsonConvert.SerializeObject(HolzInventoryItems));
		}

		public static void CloseHolzMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "Holz_CloseMenu");
		}
	}
}
