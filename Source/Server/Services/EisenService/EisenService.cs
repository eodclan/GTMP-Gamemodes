using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.EisenService
{
	public class EisenService 
		: Script
	{
		public static readonly List<Eisen> EisenList = new List<Eisen>();

		public EisenService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			EisenList.ForEach(Eisen => 
			{
				if (Eisen.Blip != null)
					API.deleteEntity(Eisen.Blip);
				if (Eisen.Ped != null)
					API.deleteEntity(Eisen.Ped);
				SaveEisen(Eisen);
			});
			EisenList.Clear();
		}

		public static void LoadEisenFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM eisens", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Eisen Eisen = new Eisen
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<EisenItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

					Eisen.Blip.sprite = 479;
					Eisen.Blip.name = "Mountain of iron";
					Eisen.Blip.shortRange = true;
					BlipService.BlipService.BlipList.Add(Eisen.Blip);

					Eisen.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, Eisen.PedPosition, Eisen.PedRotation.Z);

					EisenList.Add(Eisen);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Eisens Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Eisen geladen ..");
			}
		}

		public static Eisen LoadEisenFromDB(int id)
		{
			Eisen Eisen = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM eisens WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Eisen = new Eisen
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<EisenItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return Eisen;
		}

		public static void SaveEisen(Eisen Eisen)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", Eisen.Id.ToString() },
				{ "@Owner", Eisen.Owner },
				{ "@Storage", JsonConvert.SerializeObject(Eisen.Storage)},
				{ "@MoneyStorage", Eisen.MoneyStorage.ToString() },
				{ "@PosX", Eisen.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Eisen.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Eisen.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Eisen.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Eisen.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Eisen.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Eisen.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE eisens SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static Eisen AddEisen(Vector3 position)
		{
			Eisen Eisen = new Eisen();
			Eisen.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(Eisen.Storage)},
				{ "@MoneyStorage", Eisen.MoneyStorage.ToString() },
				{ "@PosX", Eisen.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Eisen.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Eisen.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Eisen.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Eisen.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Eisen.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Eisen.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO eisens (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			Eisen.Blip = API.shared.createBlip(Eisen.Position);
			Eisen.Blip.sprite = 479;
            Eisen.Blip.scale = 0.8f;
            Eisen.Blip.name = "Mountain of iron";
			Eisen.Blip.shortRange = true;

			BlipService.BlipService.BlipList.Add(Eisen.Blip);
			EisenList.Add(Eisen);
			return Eisen;
		}

		public static void OpenEisenMenu(Client client, Eisen Eisen)
		{
			List<EisenMenuItem> EisenItems = new List<EisenMenuItem>();
			Eisen.Storage.ForEach(item => 
			{
				EisenItems.Add(new EisenMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> EisenInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					EisenInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveEisen(Eisen);

			API.shared.triggerClientEvent(client, "Eisen_OpenMenu", JsonConvert.SerializeObject(EisenItems), JsonConvert.SerializeObject(EisenInventoryItems), Eisen.MenuImage);
		}

		public static void RefreshEisenBuyMenu(Client client, Eisen Eisen)
		{
			List<EisenMenuItem> EisenItems = new List<EisenMenuItem>();
			Eisen.Storage.ForEach(item =>
			{
				EisenItems.Add(new EisenMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> EisenInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					EisenInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "Eisen_RefreshEisenMenu", JsonConvert.SerializeObject(EisenItems), JsonConvert.SerializeObject(EisenInventoryItems));
		}

		public static void CloseEisenMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "Eisen_CloseMenu");
		}
	}
}
