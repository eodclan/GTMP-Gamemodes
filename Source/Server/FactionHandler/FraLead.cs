using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using FactionLife.Server.Model;
using FactionLife.Server.FactionHandler;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Services.CharacterService;
using Newtonsoft.Json;

namespace FactionLife.Server.Services.FactionService
{
    public class FraLead : Script
    {

        //AdminCommand to Rankups

        [Command("rankupf", Group = "Fra Commands", GreedyArg = true)]
        public void RankupAdminTeam(Client client, Client player, string rank)
        {
            Player admin = client.getData("player");

            if (admin.Account.AdminLvl != 8) { return; }

            if (admin.Account.AdminLvl == 8)
            {
                Player person = player.getData("player");
                int newrank = Convert.ToInt32(rank);

                if ((newrank < 0) || (newrank > 12)) { return; }

                FactionService.SetPlayerRank(player, newrank);

                API.sendPictureNotificationToPlayer(player, "Sie erhielten administrativ den Rang ~g~" + rank + "~w~!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminRang");

                API.sendPictureNotificationToPlayer(client, "Die Person ~g~" + API.getPlayerName(player) + " ~w~erhielt den Rang ~g~" + rank + "~w~!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminRang");
            }

        }



        [Command("invitef", Group = "Fra Commands", GreedyArg = true)]
        public void InviteTeamAdmin(Client client, Client player, string faction)
        {
            Player admin = client.getData("player");
            Player member = player.getData("player");
            if (admin.Account.AdminLvl != 8) { return; }

            if (member.Character.FactionLocked == 1) { API.sendChatMessageToPlayer(client, "~r~Die Person hat eine aktive Fraktionssperre!"); return; }

            if (admin.Account.AdminLvl == 8)
            {
                Player person = player.getData("player");
                string namefraktion = null;

                if (faction == "0")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Citizen);
                }
                else if (faction == "1")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Police);
                    namefraktion = "LSPD";
                }
                else if (faction == "2")
                {
                    FactionService.SetPlayerFaction(player, FactionType.EMS);
                    namefraktion = "LSMC";
                }
                else if (faction == "3")
                {
                    FactionService.SetPlayerFaction(player, FactionType.State);
                    namefraktion = "undefinied";
                }
                else if (faction == "4")
                {
                    FactionService.SetPlayerFaction(player, FactionType.FIB);
                    FraUtil.SetOnDuty(player, true);                            //FINAL EDIT WAITING... ?
                    namefraktion = "FIB";
                }
                else if (faction == "5")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Bloods);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "Bloods";
                }
                else if (faction == "6")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Aztecas);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "Aztecas";
                }
                else if (faction == "7")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Vagos);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "Vagos";
                }
                else if (faction == "8")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Grove);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "Grove";
                }
                else if (faction == "9")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Yakuza);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "Yakuza";
                }
                else if (faction == "10")
                {
                    FactionService.SetPlayerFaction(player, FactionType.LostMC);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "LostMC";
                }
                else if (faction == "11")
                {
                    FactionService.SetPlayerFaction(player, FactionType.SWAT);
                    FraUtil.SetOnDuty(player, true);                            //FINAL EDIT WAITING...
                    namefraktion = "SWAT";
                }
                else if (faction == "12")
                {
                    FactionService.SetPlayerFaction(player, FactionType.JVA);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "JVA";
                }
                else if (faction == "13")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Fahrschule);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "Fahrschule";
                }
                else if (faction == "14")
                {
                    FactionService.SetPlayerFaction(player, FactionType.LosSantosLocos);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "LosSantosLocos";
                }

                else if (faction == "15")
                {
                    FactionService.SetPlayerFaction(player, FactionType.Camorra);
                    FraUtil.SetOnDuty(player, true);
                    namefraktion = "Camorra";
                }

                API.sendPictureNotificationToPlayer(player, "Sie wurden administrativ in die Fraktion" + namefraktion + " hinzugefügt!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminInvite");

                API.sendPictureNotificationToPlayer(client, "Die Person ~g~ " + API.getPlayerName(player) + " ~w~wurde in die Fraktion ~g~ " + namefraktion + " ~g~hinzugefügt~w~!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminInvite");
            }

        }


        [Command("deletef", Group = "Fra Commands", GreedyArg = true)]
        public void DeleteAdminFaction(Client client, Client player)
        {
            Player admin = client.getData("player");

            if (admin.Account.AdminLvl != 8) { return; }

            if (admin.Account.AdminLvl == 8)
            {
                Player person = player.getData("player");

                FactionService.SetPlayerFaction(player, FactionType.Citizen);
                FraUtil.SetOnDuty(player, false);

                API.sendPictureNotificationToPlayer(player, "Sie wurden administrativ aus Ihrer Fraktion ~r~entfernt!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminKick");

                API.sendPictureNotificationToPlayer(client, "Die Person ~g~ " + API.getPlayerName(player) + " ~w~wurde aus der Fraktion ~r~entfernt!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminKick");
            }

        }

        [Command("factionlock", Group = "Fra Commands", GreedyArg = true)]
        public void SetPlayerFactionLocked(Client client, Client player)
        {
            Player admin = client.getData("player");

            if (admin.Account.AdminLvl != 8) { return; }
            if (admin.Account.AdminLvl == 8)
            {
                Player person = player.getData("player");

                person.Character.FactionLocked = 1;
                CharacterService.CharacterService.UpdateCharacter(person.Character);
                API.sendPictureNotificationToPlayer(player, "Sie erhielten eine administrative Fraktionssperre!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminKick");

                API.sendPictureNotificationToPlayer(client, "Die Person ~g~ " + API.getPlayerName(player) + " ~w~erhielt eine Fraktionssperre!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminKick");
            }
        }

        [Command("factionunlock", Group = "Fra Commands", GreedyArg = true)]
        public void SetPlayerFactionUnlocked(Client client, Client player)
        {
            Player admin = client.getData("player");

            if (admin.Account.AdminLvl != 8) { return; }
            if (admin.Account.AdminLvl == 8)
            {
                Player person = player.getData("player");

                person.Character.FactionLocked = 0;
                CharacterService.CharacterService.UpdateCharacter(person.Character);
                API.sendPictureNotificationToPlayer(player, "Sie erhielten eine administrative Fraktionssperre!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminKick");

                API.sendPictureNotificationToPlayer(client, "Die Person ~g~ " + API.getPlayerName(player) + " ~w~erhielt eine Fraktionssperre!", "CHAR_EPSILON", 0, 7, "Fraktion", "AdminKick");
            }
        }


        [Command("removeallfaction", Group = "Fra Commands", GreedyArg = true)]
        public void RemoveAllPlayersOfFaction(Client client, string faction)
        {
            Player admin = client.getData("player");
            if (admin.Account.AdminLvl != 8) { return; }

            if (faction == "1")
            {
                FactionService.RemoveAllOfFaction(client, 1);
            }
            else if (faction == "2")
            {
                FactionService.RemoveAllOfFaction(client, 2);
            }
            else if (faction == "3")
            {
                FactionService.RemoveAllOfFaction(client, 3);
            }
            else if (faction == "4")
            {
                FactionService.RemoveAllOfFaction(client, 4);
            }
            else if (faction == "5")
            {
                FactionService.RemoveAllOfFaction(client, 5);
            }
            else if (faction == "6")
            {
                FactionService.RemoveAllOfFaction(client, 6);
            }
            else if (faction == "7")
            {
                FactionService.RemoveAllOfFaction(client, 7);
            }
            else if (faction == "8")
            {
                FactionService.RemoveAllOfFaction(client, 8);
            }
            else if (faction == "9")
            {
                FactionService.RemoveAllOfFaction(client, 9);
            }
            else if (faction == "10")
            {
                FactionService.RemoveAllOfFaction(client, 10);
            }
            else if (faction == "11")
            {
                FactionService.RemoveAllOfFaction(client, 11);
            }
            else if (faction == "12")
            {
                FactionService.RemoveAllOfFaction(client, 12);
            }
            else if (faction == "13")
            {
                FactionService.RemoveAllOfFaction(client, 13);
            }
            else if (faction == "14")
            {
                FactionService.RemoveAllOfFaction(client, 14);
            }

            else if (faction == "15")
            {
                FactionService.RemoveAllOfFaction(client, 15);
            }

        }

        //_____________________________________________________________________________________________________________________________________________________________________

        //Faction-Leader Commands to Faction and Rankups
        //...




        [Command("invite", Group = "Fra Commands", GreedyArg = true)]
        public void InviteLeader(Client client, Client player)
        {
            Player leader = client.getData("player");
            Player member = player.getData("player");

            string namefraktion = null;

            if (leader.Character.Faction == FactionType.Citizen) { return; }

            if (leader.Character.FactionRank != 12) { return; }

            if (member.Character.Faction != FactionType.Citizen) { return; }

            if (member.Character.FactionLocked == 1) { API.sendChatMessageToPlayer(client, "~r~ Die Person besitzt eine aktive Fraktionssperre!"); return; }
            if (member.Character.Faction == leader.Character.Faction) { API.sendChatMessageToPlayer(client, "Die Person ist bereits Mitglied der Fraktion!"); return; }


            if (leader.Character.Faction == FactionType.Police)
            {
                FactionService.SetPlayerFaction(player, FactionType.Police);
                namefraktion = "LSPD";
            }
            else if (leader.Character.Faction == FactionType.EMS)
            {
                FactionService.SetPlayerFaction(player, FactionType.EMS);
                namefraktion = "LSMC";
            }
            else if (leader.Character.Faction == FactionType.State)
            {
                FactionService.SetPlayerFaction(player, FactionType.State);
                namefraktion = "undefinied";
            }
            else if (leader.Character.Faction == FactionType.FIB)
            {
                FactionService.SetPlayerFaction(player, FactionType.FIB);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "FIB";
            }
            else if (leader.Character.Faction == FactionType.Bloods)
            {
                FactionService.SetPlayerFaction(player, FactionType.Bloods);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "Bloods";
            }
            else if (leader.Character.Faction == FactionType.Aztecas)
            {
                FactionService.SetPlayerFaction(player, FactionType.Aztecas);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "Aztecas";
            }
            else if (leader.Character.Faction == FactionType.Vagos)
            {
                FactionService.SetPlayerFaction(player, FactionType.Vagos);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "Vagos";
            }
            else if (leader.Character.Faction == FactionType.Grove)
            {
                FactionService.SetPlayerFaction(player, FactionType.Grove);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "Grove";
            }
            else if (leader.Character.Faction == FactionType.Yakuza)
            {
                FactionService.SetPlayerFaction(player, FactionType.Yakuza);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "Yakuza";
            }
            else if (leader.Character.Faction == FactionType.SWAT)
            {
                FactionService.SetPlayerFaction(player, FactionType.SWAT);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "SWAT";
            }

            else if (leader.Character.Faction == FactionType.JVA)
            {
                FactionService.SetPlayerFaction(player, FactionType.JVA);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "JVA";
            }
            else if (leader.Character.Faction == FactionType.Fahrschule)
            {
                FactionService.SetPlayerFaction(player, FactionType.Fahrschule);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "Fahrschule";
            }

            else if (leader.Character.Faction == FactionType.LosSantosLocos)
            {
                FactionService.SetPlayerFaction(player, FactionType.LosSantosLocos);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "LosSantosLocos";
            }

            else if (leader.Character.Faction == FactionType.LostMC)
            {
                FactionService.SetPlayerFaction(player, FactionType.LostMC);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "LostMC";
            }

            else if (leader.Character.Faction == FactionType.Camorra)
            {
                FactionService.SetPlayerFaction(player, FactionType.Camorra);
                FraUtil.SetOnDuty(player, true);
                namefraktion = "Camorra";
            }

            API.sendPictureNotificationToPlayer(client, "Sie wurden in die Fraktion " + namefraktion + " hinzugefügt!", "CHAR_EPSILON", 0, 7, "Fraktion", "Invite");

            API.sendPictureNotificationToPlayer(player, "Die Person ~g~ " + API.getPlayerName(player) + " ~w~wurde in die Fraktion ~g~ " + namefraktion + " ~g~hinzugefügt~w~!", "CHAR_EPSILON", 0, 7, "Fraktion", "Invite");
        }



        [Command("remove", Group = "Fra Commands", GreedyArg = true)]
        public void RemoveLeader(Client client, Client player)
        {
            Player leader = client.getData("player");
            Player person = player.getData("player");

            if (leader.Character.Faction == FactionType.Citizen) { return; }
            if (leader.Character.Faction != person.Character.Faction) { return; }
            if (leader.Character.FactionRank != 12) { return; }

            if ((leader.Character.FactionRank == 12) && (leader.Character.Faction != FactionType.Citizen) && (leader.Character.Faction == person.Character.Faction))
            {
                FactionService.SetPlayerFaction(player, FactionType.Citizen);
                FactionService.SetPlayerRank(player, 0);
                FraUtil.SetOnDuty(player, false);

                CharacterService.CharacterService.UpdateCharacter(person.Character);
                API.sendPictureNotificationToPlayer(player, "Sie wurden aus Ihrer aktuellen Fraltion entfernt!", "CHAR_EPSILON", 0, 7, "Fraktion", "Invite");
                API.sendPictureNotificationToPlayer(client, "Die Person ~g~ " + API.getPlayerName(player) + " ~w~wurde aus der Fraktion ~r~entfernt~w~!", "CHAR_EPSILON", 0, 7, "Fraktion", "Invite");
            }
            

        }


        [Command("rankup", Group = "Fra Commands", GreedyArg = true)]
        public void RankupLeader(Client client, Client player, string rank)
        {
            Player leader = client.getData("player");
            Player person = player.getData("player");

            if (leader.Character.Faction == FactionType.Citizen) { return; }
            if (leader.Character.Faction != person.Character.Faction) { return; }
            if (leader.Character.FactionRank != 12) { return; }

            if ((leader.Character.Faction != FactionType.Citizen) && (leader.Character.Faction == person.Character.Faction) && (leader.Character.FactionRank == 12))
            {
                int newrank = Convert.ToInt32(rank);

                if ((newrank < 0) || (newrank > 12)) { return; }

                FactionService.SetPlayerRank(player, newrank);
                API.sendPictureNotificationToPlayer(player, "Sie haben einen neuen Rang erhalten. Neuer Rang: ~g~ " + rank + "~w~.", "CHAR_EPSILON", 0, 7, "Fraktion", "Ranginformation");
                API.sendPictureNotificationToPlayer(client, "Die Person ~g~ " + API.getPlayerName(player) + " ~w~erhielt den Rang ~g~ " + rank + "~w~.", "CHAR_EPSILON", 0, 7, "Fraktion", "Ranginformation");
            }

        }


    }
    
}