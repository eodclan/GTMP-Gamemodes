using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;

namespace FactionLife.Server.Services.LItemService
{
	class LItemService
	{
		public static readonly List<LItem> LItemList = new List<LItem>();

        public static int PrimaryColor { get; private set; }
        public static int SecondaryColor { get; private set; }

        public static void LoadLItemsFromDB()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM license_litems", parameters);
			if (result.Rows.Count != 0)
			{
				foreach (DataRow row in result.Rows)
				{
					LItemList.Add(new LItem
					{
						Id = (int)row["Id"],
						Name = (string)row["Name"],
						Description = (string)row["Description"],
						Type = (LItemType)((int)row["Type"]),
						Weight = (int)row["Weight"],
						DefaultPrice = (double)row["DefaultPrice"],
						DefaultSellPrice = (double)row["DefaultSellPrice"],
						Value1 = (int)row["Value1"],
						Value2 = (int)row["Value2"],
						Sellable = Convert.ToBoolean((int)row["Sellable"])
					});
				}
				API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " LItems Loaded..");
			}
			else
			{
				API.shared.consoleOutput(LogCat.Info, "No LItems Loaded..");
			}
		}

        public static void LItemAction(Client client, Player player, LItem LItem)
		{
			OwnedVehicle ownedVehicle = null;

            switch (LItem.Type)
			{
                case LItemType.License: // Value 1 = LicenseType | Value 7 = Intense
                    API.shared.sendNotificationToPlayer(player.Character.Player, "<C>State Center</C> ~r~Du kannst diese Lizenz nicht weg geben!");
                    break;					
			}

			LicenseLItem invLItem = player.Character.License.FirstOrDefault(x => x.LItemID == LItem.Id);

			invLItem.Count--;
			if (invLItem.Count <= 0)
				player.Character.License.Remove(invLItem);
			CharacterService.CharacterService.UpdateHUD(client);
			UpdateInventar(client);
			API.shared.sendNotificationToPlayer(player.Character.Player, "Sie verwenden ein ~g~" + LItem.Name);
		}

		public static bool UseLItem(Client client, int id)
		{
			if (!client.hasData("player"))
				return false;
			if (client.hasData("usagetimer"))
			{
				API.shared.sendNotificationToPlayer(client, "~r~Du hast bereits eine Aktion gestartet..");
				return false;
			}
			Player player = client.getData("player");
			LicenseLItem invLItem = player.Character.License.FirstOrDefault(x => x.LItemID == id);
			if (invLItem == null)
				return false;
			LItem LItem = LItemList.FirstOrDefault(x => x.Id == id);
			if (LItem == null)
				return false;
			OwnedVehicle ownedVehicle = null;
			switch (LItem.Type)
			{
                case LItemType.License: // Value 7 = LicenseType | Value 1 = Verkaufen
                    API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Du kannst diese Lizenz nicht weg geben!");
                    break;				
			}

			API.shared.triggerClientEvent(client, "License_Success");
			return true;
		}

		public static void GiveLItem(Client client, int LItemId, int count)
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
			LItem LItem = LItemList.FirstOrDefault(x => x.Id == LItemId);
			if (LItem == null) { return; }
			LicenseLItem plrinvLItem = player.Character.License.FirstOrDefault(x => x.LItemID == LItemId);
			if (plrinvLItem == null || plrinvLItem.Count <= 0 || count <= 0) { return; }
			if (plrinvLItem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Du hast nicht genügend Gegenstände dieser Art in deinem Inventar!"); return; }

			LicenseLItem targetinvLItem = target.Character.License.FirstOrDefault(x => x.LItemID == LItemId);
			if (targetinvLItem == null)
			{
				target.Character.License.Add(new LicenseLItem
				{
					LItemID = LItemId,
					Count = count
				});
			}
			else
			{
				targetinvLItem.Count += count;
			}
			API.shared.sendNotificationToPlayer(player.Character.Player, "Du gibts ~b~" + count + "x ~g~" + LItem.Name + "~w~ weg");
			API.shared.sendNotificationToPlayer(target.Character.Player, "Du erhältst ~b~" + count + "x ~g~" + LItem.Name);

			plrinvLItem.Count -= count;
			if (plrinvLItem.Count <= 0)
			{
				player.Character.License.Remove(plrinvLItem);
			}

			CharacterService.CharacterService.UpdateCharacter(player.Character);
			CharacterService.CharacterService.UpdateCharacter(target.Character);
			UpdateInventar(client);
			API.shared.triggerClientEvent(client, "License_Success");
		}

		public static void ThrowAway(Client client, int LItemId, int count)
		{
			if (!client.hasData("player"))
				return;
			Player player = client.getData("player");
			LItem LItem = LItemList.FirstOrDefault(x => x.Id == LItemId);
			if (LItem == null) { return; }
			LicenseLItem plrinvLItem = player.Character.License.FirstOrDefault(x => x.LItemID == LItemId);
			if (plrinvLItem == null || plrinvLItem.Count <= 0 || count <= 0) { return; }
			if (plrinvLItem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Du hast nicht genügend Gegenstände dieser Art in deinem Inventar!"); return; }
			plrinvLItem.Count -= count;
			if (plrinvLItem.Count <= 0)
			{
				player.Character.License.Remove(plrinvLItem);
			}
			API.shared.sendNotificationToPlayer(player.Character.Player, "Du wirfst ~b~" + count + "x ~g~" + LItem.Name + " weg");
			UpdateInventar(client);
			API.shared.triggerClientEvent(client, "License_Success");
		}

		public static void UpdateInventar(Client client)
		{
			API.shared.triggerClientEvent(client, "License_Data", JsonConvert.SerializeObject(CharacterService.CharacterService.GetLicenseMenuLItems(client)));
		}

		public static void StartUsageTimer(Client client, Player player, LItem LItem, int time)
		{
			if(time <= 0)
			{
				LItemAction(client, player, LItem);
				return;
			}
			int count = 0;
			InterfaceService.ProgressBarService.ShowBar(client, 0, time, "Benutze " + LItem.Name);
			client.setData("usagetimer", API.shared.startTimer(1000, false, () =>
			{
				InterfaceService.ProgressBarService.ChangeProgress(client, count);
				if(count >= time)
				{
					InterfaceService.ProgressBarService.HideBar(client);
					LItemAction(client, player, LItem);
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
