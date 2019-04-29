using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.FactionHandler
{
	class FireDepartmentHandler
		: Script
	{
		public FireDepartmentHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
		}

		private Ped FireDepartmentDutyPed = null;

		public void OnResourceStartHandler()
		{
			FireDepartmentDutyPed = API.createPed(PedHash.Doctor01SMM, new Vector3(-446.2191, -328.6706, 34.50191), 126.6656f);

            Blip FireDepartment = API.createBlip(new Vector3(-1034.7, -2384.323, 14.09234));
            API.setBlipSprite(FireDepartment, 72);
            API.setBlipColor(FireDepartment, 1);
            API.setBlipName(FireDepartment, "Fire Department");
        }

		public void OnResourceStopHandler()
		{
			if (FireDepartmentDutyPed != null)
			{
				API.deleteEntity(FireDepartmentDutyPed);
				FireDepartmentDutyPed = null;
			}
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
					if (!client.hasData("player")) { return; }
					Player player = client.getData("player");
					if (player.Character.Faction == FactionType.FireDepartment)
					{
						if (client.position.DistanceTo(new Vector3(-1068.18, -2379.527, 14.05764)) <= 1.3f)
						{ 						
							FireDepartmentService.SetOnDuty(client, !player.Character.OnDuty);
						}
					}
					break;
			}
		}
	}
}
