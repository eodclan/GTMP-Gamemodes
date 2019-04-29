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
	class FIBHandler 
		: Script
	{
        public FIBHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
		}

		private Ped FIBWeaponPed = null;
		private Ped FIBDutyPed = null;

		public void OnResourceStartHandler()
		{
			FIBWeaponPed = API.createPed(PedHash.SiemonYetarian, new Vector3(163.141373, -680.735657, 33.1237144), 160.392181f);
			FIBDutyPed = API.createPed(PedHash.Stbla02AMY, new Vector3(145.302216, -684.457336, 33.1303139), -111.14724f);
        }

		public void OnResourceStopHandler()
		{
			if(FIBWeaponPed != null)
			{
				API.deleteEntity(FIBWeaponPed);
				FIBWeaponPed = null;
			}

			if (FIBDutyPed != null)
			{
				API.deleteEntity(FIBDutyPed);
				FIBDutyPed = null;
			}
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
					if (!client.hasData("player")) { return; }
					Player player = client.getData("player");
					if (player.Character.Faction == FactionType.FIB)
					{
						CheckDoors(client, player); // Check for Doors

						if(client.position.DistanceTo(new Vector3(163.141373, -680.735657, 33.1237144)) <= 1.3f)
						{
							FIBService.OpenWeaponMenu(client);
						}
					}
					break;
				case "FIB_WeaponMenuItemSelected":
					FIBService.ProcessWeaponMenu(client, (string)arguments[0]);
					break;
			}
		}

		#region Check Doors
		private void CheckDoors(Client client, Player player)
		{
			if (client.position.DistanceTo(new Vector3(145.302216, -684.457336, 33.1303139)) <= 2)
			{
				if (client.hasSyncedData("onduty"))
				{
					FIBService.SetOnDuty(client, false);
				}
				else
				{
					FIBService.SetOnDuty(client, true);
				}
				return;
			}
			return;
		}
		#endregion Check Doors
	}
}