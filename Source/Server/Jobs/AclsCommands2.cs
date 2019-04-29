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

    public class AclsCommands2 : Script
    {
        [Command("aclss")]
        public void calltaxi(Client sender)
        {
            API.call("Acls2", "useAcls2", sender);
            API.sendChatMessageToPlayer(sender, "~b~Das Acls Center South wurde über dein Ruf informiert!");
        }

        [Command("aclsjobs")]
        public void startjob(Client sender)
        {
                if ((bool)API.call("Acls2", "isincircle", (NetHandle)sender.handle))
                {

                    VehicleHash flatbed = API.vehicleNameToModel("flatbed");
                    Vehicle myvehicle = API.createVehicle(flatbed, new Vector3(-1146.86194, -1977.9021, 12.7394447), new Vector3(-0.277977735, 0.159938648, 148.550125), 0, 0);
                    API.setPlayerIntoVehicle(sender, myvehicle, -1);
                    API.setEntityData(sender, "ACLS2", true);
                    API.sendChatMessageToPlayer(sender, "~g~Sie sind jetzt ein Fahrer vom Acls Center South, sobald Sie eine Benachrichtigung erhalten haben, geben Sie /aclsja ein, um den Job zu übernehmen.");
                }

                 else { API.setEntityPosition(sender, new Vector3(-1146.86194, -1977.9021, 12.7394447)); }
        }

        [Command("aclsja")] 
        public void acceptthetask(Client sender)
        {
            if (API.getEntityData(sender.handle, "ACLS2"))
            {
                double id = API.random();
                API.call("Acls2", "accepted", (Client)sender, id);
            }
            else {
                API.sendChatMessageToPlayer(sender, "~g~Du bist in kein Job.");
                //API.exported.NotifManager.ErrorNotification(sender, "Du bist in kein Job.", 5000, "bottomRight");
            }
        }

        [Command("acls2")]
        public void finishthetask(Client sender)
        {
            API.setEntityData(sender, "TASK",1.623482);
            API.sendChatMessageToPlayer(sender, "~g~Sie können jetzt den Job übernehmen.");
            //API.exported.NotifManager.InfoNotification(sender, "Sie können jetzt den Job übernehmen.", 5000, "bottomRight");
        }

    }

