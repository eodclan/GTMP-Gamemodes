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
    public class Busing : Script
    {
        public Busing()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEvent;
            API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;

        }

        public ColShape misStartColshape; // start ColShape
        public ColShape misEndColshape;

        public ColShape point1 = API.shared.createCylinderColShape(new Vector3(307.4173, -765.2422, 29.22754), 1f, 1f);
        public ColShape point2 = API.shared.createCylinderColShape(new Vector3(115.908, -784.7598, 31.29946), 1f, 1f);
        public ColShape point3 = API.shared.createCylinderColShape(new Vector3(-245.0394, -714.5654, 33.4048), 1f, 1f);
        public ColShape point4 = API.shared.createCylinderColShape(new Vector3(-1411.581, -570.2775, 30.29306), 1f, 1f);
        public ColShape point5 = API.shared.createCylinderColShape(new Vector3(-517.4875, -264.4301, 35.36923), 1f, 1f);
        public ColShape point6 = API.shared.createCylinderColShape(new Vector3(806.8471, -1354.794, 26.30441), 1f, 1f);
        public ColShape point7 = API.shared.createCylinderColShape(new Vector3(785.6602, -776.3989, 26.33604), 1f, 1f);
        public ColShape point8 = API.shared.createCylinderColShape(new Vector3(361.224, -1064.722, 29.36104), 1f, 1f);
        public ColShape point9 = API.shared.createCylinderColShape(new Vector3(466.5779, -603.6126, 28.4802), 1f, 1f);
        public ColShape point10 = API.shared.createCylinderColShape(new Vector3(425.3197, -624.0981, 28.49599), 1f, 1f);

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

        public int taskbus;
        public Marker marker;
        public Marker desbus;
        public int lastpoint;



        public void OnResourceStart()
        {
            Blip pizzaBlip = API.createBlip(new Vector3(435.8679, -645.9768, 28.73914)); // blip to start job
            API.setBlipSprite(pizzaBlip, 513);
            API.setBlipName(pizzaBlip, "Bus Job");
            API.setBlipColor(pizzaBlip, 2);
            API.createPed((PedHash)(1498487404), new Vector3(435.8679, -645.9768, 28.73914), 76);
            misStartColshape = API.createCylinderColShape(new Vector3(435.8679, -645.9768, 28.73914), 1f, 1f); // colshap for mission start point

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
                    //API.sendChatMessageToPlayer(player, "~y~[SERVER]~w~ Du möchtest als Busfahrer Geld verdienen? Benutze ~g~E~w~ um direkt anzufangen.");
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Drücken Sie Y, um den Job zu starten.", "CHAR_CHEF", 0, 2, "Trash Job", "Job annehmen");
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
            if (eventName == "MissionBusTrigger") //an eventname with no params that was triggered from the Client-side script
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
                        VehicleHash taxi = API.vehicleNameToModel("Bus");
                        Vehicle myvehicle = API.createVehicle(taxi, new Vector3(425.0195, -641.9323, 28.49604), new Vector3(-0.04177063, 0.004018761, -179.8253), 0, 0, 0);
                        API.setPlayerIntoVehicle(player, myvehicle, -1);

                        API.delay(1000, true, () =>
                        {
                            API.sendNotificationToPlayer(player, " ~w~ Fahre die markierten Punkte auf der Karte ab und tippe ~g~Q ~w~ bei Ihnen an.");
                            API.sendNotificationToPlayer(player, " ~w~ Kehre zu deinem Startpunkt zurück und benutze ~g~Y ~w~ um Feierabend zu machen.");
                        });


                        API.setEntitySyncedData(player, "IS_job", true);
                        API.setEntitySyncedData(myvehicle, "VEHICLE_FUEL", 100);
                        API.setEntitySyncedData(myvehicle, "VEHICLE_FUEL_MAX", 100);
                        API.setEntitySyncedData(myvehicle, "Vehicle_Use", "Bus_job");
                        API.setEntitySyncedData(myvehicle, "OwnerName", player.name);
                        nextBus(player);
                    }





                }

            }

            if (eventName == "endeBus") //an eventname with no params that was triggered from the Client-side script
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


            if (eventName == "objBusComplete") //an eventname with no params that was triggered from the Client-side script
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

                    taskbus = taskbus + 1;

                    if (taskbus == 10)
                    {
                        API.sendNotificationToPlayer(player, "~y~[SERVER]~g~ [Job] ~w~ Dein Gehalt wurde dir ausgezahlt: ~g~ 100 ~y~ $ ");
                        double amount = 100.0;

                        MoneyService.AddPlayerCash(player, amount);

                        taskbus = 0;

                        nextBus(player);
                    }
                    else
                    {
                        nextBus(player);
                    }

                }
            }
        }


        public void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle, int fromSeat)
        {
            if (!IsInRangeOf(player.position, new Vector3(435.8679, -645.9768, 28.73914), 20f)) { return; }
            if ((API.getEntitySyncedData(player, "IS_job") == false) || (!API.hasEntitySyncedData(player, "IS_job"))) { return; }

            if ((API.hasEntitySyncedData(vehicle, "OwnerName")) && (API.getEntitySyncedData(vehicle, "OwnerName") == player.name) && (API.getEntitySyncedData(vehicle, "Vehicle_Use") == "Bus_job") && (fromSeat == -1))
            {
                API.deleteEntity(vehicle);
            }
        }



        public void nextBus(Client player)
        {
            vpoint1 = new Vector3(307.4173, -765.2422, 29.22754);
            vpoint2 = new Vector3(115.908, -784.7598, 31.29946);
            vpoint3 = new Vector3(-245.0394, -714.5654, 33.4048);
            vpoint4 = new Vector3(-1411.581, -570.2775, 30.29306);
            vpoint5 = new Vector3(-517.4875, -264.4301, 35.36923);
            vpoint6 = new Vector3(806.8471, -1354.794, 26.30441);
            vpoint7 = new Vector3(785.6602, -776.3989, 26.33604);
            vpoint8 = new Vector3(361.224, -1064.722, 29.36104);
            vpoint9 = new Vector3(466.5779, -603.6126, 28.4802);
            vpoint10 = new Vector3(425.3197, -624.0981, 28.49599);




            if (IsInRangeOf(player.position, new Vector3(435.8679, -645.9768, 28.73914), 5f)) { API.triggerClientEvent(player, "nextBus", vpoint1, vpoint1.X, vpoint1.Y, marker); lastpoint = 0; }
            if (IsInRangeOf(player.position, vpoint1, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint2, vpoint2.X, vpoint2.Y, marker); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint2, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint3, vpoint2.X, vpoint2.Y, marker); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint3, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint4, vpoint2.X, vpoint2.Y, marker); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint4, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint5, vpoint2.X, vpoint2.Y, marker); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint5, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint6, vpoint2.X, vpoint2.Y, marker); lastpoint = 5; }
            if (IsInRangeOf(player.position, vpoint6, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint7, vpoint2.X, vpoint2.Y, marker); lastpoint = 6; }
            if (IsInRangeOf(player.position, vpoint7, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint8, vpoint2.X, vpoint2.Y, marker); lastpoint = 7; }
            if (IsInRangeOf(player.position, vpoint8, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint9, vpoint2.X, vpoint2.Y, marker); lastpoint = 8; }
            if (IsInRangeOf(player.position, vpoint9, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint10, vpoint2.X, vpoint2.Y, marker); lastpoint = 9; }
            if (IsInRangeOf(player.position, vpoint10, 5f)) { API.triggerClientEvent(player, "nextBus", vpoint1, vpoint2.X, vpoint2.Y, marker); lastpoint = 1; }

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