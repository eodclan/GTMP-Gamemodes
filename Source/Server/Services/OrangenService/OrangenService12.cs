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
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Services.FactionService;
using System.Data;
using System.Timers;

namespace FactionLife.Server.Services.DrugService
{
    class OrangenService12 : Script
    {
        public OrangenService12()
        {
            API.onClientEventTrigger += OnClientEvent;
            API.onResourceStart += OnStart;
        }

        public ColShape misStartColshape;
        //public ColShape misEndColshape;

        public void OnStart()
        {
            misStartColshape = API.createCylinderColShape(new Vector3(-1769.18, 2005.103, 119.4262), 3F, 2F); // colshap for mission start point

            misStartColshape.onEntityEnterColShape += (shape, entity) =>
            {
                Client player;

                if ((player = API.getPlayerFromHandle(entity)) != null)
                {
                    API.sendNotificationToPlayer(player, "<C>Orangen Feld:</C> ~n~Drücken Sie E, um zu Farmen");
                }
            };
        }




        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            Player player = client.getData("player");

            switch (eventName)
            {
                case "KeyboardKey_E_Pressed":

                    if (IsInRangeOf(client.position, new Vector3(-1769.18, 2005.103, 119.4262), 2f))
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
                            invitem.Count++;
                        }
                        else
                        {
                            if (count >= ItemService.ItemService.MaxInventorySlots)
                            {
                                invitem.Count++;
                            }
                            if (count == ItemService.ItemService.MaxInventorySlots)
                            {
                                invitem.Count++;
                            }
                            API.sendNotificationToPlayer(client, "Du kannst nicht so viel tragen oder bist du Hulk?");
                        }
                        API.sendNotificationToPlayer(client, "~b~Sie haben ~g~" + count + "~w~ Orangen Aufgehoben!");


                        API.delay(2000, true, () =>
                        {

                            API.setEntitySyncedData(client, "just_pickedup", false);

                        });

                    }



                    break;
            }
        }





        public static bool IsInRangeOf(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }


    }
}
