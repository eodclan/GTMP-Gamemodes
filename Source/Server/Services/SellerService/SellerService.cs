using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.SellerService
{
	public class SellerService 
		: Script
	{
		public static readonly List<Seller> SellerList = new List<Seller>();

		public SellerService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			SellerList.ForEach(Seller => 
			{
				if (Seller.Blip != null)
					API.deleteEntity(Seller.Blip);
				if (Seller.Ped != null)
					API.deleteEntity(Seller.Ped);
				SaveSeller(Seller);
			});
			SellerList.Clear();
		}

		public static void LoadSellerFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM sellers", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Seller Seller = new Seller
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<SellerItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						//Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

                    //Seller.Blip.sprite = 604;
                    //Seller.Blip.scale = 0.8f;
                    //Seller.Blip.name = "Seller";
                    //Seller.Blip.shortRange = true;
                    //BlipService.BlipService.BlipList.Add(Seller.Blip);

                    Seller.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, Seller.PedPosition, Seller.PedRotation.Z);

					SellerList.Add(Seller);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Sellers Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Seller geladen ..");
			}
		}

		public static Seller LoadSellerFromDB(int id)
		{
			Seller Seller = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM sellers WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					Seller = new Seller
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<SellerItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return Seller;
		}

		public static void SaveSeller(Seller Seller)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", Seller.Id.ToString() },
				{ "@Owner", Seller.Owner },
				{ "@Storage", JsonConvert.SerializeObject(Seller.Storage)},
				{ "@MoneyStorage", Seller.MoneyStorage.ToString() },
				{ "@PosX", Seller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Seller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Seller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Seller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Seller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Seller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Seller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE sellers SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static Seller AddSeller(Vector3 position)
		{
			Seller Seller = new Seller();
			Seller.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(Seller.Storage)},
				{ "@MoneyStorage", Seller.MoneyStorage.ToString() },
				{ "@PosX", Seller.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", Seller.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", Seller.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", Seller.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", Seller.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", Seller.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", Seller.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO sellers (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			Seller.Blip = API.shared.createBlip(Seller.Position);
			Seller.Blip.sprite = 501;
            Seller.Blip.scale = 0.8f;
            Seller.Blip.name = "Seller";
			Seller.Blip.shortRange = true;

            BlipService.BlipService.BlipList.Add(Seller.Blip);
			SellerList.Add(Seller);
			return Seller;
		}

		public static void OpenSellerMenu(Client client, Seller Seller)
		{
			List<SellerMenuItem> SellerItems = new List<SellerMenuItem>();
			Seller.Storage.ForEach(item => 
			{
				SellerItems.Add(new SellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> SellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					SellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveSeller(Seller);

			API.shared.triggerClientEvent(client, "Seller_OpenMenu", JsonConvert.SerializeObject(SellerItems), JsonConvert.SerializeObject(SellerInventoryItems), Seller.MenuImage);
		}

		public static void RefreshSellerBuyMenu(Client client, Seller Seller)
		{
			List<SellerMenuItem> SellerItems = new List<SellerMenuItem>();
			Seller.Storage.ForEach(item =>
			{
				SellerItems.Add(new SellerMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> SellerInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					SellerInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "Seller_RefreshSellerMenu", JsonConvert.SerializeObject(SellerItems), JsonConvert.SerializeObject(SellerInventoryItems));
		}

		public static void CloseSellerMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "Seller_CloseMenu");
		}
	}
}
