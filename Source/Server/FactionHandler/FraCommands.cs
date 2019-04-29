using System;
using System.Collections.Generic;
using System.Linq;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Model;
using FactionLife.Server.Services.VehicleService;

namespace FactionLife.Server.FactionHandler.FactionCom
{
    public class FactionCom : Script
    {

        [Command("frak", Alias = "r,t", Group = "Fra Commands", GreedyArg = true)]
        public void BroadcastToFraTeam(Client client, string text)
        {
            Player player = client.getData("player");

            if (player.Character.Faction == FactionType.Police) //LSPD
            {
                FraUtil.BroadcastToLSPD("~b~[LSPD] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.FIB)  //FIB
            {
                FraUtil.BroadcastToFIB("~b~[FIB] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.SWAT)  //SWAT
            {
                FraUtil.BroadcastToSWAT("~b~[SWAT] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.EMS)  //Medic
            {
                FraUtil.BroadcastToEMS("~b~[Medic] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.Vagos)  //Vagos
            {
                FraUtil.BroadcastToVagos("~y~[Vagos] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.Grove)  //Grove
            {
                FraUtil.BroadcastToGrove("~g~[Grove] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.Aztecas)  //Aztecas
            {
                FraUtil.BroadcastToAztecas("~b~[Aztecas] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.Bloods)  //Bloods
            {
                FraUtil.BroadcastToBloods("~r~[Bloods] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.Yakuza)  //Yakuza
            {
                FraUtil.BroadcastToYakuza("~b~[Yakuza] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.JVA)  //JVA
            {
                FraUtil.BroadcastToJVA("~b~[JVA] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.Fahrschule)  //Fahrschule
            {
                FraUtil.BroadcastToFahrschule("~b~[Fahrschule] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.LosSantosLocos)  //LosSantosLocos
            {
                FraUtil.BroadcastToLosSantosLocos("~y~[LosSantosLocos] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.LostMC)  //Ballas
            {
                FraUtil.BroadcastToLostMC("~q~[Ballas] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.Camorra)  //Camorra
            {
                FraUtil.BroadcastToCamorra("~c~[Camorra] " + "[Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

        }


        [Command("department", Alias = "d", Group = "Fra Commands", GreedyArg = true)]
        public void BroadcastToDep(Client client, string text)
        {
            Player player = client.getData("player");

            if (player.Character.OnDuty == false) { return; }

            if (player.Character.Faction == FactionType.Police) //LSPD
            {
                FraUtil.BroadcastToDep("~r~[Department] " + "[LSPD Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.FIB) //FIB
            {
                FraUtil.BroadcastToDep("~r~[Department] " + "[FIB Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.EMS) //EMS
            {
                FraUtil.BroadcastToDep("~r~[Department] " + "[LSMC Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.SWAT) //SWAT
            {
                FraUtil.BroadcastToDep("~r~[Department] " + "[SWAT Rang " + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }

            if (player.Character.Faction == FactionType.JVA) //JVA
            {
                FraUtil.BroadcastToDep("~r~[Department] " + "[JVA Rang" + player.Character.FactionRank + "] " + player.Character.FirstName + "_" + player.Character.LastName + ": ~w~" + text);
            }
        }
    }
}