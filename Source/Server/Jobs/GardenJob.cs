using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.IO;
using FactionLife.Server.Services.MoneyService;
using System.Xml.Linq;


namespace FactionLife.Server.JobService
{
    public class Gardening : Script
    {
        public Gardening()
        {
            API.onResourceStart += onResourceStart;
            API.onClientEventTrigger += OnClientEvent;
            API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;

        }

        public ColShape misStartColshape; // start ColShape
        public ColShape misEndColshape;

        public ColShape point1 = API.shared.createCylinderColShape(new Vector3(-1335.492, 119.3447, 55.38317), 1f, 1f);
        public ColShape point2 = API.shared.createCylinderColShape(new Vector3(-1295.532, 118.3462, 55.67009), 1f, 1f);
        public ColShape point3 = API.shared.createCylinderColShape(new Vector3(-1264.291, 107.0072, 55.35939), 1f, 1f);
        public ColShape point4 = API.shared.createCylinderColShape(new Vector3(-1231.783, 92.1292, 55.27038), 1f, 1f);
        public ColShape point5 = API.shared.createCylinderColShape(new Vector3(-1185.744, 69.16298, 54.5), 1f, 1f);
        public ColShape point6 = API.shared.createCylinderColShape(new Vector3(-1141.321, 16.39791, 48.7), 1f, 1f);
        public ColShape point7 = API.shared.createCylinderColShape(new Vector3(-1168.051, -29.97899, 44.5), 1f, 1f);
        public ColShape point8 = API.shared.createCylinderColShape(new Vector3(-1239.033, 6.37442, 47), 1f, 1f);
        public ColShape point9 = API.shared.createCylinderColShape(new Vector3(-1312.314, 63.75764, 52.5), 1f, 1f);


        Vector3 vpoint1;
        Vector3 vpoint2;
        Vector3 vpoint3;
        Vector3 vpoint4;
        Vector3 vpoint5;
        Vector3 vpoint6;
        Vector3 vpoint7;
        Vector3 vpoint8;
        Vector3 vpoint9;

        public int taskgarden;
        public int lastpoint;


        public void onResourceStart()
        {
			Blip misBlip = API.createBlip(new Vector3(-1348.501f, 142.6998f, 56.43797f)); // blip to start job
			API.setBlipSprite(misBlip, 566);
			API.setBlipName(misBlip, "Garden Job");
			API.setBlipColor(misBlip, 2);
			API.setBlipScale(misBlip, 0.8f);
            API.createPed((PedHash)(1498487404), new Vector3(-1348.501, 142.6998, 56.43797), 126);
            misStartColshape = API.createCylinderColShape(new Vector3(-1348.501, 142.6998, 56.43797), 1f, 1f); // colshap for mission start point

            misStartColshape.onEntityEnterColShape += (shape, entity) =>
            {
                Client player;

                if (((player = API.getPlayerFromHandle(entity)) == null) || ((player.isInVehicle) == true)) { return; }

                if (API.getEntitySyncedData(player, "Just_Send") == true) { return; }


                if (API.getEntitySyncedData(player, "IS_job") == true)
                {
                    API.sendNotificationToPlayer(player, "~w~ Möchtest du Feierabend machen? Benutze ~g~ Y~w~.");
                    API.setEntitySyncedData(player, "Just_Send", true);

                    API.delay(2000, true, () =>
                    {
                        API.setEntitySyncedData(player, "Just_Send", false);
                    });

                }

                else
                {
					API.shared.sendPictureNotificationToPlayer(player, "~g~Drücken Sie Y, um den Job zu starten.", "CHAR_CHEF", 0, 2, "Gärtner Job", "Job annehmen");
                    //API.sendChatMessageToPlayer(player, "~y~[SERVER]~w~ Du möchtest als Gärtner Geld verdienen? Benutze ~g~E~w~ um direkt anzufangen.");
                    API.setEntitySyncedData(player, "Just_Send", true);

                    API.delay(2000, true, () =>
                    {
                        API.setEntitySyncedData(player, "Just_Send", false);
                    });
                }



            };

        }

        public void OnClientEvent(Client player, string eventName, params object[] arguments) //arguments param can contain multiple params
        {
            if (eventName == "MissionGardenTrigger") //an eventname with no params that was triggered from the Client-side script
            {

                if (misStartColshape.containsEntity(player.handle))
                {

                    if (API.getEntitySyncedData(player, "IS_job") == true)
                    {
                        API.sendNotificationToPlayer(player, "~w~ Schönen Feierabend!");
                        API.setEntitySyncedData(player, "IS_job", false);
                        return;
                    }


                    else
                    {
                        //API.setPlayerSkin(player, PedHash.Lathandy01SMM);
                        VehicleHash taxi = API.vehicleNameToModel("Mower");
                        Vehicle myvehicle = API.createVehicle(taxi, new Vector3(-1357.037, 133.4326, 55.68765), new Vector3(0, 0, -84.32785), 0, 0, 0);
                        API.setPlayerIntoVehicle(player, myvehicle, -1);

                        API.delay(1000, true, () =>
                        {
                            API.sendNotificationToPlayer(player, " ~w~Fahre die markierten Punkte auf der Karte ab und tippe ~g~E ~w~ bei Ihnen an.");
                            API.sendNotificationToPlayer(player, " ~w~Kehre zu deinem Startpunkt zurück und benutze ~g~Y ~w~ um Feierabend zu machen.");
                        });


                        API.setEntitySyncedData(player, "IS_job", true);
                        API.setEntitySyncedData(myvehicle, "VEHICLE_FUEL", 100);
                        API.setEntitySyncedData(myvehicle, "VEHICLE_FUEL_MAX", 100);
                        API.setEntitySyncedData(myvehicle, "Vehicle_Use", "garden_job");
                        API.setEntitySyncedData(myvehicle, "OwnerName", player.name);
                        nextGarden(player);
                    }





                }

            }

            if (eventName == "endeGarden") //an eventname with no params that was triggered from the Client-side script
            {

                if (API.getEntitySyncedData(player, "IS_job") != true)
                {
                    return;
                }


                if (misStartColshape.containsEntity(player.handle))
                {
                    API.sendNotificationToPlayer(player, "~w~ Schönen Feierabend!");
                    API.setEntitySyncedData(player, "IS_job", false);

                }

            }


            if (eventName == "GardenComplete") //an eventname with no params that was triggered from the Client-side script
            {

                if (API.getEntitySyncedData(player, "IS_job") != true)
                {
                    return;
                }



                if ((point1.containsEntity(player.handle)) || (point2.containsEntity(player.handle)) || (point3.containsEntity(player.handle)) || (point4.containsEntity(player.handle)) || (point5.containsEntity(player.handle)) || (point6.containsEntity(player.handle)) || (point7.containsEntity(player.handle)) || (point8.containsEntity(player.handle)) || (point9.containsEntity(player.handle))) 
                {
                    if ((point1.containsEntity(player.handle)) && (lastpoint == 1)) { return; }
                    if ((point2.containsEntity(player.handle)) && (lastpoint == 2)) { return; }
                    if ((point3.containsEntity(player.handle)) && (lastpoint == 3)) { return; }
                    if ((point4.containsEntity(player.handle)) && (lastpoint == 4)) { return; }
                    if ((point5.containsEntity(player.handle)) && (lastpoint == 5)) { return; }
                    if ((point6.containsEntity(player.handle)) && (lastpoint == 6)) { return; }
                    if ((point7.containsEntity(player.handle)) && (lastpoint == 7)) { return; }
                    if ((point8.containsEntity(player.handle)) && (lastpoint == 8)) { return; }
                    if ((point9.containsEntity(player.handle)) && (lastpoint == 9)) { return; }

                    taskgarden = taskgarden + 1;

                    if (taskgarden == 10)
                    {
                        API.sendNotificationToPlayer(player, "~w~ Dein Gehalt wurde dir ausgezahlt: ~g~ 800 ~y~ $ ");
                        double amount = 800.0;

                        MoneyService.AddPlayerCash(player, amount);

                        taskgarden = 0;

                        nextGarden(player);
                    }
                    else
                    {
                        nextGarden(player);
                    }
                    
                }
            }
        }


        public void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle, int fromSeat)
        {
            if (!IsInRangeOf(player.position, new Vector3(-1348.501, 142.6998, 56.43797), 8f)) { return; }
            if ((API.getEntitySyncedData(player, "IS_job") == false) || (!API.hasEntitySyncedData(player, "IS_job"))) { return; }

            if ((API.hasEntitySyncedData(vehicle, "OwnerName")) && (API.getEntitySyncedData(vehicle, "OwnerName") == player.name) && (API.getEntitySyncedData(vehicle, "Vehicle_Use") == "garden_job") && (fromSeat == -1))
            {
                API.deleteEntity(vehicle);
            }
        }



        public void nextGarden(Client player)
        {
            vpoint1 = new Vector3(-1335.492, 119.3447, 55.38317);
            vpoint2 = new Vector3(-1295.532, 118.3462, 55.67009);
            vpoint3 = new Vector3(-1264.291, 107.0072, 55.35939);
            vpoint4 = new Vector3(-1231.783, 92.1292, 55.27038);
            ///
            vpoint5 = new Vector3(-1185.744, 69.16298, 54.5);
            vpoint6 = new Vector3(-1141.321, 16.39791, 48.7);
            vpoint7 = new Vector3(-1168.051, -29.97899, 44.5);
            vpoint8 = new Vector3(-1239.033, 6.37442, 47);
            vpoint9 = new Vector3(-1312.314, 63.75764, 52.5);
         


            if (IsInRangeOf(player.position, new Vector3(-1348.501, 142.6998, 56.43797), 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint1, vpoint1.X, vpoint1.Y); lastpoint = 0; }
            if (IsInRangeOf(player.position, vpoint1, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint2, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint3, vpoint2.X, vpoint2.Y); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint3, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint4, vpoint2.X, vpoint2.Y); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint4, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint5, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint5, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint6, vpoint2.X, vpoint2.Y); lastpoint = 5; }
            if (IsInRangeOf(player.position, vpoint6, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint7, vpoint2.X, vpoint2.Y); lastpoint = 6; }
            if (IsInRangeOf(player.position, vpoint7, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint8, vpoint2.X, vpoint2.Y); lastpoint = 7; }
            if (IsInRangeOf(player.position, vpoint8, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint9, vpoint2.X, vpoint2.Y); lastpoint = 8; }
            if (IsInRangeOf(player.position, vpoint9, 2f)) { API.triggerClientEvent(player, "nextGarden", vpoint1, vpoint2.X, vpoint2.Y); lastpoint = 9; }

        }


        public static bool IsInRangeOf(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }


        [Command("myseat")]
        public void CMD_MySeat(Client player)
        {
            int seat = API.getPlayerVehicleSeat(player);
            API.sendChatMessageToPlayer(player, Convert.ToString(seat));
        }



    }


}