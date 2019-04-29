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
	class SheriffHandler 
		: Script
	{
        public SheriffHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
		}

		private Ped SheriffWeaponPed = null;
		private Ped SheriffDutyPed = null;

		public void OnResourceStartHandler()
		{
			SheriffWeaponPed = API.createPed(PedHash.Cop01SFY, new Vector3(-450.1073, 6016.25439, 31.7163754), -47.35955f);
			SheriffDutyPed = API.createPed(PedHash.Cop01SMY, new Vector3(-448.4119, 6007.849, 31.7163811), -46.831955f);

            Blip Sheriff = API.createBlip(new Vector3(-449.5452f, 6011.441f, 31.71639f));
            API.setBlipSprite(Sheriff, 60);
            API.setBlipColor(Sheriff, 4);
            API.setBlipName(Sheriff, "Sheriff Department");

        }

		public void OnResourceStopHandler()
		{
			if(SheriffWeaponPed != null)
			{
				API.deleteEntity(SheriffWeaponPed);
				SheriffWeaponPed = null;
			}

			if (SheriffDutyPed != null)
			{
				API.deleteEntity(SheriffDutyPed);
				SheriffDutyPed = null;
			}
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
					if (!client.hasData("player")) { return; }
					Player player = client.getData("player");
					if (player.Character.Faction == FactionType.Sheriff)
					{
						CheckDoors(client, player); // Check for Doors

						if(client.position.DistanceTo(new Vector3(-450.1073, 6016.25439, 31.7163754)) <= 1.3f)
						{
							SheriffService.OpenWeaponMenu(client);
						}
					}
					break;
				case "Sheriff_WeaponMenuItemSelected":
					SheriffService.ProcessWeaponMenu(client, (string)arguments[0]);
					break;
			}
		}

		#region Check Doors
		private void CheckDoors(Client client, Player player)
		{
			if (client.position.DistanceTo(new Vector3(-448.4119, 6007.849, 31.7163811)) <= 2)
			{
				if (client.hasSyncedData("onduty"))
				{
					SheriffService.SetOnDuty(client, false);
				}
				else
				{
					SheriffService.SetOnDuty(client, true);
				}
				return;
			}

			if (client.position.DistanceTo(new Vector3(434.6926, -981.8649, 31.71638)) <= 1) // Mission Row Main Doors
			{
				DoorService.ToggleDoorState(19);
				if (DoorService.ToggleDoorState(18))
				{
					API.sendNotificationToPlayer(client, "~r~Verschlossen ~w~Tür");
				}
				else
				{
					API.sendNotificationToPlayer(client, "~g~Entsperrt ~w~Tür");
				}
				return;
			}
			return;
		}
		#endregion Check Doors
	}
}