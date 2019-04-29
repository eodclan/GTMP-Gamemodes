using GrandTheftMultiplayer.Server.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.IO;
using FactionLife.Server.Services.MoneyService;
using System.Xml.Linq;
using System.Timers;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Services.AdminService;
using FactionLife.Server.Services.FactionService;

namespace FactionLife.Server.Services.PayDay
{
    public class TimeCheck : Script
    {
        
        public TimeCheck()
        {
            API.onResourceStart += OnResourceStartHandler;
            API.onResourceStop += OnResourceStopHandler;
        }

        public Timer TimeCheckTimer;
        public Timer PayDayTimerTimer;

        public void OnResourceStartHandler()
        {
            TimeCheckTimer = API.startTimer(60000, false, () =>
            {
               LevelExecute();
                
            });

            PayDayTimerTimer = API.startTimer(60000, false, () =>
            {
                PayDayExe();
            });
        }

        public void OnResourceStopHandler()
        {
            API.stopTimer(TimeCheckTimer);
        }

        public static void LevelExecute()
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");

                Character character = player.Character;

                if (character != null)
                {
                    player.Character.TimeOnServer += 1;
                    if (player.Character.TimeOnServer == 60)
                    {
                        API.shared.sendPictureNotificationToPlayer(client, "Sie erhielten ein höheres Level!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Level");
                        player.Character.Level += 1;
                        player.Character.TimeOnServer = 0;
                    }
                    CharacterService.CharacterService.UpdateCharacter(player.Character);
                }
            }));

            
        }

        public static void PayDayExe()
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");

                Character character = player.Character;

                if (character != null)
                {
                    player.Character.TimeSincePayday += 1;
                    if (player.Character.TimeSincePayday == 60)
                    {
                        PayDayExecute();
                        player.Character.TimeSincePayday = 0;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    CharacterService.CharacterService.UpdateCharacter(player.Character);
                }
            }));


        }

        public static void PayDayExecute()
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach((Action<Client>)(client =>
            {
                Player player = client.getData("player");

                /// Starts Fraktion
                if (player.Character.Faction == FactionType.Police) { PaydayPolice(client); }

                if (player.Character.Faction == FactionType.EMS) { PaydayEMS(client); }

                if (player.Character.Faction == FactionType.FIB) { PaydayFIB(client); }

                if (player.Character.Faction == FactionType.Army) { PaydayArmy(client); }

                if (player.Character.Faction == FactionType.ACLS) { PaydayACLS(client); }

                if (player.Character.Faction == FactionType.Sheriff) { PaydayPolice(client); }

                if (player.Character.Faction == FactionType.Citizen) { PaydayCitizen(client); }

                ///Gangs Mafien

            }));
        }

        public static void PaydayACLS(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 1)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 1650);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~1650$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 2)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2650);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2650$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 3)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 3200);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~3200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 4)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~4400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 4400);
            }

            if (player.Character.FactionRank == 5)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~5200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 5200);
            }

            if (player.Character.FactionRank == 6)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~6400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 6400);
            }

            if (player.Character.FactionRank == 7)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~7200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 7200);
            }

            if (player.Character.FactionRank == 8)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9000);
            }

            if (player.Character.FactionRank == 9)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9500$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9500);
            }

            if (player.Character.FactionRank == 10)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~10400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 10400);
            }

            if (player.Character.FactionRank == 11)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~12000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 12000);
            }

            if (player.Character.FactionRank == 12)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~16000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 16000);
            }

        }

        public static void PaydayPolice(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 1)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2400);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 2)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2650);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2650$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 3)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 3200);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~3200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 4)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~4400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 4400);
            }

            if (player.Character.FactionRank == 5)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~5200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 5200);
            }

            if (player.Character.FactionRank == 6)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~6400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 6400);
            }

            if (player.Character.FactionRank == 7)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~7200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 7200);
            }

            if (player.Character.FactionRank == 8)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9000);
            }

            if (player.Character.FactionRank == 9)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9500$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9500);
            }

            if (player.Character.FactionRank == 10)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~10400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 10400);
            }

            if (player.Character.FactionRank == 11)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~12000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 12000);
            }

            if (player.Character.FactionRank == 12)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~16500$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 16500);
            }

        }

        public static void PaydayEMS(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 1)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 1900);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~1900$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 2)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2650);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2650$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 3)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 3200);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~3200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 4)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~4400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 4400);
            }

            if (player.Character.FactionRank == 5)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~5200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 5200);
            }

            if (player.Character.FactionRank == 6)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~6400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 6400);
            }

            if (player.Character.FactionRank == 7)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~7200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 7200);
            }

            if (player.Character.FactionRank == 8)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9000);
            }

            if (player.Character.FactionRank == 9)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9500$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9500);
            }

            if (player.Character.FactionRank == 10)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~10400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 10400);
            }

            if (player.Character.FactionRank == 11)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~12000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 12000);
            }

            if (player.Character.FactionRank == 12)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~14000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 14000);
            }


        }

        public static void PaydayFIB(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 1)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2400);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 2)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2650);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2650$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 3)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 3200);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~3200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 4)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~4400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 4400);
            }

            if (player.Character.FactionRank == 5)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~5200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 5200);
            }

            if (player.Character.FactionRank == 6)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~6400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 6400);
            }

            if (player.Character.FactionRank == 7)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~7200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 7200);
            }

            if (player.Character.FactionRank == 8)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9000);
            }

            if (player.Character.FactionRank == 9)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9500$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9500);
            }

            if (player.Character.FactionRank == 10)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~10400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 10400);
            }

            if (player.Character.FactionRank == 11)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~12000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 12000);
            }

            if (player.Character.FactionRank == 12)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~17000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 17000);
            }



        }


        public static void PaydayJVA(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 1)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2000);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 2)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2650);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2650$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 3)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 3200);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~3200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 4)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~4400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 4400);
            }

            if (player.Character.FactionRank == 5)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~5200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 5200);
            }

            if (player.Character.FactionRank == 6)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~6400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 6400);
            }

            if (player.Character.FactionRank == 7)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~7200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 7200);
            }

            if (player.Character.FactionRank == 8)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9000);
            }

            if (player.Character.FactionRank == 9)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9500$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9500);
            }

            if (player.Character.FactionRank == 10)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~10400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 10400);
            }

            if (player.Character.FactionRank == 11)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~12000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 12000);
            }

            if (player.Character.FactionRank == 12)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~16000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 16000);
            }

        }

        public static void PaydayArmy(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 1)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2100);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2100$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 2)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2650);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~2650$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 3)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 3200);
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~3200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

            if (player.Character.FactionRank == 4)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~4400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 4400);
            }

            if (player.Character.FactionRank == 5)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~5200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 5200);
            }

            if (player.Character.FactionRank == 6)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~6400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 6400);
            }

            if (player.Character.FactionRank == 7)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~7200$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 7200);
            }

            if (player.Character.FactionRank == 8)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9000);
            }

            if (player.Character.FactionRank == 9)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~9500$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 9500);
            }

            if (player.Character.FactionRank == 10)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~10400$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 10400);
            }

            if (player.Character.FactionRank == 11)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~12000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 12000);
            }

            if (player.Character.FactionRank == 12)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Ihr Arbeitgeber hat Ihnen ~g~16000$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
                MoneyService.MoneyService.AddPlayerBank(client, 16000);
            }
        }

        public static void PaydayCitizen(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 0)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 450);
                API.shared.sendPictureNotificationToPlayer(client, "Der Staat hat Ihnen ~g~450$ ~w~überwiesen!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Staatskasse");
            }

        }

        public static void PaydayFahrschule(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.FactionRank == 1)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 1200);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~1200$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 2)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 2400);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~2400$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 3)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 3200);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~3200$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 4)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 4400);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~4400 ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 5)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 5200);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~5200$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 6)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 6400);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~6400$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 7)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 7200);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~7200$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 8)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 8400);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~8400$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 9)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 8800);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~8800$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 10)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 9000);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~9000$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 11)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 9500);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~9500$ ~w~überwiesen!");
            }

            if (player.Character.FactionRank == 12)
            {
                MoneyService.MoneyService.AddPlayerBank(client, 10000);
                API.shared.sendNotificationToPlayer(client, "~w~Ihr Arbeitgeber hat Ihnen ~g~10000$ ~w~überwiesen!");
            }
        }


        [Command("paydaynow")]
        public void CMD_MySeat(Client client)
        {

            PayDayExecute();
            API.sendChatMessageToPlayer(client, "~w~Ok...");
        }

        [Command("mytime")]
        public void CheckPlayTime(Client client)
        {
            Player player = client.getData("player");
            API.sendChatMessageToPlayer(client, player.Character.TimeOnServer.ToString());
        }

        [Command("setlevel")]
        public void SetPlayerLevel(Client client, Client player, int level)
        {
            Player admin = client.getData("player");
            Player target = player.getData("player");

            if (admin.Account.AdminLvl != 8) { return; }

            API.shared.sendNotificationToPlayer(client, "~g~Der Spieler administrativ ein höheres Level!");
            API.shared.sendNotificationToPlayer(player, "~g~Sie erhielten administrativ ein höheres Level!");
            target.Character.Level = level;
            target.Character.TimeOnServer = 0;
            CharacterService.CharacterService.UpdateCharacter(target.Character);
        }
    }
}