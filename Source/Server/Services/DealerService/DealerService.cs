using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.DealerService
{
	public class DealerService 
		: Script
	{
		public static readonly List<Dealer> DealerList = new List<Dealer>();

		public DealerService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			DealerList.ForEach(Dealer => 
			{
				if (Dealer.Blip != null)
					API.deleteEntity(Dealer.Blip);
				if (Dealer.Ped != null)
					API.deleteEntity(Dealer.Ped);
				SaveDealer(Dealer);
			});
			DealerList.Clear();
		}

		public static void LoadDealerFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM dealers", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Dealer Dealer = new Dealer
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<DealerItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						//Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

					//Dealer.Blip.sprite = 515;
					//Dealer.Blip.name = "Dealer";
					//Dealer.Blip.shortRange = true;
					//BlipService.BlipService.BlipList.Add(Dealer.Blip);

					Dealer.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, Dealer.PedPosition, Dealer.PedRotation.Z);
                    
                    DealerList.Add(Dealer);
                }

				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Dealers Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Dealer geladen ..");
			}
		}

		public static Dealer LoadDealerFromDB(int id)
		{
			Dealer Dealer = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM dealers WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Dealer = new Dealer
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<DealerItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return Dealer;
		}

		public static void SaveDealer(Dealer Dealer)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", Dealer.Id.ToString() },
				{ "@Owner", Dealer.Owner },
				{ "@Storage", JsonConvert.SerializeObject(Dealer.Storage)},
				{ "@MoneyStorage", Dealer.MoneyStorage.ToString() },
				{ "@PosX", Dealer.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Dealer.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Dealer.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Dealer.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Dealer.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Dealer.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Dealer.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE dealers SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static Dealer AddDealer(Vector3 position)
		{
			Dealer Dealer = new Dealer();
			Dealer.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(Dealer.Storage)},
				{ "@MoneyStorage", Dealer.MoneyStorage.ToString() },
				{ "@PosX", Dealer.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Dealer.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Dealer.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Dealer.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Dealer.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Dealer.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Dealer.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO dealers (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			Dealer.Blip = API.shared.createBlip(Dealer.Position);
			Dealer.Blip.sprite = 501;
            Dealer.Blip.scale = 0.8f;
            Dealer.Blip.name = "Dealer";
			Dealer.Blip.shortRange = true;

			BlipService.BlipService.BlipList.Add(Dealer.Blip);
			DealerList.Add(Dealer);
			return Dealer;
		}

		public static void OpenDealerMenu(Client client, Dealer Dealer)
		{
			List<DealerMenuItem> DealerItems = new List<DealerMenuItem>();
			Dealer.Storage.ForEach(item => 
			{
				DealerItems.Add(new DealerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> DealerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					DealerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveDealer(Dealer);

			API.shared.triggerClientEvent(client, "Dealer_OpenMenu", JsonConvert.SerializeObject(DealerItems), JsonConvert.SerializeObject(DealerInventoryItems), Dealer.MenuImage);
		}

		public static void RefreshDealerBuyMenu(Client client, Dealer Dealer)
		{
			List<DealerMenuItem> DealerItems = new List<DealerMenuItem>();
			Dealer.Storage.ForEach(item =>
			{
				DealerItems.Add(new DealerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> DealerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					DealerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "Dealer_RefreshDealerMenu", JsonConvert.SerializeObject(DealerItems), JsonConvert.SerializeObject(DealerInventoryItems));
		}

		public static void CloseDealerMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "Dealer_CloseMenu");
		}
	}
}
