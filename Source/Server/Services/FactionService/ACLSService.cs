using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FactionLife.Server.Services.FactionService
{
	class ACLSService
		: Script
	{
        public static Client client { get; private set; }

        public ACLSService()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
            // Kommt noch was rein später
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
					ClothingService.ClothingService.ApplyOutfit(client, 11); // Male Cop Outfit
				}
				else
				{
					ClothingService.ClothingService.ApplyOutfit(client, 12); // Female Cop Outfit
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
					if(otherplayer.Character.Faction == FactionType.ACLS && otherplayer.Character.OnDuty)
					{
                        if (onDuty)
                        {
                            API.shared.sendPictureNotificationToPlayer(otherclient, "Herr " + player.Character.FirstName + " " + player.Character.LastName + " ~w~ist jetzt ~g~im Dienst ~w~gegangen", "CHAR_ASHLEY", 0, 2, "Dienststelle", "ACLS Information");
                        }
                        else
                        {
                            API.shared.sendPictureNotificationToPlayer(otherclient, "Herr " + player.Character.FirstName + " " + player.Character.LastName + " ~w~ist jetzt ~g~ausser Dienst ~w~gegangen", "CHAR_ASHLEY", 0, 2, "Dienststelle", "ACLS Information");
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
			if (targetclient == null) { return; }
			if (!client.hasData("player") || !targetclient.hasData("player")) { return; }
			Player player = client.getData("player");
			Player target = targetclient.getData("player");
			if(!MoneyService.MoneyService.HasPlayerEnoughBank(targetclient, amount))
			{
				API.shared.sendNotificationToPlayer(client, "~r~Ziel hat nicht genug Geld..");
			}

			// Todo
		}

		public static void CloseInteractionMenu(Client client)
		{
			API.shared.triggerClientEvent(client, "ACLS_CloseInteractionMenu");
		}

		#region ACLS Interaction Menu
		public static List<MenuItem> BuildInteractionMenu(Player player)
		{
			List<MenuItem> menuItemList = new List<MenuItem>();

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
			//Player otherPlayer = null;

			switch (itemvalue)
			{
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
		#endregion ACLS Interaction Menu

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
