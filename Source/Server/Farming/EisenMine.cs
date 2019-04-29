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
    public class EisenMine : Script
    {
        public EisenMine()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEvent;
            API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;

        }

        public ColShape misStartColshape; // start ColShape
        public ColShape misEndColshape;

        public ColShape point1 = API.shared.createCylinderColShape(new Vector3(2934.423, 2794.797, 40.61414), 5f, 5f);

        public ColShape point2 = API.shared.createCylinderColShape(new Vector3(2933.154, 2812.132, 43.40099), 5f, 5f);

        public ColShape point3 = API.shared.createCylinderColShape(new Vector3(2933.154, 2812.132, 43.40099), 5f, 5f);

        public ColShape point4 = API.shared.createCylinderColShape(new Vector3(2933.154, 2812.132, 43.40099), 5f, 5f);

        public ColShape point5 = API.shared.createCylinderColShape(new Vector3(2933.154, 2812.132, 43.40099), 5f, 5f);

        public ColShape point6 = API.shared.createCylinderColShape(new Vector3(2933.154, 2812.132, 43.40099), 5f, 5f);

        Vector3 vpoint1;
        Vector3 vpoint2;
        Vector3 vpoint3;
        Vector3 vpoint4;
        Vector3 vpoint5;
        Vector3 vpoint6;

        public int taskeisenerz;
        public int lastpoint;



        public void OnResourceStart()
        {
            API.createMarker(30, new Vector3(2941.89, 2798.257, 40.81898), new Vector3(), new Vector3(), new Vector3(2, 2, 2), 155, 0, 0, 155); // marker to start the job
            Blip misBlip = API.createBlip(new Vector3(2941.89f, 2798.257f, 40.81898f)); // blip to start job
            API.setBlipSprite(misBlip, 364);
            API.setBlipName(misBlip, "Eisen Mine");
            API.setBlipColor(misBlip, 4);
            API.setBlipScale(misBlip, 0.8f);
            misStartColshape = API.createCylinderColShape(new Vector3(2941.89, 2798.257, 40.81898), 5f, 5f); // colshap for mission start point

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
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Drücken Sie Y, um zu Farmen.", "CHAR_CHEF", 0, 2, "Eisen Mine", "Job annehmen");
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
            if (eventName == "StartEisenMine") //an eventname with no params that was triggered from the Client-side script
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
                            API.sendNotificationToPlayer(player, " ~w~ Gehe zu den Punkte auf der Karte ab und tippe ~g~Q ~w~ bei Ihnen an.");
                            API.sendNotificationToPlayer(player, " ~w~ Dannach kehre zum Startpunkt zurück und benutze ~g~Y ~w~ um Feierabend zu machen.");
                        });


                        API.setEntitySyncedData(player, "IS_job", true);
                        nextEisenMine(player);
                    }





                }

            }

            if (eventName == "endeEisenMine") //an eventname with no params that was triggered from the Client-side script
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


            if (eventName == "EisenMineComplete") //an eventname with no params that was triggered from the Client-side script
            {

                if (API.getEntitySyncedData(player, "IS_job") != true)
                {
                    return;
                }



                if ((point1.containsEntity(player.handle)) || (point2.containsEntity(player.handle)) || (point3.containsEntity(player.handle)) || (point4.containsEntity(player.handle)) || (point5.containsEntity(player.handle)) || (point6.containsEntity(player.handle)))
                {
                    if ((point1.containsEntity(player.handle)) && (lastpoint == 1)) { return; }
                    if ((point2.containsEntity(player.handle)) && (lastpoint == 2)) { return; }
                    if ((point3.containsEntity(player.handle)) && (lastpoint == 3)) { return; }
                    if ((point4.containsEntity(player.handle)) && (lastpoint == 4)) { return; }
                    if ((point5.containsEntity(player.handle)) && (lastpoint == 5)) { return; }
                    if ((point6.containsEntity(player.handle)) && (lastpoint == 6)) { return; }

                    taskeisenerz = taskeisenerz + 1;

                    if (taskeisenerz == 6)
                    {
                        taskeisenerz = 0;
                        nextEisenMine(player);
                    }
                    else
                    {
                        Random rnd = new Random();
                        int amount = rnd.Next(13, 15);
                        API.playPlayerAnimation(player, (int)(AnimationFlags.Loop), "random@burial", "a_burial");
                        API.delay(15500, true, () => {
                            API.stopPlayerAnimation(player);
                        });

                        
                        Player client = player.getData("player");

                        InventoryItem invitem = client.Character.Inventory.FirstOrDefault(x => x.ItemID == 23);
                        if (invitem == null)
                        {
                            client.Character.Inventory.Add(new InventoryItem
                            {
                                ItemID = 23,
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
                        API.shared.sendPictureNotificationToPlayer(player, "~g~Du hast " + amount + "x Eisenerz erhalten.", "CHAR_CHEF", 0, 2, "Eisen Mine", "Erhalten");
                        //client.Character.Inventory.Add(new InventoryItem { ItemID = 23, Count = amount });
                        nextEisenMine(player);
                    }

                }
            }
        }


        public void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle, int fromSeat)
        {
            if (!IsInRangeOf(player.position, new Vector3(2933.154, 2812.132, 43.40099), 10f)) { return; }
            if ((API.getEntitySyncedData(player, "IS_job") == false) || (!API.hasEntitySyncedData(player, "IS_job"))) { return; }
        }



        public void nextEisenMine(Client player)
        {
            vpoint1 = new Vector3(2941.89, 2798.257, 40.81898);
            vpoint2 = new Vector3(2933.154, 2812.132, 43.40099);
            vpoint3 = new Vector3(2933.154, 2812.132, 43.40099);
            vpoint4 = new Vector3(2933.154, 2812.132, 43.40099);
            vpoint5 = new Vector3(2933.154, 2812.132, 43.40099);
            vpoint6 = new Vector3(2933.154, 2812.132, 43.40099);

            if (IsInRangeOf(player.position, new Vector3(2933.154, 2812.132, 43.40099), 10f)) { API.triggerClientEvent(player, "nextEisenMine", vpoint1, vpoint1.X, vpoint1.Y); lastpoint = 0; }
            if (IsInRangeOf(player.position, vpoint1, 5f)) { API.triggerClientEvent(player, "nextEisenMine", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 1; }
            if (IsInRangeOf(player.position, vpoint2, 5f)) { API.triggerClientEvent(player, "nextEisenMine", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 2; }
            if (IsInRangeOf(player.position, vpoint3, 5f)) { API.triggerClientEvent(player, "nextEisenMine", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 3; }
            if (IsInRangeOf(player.position, vpoint4, 5f)) { API.triggerClientEvent(player, "nextEisenMine", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 4; }
            if (IsInRangeOf(player.position, vpoint5, 5f)) { API.triggerClientEvent(player, "nextEisenMine", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 5; }
            if (IsInRangeOf(player.position, vpoint6, 5f)) { API.triggerClientEvent(player, "nextEisenMine", vpoint2, vpoint2.X, vpoint2.Y); lastpoint = 6; }
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