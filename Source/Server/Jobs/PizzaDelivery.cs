using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Threading;
using MySql.Data.MySqlClient;

namespace FactionLife.Server.Services.MoneyService
{
    public class PizzaDelivery : Script
    {
        private Vector3 PositionJobP = new Vector3(-842.6381, -1128.332, 7.024996);
        private Vector3 positionVehP = new Vector3(-868.866, -1124.682, 7.069582);

        public PizzaDelivery()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {
            Blip myBlip = API.createBlip(PositionJobP);
            API.setBlipShortRange(myBlip, true);
            API.setBlipSprite(myBlip, 468);
            API.setBlipName(myBlip, "Mexico Express");
            CylinderColShape m_colShape = API.createCylinderColShape(PositionJobP, 1.0f, 1.0f);
            Marker marker = API.createMarker(1, PositionJobP - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            m_colShape.onEntityEnterColShape += (shape, entity) =>
            {
                
                var players = API.getPlayerFromHandle(entity);
                if (!(bool)API.getEntityData(players, "P_Pizza")) return;
                if (players == null)
                {
                    return;
                }
                else
                {
                    List<String> Actions = new List<string>();
                    Actions.Add("Starten Sie die Mission");
                    Actions.Add("Ende des Dienstes");
                    Actions.Add("Verlasse das Menü");
                    API.triggerClientEvent(players, "bettermenuManager", 116, "Pizza Liefermenü", "", false, Actions);
                }

            };
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)args[0] == 116)
                {
                    if ((int)args[1] == 0)
                    {
                        PriseServicePizza(sender);
                        StartMission(sender);                        
                    } else
                    {
                        QuitterServicePizza(sender);
                    }                   
                }
            }
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("HHmm");
        }

        public void StartMission(Client player)
        {
            if(player.getData("LivreurPizza") == false || player.getData("LivreurPizza") == null)
            {
                  Vector3[] JobMarkers =  { new Vector3(-1040.424, -1475.227, 5.575875),new Vector3(-1097.737, -1673.224, 8.394047), new Vector3(-741.5182, -982.2773, 17.44151), new Vector3(-467.1438, -679.6216, 32.70993),
                  new Vector3(5.985398, -986.4579, 29.35736), new Vector3(441.4407, -981.1768, 30.6896),new Vector3(449.7232, -644.7805, 28.47805),new Vector3(-30.61526, -346.6658, 46.53485),new Vector3(17.97623, -13.58039, 70.11619),
                  new Vector3(-379.6579, 217.9398, 83.65906),new Vector3(-666.9662, -433.2206, 35.01893),new Vector3(-657.9357, -679.1603, 31.47902),new Vector3(-1366.584, 56.17993, 54.09818),new Vector3(-1369.744, -176.4956, 47.46559),
                  new Vector3(-1533.312, -275.5589, 49.73774),new Vector3(-1583.734, -266.0983, 48.27566),new Vector3(-1721.909, -482.7706, 41.61914),new Vector3(-1855.313, -1223.055, 13.01722),new Vector3(-1240.645, -785.8335, 18.64279),
                  new Vector3(-1254.626, -671.2836, 25.99363),new Vector3(-1066.207, -503.0737, 36.0687),new Vector3(-766.38, -917.3273, 21.29762)};
                  VehicleHash vehicleJob = API.vehicleNameToModel("Brioso");
                player.setData("LivreurPizza", true);
           
                Vehicle vehicle = API.createVehicle(vehicleJob, positionVehP, new Vector3(), 20, 20);
                API.setEntitySyncedData(vehicle, "VEHICLE_FUEL", 100);
                API.setEntitySyncedData(vehicle, "VEHICLE_FUEL_MAX", 100);
                API.setEntityData(vehicle, "weight", 0);
                API.setEntityData(vehicle, "weight_max", 0);
                string plate = "FL-" + GetTimestamp(DateTime.Now);
                API.setVehicleNumberPlate(vehicle, plate);
                vehicle.setSyncedData("LivreurPizza", true);
                //EntityManager.Add(vehicle);

                API.sendNotificationToPlayer(player, "Steigen Sie in ein Fahrzeug und gehen Sie zu verschiedenen Markierungen..");
                for (int i = 5; i > 0; i--)
                {
                    Random rnd = new Random();
                    int index = rnd.Next(0, JobMarkers.Length-1);
                    
                    if (i == 5)
                    {
                        API.sendNotificationToPlayer(player, "Sie haben "+ i +" Pizza zu liefern, Lieferpunkte sind auf roten Markierungen");
                    }
                    else
                    {
                        API.sendNotificationToPlayer(player, "Sie haben immer noch "+ i +" Pizza zu liefern");
                    }
                    MarkerManager(player, JobMarkers[index]);

                }
                FinMission(player);
            }
            else
            {
                API.sendNotificationToPlayer(player, "Sie haben bereits ein Fahrzeug");
            }
        }
        public void FinMission(Client player)
        {
            API.sendNotificationToPlayer(player,"Sie haben Ihre Lieferungen abgeschlossen, gehen Sie zurück zum Depot, um Ihr Gehalt zurückzuerhalten.");
            Vector3 position = new Vector3(-851.5518, -1086.633, 9.249024);


            API.shared.triggerClientEvent(player, "markerblip", position - new Vector3(0, 0, 1f));
            ColShape colshape;
            colshape = API.createCylinderColShape(position,10f, 1f);
            bool colision = false;
            colshape.onEntityEnterColShape += (shape, entity) =>
            {
                var players = API.shared.getPlayerFromHandle(entity);

                if (players == null)
                {
                    return;
                }
                else
                {
                    if (players.handle == player.handle)
                    {
                        if (player.isInVehicle)
                        {
                            Vehicle vehicle = player.vehicle;
                            if (vehicle.getSyncedData("LivreurPizza") == true)
                            {
                                API.sendNotificationToPlayer(player, "Sie haben Ihre Lieferungen abgeschlossen");
                                API.deleteEntity(API.getPlayerVehicle(player));
                                API.triggerClientEvent(player, "removemarkerblip");
                                Paye(player);
                            }
                            else
                            {
                                API.sendNotificationToPlayer(player, "Sie müssen in einem Fahrzeug Ihres Handels sein");
                            }
                        }
                        else
                        {
                            API.sendNotificationToPlayer(player, "Sie müssen in einem Fahrzeug Ihres Handels sein");
                        }
                    }
                }

            };
            while (colision == false)
            {
                Thread.Sleep(1000);
            }
            API.deleteColShape(colshape);
            Paye(player);
        }

        private static void PriseServicePizza(Client player)
        {
            API.shared.setPlayerClothes(player, 11, 26, 0); //Survet
            API.shared.setPlayerClothes(player, 3, 92, 0); // Bras
            API.shared.setPlayerClothes(player, 8, 15, 0); // Chemise
            API.shared.setPlayerClothes(player, 6, 0, 0); // Chaussure
            API.shared.setPlayerClothes(player, 4, 13, 0); // Pantalon
        }

        private static void QuitterServicePizza(Client player)
        {
            API.shared.setPlayerClothes(player, 4, player.getData("Pants"), 0);
            API.shared.setPlayerClothes(player, 8, player.getData("Chemise"), 0);
            API.shared.setPlayerClothes(player, 11, player.getData("Survet"), 0);
            API.shared.setPlayerClothes(player, 6, player.getData("Chaussures"), 0);
            API.shared.setPlayerClothes(player, 3, player.getData("Bras"), 0);   
        }
        public void Paye(Client player)
        {
            player.setData("LivreurPizza", false);
            int money = player.getSyncedData("Money");
            Random rnd = new Random();
            int paye = rnd.Next(750, 1500);
            money += paye;
            player.setSyncedData("Money", money);
            API.sendNotificationToPlayer(player, "Sie haben gerade "+ paye +" $ erhalten.");
            //MoneyService.AddPlayerCash(player);
        }
        public bool MarkerManager(Client player,Vector3 position)
        {
            API.shared.triggerClientEvent(player, "markerblip", position - new Vector3(0, 0, 1f));
            ColShape colshape;
            colshape = API.createCylinderColShape(position, 1f, 1f);
            bool colision = false;
            colshape.onEntityEnterColShape += (shape, entity) =>
            {
                var players = API.shared.getPlayerFromHandle(entity);

                if (players == null)
                {
                    return;
                }
                else
                {
                    if (players.handle == player.handle)
                    {
                        API.triggerClientEvent(player, "removemarkerblip");
                        colision = true;
                        
                    }
                }
            };
            while (colision == false)
            {
                Thread.Sleep(1000);
            }
            API.deleteColShape(colshape);
            return false;
        }
    }
}
