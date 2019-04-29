using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.GoldService
{
	public class GoldService 
		: Script
	{
		public static readonly List<Gold> GoldList = new List<Gold>();

		public GoldService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			GoldList.ForEach(Gold => 
			{
				if (Gold.Blip != null)
					API.deleteEntity(Gold.Blip);
				if (Gold.Ped != null)
					API.deleteEntity(Gold.Ped);
				SaveGold(Gold);
			});
			GoldList.Clear();
		}

		public static void LoadGoldFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM golds", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Gold Gold = new Gold
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<GoldItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

					Gold.Blip.sprite = 570;
					Gold.Blip.name = "Gold Mine";
					Gold.Blip.shortRange = true;
					BlipService.BlipService.BlipList.Add(Gold.Blip);

					//Gold.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, Gold.PedPosition, Gold.PedRotation.Z);

					GoldList.Add(Gold);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Golds Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Gold geladen ..");
			}
		}

		public static Gold LoadGoldFromDB(int id)
		{
			Gold Gold = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM golds WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Gold = new Gold
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<GoldItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return Gold;
		}

		public static void SaveGold(Gold Gold)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", Gold.Id.ToString() },
				{ "@Owner", Gold.Owner },
				{ "@Storage", JsonConvert.SerializeObject(Gold.Storage)},
				{ "@MoneyStorage", Gold.MoneyStorage.ToString() },
				{ "@PosX", Gold.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Gold.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Gold.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Gold.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Gold.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Gold.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Gold.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE golds SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static Gold AddGold(Vector3 position)
		{
			Gold Gold = new Gold();
			Gold.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(Gold.Storage)},
				{ "@MoneyStorage", Gold.MoneyStorage.ToString() },
				{ "@PosX", Gold.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Gold.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Gold.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Gold.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Gold.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Gold.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Gold.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO golds (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			Gold.Blip = API.shared.createBlip(Gold.Position);
			Gold.Blip.sprite = 570;
            Gold.Blip.name = "Gold Mine";
			Gold.Blip.shortRange = true;

			BlipService.BlipService.BlipList.Add(Gold.Blip);
			GoldList.Add(Gold);
			return Gold;
		}

		public static void OpenGoldMenu(Client client, Gold Gold)
		{
			List<GoldMenuItem> GoldItems = new List<GoldMenuItem>();
			Gold.Storage.ForEach(item => 
			{
				GoldItems.Add(new GoldMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> GoldInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					GoldInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveGold(Gold);

			API.shared.triggerClientEvent(client, "Gold_OpenMenu", JsonConvert.SerializeObject(GoldItems), JsonConvert.SerializeObject(GoldInventoryItems), Gold.MenuImage);
		}

		public static void RefreshGoldBuyMenu(Client client, Gold Gold)
		{
			List<GoldMenuItem> GoldItems = new List<GoldMenuItem>();
			Gold.Storage.ForEach(item =>
			{
				GoldItems.Add(new GoldMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> GoldInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					GoldInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "Gold_RefreshGoldMenu", JsonConvert.SerializeObject(GoldItems), JsonConvert.SerializeObject(GoldInventoryItems));
		}

		public static void CloseGoldMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "Gold_CloseMenu");
		}
	}
}
