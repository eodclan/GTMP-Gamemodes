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
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Model;

namespace FactionLife.Server.JobService
{
    public class FisherJob : Script
    {
        public FisherJob()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEvent;
            API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;

        }

        public ColShape misStartColshape; // start ColShape
        public ColShape misEndColshape;

        public ColShape point1 = API.shared.createCylinderColShape(new Vector3(-1656.672, -1105.112, 1.14157), 5f, 5f);

        public ColShape point2 = API.shared.createCylinderColShape(new Vector3(-1692.87, -1081.361, 0.9677091), 5f, 5f);

        public ColShape point3 = API.shared.createCylinderColShape(new Vector3(-1720.1065, -1065.534, 1.142875), 5f, 5f);

        public ColShape point4 = API.shared.createCylinderColShape(new Vector3(-1760.694, -1029.063, 1.479734), 5f, 5f);

        public ColShape point5 = API.shared.createCylinderColShape(new Vector3(-1801.436, -1001.805, 1.02071), 5f, 5f);

        public ColShape point6 = API.shared.createCylinderColShape(new Vector3(-1817.56, -948.8705, 1.519469), 5f, 5f);

        public ColShape point7 = API.shared.createCylinderColShape(new Vector3(-1856.03, -875.8522, 1.648905), 5f, 5f);

        public ColShape point8 = API.shared.createCylinderColShape(new Vector3(-1883.768, -844.1508, 1.341838), 5f, 5f);

        public ColShape point9 = API.shared.createCylinderColShape(new Vector3(-1656.672, -1105.112, 1.14157), 5f, 5f);

        public ColShape point10 = API.shared.createCylinderColShape(new Vector3(-1692.87, -1081.361, 0.9677091), 5f, 5f);

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

        public int taskfisher;
        public Blip desfishblip;
        public int lastpoint;



        public void OnResourceStart()
        {
            Blip misBlip = API.createBlip(new Vector3(-1664.954, -1003.702, 7.390153)); // blip to start job
            API.setBlipSprite(misBlip, 68);
            API.setBlipName(misBlip, "Fisher Job");
            API.setBlipColor(misBlip, 2);
            API.createPed((PedHash)(1498487404), new Vector3(-1664.954, -1003.702, 7.390153), -8);
            misStartColshape = API.createCylinderColShape(new Vector3(-1664.954, -1003.702, 7.390153), 5f, 5f); // colshap for mission start point

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
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Drücken Sie Y, um den Job zu starten.", "CHAR_BOATSITE", 0, 2, "Fisher Job", "Job annehmen");
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
            if (eventName == "StartFisherMission") //an eventname with no params that was triggered from the Client-side script
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

                        API.delay(1000, true, () =>
                        {
                            API.sendNotificationToPlayer(player, " ~w~ Fahre die markierten Punkte auf der Karte ab und tippe ~g~Q ~w~ bei Ihnen an.");
                            API.sendNotificationToPlayer(player, " ~w~ Kehre zu deinem Startpunkt zurück und benutze ~g~Y ~w~ um Feierabend zu machen.");
                        });


                        API.setEntitySyncedData(player, "IS_job", true);
                        nextfisherJob(player);
                    }





                }

            }

            if (eventName == "endeFisherJob") //an eventname with no params that was triggered from the Client-side script
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


            if (eventName == "FisherComplete") //an eventname with no params that was triggered from the Client-side script
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

                    taskfisher = taskfisher + 1;

                    if (taskfisher == 10)
                    {
                        Random rnd = new Random();
                        int amount = rnd.Next(1, 5);

                        API.playPlayerAnimation(player, (int)(AnimationFlags.Loop), "amb@world_human_stand_fishing@base", "base");
                        API.delay(2500, true, () => {
                            API.stopPlayerAnimation(player);
                        });
                        API.shared.sendPictureNotificationToPlayer(player, "~g~Gute Arbeit, du hast ~w~" + amount + "x  Fisch gefangen.", "CHAR_BOATSITE", 0, 2, "Fisher Job", "Dein Fang");
                        Player client = player.getData("player");
                        client.Character.Inventory.Add(new InventoryItem { ItemID = 266, Count = amount });

                        taskfisher = 0;

                        nextfisherJob(player);
                    }
                    else
                    {
                        nextfisherJob(player);
                    }

                }
            }
        }


        public void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle, int fromSeat)
        {
            if (!IsInRangeOf(player.position, new Vector3(-1664.954, -1003.702, 7.390153), 10f)) { return; }
            if ((API.getEntitySyncedData(player, "IS_job") == false) || (!API.hasEntitySyncedData(player, "IS_job"))) { return; }
        }



        public void nextfisherJob(Client player)
        {
            vpoint1 = new Vector3(-1656.672, -1105.112, 1.14157);

            vpoint2 = new Vector3(-1692.87, -1081.361, 0.9677091);

            vpoint3 = new Vector3(-1720.1065, -1065.534, 1.142875);

            vpoint4 = new Vector3(929.7446, -162.3792, 74.56581);

            vpoint5 = new Vector3(-1801.436, -1001.805, 1.02071);

            vpoint6 = new Vector3(-1817.56, -948.8705, 1.519469);

            vpoint7 = new Vector3(-1856.03, -875.8522, 1.648905);

            vpoint8 = new Vector3(-1883.768, -844.1508, 1.341838);

            vpoint9 = new Vector3(-1656.672, -1105.112, 1.14157);

            vpoint10 = new Vector3(-1692.87, -1081.361, 0.9677091);




            if (IsInRangeOf(player.position, new Vector3(-1664.954, -1003.702, 7.390153), 10f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint1, vpoint1.X, vpoint1.Y, desfishblip); lastpoint = 0; }
            if (IsInRangeOf(player.position, vpoint1, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint2, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint2, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint3, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint3, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint4, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint4, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint5, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint5, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint6, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint6, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint7, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint7, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint8, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint8, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint9, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint9, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint10, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint10, 5f)) { API.triggerClientEvent(player, "nextfisherJob", vpoint1, vpoint2.X, vpoint2.Y, desfishblip); lastpoint = 4; }


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