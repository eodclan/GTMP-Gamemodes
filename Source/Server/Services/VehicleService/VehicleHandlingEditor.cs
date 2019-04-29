using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace HandlingEditor
{
	public class Main
		: Script
	{
		public Main()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client player, string eventName, params object[] arguments)
		{
			if (eventName == "handling")
			{
				if (player.isInVehicle)
				{
					API.triggerClientEvent(player, "openHandling", player.vehicle.model);
				}
			}
		}
	}
}