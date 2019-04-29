using GrandTheftMultiplayer.Server.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Base;
using FactionLife.Server.Model;
using FactionLife.Server.Services.AccountService;
using FactionLife.Server.Services.CharacterService;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.IO;
using FactionLife.Server.Services.MoneyService;
using System.Xml.Linq;
using System.Timers;
using Newtonsoft.Json;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Services.AdminService;
using FactionLife.Server.Services.FactionService;
using System.Collections;

namespace FactionLife.Server
{
    class RestartHandler
    : Script
    {

        public RestartHandler()
        {
            API.onResourceStart += OnResourceStartHandler;
            API.onResourceStop += OnResourceStopHandler;
        }

        public Timer PayDayTimer;
        private string oldServerPassword;

        public void OnResourceStartHandler()
        {
            PayDayTimer = API.startTimer(60000, false, () =>
            {
                if ((DateTime.Now.Hour == 23) || (DateTime.Now.Hour == 3) || (DateTime.Now.Hour == 7) || (DateTime.Now.Hour == 11) || (DateTime.Now.Hour == 15) || (DateTime.Now.Hour == 19))
                {
                    if (DateTime.Now.Minute == 48) { ExecuteRebootWarning(10); }
                    if (DateTime.Now.Minute == 55) { ExecuteRebootWarning(3); }
                    if (DateTime.Now.Minute == 58) { ExecuteReboot(); }
                }

            });
        }

        public void OnResourceStopHandler()
        {
            API.stopTimer(PayDayTimer);
        }

        public void ExecuteRebootWarning(int time)
        {
            API.sendPictureNotificationToAll("Die Sonnenwende beginnt in " + time + " Minuten. Wir bitten euch die Insel zu verlassen!", "CHAR_SOCIAL_CLUB", 0, 3, "Sonnenwende", "Stomversorgung Los Santos");
        }

        public void ExecuteReboot()
        {
            oldServerPassword = API.getServerPassword();
            string reason = "Die Sonnenwende beginnt!";
            Random rand = new Random();

            Console.BackgroundColor = ConsoleColor.DarkRed;
            API.shared.setServerPassword(rand.Next().ToString());
            API.consoleOutput("Die Sonnenwende wird ausgeführt");
            API.consoleOutput("Server mit folgendem Passwort gesperrt: (Sonnenwende) " + API.getServerPassword());
            Console.ResetColor();


            List<Client> clientlist = API.getAllPlayers();

            foreach (Client client in clientlist)
            {

                PlayerSaveStuff(client);

                API.delay(3500, true, () =>
                {
                    API.kickPlayer(client, reason);

                });

                
            }

            API.delay(5000, true, () =>
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                API.consoleOutput("Spielstände gespeichert, Alle Spieler gekickt...");
                Console.ResetColor();
                API.setServerPassword(oldServerPassword);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                API.consoleOutput("Der Server ist freigegeben..");
                Console.ResetColor();
            });
            

        }





        private void PlayerSaveStuff(Client client)
        {
            if (client.hasData("player"))
            {
                Player player = (Player)client.getData("player");
                CharacterService.UpdateHealthandArmor(client, player.Character);
                CharacterService.UpdateCharacter(player.Character);
                AccountService.SaveAndLogoutPlayer(player.Account);
            }
        }





        [Command("reboottest")]
        public void RebootAdminTest(Client client, int timetorestart)
        {
            Player player = client.getData("player");
            if (player.Account.AdminLvl != 8) { return; }

            ExecuteRebootWarning(timetorestart);
        }

        [Command("rebootnow")]
        public void RebootAdminNow (Client client)
        {
            Player player = client.getData("player");
            if (player.Account.AdminLvl != 8) { return; }

            ExecuteReboot();
        }


}
}