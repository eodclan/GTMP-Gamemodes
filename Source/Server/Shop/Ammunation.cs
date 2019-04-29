using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace FactionLife.Server.Services.AmmuShop
{
    class Ammunation : Script
    {
        private Ped pnj;
        private Blip blip;
        private static List<KeyValuePair<int, int>> products = new List<KeyValuePair<int, int>>();
        public Ammunation()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {

            products.Add(new KeyValuePair<int, int>(ITEM_ID_HEAVYPISTOL, 45000));
            products.Add(new KeyValuePair<int, int>(ITEM_ID_PISTOL, 52000));
            //products.Add(new KeyValuePair<int, int>(ITEM_ID_PUMPSHOTGUN, 1200));
            //products.Add(new KeyValuePair<int, int>(ITEM_ID_AMMOSHOTGUN, 60));
            //products.Add(new KeyValuePair<int, int>(ITEM_ID_AMMOPISTOL, 40));

            List<KeyValuePair<Vector3, Vector3>> Ammunationpos = new List<KeyValuePair<Vector3, Vector3>>();
            Ammunationpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(253.72, -51.22, 69.93), new Vector3(0, 0, 25)));
            Ammunationpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(-661.82, -933.24, 21.83), new Vector3(0, 0, 179)));
            //Ammunationpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(1304.06, -395.23, 36.69), new Vector3(0, 0, 72))); BAD POS
            Ammunationpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(842.01, -1035.64, 28.19), new Vector3(0, 0, 0)));
            Ammunationpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(809.73, -2159.34, 29.62), new Vector3(0, 0, 364)));
            Ammunationpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(23.73, -1105.47, 29.79), new Vector3(0, 0, 158)));

            foreach (KeyValuePair<Vector3, Vector3> keyValue in Ammunationpos)
            {
                Vector3 pos = keyValue.Key;
                Vector3 rot = keyValue.Value;

                pnj = API.createPed((PedHash)(int)-1643617475, pos, 1, 0);
                API.setEntityRotation(pnj, rot);
                API.setEntitySyncedData(pnj, "Interaction", "Ammunation");
                //API.playPedScenario(pnj, "PROP_HUMAN_BUM_SHOPPING_CART");
                blip = API.shared.createBlip(pos);
                blip.shortRange = true;
                API.shared.setBlipSprite(blip, 110);
                API.setBlipName(blip, "Ammu-Nation");
            }

            int[] doors = {
                // Ammu Nation Popular Street Shooting Range Door
                API.shared.exported.doormanager.registerDoor((int)452874391, new Vector3(827.5342, -2160.493, 29.76884)),

                // Ammu Nation Adam's Apple Boulevard Doors
                API.shared.exported.doormanager.registerDoor((int)-8873588, new Vector3(18.572, -1115.495, 29.94694)),
                API.shared.exported.doormanager.registerDoor((int)97297972, new Vector3(16.12787, -1114.606, 29.94694)),

                // Ammu Nation Vinewood Plaza Doors
                API.exported.doormanager.registerDoor((int)-8873588, new Vector3(243.8379, -46.52324, 70.09098)),
                API.exported.doormanager.registerDoor((int)97297972, new Vector3(244.7275 , -44.07911, 70.09098)),

            };

            foreach (int door in doors)
            {
                API.exported.doormanager.setDoorState(door, false, 0);
            }
        }

        public static void OpenMenuAmmunation(Client sender)
        {
            List<string> Actions = new List<string>();
            List<string> label = new List<string>();
            foreach (KeyValuePair<int, int> entry in products)
            {
                Item item = ItemByID(entry.Key);
                Actions.Add(item.Name);
                label.Add("Prix: " + entry.Value + "$");
            }
            API.shared.triggerClientEvent(sender, "bettermenuManager", 181, "Ammunation", "Sélectionner une arme:", false, Actions, label);
            API.shared.setEntityData(sender, "ProductsOfUsingShop", products);
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 181)
                {
                    var Products = API.getEntityData(sender, "ProductsOfUsingShop");
                    var item = ItemByID(Products[(int)arguments[1]].Key);
                    var price = Products[(int)arguments[1]].Value;
                    API.resetEntityData(sender, "ProductsOfUsingShop");
                    //InventoryHolder ih = API.getEntityData(sender, "InventoryHolder");
                    if (ih.CheckWeightInventory(item, 1))
                    {
                        if (Money.TakeMoney(sender, price))
                        {
                            ih.AddItemToInventory(item, 1);
                            UpdatePlayerMoney(sender);
                            API.triggerClientEvent(sender, "display_subtitle", "L'arme a été ajouté à votre inventaire", 3000);
                        }
                        else
                        {
                            API.triggerClientEvent(sender, "display_subtitle", "Désolé, vous n'avez pas assez d'argent", 3000);
                        }
                    }
                    else
                    {
                        API.triggerClientEvent(sender, "display_subtitle", "Désolé, vous n'avez pas assez de place dans votre inventaire.", 3000);
                    }
                }
            }
        }

        private void OnResourceStop()
        {
            API.deleteEntity(pnj);
            API.deleteEntity(blip);
        }
    }
}
