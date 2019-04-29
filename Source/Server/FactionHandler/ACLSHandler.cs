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
	class ACLSHandler
		: Script
	{
        public ACLSHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
		}

		private Ped ACLSDutyPed = null;

		public void OnResourceStartHandler()
		{
			ACLSDutyPed = API.createPed(PedHash.ChiBoss01GMM, new Vector3(110.056129, 6631.3833, 31.7872372), 171.678848f);
        }

		public void OnResourceStopHandler()
		{
			if (ACLSDutyPed != null)
			{
				API.deleteEntity(ACLSDutyPed);
				ACLSDutyPed = null;
			}
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "KeyboardKey_E_Pressed":
					if (!client.hasData("player")) { return; }
					Player player = client.getData("player");
					if (player.Character.Faction == FactionType.ACLS)
					{
						if (client.position.DistanceTo(new Vector3(110.056129, 6631.3833, 31.7872372)) <= 1.3f)
						{ 						
							ACLSService.SetOnDuty(client, !player.Character.OnDuty);
						}
					}
                    break;
			}
		}
	}
}
