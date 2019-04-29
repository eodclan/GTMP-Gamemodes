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
    public class PostJob : Script
    {
        public PostJob()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEvent;
            API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;

        }

        public ColShape misStartColshape; // start ColShape
        public ColShape misEndColshape;

        public ColShape point1 = API.shared.createCylinderColShape(new Vector3(-460.2973, -361.8718, 33.95075), 5f, 5f);

        public ColShape point2 = API.shared.createCylinderColShape(new Vector3(411.2952, -1013.985, 29.32514), 5f, 5f);

        public ColShape point3 = API.shared.createCylinderColShape(new Vector3(274.599, -392.6435, 44.95033), 5f, 5f);

        public ColShape point4 = API.shared.createCylinderColShape(new Vector3(929.7446, -162.3792, 74.56581), 5f, 5f);

        public ColShape point5 = API.shared.createCylinderColShape(new Vector3(-1103.467, -1990.639, 13.14334), 5f, 5f);

        public ColShape point6 = API.shared.createCylinderColShape(new Vector3(-2174.77, -365.2317, 13.08957), 5f, 5f);

        public ColShape point7 = API.shared.createCylinderColShape(new Vector3(-801.0533, -233.6835, 37.15225), 5f, 5f);

        public ColShape point8 = API.shared.createCylinderColShape(new Vector3(1241.374, -601.6973, 68.4), 5f, 5f);

        public ColShape point9 = API.shared.createCylinderColShape(new Vector3(1271.305, -683.5297, 65.1), 5f, 5f);

        public ColShape point10 = API.shared.createCylinderColShape(new Vector3(1229.065, -725.6357, 59.9), 5f, 5f);

        Vector3 vpoint1;
        Vector3 vpoint2;
        Vector3 vpoint3;
        Vector3 vpoint4;
        Vector3 vpoint5;
        Vector3 vpoint6;
        Vector3 vpoint7;
        Vector3 vpoint8;
        Vector3 vpoint9;
        Vector3 vpoint10;

        public int taskpost;
        public Marker despostBlip;
        public int lastpoint;



        public void OnResourceStart()
        {
            Blip misBlip = API.createBlip(new Vector3(151.8946, 6644.426, 31.60765)); // blip to start job
            API.setBlipSprite(misBlip, 478);
            API.setBlipName(misBlip, "Go Postal");
            API.setBlipColor(misBlip, 2);
            API.createPed((PedHash)(1498487404), new Vector3(151.8946, 6644.426, 31.60765), -8);
            misStartColshape = API.createCylinderColShape(new Vector3(151.8946, 6644.426, 31.60765), 5f, 5f); // colshap for mission start point

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
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Drücken Sie Y, um den Job zu starten.", "CHAR_CHEF", 0, 2, "Paint Job", "Job annehmen");
                    //API.sendNotificationToPlayer(player, "~w~ Du möchtest als PizzaLiferand Geld verdienen? Benutze ~g~E~w~ um direkt anzufangen.");
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
            if (eventName == "StartMissionPost") //an eventname with no params that was triggered from the Client-side script
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
                        VehicleHash taxi = API.vehicleNameToModel("Boxville");
                        Vehicle myvehicle = API.createVehicle(taxi, new Vector3(141.1384, 6636.001, 31.62045), new Vector3(1.146717, -11.70121, -57.9362), 0, 0, 0);
                        API.setPlayerIntoVehicle(player, myvehicle, -1);

                        API.delay(1000, true, () =>
                        {
                            API.sendNotificationToPlayer(player, " ~w~ Fahre die markierten Punkte auf der Karte ab und tippe ~g~Q ~w~ bei Ihnen an.");
                            API.sendNotificationToPlayer(player, " ~w~ Kehre zu deinem Startpunkt zurück und benutze ~g~Y ~w~ um Feierabend zu machen.");
                        });


                        API.setEntitySyncedData(player, "IS_job", true);
                        API.setEntitySyncedData(myvehicle, "VEHICLE_FUEL", 100);
                        API.setEntitySyncedData(myvehicle, "VEHICLE_FUEL_MAX", 100);
                        API.setEntitySyncedData(myvehicle, "Vehicle_Use", "Post_job");
                        API.setEntitySyncedData(myvehicle, "OwnerName", player.name);
                        nextpostJob(player);
                    }





                }

            }

            if (eventName == "endePostJob") //an eventname with no params that was triggered from the Client-side script
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


            if (eventName == "objComplete") //an eventname with no params that was triggered from the Client-side script
            {

                if (API.getEntitySyncedData(player, "IS_job") != true)
                {
                    return;
                }



                if ((point1.containsEntity(player.handle)) || (point2.containsEntity(player.handle)) || (point3.containsEntity(player.handle)) || (point4.containsEntity(player.handle)) || (point5.containsEntity(player.handle)) || (point6.containsEntity(player.handle)) || (point7.containsEntity(player.handle)) || (point8.containsEntity(player.handle)) || (point9.containsEntity(player.handle)) || (point10.containsEntity(player.handle)))
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
                    if ((point10.containsEntity(player.handle)) && (lastpoint == 10)) { return; }

                    taskpost = taskpost + 1;

                    if (taskpost == 10)
                    {
                        API.sendNotificationToPlayer(player, "~w~ Dein Gehalt wurde dir ausgezahlt: ~g~ 2975 ~y~ $ ");
                        double amount = 2975.0;

                        MoneyService.AddPlayerCash(player, amount);

                        taskpost = 0;

                        nextpostJob(player);
                    }
                    else
                    {
                        nextpostJob(player);
                    }

                }
            }
        }


        public void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle, int fromSeat)
        {
            if (!IsInRangeOf(player.position, new Vector3(141.1384, 6636.001, 31.62045), 10f)) { return; }
            if ((API.getEntitySyncedData(player, "IS_job") == false) || (!API.hasEntitySyncedData(player, "IS_job"))) { return; }

            if ((API.hasEntitySyncedData(vehicle, "OwnerName")) && (API.getEntitySyncedData(vehicle, "OwnerName") == player.name) && (API.getEntitySyncedData(vehicle, "Vehicle_Use") == "Post_job") && (fromSeat == -1))
            {
                API.deleteEntity(vehicle);
            }
        }



        public void nextpostJob(Client player)
        {
            vpoint1 = new Vector3(-460.2973, -361.8718, 33.95075);

            vpoint2 = new Vector3(411.2952, -1013.985, 29.32514);

            vpoint3 = new Vector3(274.599, -392.6435, 44.95033);

            vpoint4 = new Vector3(929.7446, -162.3792, 74.56581);

            vpoint5 = new Vector3(-1103.467, -1990.639, 13.14334);

            vpoint6 = new Vector3(-2174.77, -365.2317, 13.08957);

            vpoint7 = new Vector3(-801.0533, -233.6835, 37.15225);

            vpoint8 = new Vector3(1241.374, -601.6973, 68.4);

            vpoint9 = new Vector3(1271.305, -683.5297, 65.1);

            vpoint10 = new Vector3(1229.065, -725.6357, 59.9);




            if (IsInRangeOf(player.position, new Vector3(151.8946, 6644.426, 31.60765), 10f)) { API.triggerClientEvent(player, "nextpostJob", vpoint1, vpoint1.X, vpoint1.Y); lastpoint = 0; }
            if (IsInRangeOf(player.position, vpoint1, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint2, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint3, vpoint2.X, vpoint2.Y); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint3, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint4, vpoint2.X, vpoint2.Y); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint4, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint5, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint5, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint6, vpoint2.X, vpoint2.Y); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint6, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint7, vpoint2.X, vpoint2.Y); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint7, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint8, vpoint2.X, vpoint2.Y); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint8, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint9, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint9, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint10, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint10, 5f)) { API.triggerClientEvent(player, "nextpostJob", vpoint1, vpoint2.X, vpoint2.Y); lastpoint = 4; }


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
            API.sendNotificationToPlayer(player, Convert.ToString(seat));
        }



    }


}