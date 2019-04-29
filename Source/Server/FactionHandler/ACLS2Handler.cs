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
	class ACLS2Handler
		: Script
	{
        public ACLS2Handler()
		{
			API.onClientEventTrigger += OnClientEvent;
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
		}

		private Ped ACLSDutyPed = null;

		public void OnResourceStartHandler()
		{
			ACLSDutyPed = API.createPed(PedHash.ChiBoss01GMM, new Vector3(-1150.55859, -2034.8446, 13.1607018), 126.297264f);
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
						if (client.position.DistanceTo(new Vector3(-1150.55859, -2034.8446, 13.1607018)) <= 1.3f)
						{ 						
							ACLSService.SetOnDuty(client, !player.Character.OnDuty);
						}
					}
                    break;
			}
		}
	}
}
