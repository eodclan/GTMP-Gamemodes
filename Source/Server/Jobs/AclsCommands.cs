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

    public class AclsCommands : Script
    {
        [Command("acls")]
        public void calltaxi(Client sender)
        {
            API.call("Acls", "useAcls", sender);
            //API.exported.NotifManager.InfoNotification(sender, "Das ACLS Center wurde über dein Ruf informiert!", 5000, "bottomRight");
            API.sendChatMessageToPlayer(sender, "~b~Das Acls Center North wurde über dein Ruf informiert!");
        }

        [Command("aclsjob")]
        public void startjob(Client sender)
        {
                if ((bool)API.call("Acls", "isincircle", (NetHandle)sender.handle))
                {

                    VehicleHash flatbed = API.vehicleNameToModel("flatbed");
                    Vehicle myvehicle = API.createVehicle(flatbed, new Vector3(119.697754, 6605.844, 31.953928), new Vector3(-0.277977735, 0.159938648, 148.550125), 0, 0);
                    API.setPlayerIntoVehicle(sender, myvehicle, -1);
                    API.setEntityData(sender, "ACLS", true);
                    API.shared.givePlayerWeapon(sender, (WeaponHash)1317494643, 1, true, true);
                    API.shared.givePlayerWeapon(sender, (WeaponHash)1951375401, 1, true, true);
                    API.sendChatMessageToPlayer(sender, "~g~Sie sind jetzt ein Fahrer vom Acls Center North, sobald Sie eine Benachrichtigung erhalten haben, geben Sie /aclsok ein, um den Job zu übernehmen.");
                }

                 else { API.setEntityPosition(sender, new Vector3(119.697754, 6605.844, 31.953928)); }
        }

        [Command("aclsok")] 
        public void acceptthetask(Client sender)
        {
            if (API.getEntityData(sender.handle, "ACLS"))
            {
                double id = API.random();
                API.call("Acls", "accepted", (Client)sender, id);
            }
            else {
                API.sendChatMessageToPlayer(sender, "~g~Du bist in kein Job.");
                //API.exported.NotifManager.ErrorNotification(sender, "Du bist in kein Job.", 5000, "bottomRight");
            }
        }

        [Command("acls")]
        public void finishthetask(Client sender)
        {
            API.setEntityData(sender, "TASK",1.623482);
            API.sendChatMessageToPlayer(sender, "~g~Sie können jetzt den Job übernehmen.");
            //API.exported.NotifManager.InfoNotification(sender, "Sie können jetzt den Job übernehmen.", 5000, "bottomRight");
        }

    }

