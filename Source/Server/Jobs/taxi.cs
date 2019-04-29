using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using System;

namespace FactionLife.Server.Services.FactionService
{
    public class Taxi : Script
    {
        public Taxi()
        {

            API.onResourceStart += onResourceStart;
        }

        public ColShape Infotaxi;

        public void onResourceStart()
        {
            API.createMarker(1, new Vector3(895.6638, -179.3419, 73.70026), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
            Blip Taxi = API.createBlip(new Vector3(895.6638, -179.3419, 74.70034));
            API.setBlipSprite(Taxi, 198);
            API.setBlipColor(Taxi, 5);
            API.setBlipName(Taxi, "Taxi Center");
            Infotaxi = API.createCylinderColShape(new Vector3(895.6638, -179.3419, 73.70026), 2f, 3f);

            Infotaxi.onEntityEnterColShape += (shape, entity) =>
            {
                Client player;
                if ((player = API.getPlayerFromHandle(entity)) != null)
                {
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Geben Sie /taxijob ein, um den Job zu starten", "CHAR_TAXI", 0, 1, "Taxi Center", "Job annehmen");
                }


            };

        }
        Client sen;
        double senderxcoords, senderycoords;

        public void useTaxis(Client sender)
        {
            sen = sender;
            senderxcoords = API.getEntityPosition(sender.handle).X;
            senderycoords = API.getEntityPosition(sender.handle).Y;
            List<Client> taxiPlayers = new List<Client>();
            foreach (var driver in API.getAllPlayers())
            {
                if (API.getEntityData(driver, "TAXI")!=null && API.getEntityData(driver, "TASK") != 1.623482)
                {
                    API.sendPictureNotificationToPlayer(driver, sender.name + " fragt nach einem Taxi, möchtest du es nehmen?", "CHAR_TAXI", 0, 1, "Taxi Center", "Nachricht");

                }
            }
        }
        static int i = 0;

        public void accepted(Client driver, double d)
        {
            foreach (var driver2 in API.getAllPlayers())
            {
                if (API.getEntityData(driver2, "TASK") == d)
                {
                    API.sendPictureNotificationToPlayer(driver2, "Diese Aufgabe wurde bereits erledigt", "CHAR_TAXI", 0, 1, "Taxi Center", "Nachricht");
                    i = 1;
                }

            }

            if (i == 0)
            {
                API.sendPictureNotificationToPlayer(driver, "Sie haben die Aufgabe angenommen, für den Client wurde ein Wegpunkt festgelegt", "CHAR_TAXI", 0, 1, "Taxi Center", "Nachricht");
                API.triggerClientEvent(driver, "markonmap", senderxcoords, senderycoords);
                API.setEntityData(driver, "TASK", d);
                API.sendPictureNotificationToPlayer(sen, driver.name + " kommt, um dich abzuholen, sei bitte geduldig und bleib bei den Ort ", "CHAR_TAXI", 0, 1, "Taxi Center.", "Nachricht");

            }

        } 
        public bool isincircle(NetHandle lit)
        {
            return Infotaxi.containsEntity(lit);

        }

    } 

}