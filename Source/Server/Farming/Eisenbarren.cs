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

namespace FactionLife.Server.Services.EisenbarrenService
{
    class EisenbarrenService : Script
    {
        public EisenbarrenService()
        {
            API.onClientEventTrigger += OnClientEvent;
            API.onResourceStart += OnStart;
        }

        private Ped QuetscherPed = null;
        public ColShape misStartColshape;

        public void OnStart()
        {
            QuetscherPed = API.createPed(PedHash.Construct02SMY, new Vector3(1073.423, -1989.481, 30.89097), -69); //HIER NOCH DIE POSITION EINTRAGEN!!!
            Blip misBlip = API.createBlip(new Vector3(1073.423, -1989.481, 30.89097)); // blip to start job
            API.setBlipSprite(misBlip, 501);
            API.setBlipName(misBlip, "Eisen Verarbeiter");
            API.setBlipColor(misBlip, 2);
            API.setBlipScale(misBlip, 0.8f);

            misStartColshape = API.createCylinderColShape(new Vector3(1073.423, -1989.481, 30.89097), 3F, 2F); // colshap for mission start point

            misStartColshape.onEntityEnterColShape += (shape, entity) =>
            {
                Client player;

                if ((player = API.getPlayerFromHandle(entity)) != null)
                {
                    API.sendNotificationToPlayer(player, "<C>Eisen Verarbeiter:</C> ~n~Benutzen Sie E, um die Arbeit zu beginnen!");
                }
            };
        }




        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            Player player = client.getData("player");

            switch (eventName)
            {
                case "KeyboardKey_E_Pressed":

                    if (IsInRangeOf(client.position, new Vector3(1073.423, -1989.481, 30.89097), 1f))
                    {
                        if (API.getEntitySyncedData(client, "base") == true) { return; }
                        if (client.isInVehicle) { return; }

                        API.setEntitySyncedData(client, "dock", true);
                        API.playPlayerAnimation(client, 0, "amb@world_human_stand_mobile@male@text@base", "base");

                        //.............
                        //sets the amount of "Orange" given. (between 1+2)....
                        Random rnd = new Random();
                        int count = rnd.Next(1, 3);


                        //Adding........
                        InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 22);
                        if (invitem == null)
                        {
                            player.Character.Inventory.Add(new InventoryItem
                            {
                                ItemID = 22,
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

                        InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 23);
                        if ((invItem == null) || (invItem.Count < 3)) { API.sendNotificationToPlayer(client, "~r~Du brauchst mindestens 10 Eisenerz!"); ; API.delay(350, true, () => { API.setEntitySyncedData(client, "just_sell", false); }); return; }

                        if (invItem.Count >= 10)
                        {

                            invItem.Count = invItem.Count - 10;

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
            InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 22);
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

            API.sendNotificationToPlayer(client, "~g~Sie haben ~g~1 ~g~ Eisenbarren erhalten");

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
