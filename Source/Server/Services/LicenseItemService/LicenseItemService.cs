using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;

namespace FactionLife.Server.Services.LicensesItemService
{
	class LicensesItemService
	{
		public static readonly List<LicensesItem> LicensesItemList = new List<LicensesItem>();

		public static void LoadLicensesItemsFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM licenses_items", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					LicensesItemList.Add(new LicensesItem
					{
						Id = (int)row["Id"],
						Name = (string)row["Name"],
						Description = (string)row["Description"],
						Type = (LicensesItem)((int)row["Type"]),
						Weight = (int)row["Weight"],
						DefaultPrice = (double)row["DefaultPrice"],
						DefaultSellPrice = (double)row["DefaultSellPrice"],
						Value1 = (int)row["Value1"],
						Value2 = (int)row["Value2"],
						Sellable = Convert.ToBoolean((int)row["Sellable"])
					});
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " License Items Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "No License Items Loaded..");
			}
		}

		public static void LicensesItemAction(Client client, Player player, LicensesItem licensesitem)
		{
			OwnedVehicle ownedVehicle = null;
			switch (LicensesItem.Type)
			{
				case LicensesItem.FSchein:
					break;
				case LicensesItem.Perso:
					break;
				case LicensesItem.TaxiF:
					break;
				case LicensesItem.Phone:
					break;					
            }

			InventoryLicensesItem invLicensesItem = player.Character.Inventory.FirstOrDefault(x => x.LicensesItemID == LicensesItem.Id);

			invLicensesItem.Count--;
			if (invLicensesItem.Count <= 0)
				player.Character.Inventory.Remove(invLicensesItem);
			CharacterService.CharacterService.UpdateHUD(client);
			UpdateInventar(client);
			API.shared.sendNotificationToPlayer(player.Character.Player, "Sie verwenden ein ~g~" + LicensesItem.Name);
		}

		public static bool UseLicensesItem(Client client, int id)
		{
			if (!client.hasData("player"))
				return false;
			if (client.hasData("usagetimer"))
			{
				API.shared.sendNotificationToPlayer(client, "~r~Du hast bereits eine Aktion gestartet..");
				return false;
			}
			Player player = client.getData("player");
			InventoryLicensesItem invLicensesItem = player.Character.Inventory.FirstOrDefault(x => x.LicensesItemID == id);
			if (invLicensesItem == null)
				return false;
			LicensesItem LicensesItem = LicensesItemList.FirstOrDefault(x => x.Id == id);
			if (LicensesItem == null)
				return false;
			OwnedVehicle ownedVehicle = null;
			switch (LicensesItem.Type)
			{
				case LicensesItem.FSchein:
					break;
				case LicensesItem.Perso:
					break;
				case LicensesItem.TaxiF:
					break;
				case LicensesItem.Phone:
					break;
            }

			API.shared.triggerClientEvent(client, "Inventory_Success");
			return true;
		}

		public static void GiveLicensesItem(Client client, int LicensesItemId, int count)
		{
			if (!client.hasData("player"))
				return;
			Player player = client.getData("player");
			List<Player> playersaround = new List<Player>();
			API.shared.getPlayersInRadiusOfPlayer(1, client).ForEach(x => {
				if (x.hasData("player"))
				{
					playersaround.Add((Player)x.getData("player"));
				}
			});
			if (playersaround.Any(x => x == player))
			{
				playersaround.Remove(player);
			}
			if (playersaround.Count > 1)
			{
				API.shared.sendNotificationToPlayer(client, "~r~Zu viele Leute um dich herum..");
				return;
			}
			if (playersaround.Count < 1)
			{
				API.shared.sendNotificationToPlayer(client, "~r~Niemand ist um dich herum..");
				return;
			}
			Player target = playersaround.FirstOrDefault();
			if (target == null) { API.shared.sendNotificationToPlayer(client, "~r~Ooops something went wrong.."); return; }
			LicensesItem LicensesItem = LicensesItemList.FirstOrDefault(x => x.Id == LicensesItemId);
			if (LicensesItem == null) { return; }
			InventoryLicensesItem plrinvLicensesItem = player.Character.Inventory.FirstOrDefault(x => x.LicensesItemID == LicensesItemId);
			if (plrinvLicensesItem == null || plrinvLicensesItem.Count <= 0 || count <= 0) { return; }
			if (plrinvLicensesItem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Du hast nicht genügend Gegenstände dieser Art in deinem Inventar!"); return; }

			InventoryLicensesItem targetinvLicensesItem = target.Character.Inventory.FirstOrDefault(x => x.LicensesItemID == LicensesItemId);
			if (targetinvLicensesItem == null)
			{
				target.Character.Inventory.Add(new InventoryLicensesItem
				{
					LicensesItemID = LicensesItemId,
					Count = count
				});
			}
			else
			{
				targetinvLicensesItem.Count += count;
			}
			API.shared.sendNotificationToPlayer(player.Character.Player, "Sie geben ~b~" + count + "x ~g~" + LicensesItem.Name + "~w~ weg");
			API.shared.sendNotificationToPlayer(target.Character.Player, "Du erhältst ~b~" + count + "x ~g~" + LicensesItem.Name);

			plrinvLicensesItem.Count -= count;
			if (plrinvLicensesItem.Count <= 0)
			{
				player.Character.Inventory.Remove(plrinvLicensesItem);
			}

			CharacterService.CharacterService.UpdateCharacter(player.Character);
			CharacterService.CharacterService.UpdateCharacter(target.Character);
			UpdateInventar(client);
			API.shared.triggerClientEvent(client, "Inventory_Success");
		}

		public static void ThrowAway(Client client, int LicensesItemId, int count)
		{
			if (!client.hasData("player"))
				return;
			Player player = client.getData("player");
			LicensesItem LicensesItem = LicensesItemList.FirstOrDefault(x => x.Id == LicensesItemId);
			if (LicensesItem == null) { return; }
			InventoryLicensesItem plrinvLicensesItem = player.Character.Inventory.FirstOrDefault(x => x.LicensesItemID == LicensesItemId);
			if (plrinvLicensesItem == null || plrinvLicensesItem.Count <= 0 || count <= 0) { return; }
			if (plrinvLicensesItem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Du hast nicht genügend Gegenstände dieser Art in deinem Inventar!"); return; }
			plrinvLicensesItem.Count -= count;
			if (plrinvLicensesItem.Count <= 0)
			{
				player.Character.Inventory.Remove(plrinvLicensesItem);
			}
			API.shared.sendNotificationToPlayer(player.Character.Player, "Du wirfst ~b~" + count + "x ~g~" + LicensesItem.Name + " weg");
			UpdateInventar(client);
			API.shared.triggerClientEvent(client, "Inventory_Success");
		}

		public static void UpdateInventar(Client client)
		{
			API.shared.triggerClientEvent(client, "Inventory_Data", JsonConvert.SerializeObject(CharacterService.CharacterService.GetInventoryMenuLicensesItems(client)));
		}

		public static void StartUsageTimer(Client client, Player player, LicensesItem LicensesItem, int time)
		{
			if(time <= 0)
			{
				LicensesItemAction(client, player, LicensesItem);
				return;
			}
			int count = 0;
			InterfaceService.ProgressBarService.ShowBar(client, 0, time, "Use " + LicensesItem.Name);
			client.setData("usagetimer", API.shared.startTimer(1000, false, () =>
			{
				InterfaceService.ProgressBarService.ChangeProgress(client, count);
				if(count >= time)
				{
					InterfaceService.ProgressBarService.HideBar(client);
					LicensesItemAction(client, player, LicensesItem);
					API.shared.stopTimer(client.getData("usagetimer"));
					client.resetData("usagetimer");
					API.shared.stopPlayerAnimation(client);
					return;
				}

				count++;
			}));
		}
	}
}
