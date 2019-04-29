using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Linq;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Services.CharacterService;

namespace FactionLife.Server.FactionHandler
{
    public class FraUtil
    {
        
        public static void BroadcastToLSPD(string message) //LSPD Teamfunk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Police) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }
		
        public static void BroadcastToSheriff(string message) //FIB Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.FIB) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }		

        public static void BroadcastToFIB(string message) //FIB Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.FIB) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToSWAT(string message) //Army Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.SWAT) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToEMS(string message) //EMS Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.EMS) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToVagos(string message) //Vagos Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Vagos) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToGrove(string message) //Grove Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Grove) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToAztecas(string message) //Aztecas Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Aztecas) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToBloods(string message) //Bloods Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Bloods) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToYakuza(string message) //Yakuza Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Yakuza) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            });
        }

        public static void BroadcastToJVA(string message) //JVA Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.JVA) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            }));
        }

        public static void BroadcastToFahrschule(string message) //Fahrschule Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Fahrschule) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            }));
        }

        public static void BroadcastToLosSantosLocos(string message) //LosSantosLocos Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.LosSantosLocos) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            }));
        }

        public static void BroadcastToLostMC(string message) //LostMC Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.LostMC) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            }));
        }

        public static void BroadcastToCamorra(string message) //Camorra Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Camorra) && player.Character.OnDuty)
                    API.shared.sendChatMessageToPlayer(client, message);
            }));
        }

        public static void BroadcastToDep(string message) //Department Funk
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Police) || (player.Character.Faction == FactionType.FIB) || (player.Character.Faction == FactionType.SWAT) || (player.Character.Faction == FactionType.EMS  || (player.Character.Faction == FactionType.JVA)))
                    if (player.Character.OnDuty)
                        API.shared.sendChatMessageToPlayer(client, message);
            }));
        }



        public static void SetOnDuty(Client client, bool onDuty)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            player.Character.OnDuty = onDuty;

            CharacterService.UpdateCharacter(player.Character);
            
        }

    }
}