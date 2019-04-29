using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using System;
using System.Linq;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using System.Timers;

namespace FactionLife.Server.Services.RobBank
{
    class RobBank
        : Script
    {

        public void Main()
        {
            API.onResourceStart += OnStart;

            API.onEntityExitColShape += OnEntityExitColShapeHandler;
        }

        public Timer BankTimer;

        public void OnResourceStartHandler()
        {

            BankTimer = API.startTimer(60000, false, () =>
            {
                if ((DateTime.Now.Hour == 0) || (DateTime.Now.Hour == 1) || (DateTime.Now.Hour == 2) || (DateTime.Now.Hour == 3) || (DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5) || (DateTime.Now.Hour == 6) || (DateTime.Now.Hour == 7) || (DateTime.Now.Hour == 8) || (DateTime.Now.Hour == 9) || (DateTime.Now.Hour == 10) || (DateTime.Now.Hour == 11) || (DateTime.Now.Hour == 12) || (DateTime.Now.Hour == 13) || (DateTime.Now.Hour == 14) || (DateTime.Now.Hour == 15) || (DateTime.Now.Hour == 16) || (DateTime.Now.Hour == 17) || (DateTime.Now.Hour == 18) || (DateTime.Now.Hour == 19) || (DateTime.Now.Hour == 20) || (DateTime.Now.Hour == 21) || (DateTime.Now.Hour == 22) || (DateTime.Now.Hour == 23))
                {
                    ;
                }
            });
        }

        public void OnResourceStopHandler()
        {
            API.stopTimer(BankTimer);
        }

        private Vector3 Shop1 = new Vector3(147.2792, -1045.185, 29.36803);
        private Vector3 Shop2 = new Vector3(253.2422, 221.1075, 106.2865);
        private Vector3 Shop3 = new Vector3(-1212.754, -330.7684, 37.78699);
        private Vector3 Shop4 = new Vector3(1175.484, 2760.805, 38.09407);
		
        private ColShape Shop1ColShape = null;
        private ColShape Shop2ColShape = null;
        private ColShape Shop3ColShape = null;
        private ColShape Shop4ColShape = null;
        //private Timer Shop1Timer;
        //private Timer Shop2Timer;
        //private Timer Shop3Timer;
        //private Timer Shop1Timer;

        public void OnStart()
        {
            ///
            Shop1ColShape = API.createCylinderColShape(new Vector3(147.2792, -1045.185, 29.36803), 4, 10);
            Shop2ColShape = API.createCylinderColShape(new Vector3(253.2422, 221.1075, 106.2865), 4, 10);
            Shop3ColShape = API.createCylinderColShape(new Vector3(-1212.754, -330.7684, 37.78699), 4, 10);
            Shop4ColShape = API.createCylinderColShape(new Vector3(1175.484, 2760.805, 38.09407), 4, 10);
        }
		
        public void OnEntityExitColShapeHandler(ColShape shape, NetHandle entity)
        {
            if (API.getEntityType(entity) == EntityType.Player)
            {
                Client client = API.getPlayerFromHandle(entity);

                if (shape == Shop1ColShape) { API.sendChatMessageToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 1); }
                if (shape == Shop2ColShape) { API.sendChatMessageToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 2); }
                if (shape == Shop3ColShape) { API.sendChatMessageToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 3); }
                if (shape == Shop4ColShape) { API.sendChatMessageToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 4); }

                API.setEntitySyncedData(client, "ShopRob", false);
            }
        }

        public void StopRaub(Client client, int shopid)
        {
            
                Shop1ColShape.onEntityEnterColShape += (shape, entity) =>
                {
                    Client player;
                    Vector3 bank1 = new Vector3(147.2792, -1045.185, 29.36803);
                    Vector3 bank2 = new Vector3(253.2422, 221.1075, 106.2865);
                    Vector3 bank3 = new Vector3(-1212.754, -330.7684, 37.78699);
                    Vector3 bank4 = new Vector3(1175.484, 2760.805, 38.09407);
               
                    if ((player = API.getPlayerFromHandle(entity)) != null)
                    {
                        API.sendChatMessageToPlayer(client, "~r~ Der Raubüberfall wurde abgebrochen!");

                        if (shopid == 1) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert !"); }
                        if (shopid == 2) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert !"); }
                        if (shopid == 3) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert !"); }
                        if (shopid == 4) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert !"); }

                        API.sendChatMessageToPlayer(client, "Der Raubüberfall wurde abgebrochen!");

                        API.setEntitySyncedData(client, "ShopRob", false);
                    }
                };
        }

        public void StartShopTimer(Client client, int shopid)
        {
            Random rnd = new Random();
            int time = rnd.Next(120000, 240000);

            if (shopid == 1) { API.startTimer(120000, true, () => { EndRobbing(client, 1); }); }
            if (shopid == 2) { API.startTimer(time, true, () => { EndRobbing(client, 2); }); }
            if (shopid == 3) { API.startTimer(time, true, () => { EndRobbing(client, 3); }); }
            if (shopid == 4) { API.startTimer(time, true, () => { EndRobbing(client, 4); }); }
        }

        public void EndRobbing(Client client, int shopid)
        {
            if ((shopid == 1) && (!IsInRangeOf(client.position, Shop1, 10))) { return; }
            if ((shopid == 2) && (!IsInRangeOf(client.position, Shop2, 10))) { return; }
            if ((shopid == 3) && (!IsInRangeOf(client.position, Shop3, 10))) { return; }
            if ((shopid == 4) && (!IsInRangeOf(client.position, Shop1, 10))) { return; }

            if (API.getEntitySyncedData(client, "ShopRob") == true)
            {
                Player player = client.getData("player");

                Random rnd = new Random();
                double amount = rnd.Next(100000, 300000);

                MoneyService.MoneyService.AddPlayerCash(client, amount);

                API.sendChatMessageToPlayer(client, "Raubüberfall abgeschlossen.");

                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde abgeschlossen! Die Täter flüchten!");

                API.setEntitySyncedData(client, "ShopRob", false);
            }
        }

        public void SetPoliceNav(int shopid)
        {
            API.shared.getAllPlayers().Where(x => x.hasData("player")).ToList().ForEach(client =>
            {
                Player player = client.getData("player");
                if ((player.Character.Faction == FactionType.Police) && player.Character.OnDuty)

                    switch (shopid)
                    {

                        case 1:
                            API.triggerClientEvent(client, "PoliceNavi1", "S1", Shop1.X, Shop1.Y);
                            break;
                        case 2:
                            API.triggerClientEvent(client, "PoliceNavi1", "S2", Shop2.X, Shop2.Y);
                            break;
                        case 3:
                            API.triggerClientEvent(client, "PoliceNavi1", "S3", Shop3.X, Shop3.Y);
                            break;
                        case 4:
                            API.triggerClientEvent(client, "PoliceNavi1", "S4", Shop3.X, Shop3.Y);
                            break;
                    }
            });
        }

        public static bool IsInRangeOf(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }

        [Command("robbank", GreedyArg = true)]
        public void Robshop(Client client)
        {
            Player player = client.getData("player");

            if ((API.getEntitySyncedData(client, "robbank") == true) && ((player.Character.Faction == FactionType.FIB) || (player.Character.Faction == FactionType.Police) || (player.Character.Faction == FactionType.Army) || (player.Character.Faction == FactionType.JVA)))
            {
                API.sendChatMessageToPlayer(client, "Sie sind nicht befugt!");
             
                return;
            }

            /* if (API.getEntitySyncedData(client, "ShopRob") == true) {
                 return;
             }*/

            else if (IsInRangeOf(client.position, Shop1, 10))
            {
                API.setEntitySyncedData(client, "robbank", true);
                API.sendChatMessageToPlayer(client, "Sie beginnen die Bank auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToSheriff("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                SetPoliceNav(1);
                StartShopTimer(client, 1);
            }

            else if (IsInRangeOf(client.position, Shop2, 10))
            {
                API.setEntitySyncedData(client, "robbank", true);
                API.sendChatMessageToPlayer(client, "Sie beginnen die Bank auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToSheriff("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                SetPoliceNav(2);
                StartShopTimer(client, 2);
            }

            else if (IsInRangeOf(client.position, Shop3, 10))
            {
                API.setEntitySyncedData(client, "robbank", true);
                API.sendChatMessageToPlayer(client, "Sie beginnen die Bank auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToSheriff("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                SetPoliceNav(3);
                StartShopTimer(client, 3);
            }

            else if (IsInRangeOf(client.position, Shop4, 10))
            {
                API.setEntitySyncedData(client, "robbank", true);
                API.sendChatMessageToPlayer(client, "Sie beginnen die Bank auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                FactionHandler.FraUtil.BroadcastToSheriff("~r~ Ein Raubüberfall wurde gemeldet auf eine Bank!");
                SetPoliceNav(4);
                StartShopTimer(client, 4);
            }
        }
    }
}
