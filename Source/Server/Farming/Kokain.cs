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
using FactionLife.Server.Services.ItemService;

namespace FactionLife.Server.JobService
{
    public class KokainPlantage : Script
    {
        public KokainPlantage()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEvent;
            API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;

        }

        public ColShape misStartColshape; // start ColShape
        public ColShape misEndColshape;

        public ColShape point1 = API.shared.createCylinderColShape(new Vector3(-1882.097, 4467.138, 30.67936), 5f, 5f);

        public ColShape point2 = API.shared.createCylinderColShape(new Vector3(-1867.389, 4463.038, 32.54126), 5f, 5f);

        public ColShape point3 = API.shared.createCylinderColShape(new Vector3(-1850.881, 4469.026, 31.8154), 5f, 5f);

        public ColShape point4 = API.shared.createCylinderColShape(new Vector3(-1849.703, 4486.775, 25.71666), 5f, 5f);

        public ColShape point5 = API.shared.createCylinderColShape(new Vector3(-1875.125, 4492.849, 25.18374), 5f, 5f);

        public ColShape point6 = API.shared.createCylinderColShape(new Vector3(-1873.577, 4502.96, 25.04), 5f, 5f);

        public ColShape point7 = API.shared.createCylinderColShape(new Vector3(-1888.699, 4489.27, 27.57977), 5f, 5f);

        public ColShape point8 = API.shared.createCylinderColShape(new Vector3(-1908.154, 4494.75, 29.28074), 5f, 5f);

        public ColShape point9 = API.shared.createCylinderColShape(new Vector3(-1882.097, 4467.138, 30.67936), 5f, 5f);

        public ColShape point10 = API.shared.createCylinderColShape(new Vector3(-1867.389, 4463.038, 32.54126), 5f, 5f);

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

        public int taskkokain;
        public Blip deskokainblip;
        public int lastpoint;



        public void OnResourceStart()
        {
            API.createMarker(30, new Vector3(-1844.932, 4479.647, 26.35476), new Vector3(), new Vector3(), new Vector3(2, 2, 2), 155, 0, 0, 155); // marker to start the job
            Blip misBlip = API.createBlip(new Vector3(-1844.932f, 4479.647f, 26.35476f)); // blip to start job
            API.setBlipSprite(misBlip, 51);
            API.setBlipName(misBlip, "Kokain Plantage");
            API.setBlipColor(misBlip, 4);
            API.setBlipScale(misBlip, 0.8f);
            misStartColshape = API.createCylinderColShape(new Vector3(-1844.932, 4479.647, 26.35476), 5f, 5f); // colshap for mission start point

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
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Drücken Sie Y, um zu Farmen.", "CHAR_CHEF", 0, 2, "Kokain Plantage", "Job annehmen");
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
            if (eventName == "StartKokainMission") //an eventname with no params that was triggered from the Client-side script
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
                            API.sendNotificationToPlayer(player, " ~w~ Gehe die markierten Punkte auf der Karte ab und tippe ~g~Q ~w~ bei Ihnen an.");
                            API.sendNotificationToPlayer(player, " ~w~ Kehre zu deinem Startpunkt zurück und benutze ~g~Y ~w~ um Feierabend zu machen.");
                        });


                        API.setEntitySyncedData(player, "IS_job", true);
                        nextKokain(player);
                    }





                }

            }

            if (eventName == "endeKokainJob") //an eventname with no params that was triggered from the Client-side script
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


            if (eventName == "KokainComplete") //an eventname with no params that was triggered from the Client-side script
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

                    taskkokain = taskkokain + 1;

                    if (taskkokain == 10)
                    {
                        taskkokain = 0;
                        nextKokain(player);
                    }
                    else
                    {
                        Random rnd = new Random();
                        int amount = rnd.Next(3, 5);
                        API.playPlayerAnimation(player, 0, "anim@mp_snowball", "pickup_snowball");
                        API.delay(4500, true, () => {
                            API.stopPlayerAnimation(player);
                        });

                        API.shared.sendPictureNotificationToPlayer(player, "~g~Du hast " + amount + "x Kokain erhalten.", "CHAR_CHEF", 0, 2, "Kokain Plantage", "Erhalten");
                        Player client = player.getData("player");


                        InventoryItem invitem = client.Character.Inventory.FirstOrDefault(x => x.ItemID == 261);
                        if (invitem == null)
                        {
                            client.Character.Inventory.Add(new InventoryItem
                            {
                                ItemID = 261,
                                Count = amount
                            });
                            invitem.Count++;
                        }
                        else
                        {
                            if (amount >= ItemService.MaxInventorySlots)
                            {
                                invitem.Count++;
                            }
                            if (amount == ItemService.MaxInventorySlots)
                            {
                                invitem.Count++;
                            }
                            API.sendNotificationToPlayer(player, "Du kannst nicht so viel tragen oder bist du Hulk?");
                        }

                        //client.Character.Inventory.Add(new InventoryItem {
                        //    ItemID = 261,
                        //    Count = amount
                        //});
                        nextKokain(player);
                    }

                }
            }
        }


        public void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle, int fromSeat)
        {
            if (!IsInRangeOf(player.position, new Vector3(-1844.932, 4479.647, 26.35476), 10f)) { return; }
            if ((API.getEntitySyncedData(player, "IS_job") == false) || (!API.hasEntitySyncedData(player, "IS_job"))) { return; }
        }



        public void nextKokain(Client player)
        {
            vpoint1 = new Vector3(-1882.097, 4467.138, 30.67936);

            vpoint2 = new Vector3(-1867.389, 4463.038, 32.54126);

            vpoint3 = new Vector3(-1850.881, 4469.026, 31.8154);

            vpoint4 = new Vector3(929.7446, -162.3792, 74.56581);

            vpoint5 = new Vector3(-1875.125, 4492.849, 25.18374);

            vpoint6 = new Vector3(-1873.577, 4502.96, 25.04);

            vpoint7 = new Vector3(-1888.699, 4489.27, 27.57977);

            vpoint8 = new Vector3(-1908.154, 4494.75, 29.28074);

            vpoint9 = new Vector3(-1882.097, 4467.138, 30.67936);

            vpoint10 = new Vector3(-1867.389, 4463.038, 32.54126);




            if (IsInRangeOf(player.position, new Vector3(-1844.932, 4479.647, 26.35476), 10f)) { API.triggerClientEvent(player, "nextKokain", vpoint1, vpoint1.X, vpoint1.Y); lastpoint = 0; }
            if (IsInRangeOf(player.position, vpoint1, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint2, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint3, vpoint2.X, vpoint2.Y); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint3, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint4, vpoint2.X, vpoint2.Y); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint4, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint5, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint5, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint6, vpoint2.X, vpoint2.Y); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint6, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint7, vpoint2.X, vpoint2.Y); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint7, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint8, vpoint2.X, vpoint2.Y); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint8, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint9, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint9, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint10, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint10, 5f)) { API.triggerClientEvent(player, "nextKokain", vpoint1, vpoint2.X, vpoint2.Y); lastpoint = 4; }


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