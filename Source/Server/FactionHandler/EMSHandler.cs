﻿using GrandTheftMultiplayer.Server.API;
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
	class EMSHandler
		: Script
	{
        public EMSHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
		}

		private Ped EMSDutyPed = null;

		public void OnResourceStartHandler()
		{
			EMSDutyPed = API.createPed(PedHash.Doctor01SMM, new Vector3(-446.2191, -328.6706, 34.50191), 126.6656f);

            Blip EMS = API.createBlip(new Vector3(-446.2191, -328.6706, 34.50191));
            API.setBlipSprite(EMS, 526);
            API.setBlipColor(EMS, 1);
            API.setBlipName(EMS, "Medical Hospital");
        }

		public void OnResourceStopHandler()
		{
			if (EMSDutyPed != null)
			{
				API.deleteEntity(EMSDutyPed);
				EMSDutyPed = null;
			}
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
					if (!client.hasData("player")) { return; }
					Player player = client.getData("player");
					if (player.Character.Faction == FactionType.EMS)
					{
						if (client.position.DistanceTo(new Vector3(-446.2191, -328.6706, 34.50191)) <= 1.3f)
						{ 						
							EMSService.SetOnDuty(client, !player.Character.OnDuty);
						}
					}
                    break;
			}
		}
	}
}
