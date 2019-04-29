using FactionLife.Server.Base;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Data;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Model;
using FactionLife.Server.Services.MoneyService;

namespace FactionLife.Server.Services.VehicleService
{
    class Carwash : Script
    {
        private double toPay = 500;
        public ColShape misStartColshape;

        private List<Vector3> wash = new List<Vector3>()
        {
            new Vector3(20.90365f, -1391.978f, 28.9f),
            new Vector3(-699.8218f, -934.9742f, 18.5f)
        };

        public Carwash()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            foreach (Vector3 Carwash in wash)
            {
                ColShape carwash = API.createSphereColShape(new Vector3(Carwash.X, Carwash.Y, Carwash.Z), 5f);
                API.createMarker(75, new Vector3(Carwash.X, Carwash.Y, Carwash.Z), new Vector3(), new Vector3(), new Vector3(5f, 5f, 1f), 155, 0, 0, 155);
                //API.createMarker(75, new Vector3(Carwash.X, Carwash.Y, Carwash.Z - 1), new Vector3(), new Vector3(5f, 5f, 1f), 1f, new Color(0, 0, 153));
                Blip misBlip = API.createBlip(new Vector3(Carwash.X, Carwash.Y, Carwash.Z)); // blip to start job
                API.setBlipSprite(misBlip, 225);
                API.setBlipName(misBlip, "Carwash");
                API.setBlipColor(misBlip, 38);
                API.setBlipScale(misBlip, 0.8f);
                carwash.onEntityEnterColShape += OnEntityEnterColShapeHandler;
                carwash.onEntityExitColShape += OnEntityExitColShapeHandler;
                misStartColshape = API.createCylinderColShape(new Vector3(Carwash.X, Carwash.Y, Carwash.Z), 5f, 3f); // colshap for mission start point

                misStartColshape.onEntityEnterColShape += (shape, entity) =>
                {
                    Client player;

                    if ((player = API.getPlayerFromHandle(entity)) != null)
                    {
                        API.shared.sendPictureNotificationToPlayer(player, "~g~Drücke die Taste J, um dein Fahrzeug zu waschen.", "CHAR_STRIPPER_JULIET", 1, 1, "Juliet", "Wasch Nixe");
                    }
                };
            }
        }

        private void OnClientEventTrigger(Client player, string eventname, params object[] arguments)
        {
                switch (eventname)
                {
                    case "waschen":
                        player.setSyncedData("IsInCarwash", false);
                        API.sendPictureNotificationToPlayer(player, "~g~Glückwunsch, dein Fahrzeug sieht aus wie neu!", "CHAR_STRIPPER_JULIET", 1, 1, "Juliet", "Wasch Nixe");
                        API.repairVehicle(API.getPlayerVehicle(player));//entfernt den groben schmutz
                        MoneyService.MoneyService.RemovePlayerBank(player, toPay);
                        break;
                }
        }

        private void OnEntityEnterColShapeHandler(ColShape shape, NetHandle entity)
        {
            Client player = API.getPlayerFromHandle(entity);
            if (player == null || shape == null || !player.isInVehicle || player.vehicleSeat != -1)
                return;

            player.setSyncedData("IsInCarwash", true);
            if (player.getData("bank") < toPay)
            {
                API.sendPictureNotificationToPlayer(player, "Eine Wäsche kostet ~r~" + toPay.ToString() + "$~w~." + "\nAber dafür reicht dein Geld nicht.", "CHAR_STRIPPER_JULIET", 1, 1, "Juliet", "Wasch Nixe");
            }
            else
            {
                API.sendPictureNotificationToPlayer(player, "Eine Wäsche kostet ~r~" + toPay.ToString() + "$~w~." + "\nDu bekommst auch 'nen gut riechenden Duftbaum dazu.", "CHAR_STRIPPER_JULIET", 1, 1, "Juliet", "Wasch Nixe");
            }
        }

        private void OnEntityExitColShapeHandler(ColShape shape, NetHandle entity)
        {
            Client player = API.getPlayerFromHandle(entity);
            if (player == null || shape == null)
                return;

            player.setSyncedData("IsInCarwash", false);
        }
    }
}