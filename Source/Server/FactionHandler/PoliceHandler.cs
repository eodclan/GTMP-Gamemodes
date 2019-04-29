using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.ClothingService;
using FactionLife.Server.Services.DoorService;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.FactionHandler
{
	class PoliceHandler 
		: Script
	{
        public PoliceHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
		}

		private Ped PoliceWeaponPed = null;
		private Ped PoliceDutyPed = null;

		public void OnResourceStartHandler()
		{
			PoliceWeaponPed = API.createPed(PedHash.Cop01SMY, new Vector3(454.146, -980.071, 30.68959), 84.72414f);
			PoliceDutyPed = API.createPed(PedHash.Cop01SMY, new Vector3(460.1322, -990.9448, 30.6896), 80.99126f);

            Blip Police = API.createBlip(new Vector3(441.4052f, -981.1326f, 30.6896f));
            API.setBlipSprite(Police, 60);
            API.setBlipColor(Police, 4);
            API.setBlipName(Police, "LSPD");

        }

		public void OnResourceStopHandler()
		{
			if(PoliceWeaponPed != null)
			{
				API.deleteEntity(PoliceWeaponPed);
				PoliceWeaponPed = null;
			}

			if (PoliceDutyPed != null)
			{
				API.deleteEntity(PoliceDutyPed);
				PoliceDutyPed = null;
			}
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
					if (!client.hasData("player")) { return; }
					Player player = client.getData("player");
					if (player.Character.Faction == FactionType.Police)
					{
						CheckDoors(client, player); // Check for Doors

						if(client.position.DistanceTo(new Vector3(452.3879, -980.1114, 30.6896)) <= 1.3f)
						{
							PoliceService.OpenWeaponMenu(client);
						}
					}
					break;
				case "Police_WeaponMenuItemSelected":
					PoliceService.ProcessWeaponMenu(client, (string)arguments[0]);
					break;
			}
		}

		#region Check Doors
		private void CheckDoors(Client client, Player player)
		{
			if (client.position.DistanceTo(new Vector3(460.1322, -990.9448, 30.6896)) <= 2)
			{
				if (client.hasSyncedData("onduty"))
				{
					PoliceService.SetOnDuty(client, false);
				}
				else
				{
					PoliceService.SetOnDuty(client, true);
				}
				return;
			}

            if (client.position.DistanceTo(new Vector3(434.6926, -981.8649, 30.71322)) <= 1) // Mission Row Main Doors
            {
                DoorService.ToggleDoorState(19);
                if (DoorService.ToggleDoorState(18))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(464.3711, -983.8344, 43.69287)) <= 1) // Mission Row Top Door
            {
                if (DoorService.ToggleDoorState(27))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(463.7195, -992.5253, 24.91487)) <= 1) // Mission Row Main Cell Door
            {
                if (DoorService.ToggleDoorState(26))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }
            //NEW
            if (client.position.DistanceTo(new Vector3(463.4782, -1003.538, 25.00599)) <= 1) // Mission Row Back To Cells and Parking Door
            {
                if (DoorService.ToggleDoorState(22))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }
            //NEW
            if (client.position.DistanceTo(new Vector3(444.7116, -989.2593, 30.68959)) <= 1) // Mission Row Police Station Cell And Briefing Doors
            {
                DoorService.ToggleDoorState(30);
                if (DoorService.ToggleDoorState(31))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }
            //NEW
            if (client.position.DistanceTo(new Vector3(446.5728, -980.0106, 30.8393)) <= 1) // Mission Row Police Station Captan's Office Door
            {
                if (DoorService.ToggleDoorState(32))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }
            //NEW
            if (client.position.DistanceTo(new Vector3(450.2558, -982.8545, 30.68959)) <= 1) // Mission Row Police Station Armory Double Door
            {
                DoorService.ToggleDoorState(33);
                if (DoorService.ToggleDoorState(34))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            //NEW
            if (client.position.DistanceTo(new Vector3(450.1041, -985.7384, 30.8393)) <= 1) // Mission Row Police Station Locker Rooms Door
            {
                if (DoorService.ToggleDoorState(35))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(464.1983, -1003.415, 24.91487)) <= 1) // Mission Row Back Cell Door (Not Working)
            {
                if (DoorService.ToggleDoorState(20))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(462.0896, -993.7011, 24.91486)) <= 1) // Mission Row Cell Door Right
            {
                if (!player.Character.OnDuty) { return; }
                if (DoorService.ToggleDoorState(23))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(462.0993, -998.6472, 24.91486)) <= 1) // Mission Row Cell Door Middle
            {
                if (!player.Character.OnDuty) { return; }
                if (DoorService.ToggleDoorState(24))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(462.2214, -1002.128, 24.91486)) <= 1) // Mission Row Cell Door Left
            {
                if (!player.Character.OnDuty) { return; }
                if (DoorService.ToggleDoorState(25))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(1844.998, 2597.482, 44.63626)) <= 5) // JVA Main Door 1
            {
                if (DoorService.ToggleDoorState(39))
                {
                    API.sendNotificationToPlayer(client, "Tür ~r~abgeschlossen.");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "Tür ~g~aufgeschlossen.");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(1818.543, 2597.482, 44.60749)) <= 5) // JVA Main Door 2
            {
                if (DoorService.ToggleDoorState(40))
                {
                    API.sendNotificationToPlayer(client, "Tür ~r~abgeschlossen.");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "Tür ~g~aufgeschlossen.");
                }
                return;
            }


            if (client.position.DistanceTo(new Vector3(1806.939, 2616.975, 44.60093)) <= 5) // JVA Main Door 3
            {
                if (DoorService.ToggleDoorState(41))
                {
                    API.sendNotificationToPlayer(client, "Tür ~r~abgeschlossen.");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "Tür ~g~aufgeschlossen.");
                }
                return;
            }

            if (client.position.DistanceTo(new Vector3(468.6248, -1014.067, 26.38638)) <= 1) // Mission Row Back Doors
            {
                if (DoorService.ToggleDoorState(20))
                {
                    API.sendNotificationToPlayer(client, "~r~Tür ~w~abgeschlossen");
                }
                else
                {
                    API.sendNotificationToPlayer(client, "~g~Tür ~w~aufgeschlossen");
                }
                DoorService.ToggleDoorState(21);
                return;
            }
            return;
        }
        #endregion Check Doors
    }
}