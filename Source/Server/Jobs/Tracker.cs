using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.Tracker
{

    public class Trucker : Script
    {
        public Trucker()
    {
        API.onResourceStart += onResourceStart;
        API.onClientEventTrigger += OnClientEvent;
    }
    public ColShape misStartColshape;
    public ColShape misEndColshape;

    public Marker marker;
    public Blip Truckerblip;
    public int taskdone;

    public void onResourceStart()
    {
        //Ped Trucker
        API.createPed(PedHash.Construct02SMY, new Vector3(1015.64282f, -2527.685f, 28.3019848f), 0);
        //Blip Trucker
        Blip Truckblip = API.createBlip(new Vector3(1015.64282f, -2527.685f, 28.3019848f), 0);
        API.setBlipSprite(Truckblip, 67);
        API.setBlipShortRange(Truckblip, true);
        API.setBlipName(Truckblip, "TruckerJob");

        API.createVehicle(VehicleHash.Pounder, new Vector3(1008.97632, -2493.99878, 28.3717),new Vector3(-0.277977735, 0.159938648, 148.550125), 0, 0);
        API.createVehicle(VehicleHash.Pounder, new Vector3(1019.62909,-2495.74634,28.50134), new Vector3(-1.14973176,0.76396846,153.471558f), 0, 0);
        misStartColshape = API.createCylinderColShape(new Vector3(1015.64282, -2527.685, 28.3019848), 5f, 3f); // colshap for mission start point

        misStartColshape.onEntityEnterColShape += (shape, entity) =>
        {
            Client player;

            if ((player = API.getPlayerFromHandle(entity)) != null)
            {
                API.sendChatMessageToPlayer(player, "~b~ Press E to start the job");
            }
        };

    }
    public void OnClientEvent(Client player, string eventName, params object[] arguments) //arguments param can contain multiple params
    {
        switch (eventName)
        {
            case "StartTourLPS":
                {
                    if (misStartColshape.containsEntity(player.handle))
                    {
                        API.setPlayerSkin(player, PedHash.Cletus);
                        API.setEntityData(player, "Trucker", true);
                        API.sendChatMessageToPlayer(player, "~g~Du bist nun als Trucker angestellt");
                        nextDes(player);
                    }
                    break;
                }
            case "objCompleteLPS":
                {
                    if (API.getEntityData(player, "Trucker") != null)
                    {
                        if (misEndColshape.containsEntity(player.handle))
                        {
                            taskdone++;
                            if (taskdone == 5)
                            {
                                API.sendChatMessageToPlayer(player, "~g~ Du hast deine Auslieferung beendet");
                                taskdone = 0;
                                API.setEntityData(player, "Trucker", false);
                                API.deleteColShape(misEndColshape);
                            }
                            else
                            {
                                API.sendChatMessageToPlayer(player, "~g~Fahre zum nächsten Kunden " + taskdone.ToString() + "/5");
                                nextDes(player);
                            }
                        }
                        else API.sendChatMessageToPlayer(player, "~g~Du bist zu weit weg");
                    }
                   // else;
                    break;
                }
        }
    }
    public void nextDes(Client player)
    {
        if (misEndColshape != null)
        {
            API.deleteColShape(misEndColshape);
        }
        //Random random  = new Random();

        List<Vector3> des = new List<Vector3>();

        des.Add(new Vector3(1363.05, -2073.559, 50.99854));
        des.Add(new Vector3(1198.817, -1358.526, 34.2266));
        des.Add(new Vector3(825.7181, 1362.43, 349.3494));
        des.Add(new Vector3(800.0999, 1371.068, 346.5405));
        des.Add(new Vector3(735.2936, 1360.412, 338.0286));
        des.Add(new Vector3(653.0134, 1381.875, 323.9514));

        Vector3 despoint = des;
        misEndColshape = API.createCylinderColShape(despoint, 5F, 3F);
      
       API.triggerClientEvent(player, "nextDes", despoint, despoint.X, despoint.Y, Truckerblip, marker);
    }
    }
}