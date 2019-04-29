using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using GrandTheftMultiplayer.Server.Constant;
using FactionLife.Server.Services.FactionService;
using System.Data;
using System.Timers;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Managers;

namespace FactionLife.Server.Services.HolzVer
{
    class HolzVera : Script
    {
        public HolzVera()
        {
            API.onClientEventTrigger += OnClientEvent;
            API.onResourceStart += OnStart;
        }
        
        private Ped QuetscherPed = null;
        public ColShape misStartColshape;

        public void OnStart()
        {
            QuetscherPed = API.createPed(PedHash.Autoshop01SMM, new Vector3(-1887.5152, 2049.646, 140.9816), 71);  //HIER NOCH DIE POSITION EINTRAGEN!!!

            API.createMarker(30, new Vector3(2315.95, 5002.913, 42.33128), new Vector3(), new Vector3(), new Vector3(2, 2, 2), 155, 0, 0, 155); // marker to start the job
            misStartColshape = API.createCylinderColShape(new Vector3(2315.95, 5002.913, 42.33128), 3F, 2F); // colshap for mission start point

            misStartColshape.onEntityEnterColShape += (shape, entity) =>
            {
                Client player;

                if ((player = API.getPlayerFromHandle(entity)) != null)
                {
                    API.sendNotificationToPlayer(player, "<C>Orangen Feld:</C> ~n~Drücken Sie bei ihrgent ein Baum E, um die Arbeit zu beginnen!");
                }
            };
        }




        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            Player player = client.getData("player");

            switch (eventName)
            {
                case "KeyboardKey_E_Pressed":

              if (IsInRangeOf(client.position, new Vector3(0, 0, 0), 60f))
                        {
                        if (API.getEntitySyncedData(client, "just_pickedup") == true) { return; }
                        if (client.isInVehicle) { return; }

                        API.setEntitySyncedData(client, "just_pickedup", true);
                        API.playPlayerAnimation(client, 0, "anim@mp_snowball", "pickup_snowball");

                        //.............
                        //sets the amount of "Orange" given. (between 1+2)....
                        Random rnd = new Random();
                        int count = rnd.Next(1, 3);


                        //Adding........

                        InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 32);
                        if (invitem == null)
                        {
                            player.Character.Inventory.Add(new InventoryItem
                            {
                                ItemID = 32,
                                Count = count
                            });
                        }
                        else
                        {
                            if (count == 1)
                            {
                                invitem.Count++;
                            }
                            if (count == 2)
                            {
                                invitem.Count++;
                                invitem.Count++;
                            }
                            
                        }
                        API.sendPictureNotificationToPlayer(client, "Sie haben ~g~" + count + "~w~ Orangen Aufgehoben", "CHAR_BUGSTARS", 0, 3, "Tongva Hils", "Orangen Farm");


                        API.delay(2000, true, () =>
                        {

                            API.setEntitySyncedData(client, "just_pickedup", false);

                        });

                    }



                    if (IsInRangeOf(client.position, new Vector3(-1887.5152, 2049.646, 140.9816), 3f))
                    {

                        if (API.getEntitySyncedData(client, "just_sell") == true) { return; }
                        if (client.isInVehicle) { return; }

                        API.setEntitySyncedData(client, "just_sell", true);

                        InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 32); 
                        if ((invItem == null) || (invItem.Count < 3)) { API.sendNotificationToPlayer(client, "~r~Du brauchst mindestens 3 Orangen!"); ; API.delay(350, true, () => { API.setEntitySyncedData(client, "just_sell", false); }); return; }

                        if (invItem.Count >= 3)
                        {

                            invItem.Count = invItem.Count - 3;
                            
                            if (invItem.Count <= 0)
                                player.Character.Inventory.Remove(invItem);

                            AddOrangen(client);
                            

                        }


                    }



                    break;
            }
        }


        public void AddOrangen(Client client)
        {
            Player player = client.getData("player");
            InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 33);
            if (invitem == null)
            {
                player.Character.Inventory.Add(new InventoryItem
                {
                    ItemID = 33,
                    Count = 1
                });
            }
            else
            {
              invitem.Count++;
            }

            API.sendNotificationToPlayer(client, "~b~Sie haben ~g~1 ~w~ Orangensaft erhalten");

            API.delay(1000, true, () =>
            {

                API.setEntitySyncedData(client, "just_sell", false);

            });
        }





        public static bool IsInRangeOf(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }


    }
}
