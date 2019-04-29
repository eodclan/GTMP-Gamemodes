using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;

public class TaxiCommands : Script
    {
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("ss");
        }

        [Command("taxi")]
        public void calltaxi(Client sender)
        {
            API.call("Taxi", "useTaxis", sender);
            API.sendPictureNotificationToPlayer(sender, "Es wird versucht ein Taxi für dich ausfindig zu machen.", "CHAR_TAXI", 0, 1, "Taxi Center", "Nachricht");
        }

        [Command("taxijob", Group = "dev", SensitiveInfo = false)]
        public void startjob(Client sender)
        {
                if ((bool)API.call("Taxi", "isincircle", (NetHandle)sender.handle))
                {

                    VehicleHash taxi = API.vehicleNameToModel("taxi");
                    Vehicle myvehicle = API.createVehicle(taxi, new Vector3(917.0233, -163.6854, 74.70861), new Vector3(0, 0, 143.9855), 0, 0, 0);
                    API.setPlayerIntoVehicle(sender, myvehicle, -1);
                    API.setEntityData(sender, "TAXI", true);
                    API.setVehicleNumberPlate(myvehicle, "TAXI" + GetTimestamp(DateTime.Now));
                    API.sendPictureNotificationToPlayer(sender, "Sie sind jetzt ein Taxifahrer, sobald Sie eine Benachrichtigung erhalten haben, geben Sie /accept ein, um den Job zu übernehmen.", "CHAR_TAXI", 0, 1, "Taxi Center", "Nachricht");
                }
                else { API.setEntityPosition(sender, new Vector3(917.0233, -163.6854, 74.70861)); }
        }

        [Command("accept")] 
        public void acceptthetask(Client sender)
        {
            if (API.getEntityData(sender.handle, "TAXI"))
            {
                double id = API.random();
                API.call("Taxi", "accepted", (Client)sender, id);
            }
            else
            {
                API.sendPictureNotificationToPlayer(sender, "Du bist in kein Job.", "CHAR_TAXI", 0, 1, "Taxi Center", "Nachricht");
            }
        }

        [Command("done")]
        public void finishthetask(Client sender)
        {
            API.setEntityData(sender, "TASK",1.623482);
            API.sendPictureNotificationToPlayer(sender, "Sie können jetzt den Job übernehmen.", "CHAR_TAXI", 0, 1, "Taxi Center", "Nachricht");
        }
}

