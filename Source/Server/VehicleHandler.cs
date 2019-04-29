using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.VehicleService;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace FactionLife.Server
{
	class VehicleHandler 
		: Script
	{
		public VehicleHandler()
		{
			API.onResourceStart += OnResourceStartHandler;
			API.onResourceStop += OnResourceStopHandler;
			API.onClientEventTrigger += OnClientEvent;
		}
		public Timer Vehicletimer = null;

		public void OnResourceStartHandler()
		{
			Vehicletimer = API.startTimer(60000, false, () => 
			{
				VehicleService.OwnedVehicleList.ForEach(ownedVehicle =>
				{
					if (ownedVehicle.ActiveHandle.engineStatus)
					{
						ownedVehicle.Fuel--;
						if (ownedVehicle.Fuel < 0)
							ownedVehicle.Fuel = 0;
						if (ownedVehicle.Fuel > 100)
							ownedVehicle.Fuel = 100; // Max Fuel 100
						ownedVehicle.ActiveHandle.setSyncedData("fuel", ownedVehicle.Fuel);
                        if (ownedVehicle.Fuel <= 0)
                            ownedVehicle.ActiveHandle.engineStatus = false;
						VehicleService.UpdateVehicle(ownedVehicle);
					}
				});
			});
		}

		public void OnResourceStopHandler()
		{
			API.stopTimer(Vehicletimer);
			Vehicletimer = null;
			VehicleService.OwnedVehicleList.ForEach(ownedVehicle =>
			{
				API.deleteEntity(ownedVehicle.ActiveHandle);
				VehicleService.OwnedVehicleList.Remove(ownedVehicle);
			});
			VehicleService.OwnedVehicleList.Clear();
		}
        public static void PlayUnlockSound(Client client, OwnedVehicle ownedVehicle, bool locked) {
            Vector3 position = ownedVehicle.ActiveHandle.position;
            List<Client> clientsInArea = API.shared.getPlayersInRadiusOfPosition(3.2f, position);
            if (locked)
            {
                foreach (Client clientInArea in clientsInArea)
                {
                    API.shared.triggerClientEvent(clientInArea, "PlayLockVehicleSound", position);
                }
            }
            else {
                foreach (Client clientInArea in clientsInArea)
                {
                    API.shared.triggerClientEvent(clientInArea, "PlayUnlockVehicleSound", position);
                }
            }

        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments) //arguments param can contain multiple params
		{
			Player player = null;
			OwnedVehicle ownedVehicle = null;

			switch (eventName)
			{
				case "KeyboardKey_U_Pressed":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
                    ownedVehicle = VehicleService.OwnedVehicleList.FirstOrDefault(x => (x.Owner == client.socialClubName && x.OwnerCharId == player.Character.Id && x.ActiveHandle.position.DistanceTo(client.position) <= 4f) || (x.Id == player.Character.Key1 || x.Id == player.Character.Key2 || x.Id == player.Character.Key3 || x.Id == player.Character.Key4 || x.Id == player.Character.Key5) && x.ActiveHandle.position.DistanceTo(client.position) <= 4f);
                    if (ownedVehicle == null)
						return;
					if (ownedVehicle.ActiveHandle.locked)
					{
						ownedVehicle.ActiveHandle.locked = false;
						API.sendNotificationToPlayer(client, "Die Fahrzeugtüren Ihres ~b~" + ownedVehicle.ModelName + " ~w~sind jetzt ~g~entsperrt");
                        PlayUnlockSound(client, ownedVehicle, false);
					}
					else
					{
						ownedVehicle.ActiveHandle.locked = true;
						API.sendNotificationToPlayer(client, "Die Fahrzeugtüren Ihres ~b~" + ownedVehicle.ModelName + "~w~ sind jetzt ~r~verschlossen");
                        PlayUnlockSound(client, ownedVehicle, true);

                    }

                    break;
				case "KeyboardKey_Z_Pressed":
					if (!client.isInVehicle)
						return;
					if (API.shared.getPlayerVehicleSeat(client) != -1)
						return;
					if (!client.hasData("player"))
						return;
					if (!client.vehicle.hasData("vehicle"))
						return;
					player = client.getData("player");
					ownedVehicle = client.vehicle.getData("vehicle");
					if((client.socialClubName.ToLower() == ownedVehicle.Owner.ToLower() && player.Character.Id == ownedVehicle.OwnerCharId) || (ownedVehicle.Id == player.Character.Key1 || ownedVehicle.Id == player.Character.Key2 || ownedVehicle.Id == player.Character.Key3 || ownedVehicle.Id == player.Character.Key4 || ownedVehicle.Id == player.Character.Key5))

                    {
						if (client.vehicle.engineStatus)
						{
							client.vehicle.engineStatus = false;
							API.shared.sendNotificationToPlayer(client, "Fahrzeugmotor ist ~r~aus");
						}
						else
						{
							if(ownedVehicle.Fuel <= 0 || ownedVehicle.Fuel <= 0 && ownedVehicle.ActiveHandle.engineStatus) {
                                ownedVehicle.ActiveHandle.engineStatus = false;
                                return;
                            }
							if(ownedVehicle.ActiveHandle.engineHealth <= 0) { return; }
							client.vehicle.engineStatus = true;
							API.shared.sendNotificationToPlayer(client, "Fahrzeugmotor ist ~g~an");

                            API.setEntitySyncedData(ownedVehicle.ActiveHandle, "currentfuel", ownedVehicle.Fuel);
                            API.setEntitySyncedData(ownedVehicle.ActiveHandle, "maxfuel", VehicleService.GetVehicleInfo(ownedVehicle.ModelName).MaxFuel);

                        }
					}
					else
					{
						API.shared.sendNotificationToPlayer(client, "~r~Sie sind nicht der Besitzer dieses Fahrzeugs!");
					}
					break;
				case "KeyboardKey_K_Pressed":
					if (!client.hasData("player"))
						return;
					player = client.getData("player");
					if (client.isInVehicle)
					{
						if (!client.vehicle.hasData("vehicle")) { return; }
						ownedVehicle = client.vehicle.getData("vehicle");
					}
					else
					{
						ownedVehicle = VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 4f);
					}
					if (ownedVehicle == null)
						return;
					List<MenuItem> vehMenu = VehicleService.BuildVehicleMenu(player, ownedVehicle);
					if(vehMenu == null) { return; }
					API.triggerClientEvent(client, "Vehicle_OpenMenu", JsonConvert.SerializeObject(vehMenu));
					break;
				case "Vehicle_MainMenuItemSelected":
					VehicleService.ProcessVehicleMenuReturn(client, (string)arguments[0]);
					break;
				case "Vehicle_InventoryMenuItemSelected":
					if((int)arguments[1] <= 0) { return; }
					VehicleService.PutOutOfVehicleInventory(client, (int)arguments[0], (int)arguments[1]);
					break;
				case "Vehicle_PlayerInventoryMenuItemSelected":
					if ((int)arguments[1] <= 0) { return; }
					VehicleService.PutIntoVehicleInventory(client, (int)arguments[0], (int)arguments[1]);
					break;
				case "Vehicle_CloseTrunk":
					VehicleService.ChangeDoorState(client, 5, false);
					break;
			}
		}
	}
}
