using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Services.MoneyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FactionLife.Server.Services.FactionService
{
	class SheriffService
		: Script
	{
        public static Client client { get; private set; }

        public SheriffService()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "Sheriff_ApplyCuffedAnimation":
					API.playPlayerAnimation(client, (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl), "mp_arresting", "idle");
					break;
			}
		}

        public static void SetPlayerCuffed(Client client, bool cuffed)
		{
			API.shared.stopPlayerAnimation(client);
			API.shared.freezePlayer(client, false);
			if (cuffed) API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl), "mp_arresting", "idle");
			API.shared.triggerClientEvent(client, "Client_cuffed", cuffed);
			client.setSyncedData("cuffed", cuffed);
			if (!cuffed) { client.resetSyncedData("cuffed"); }
			API.shared.triggerClientEvent(client, "Client_Cuffed", cuffed);
			if (!client.hasData("player")) { return; }
			Player player = client.getData("player");
			player.Character.IsCuffed = cuffed;
			player.Character.HasHandsup = false;
		}

		public static void SetOnDuty(Client client, bool onDuty)
		{
			if (!client.hasData("player")) { return; }
			Player player = client.getData("player");

			player.Character.OnDuty = onDuty;

			if (onDuty)
			{
				if (player.Character.Gender == Gender.Male)
				{
					ClothingService.ClothingService.ApplyOutfit(client, 17); // Male Cop Outfit
                }
				else
				{
					ClothingService.ClothingService.ApplyOutfit(client, 18); // Female Cop Outfit
                }
            }
			else
			{
				CharacterService.CharacterService.ApplyAppearance(client);
			}

			client.setSyncedData("onduty", onDuty);
			client.setSyncedData("faction", (int)player.Character.Faction);
			if (!onDuty) {
				client.resetSyncedData("onduty");
				client.resetSyncedData("faction");
				API.shared.removeAllPlayerWeapons(client);
			}


			API.shared.getAllPlayers().ForEach(otherclient => {
				if (otherclient.hasData("player"))
				{
					Player otherplayer = otherclient.getData("player");
					if(otherplayer.Character.Faction == FactionType.Sheriff && otherplayer.Character.OnDuty)
					{
                        if (onDuty)
                        {
                            API.shared.sendPictureNotificationToPlayer(otherclient, "Herr " + player.Character.FirstName + " " + player.Character.LastName + " ~w~ist jetzt ~g~im Dienst ~w~gegangen", "CHAR_CALL911", 0, 2, "Dienststelle", "Sheriff Information");
                        }
                        else
                        {
                            API.shared.sendPictureNotificationToPlayer(otherclient, "Herr " + player.Character.FirstName + " " + player.Character.LastName + " ~w~ist jetzt ~g~ausser Dienst ~w~gegangen", "CHAR_CALL911", 0, 2, "Dienststelle", "Sheriff Information");
                        }
                    }
				}
			});
		}

		public static void SearchThrough(Client client, Client targetclient)
		{
			if(targetclient == null) { return; }
			if(!client.hasData("player") || !targetclient.hasData("player")) { return; }
			Player player = client.getData("player");
			Player target = targetclient.getData("player");

			// ToDo..
		}

		public static void Ticket(Client client, Client targetclient, double amount)
		{
            if (MoneyService.MoneyService.HasPlayerEnoughCash(client, amount))
            {
                if (targetclient == null)
                {
                    return;
                }
                if (!client.hasData("player") || !targetclient.hasData("player"))
                {
                    return;
                }
                Player player = client.getData("player");
                Player target = targetclient.getData("player");
                player.Character.Cash -= amount;
                target.Character.Cash += amount;
                player.Account.Player.setSyncedData("cash", player.Character.Cash);
                CharacterService.CharacterService.UpdateCharacter(player.Character);
                target.Account.Player.setSyncedData("cash", target.Character.Cash);
                CharacterService.CharacterService.UpdateCharacter(target.Character);
                API.shared.sendNotificationToPlayer(client, "<C>Maze Bank:</C> ~r~Du hast gerade ein Geld Betrag weitergegeben!");
            }
            else
            {
                API.shared.sendNotificationToPlayer(client, "<C>Maze Bank:</C> ~r~Sie haben nicht genug Geld in Ihrem Geldbeutel.");
            }
        }

		public static void CloseInteractionMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "Sheriff_CloseInteractionMenu");
		}

		#region Sheriff Interaction Menu
		public static List<MenuItem> BuildInteractionMenu(Player player)
		{
			List<MenuItem> menuItemList = new List<MenuItem>();

			// Person Options
			Player otherPlayer = CharacterService.CharacterService.GetNextPlayerInNearOfPlayer(player);
			if (otherPlayer != null)
			{
				if (otherPlayer.Character.IsCuffed)
				{
					menuItemList.Add(new MenuItem
					{
						Title = "~b~Handschellen ablegen",
						Value1 = "togglecuffed"
					});
				}
				else
				{
					if (otherPlayer.Character.HasHandsup)
					{
						menuItemList.Add(new MenuItem
						{
							Title = "~b~Handschellen anlegen",
							Value1 = "togglecuffed"
						});
					}
				}
			}

            // Vehicle Options
            if (!player.Character.Player.isInVehicle)
			{
				OwnedVehicle ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(player.Character.Player.position) <= 4f);
				if (ownedVehicle != null)
				{

					if (!player.Character.Player.hasData("impoundtimer"))
					{
						menuItemList.Add(new MenuItem
						{
							Title = "~b~Fahrzeug beschlagnahmt",
							Value1 = "vehicleimpound"
						});
                    }

					bool CuffedPlayerInVehicle = false;
					ownedVehicle.ActiveHandle.occupants.ToList().ForEach(occ => {
						if (occ.hasData("player"))
						{
							Player occplr = occ.getData("player");
							if (occplr.Character.IsCuffed)
							{
								CuffedPlayerInVehicle = true;
							}
						}
					});
					if (CuffedPlayerInVehicle)
					{
						menuItemList.Add(new MenuItem
						{
							Title = "~b~Ziehen Sie den Spieler aus den Fahrzeug",
							Value1 = "pulloutcuffedplayer"
						});
					}
				}
			}
			return menuItemList;
		}
		

		public static void ProcessInteractionMenu(Client client, string itemvalue)
		{
			if (!client.hasData("player")) { return; }
			Player player = client.getData("player");
			OwnedVehicle ownedVehicle = null;
			Player otherPlayer = null;

			switch (itemvalue)
			{
				case "togglecuffed":
					otherPlayer = CharacterService.CharacterService.GetNextPlayerInNearOfPlayer(player);
					if (otherPlayer != null)
					{
						SetPlayerCuffed(otherPlayer.Character.Player, !otherPlayer.Character.IsCuffed);
					}
					CloseInteractionMenu(client);
					break;
                case "vehicleimpound":
					ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(player.Character.Player.position) <= 4f);
					if(ownedVehicle != null)
					{
						if (client.hasData("impoundtimer")) { CloseInteractionMenu(client); return; }
						client.setData("impoundtimercount", 0);
						client.setData("impoundvehicle", ownedVehicle);
						InterfaceService.ProgressBarService.ShowBar(client, 0, 30, "Impound " + ownedVehicle.ModelName);
						client.setData("impoundtimer", API.shared.startTimer(1000, false, () => {
							OwnedVehicle impoundveh = client.getData("impoundvehicle");
							int count = client.getData("impoundtimercount");
							if(count == 30)
							{
								ImpoundVehicle(client, impoundveh);
                                MoneyService.MoneyService.AddPlayerCash(client, 290);
                                API.shared.sendNotificationToPlayer(client, $"~y~State Center: ~n~~o~Du hast dafür 290$ erhalten.");
                                return;
							}

							if (client.position.DistanceTo(impoundveh.ActiveHandle.position) > 6f)
							{
								API.shared.sendNotificationToPlayer(client, "~r~Aufschub abgesagt! ~n~~o~Zu weit vom Zielfahrzeug entfernt.");
								StopImpoundTimer(client);
								return;
							}

							count++;
							InterfaceService.ProgressBarService.ChangeProgress(client, count);
							client.setData("impoundtimercount", count);
						}));

						CloseInteractionMenu(client);
					}
					break;
				case "pulloutcuffedplayer":
					ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(player.Character.Player.position) <= 4f);
					if (ownedVehicle != null)
					{
						ownedVehicle.ActiveHandle.occupants.ToList().ForEach(occ => {
							if (occ.hasData("player"))
							{
								Player occplr = occ.getData("player");
								if (occplr.Character.IsCuffed)
								{
									API.shared.warpPlayerOutOfVehicle(occ);
								}
							}
						});
						CloseInteractionMenu(client);
					}
					break;
			}
		}
		#endregion Sheriff Interaction Menu

		#region Sheriff Weapon Menu
		public static List<MenuItem> BuildWeaponMenu(Player player)
		{
			List<MenuItem> menuItemList = new List<MenuItem>();

			// Cop Rank 0 Weapons
			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.Flashlight.ToString(),
				Value1 = WeaponHash.Flashlight.ToString()
			});

			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.Parachute.ToString(),
				Value1 = WeaponHash.Parachute.ToString()
			});

			if (player.Character.FactionRank < 1) { return menuItemList; }
			// Cop Rank 1 Weapons
			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.Nightstick.ToString(),
				Value1 = WeaponHash.Nightstick.ToString()
			});

			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.StunGun.ToString(),
				Value1 = WeaponHash.StunGun.ToString()
			});

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.Pistol.ToString(),
                Value1 = WeaponHash.Pistol.ToString()
            });

            if (player.Character.FactionRank < 2) { return menuItemList; }
			// Cop Rank 2 Weapons
			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.Flare.ToString(),
				Value1 = WeaponHash.Flare.ToString()
			});

			if (player.Character.FactionRank < 3) { return menuItemList; }
			// Cop Rank 3 Weapons
			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.PumpShotgun.ToString(),
				Value1 = WeaponHash.PumpShotgun.ToString()
			});

			if (player.Character.FactionRank < 4) { return menuItemList; }
			// Cop Rank 4 Weapons
			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.SMG.ToString(),
				Value1 = WeaponHash.SMG.ToString()
			});

			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.CarbineRifle.ToString(),
				Value1 = WeaponHash.CarbineRifle.ToString()
			});

			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.SmokeGrenade.ToString(),
				Value1 = WeaponHash.SmokeGrenade.ToString()
			});

			menuItemList.Add(new MenuItem
			{
				Title = WeaponHash.BZGas.ToString(),
				Value1 = WeaponHash.BZGas.ToString()
			});

			return menuItemList;
		}


        public static void ProcessWeaponMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.Faction != FactionType.Sheriff || !player.Character.OnDuty) { return; }
            API.shared.givePlayerWeapon(client, API.shared.weaponNameToModel(itemvalue), 120, true);
            CharacterService.CharacterService.UpdatePlayerWeapons(player.Character);
            API.shared.sendNotificationToPlayer(client, "~y~WAFFENKAMMER~w~~n~Du hast folgendes bekommen: ~b~" + itemvalue);
            API.shared.triggerClientEvent(client, "Sheriff_CloseWeaponMenu");
        }

        public static void OpenWeaponMenu(Client client)
		{
			if (!client.hasData("player")) { return; }
			Player player = client.getData("player");
			if (player.Character.IsCuffed) { return; }
			API.shared.triggerClientEvent(client, "Sheriff_OpenWeaponMenu", JsonConvert.SerializeObject(BuildWeaponMenu(player)));
		}
		#endregion Sheriff Weapon Menu

		public static bool TogglePlayerHandsup(Player player)
		{
			player.Character.HasHandsup = !player.Character.HasHandsup;
			API.shared.stopPlayerAnimation(player.Character.Player);
			API.shared.freezePlayer(player.Character.Player, player.Character.HasHandsup);
			if (player.Character.HasHandsup) { API.shared.playPlayerAnimation(player.Character.Player, (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody), "mp_am_hold_up", "handsup_base"); }
			return player.Character.HasHandsup;
		}

		#region Vehicle Impound Timer
		public static void StopImpoundTimer(Client client)
		{
			if (!client.hasData("impoundtimer")) { return; }
			if ((Timer)client.getData("impoundtimer") == null) { return; }
			API.shared.stopTimer((Timer)client.getData("impoundtimer"));
			client.resetData("impoundtimer");
			client.resetData("impoundtimercount");
			client.resetData("impoundvehicle");
			InterfaceService.ProgressBarService.HideBar(client);
		}

		public static void ImpoundVehicle(Client client, OwnedVehicle ownedVehicle)
		{
			StopImpoundTimer(client);
			InterfaceService.ProgressBarService.HideBar(client);
			if (ownedVehicle.ActiveHandle == null) { return; }
			API.shared.sendNotificationToPlayer(client, $"Fahrzeug wurde beschlagnahmt: ({ownedVehicle.ModelName})");
			GarageService.ParkVehicle(ownedVehicle);
		}
		#endregion Vehicle Impound Timer
	}
}
