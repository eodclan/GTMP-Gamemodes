using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Services.VehicleService;
using GrandTheftMultiplayer.Server.Constant;
using System.Collections.Generic;
using System.Linq;
using System;
using GrandTheftMultiplayer.Shared;

namespace FactionLife.Server
{
    class GarageHandler
        : Script
    {
        public GarageHandler()
        {
            API.onClientEventTrigger += OnClientEvent;
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments) //arguments param can contain multiple params
        {
            string description1;
            string description2;
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (eventName == "KeyboardKey_E_Pressed")
            {
                if (!client.isInVehicle)
                {
                    if (client.hasData("player"))
                    {
                        if ((client.getSyncedData("Opened_Garage") == true)) { return; }

                        Garage garage = GarageService.GarageList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2f);
                        if (garage == null)
                        {
                            //API.sendNotificationToPlayer(client, "~r~No garage nearby!");
                            return;
                        }

                        if (garage.FactionType != FactionType.Citizen)
                        {
                            if (garage.FactionType != player.Character.Faction) { return; }
                        }

                        List<GarageMenuItem> menuitems = new List<GarageMenuItem>();
                        VehicleService.GetUserVehicles(client).ForEach(ownedVehicle =>
                        {
                            VehicleInfo vehicleInfo = VehicleService.GetVehicleInfo(ownedVehicle.ModelName);
                            if (vehicleInfo != null && GarageService.IsVehicleTypeAllowed(vehicleInfo.Type, garage.Type))
                            {
                                if (player.Character.OnDuty)
                                {
                                    if (ownedVehicle.OwnerCharId != player.Character.Id) { description1 = "~b~Treibstoffart~w~: " + VehicleService.GetVehicleInfo(ownedVehicle.ModelName).Fuel.ToString() + "   ~b~Fahrzeug~w~: Fremdwagen" + "   ~b~ID~w~: " + ownedVehicle.Id + ""; }
                                    else { description1 = "~b~Treibstoffart~w~: " + VehicleService.GetVehicleInfo(ownedVehicle.ModelName).Fuel.ToString() + "   ~b~Fahrzeug~w~: Eigenwagen" + "   ~b~ID~w~: " + ownedVehicle.Id + ""; }
                                    if (ownedVehicle.Faction == FactionType.Citizen || ownedVehicle.Faction == player.Character.Faction)

                                        menuitems.Add(new GarageMenuItem
                                        {
                                            Id = ownedVehicle.Id,
                                            ModelName = ownedVehicle.ModelName,
                                            VehHash = ownedVehicle.Model,
                                            NumberPlate = ownedVehicle.NumberPlate,
                                            Description = description1
                                        });
                                }
                                else
                                {
                                    if (ownedVehicle.Faction == FactionType.Citizen)
                                    {
                                        if (ownedVehicle.OwnerCharId != player.Character.Id) { description2 = "~b~Treibstoffart~w~: " + VehicleService.GetVehicleInfo(ownedVehicle.ModelName).Fuel.ToString() + "   ~b~Fahrzeug~w~: Fremdwagen" + "   ~b~ID~w~: " + ownedVehicle.Id + ""; }
                                        else { description2 = "~b~Treibstoffart~w~: " + VehicleService.GetVehicleInfo(ownedVehicle.ModelName).Fuel.ToString() + "   ~b~Fahrzeug~w~: Privat" + "   ~b~ID~w~: " + ownedVehicle.Id + ""; }

                                        menuitems.Add(new GarageMenuItem
                                        {
                                            Id = ownedVehicle.Id,
                                            ModelName = ownedVehicle.ModelName,
                                            VehHash = ownedVehicle.Model,
                                            NumberPlate = ownedVehicle.NumberPlate,
                                            Description = description2
                                        });
                                    }

                                }
                            }
                        });
                        API.triggerClientEvent(client, "Garage_OpenMenu", JsonConvert.SerializeObject(menuitems));
                        API.setEntitySyncedData(client, "Opened_Garage", true);
                        API.delay(2000, true, () =>
                        {
                            API.setEntitySyncedData(client, "Opened_Garage", false);

                        });
                        return;
                    }
                }
            }

            if (eventName == "Garage_UseVehicle")
            {
                if ((client.getSyncedData("Used_Vehicle") == true)) { return; }

                Garage garage = GarageService.GarageList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2f);
                if (garage == null)
                {
                    API.sendNotificationToPlayer(client, "~r~Keine Garage in der Nähe!");
                    return;
                }

                if (garage.FactionType != FactionType.Citizen)
                {
                    if (garage.FactionType != player.Character.Faction) { return; }
                }

                OwnedVehicle ownedVehicle = VehicleService.LoadFromDatabase((int)arguments[0]);
                if (ownedVehicle == null)
                {
                    API.sendNotificationToPlayer(client, "~r~Das angeforderte Fahrzeug konnte nicht gefunden werden!");
                    return;
                }

                bool freeSpawnAvailable = false;
                GarageSpawn freespawn = null;
                garage.Spawnpoints.ForEach(spawn =>
                {
                    if (!freeSpawnAvailable)
                    {
                        OwnedVehicle foundblockingveh = VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(spawn.Position) <= 6f);
                        if (foundblockingveh == null)
                        {
                            freeSpawnAvailable = true;
                            freespawn = spawn;
                        }
                    }
                });

                API.setEntitySyncedData(client, "Used_Vehicle", true);

                if (freespawn == null)
                {
                    API.sendNotificationToPlayer(client, "~r~Kein freier Spawnpunkt gefunden!");
                    return;
                }

                Vehicle newVehicle = API.createVehicle(ownedVehicle.Model, freespawn.Position, freespawn.Rotation, ownedVehicle.PrimaryColor, ownedVehicle.SecondaryColor);
                NetHandle newVehicleC = newVehicle;
                

                switch (ownedVehicle.Faction)
                {
                    case FactionType.Citizen:
                        newVehicle.numberPlate = ownedVehicle.NumberPlate;
                        break;
                    case FactionType.Bloods:
                        newVehicle.numberPlate = "Bloods";
                        break;
                    case FactionType.Vagos:
                        newVehicle.numberPlate = "VAGOS";
                        break;
                    case FactionType.LosSantosLocos:
                        newVehicle.numberPlate = "LSL";
                        break;
                    case FactionType.LostMC:
                        newVehicle.numberPlate = "LOST MC";
                        break;
                    case FactionType.Yakuza:
                        newVehicle.numberPlate = "Yakuza";
                        break;
                    case FactionType.Grove:
                        newVehicle.numberPlate = "Grove";
                        break;
                    case FactionType.Aztecas:
                        newVehicle.numberPlate = "Aztecas";
                        break;
                    case FactionType.Camorra:
                        newVehicle.numberPlate = "Camorra";
                        break;
                    case FactionType.Fahrschule:
                        newVehicle.numberPlate = "Fahrschule";
                        break;
                    case FactionType.SWAT:
                        newVehicle.numberPlate = "SWAT";
                        API.setVehicleEnginePowerMultiplier(newVehicleC, 30);
                        break;
                    case FactionType.EMS:
                        newVehicle.numberPlate = "MEDIC";
                        API.setVehicleEnginePowerMultiplier(newVehicleC, 30);
                        break;
                    case FactionType.Police:
                        newVehicle.numberPlate = "LSPD";
                        API.setVehicleEnginePowerMultiplier(newVehicleC, 30);
                        break;
                    case FactionType.FIB:
                        newVehicle.numberPlate = "FIB";
                        API.setVehicleEnginePowerMultiplier(newVehicleC, 30);
                        break;
                }
                ownedVehicle.ActiveHandle = newVehicle;
                newVehicle.setData("vehicle", ownedVehicle);
                newVehicle.setSyncedData("fuel", ownedVehicle.Fuel);
                newVehicle.setSyncedData("maxfuel", VehicleService.GetVehicleInfo(ownedVehicle.ModelName).MaxFuel); // Read From DB Later
                newVehicle.engineHealth = ownedVehicle.EngineHealth;
                newVehicle.locked = true;
                newVehicle.engineStatus = false;
                newVehicle.livery = ownedVehicle.Livery;
                VehicleService.OwnedVehicleList.Add(ownedVehicle);
                VehicleService.SetInUse(ownedVehicle, true);
                API.sendNotificationToPlayer(client, "~g~Fahrzeug erfolgreich ausgeparkt!");
                GarageService.CloseMenu(client);

                if (ownedVehicle.TyreSmokeState == 1) { newVehicle.setMod(20, 1); }
                newVehicle.tyreSmokeColor = new Color(ownedVehicle.tyreSmokeColorR, ownedVehicle.tyreSmokeColorG, ownedVehicle.tyreSmokeColorB);

                if (ownedVehicle.wheelColor == 1) {

                    for (int slot = 0; slot <= ownedVehicle.wheelColor; slot++)
                    {
                        newVehicle.setMod(16, 1);
                    }
                    //newVehicle.wheelColor = new Color(ownedVehicle.wheelColor, ownedVehicle.wheelColor, ownedVehicle.wheelColor);
                    API.setVehicleWheelColor(newVehicle, ownedVehicle.wheelColor);
                    client.vehicle.setMod(16, 1);
                    client.vehicle.wheelColor = ownedVehicle.wheelColor;

                }

                if (ownedVehicle.NeonCarState == 1)
                {
                    for (int slot = 0; slot <= 3; slot++)
                    {
                        API.setVehicleNeonState(newVehicle, slot, true);
                    }

                    API.setVehicleNeonColor(newVehicle, ownedVehicle.neonCarColorR, ownedVehicle.neonCarColorG, ownedVehicle.neonCarColorB);
                }

                API.delay(2000, true, () =>
                {
                    API.setEntitySyncedData(client, "Used_Vehicle", false);

                });

                return;
            }

            if (eventName == "Garage_ParkVehicle")
            {
                Garage garage = GarageService.GarageList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2f);
                if (garage == null)
                {
                    API.sendNotificationToPlayer(client, "~r~Keine Garage in der Nähe!");
                    return;
                }
                if (garage.FactionType != FactionType.Citizen)
                {
                    if (garage.FactionType != player.Character.Faction) { return; }
                }

                float vehicleDistance = 10f; // Default Distance Vehicle <=> Garage
                switch (garage.Type)
                {
                    case GarageType.GroundVehicle:
                        vehicleDistance = 10f;
                        break;
                    case GarageType.Helicopter:
                    case GarageType.Plane:
                        vehicleDistance = 20f;
                        break;
                    case GarageType.Boat:
                        vehicleDistance = 30f;
                        break;
                }

                OwnedVehicle ownedVehicle = VehicleService.OwnedVehicleList.FirstOrDefault(x => x.Owner == client.socialClubName && x.OwnerCharId == player.Character.Id && x.ActiveHandle.position.DistanceTo(garage.Position) <= vehicleDistance);
                OwnedVehicle strangeOwnedVehicle = VehicleService.OwnedVehicleList.FirstOrDefault(x => (x.Id == player.Character.Key1 || x.Id == player.Character.Key2 || x.Id == player.Character.Key3 || x.Id == player.Character.Key4 || x.Id == player.Character.Key5) && x.ActiveHandle.position.DistanceTo(garage.Position) <= vehicleDistance);

                if ((ownedVehicle == null) && (strangeOwnedVehicle == null))
                {
                    API.sendNotificationToPlayer(client, "~r~Konnte kein Fahrzeug zum Parken finden!");
                    return;
                }

                if ((ownedVehicle == null) && (strangeOwnedVehicle != null))
                {
                    GarageService.ParkVehicle(strangeOwnedVehicle);
                    API.sendNotificationToPlayer(client, "~g~Fahrzeug erfolgreich geparkt!");
                    GarageService.CloseMenu(client);
                    return;
                }

                GarageService.ParkVehicle(ownedVehicle);
                API.sendNotificationToPlayer(client, "~g~Fahrzeug erfolgreich geparkt!");
                GarageService.CloseMenu(client);
                return;
            }
        }
    }
}
