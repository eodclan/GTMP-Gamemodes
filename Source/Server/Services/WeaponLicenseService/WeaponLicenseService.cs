using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.WeaponLicenseService
{
	public class WeaponLicenseService 
		: Script
	{
		public static readonly List<WeaponLicense> WeaponLicenseList = new List<WeaponLicense>();

		public WeaponLicenseService()
		{
			API.onResourceStop += OnResourceStopHandler;
		}

		public void OnResourceStopHandler()
		{
			WeaponLicenseList.ForEach(WeaponLicense => 
			{
				if (WeaponLicense.Blip != null)
					API.deleteEntity(WeaponLicense.Blip);
				if (WeaponLicense.Ped != null)
					API.deleteEntity(WeaponLicense.Ped);
				SaveWeaponLicense(WeaponLicense);
			});
			WeaponLicenseList.Clear();
		}

		public static void LoadWeaponLicenseFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM wlicenses", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					WeaponLicense WeaponLicense = new WeaponLicense
					{
						Id = (int) row["Id"],
						Owner = (string) row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<WeaponLicenseItem>>((string) row["Storage"]),
						MoneyStorage = (double) row["MoneyStorage"],
						Position = new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]),
						PedPosition = new Vector3((float) row["PedPosX"], (float) row["PedPosY"], (float) row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float) row["PedRot"]),
						MenuImage = (string) row["MenuImage"],
						//Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
					};

					//WeaponLicense.Blip.sprite = 498;
					//WeaponLicense.Blip.name = "State";
					//WeaponLicense.Blip.shortRange = true;
					//BlipService.BlipService.BlipList.Add(WeaponLicense.Blip);

					WeaponLicense.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, WeaponLicense.PedPosition, WeaponLicense.PedRotation.Z);

					WeaponLicenseList.Add(WeaponLicense);
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Weapon Licenses Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "Keine Fahrschule geladen ..");
			}
		}

		public static WeaponLicense LoadWeaponLicenseFromDB(int id)
		{
			WeaponLicense WeaponLicense = null;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{"@Id", id.ToString()}
			};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM wlicenses WHERE Id = @Id LIMIT 1", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					WeaponLicense = new WeaponLicense
					{
						Id = (int)row["Id"],
						Owner = (string)row["Owner"],
						Storage = JsonConvert.DeserializeObject<List<WeaponLicenseItem>>((string)row["Storage"]),
						MoneyStorage = (double)row["MoneyStorage"],
						Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
						PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
						PedRotation = new Vector3(0, 0, (float)row["PedRot"])
					};
				}
			}
			return WeaponLicense;
		}

		public static void SaveWeaponLicense(WeaponLicense WeaponLicense)
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Id", WeaponLicense.Id.ToString() },
				{ "@Owner", WeaponLicense.Owner },
				{ "@Storage", JsonConvert.SerializeObject(WeaponLicense.Storage)},
				{ "@MoneyStorage", WeaponLicense.MoneyStorage.ToString() },
				{ "@PosX", WeaponLicense.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", WeaponLicense.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", WeaponLicense.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", WeaponLicense.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", WeaponLicense.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", WeaponLicense.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", WeaponLicense.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE wlicenses SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
				"PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
		}

		public static WeaponLicense AddWeaponLicense(Vector3 position)
		{
			WeaponLicense WeaponLicense = new WeaponLicense();
			WeaponLicense.Position = position;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@Storage", JsonConvert.SerializeObject(WeaponLicense.Storage)},
				{ "@MoneyStorage", WeaponLicense.MoneyStorage.ToString() },
				{ "@PosX", WeaponLicense.Position.X.ToString().Replace(",", ".") },
				{ "@PosY", WeaponLicense.Position.Y.ToString().Replace(",", ".") },
				{ "@PosZ", WeaponLicense.Position.Z.ToString().Replace(",", ".") },
				{ "@PedPosX", WeaponLicense.PedPosition.X.ToString().Replace(",", ".") },
				{ "@PedPosY", WeaponLicense.PedPosition.Y.ToString().Replace(",", ".") },
				{ "@PedPosZ", WeaponLicense.PedPosition.Z.ToString().Replace(",", ".") },
				{ "@PedRot", WeaponLicense.PedRotation.Z.ToString().Replace(",", ".") }
			};

			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO wlicenses (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
				"VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

			WeaponLicense.Blip = API.shared.createBlip(WeaponLicense.Position);
			WeaponLicense.Blip.sprite = 52;
			WeaponLicense.Blip.name = "Weapon License";
			WeaponLicense.Blip.shortRange = true;

			BlipService.BlipService.BlipList.Add(WeaponLicense.Blip);
			WeaponLicenseList.Add(WeaponLicense);
			return WeaponLicense;
		}

		public static void OpenWeaponLicenseMenu(Client client, WeaponLicense WeaponLicense)
		{
			List<WeaponLicenseMenuItem> WeaponLicenseItems = new List<WeaponLicenseMenuItem>();
			WeaponLicense.Storage.ForEach(item => 
			{
				WeaponLicenseItems.Add(new WeaponLicenseMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> WeaponLicenseInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					WeaponLicenseInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			SaveWeaponLicense(WeaponLicense);

			API.shared.triggerClientEvent(client, "WeaponLicense_OpenMenu", JsonConvert.SerializeObject(WeaponLicenseItems), JsonConvert.SerializeObject(WeaponLicenseInventoryItems), WeaponLicense.MenuImage);
		}

		public static void RefreshWeaponLicenseBuyMenu(Client client, WeaponLicense WeaponLicense)
		{
			List<WeaponLicenseMenuItem> WeaponLicenseItems = new List<WeaponLicenseMenuItem>();
			WeaponLicense.Storage.ForEach(item =>
			{
				WeaponLicenseItems.Add(new WeaponLicenseMenuItem
				{
					Id = item.Id,
					Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
					BuyPrice = item.BuyPrice,
					SellPrice = item.SellPrice,
					Count = item.Count
				});
			});

			List<InventoryMenuItem> WeaponLicenseInventoryItems = new List<InventoryMenuItem>();
			Player player = client.getData("player");
			player.Character.Inventory.ForEach(item =>
			{
				Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
				if (invitem == null)
					return;
				if (invitem.Sellable)
				{
					WeaponLicenseInventoryItems.Add(new InventoryMenuItem
					{
						Id = item.ItemID,
						Name = invitem.Name,
						Description = invitem.DefaultSellPrice.ToString(),
						Count = item.Count
					});
				}
			});

			API.shared.triggerClientEvent(client, "WeaponLicense_RefreshWeaponLicenseMenu", JsonConvert.SerializeObject(WeaponLicenseItems), JsonConvert.SerializeObject(WeaponLicenseInventoryItems));
		}

		public static void CloseWeaponLicenseMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "WeaponLicense_CloseMenu");
		}
	}
}
