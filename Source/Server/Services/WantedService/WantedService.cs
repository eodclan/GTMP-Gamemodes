using System;
using System.Collections.Generic;
using System.Linq;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using GrandTheftMultiplayer.Server.Managers;
using FactionLife.Server.Services.FactionService;
using MySql.Data.MySqlClient;
using System.Data;

namespace FactionLife.Server.Services.WantedService
{
    class WantedService
        : Script
    {

        [Command("clearwanteds", Alias = "tc", Group = "Wanted Commands")]
        public void TicketClear(Client client, Client target)
        {
            Player player = client.getData("player");
            Player suspect = target.getData("player");


            if ((player.Character.Faction == FactionType.FIB) || (player.Character.Faction == FactionType.Police) || (player.Character.Faction == FactionType.Sheriff))
            {

                API.shared.sendPictureNotificationToPlayer(target, "Sie haben Ihr Ticket bezahlt! Ihre Wanteds: ~r~0~w~", "CHAR_CALL911", 0, 0, "~r~Staatsinformation", "Ihr Akten");

                FactionHandler.FraUtil.BroadcastToDep("~r~[Department] Beamter " + player.Character.FirstName + "_" + player.Character.LastName + " hat die Akten von " + suspect.Character.FirstName + "_" + suspect.Character.LastName + " gelöscht!");


                suspect.Character.Wanteds = 0;

                CharacterService.CharacterService.UpdateCharacter(suspect.Character);

            }
            CharacterService.CharacterService.UpdateCharacter(suspect.Character);
        }


        [Command("checkwanted", Alias = "cw", Group = "Wanted Commands", GreedyArg = true)]
        public void CheckWantedOfSuspect(Client client, Client suspect)
        {
            Player player = client.getData("player");
            Player ticketps = suspect.getData("player");
            if ((player.Character.Faction == FactionType.FIB) || (player.Character.Faction == FactionType.Police) || (player.Character.Faction == FactionType.Sheriff))
            {
                API.sendChatMessageToPlayer(client, "Die Person hat aktuell ~y~" + ticketps.Character.Wanteds + " Wanteds offen!");
            }
        }


        [Command("wanteds", Alias = "su", Group = "Wanted Commands", GreedyArg = true)]
        public void WantedSet(Client client, Client suspect, int Wanteds, string Grund)
        {

            Player player = client.getData("player");
            Player ticketps = suspect.getData("player");
            int idd = ticketps.Character.Id;

            if ((player.Character.Faction == FactionType.FIB) || (player.Character.Faction == FactionType.Police) || (player.Character.Faction == FactionType.Sheriff))
            {

                ticketps.Character.Wanteds = Wanteds;

                CharacterService.CharacterService.UpdateCharacter(ticketps.Character);


                API.shared.sendPictureNotificationToPlayer(suspect, "Sie haben einen neuen Gesuchtenstatus erhaten! Ihre Wanteds: ~r~" + Wanteds + "~w~", "CHAR_CALL911", 0, 0, "~r~Staatsinformation", "Ihr Akten");


                API.shared.sendChatMessageToPlayer(client, "~g~[Fraktion] Akten erfolgreich aktualisiert!");

                FactionHandler.FraUtil.BroadcastToDep("~r~[Department] " + "[LSPD Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~y~" + ticketps.Character.FirstName + "_" + ticketps.Character.LastName + " ~r~hat ein Verbrechen begangen!");

                API.sendChatMessageToPlayer(suspect, "Sie erhielten " + Wanteds + " Wanteds für " + Grund + "!");



            }
        }






        public void RemoveLicense(Client client, int LicID)
        {

            Player schuler = client.getData("player");

            License lice1 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 1);
            License lice2 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 2);
            License lice3 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 3);
            License lice4 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 4);
            License lice5 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 5);
            License lice6 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 6);
            License lice7 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 7);
            License lice8 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 8);
            License lice9 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 9);
            License lice10 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 10);


            switch (LicID)
            {
                case 1:
                    schuler.Character.Licenses.Remove(lice1);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 2:
                    schuler.Character.Licenses.Remove(lice2);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 3:
                    schuler.Character.Licenses.Remove(lice3);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 4:
                    schuler.Character.Licenses.Remove(lice4);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 5:
                    schuler.Character.Licenses.Remove(lice5);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 6:
                    schuler.Character.Licenses.Remove(lice6);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 7:
                    schuler.Character.Licenses.Remove(lice7);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 8:
                    schuler.Character.Licenses.Remove(lice8);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 9:
                    schuler.Character.Licenses.Remove(lice9);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;
                case 10:
                    schuler.Character.Licenses.Remove(lice10);
                    API.sendChatMessageToPlayer(client, "~r~Ihnen wurde eine Lizenz entzogen!");
                    CharacterService.CharacterService.UpdateCharacter(schuler.Character);
                    break;


            }

            CharacterService.CharacterService.UpdateCharacter(schuler.Character);


        }





        [Command("takelic", Alias = "tl", Group = "Police Commands")]
        public void TakeLicenseOfSuspect(Client client, Client suspect, int FührerscheinID)
        {

            Player player = client.getData("player");
            Player ticketps = suspect.getData("player");

            if ((player.Character.Faction == FactionType.FIB) || (player.Character.Faction == FactionType.Police))
            {

                RemoveLicense(suspect, FührerscheinID);




            }


        }






    }
}
