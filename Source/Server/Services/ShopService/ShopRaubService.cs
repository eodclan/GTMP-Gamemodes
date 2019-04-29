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

namespace FactionLife.Server.Services.ShopRob
{
    class ShopRob
        : Script
    {

        public void Main()
        {
            API.onResourceStart += OnStart;

            API.onEntityExitColShape += OnEntityExitColShapeHandler;
        }

        private Vector3 Shop1 = new Vector3(373.8327, 326.2473, 103.5664);
        private Vector3 Shop2 = new Vector3(-1487.329, -378.9935, 40.1634);
        private Vector3 Shop3 = new Vector3(-1222.853, -907.1208, 12.32635);
        private Vector3 Shop4 = new Vector3(-47.77902, -1757.163, 29.42101);
        private Vector3 Shop5 = new Vector3(1135.689, -982.5187, 46.41581);
        private Vector3 Shop6 = new Vector3(1163.354, -322.7556, 69.20509);
        private Vector3 Shop7 = new Vector3(25.71709, -1347.987, 29.49703);
        private Vector3 Shop8 = new Vector3(-707.4104, -913.54, 19.21559);
        private Vector3 Shop9 = new Vector3(-1771.866, -1572.237, 4.663626);
        private Vector3 Shop10 = new Vector3(-2967.837, 39.6269, 15.04331);
        private Vector3 Shop11 = new Vector3(-3039.178, 585.8055, 7.90893);
        private Vector3 Shop12 = new Vector3(-3241.737, 1001.218, 12.83071);
        private Vector3 Shop13 = new Vector3(547.793, 2671.213, 42.5165);
        private Vector3 Shop14 = new Vector3(1165.44, 2709.346, 38.1577);
        private Vector3 Shop15 = new Vector3(1961.318, 3740.439, 32.34375);
        private Vector3 Shop16 = new Vector3(1698.751, 4924, 42.06367);
        private Vector3 Shop17 = new Vector3(1729.075, 6414.805, 35.03723);
        private Vector3 Shop18 = new Vector3(2678.501, 3280.611, 55.24114);
        private Vector3 Shop19 = new Vector3(2557.365, 382.0884, 108.623);


        private ColShape Shop1ColShape = null;
        private ColShape Shop2ColShape = null;
        private ColShape Shop3ColShape = null;
        private ColShape Shop4ColShape = null;
        private ColShape Shop5ColShape = null;
        private ColShape Shop6ColShape = null;
        private ColShape Shop7ColShape = null;
        private ColShape Shop8ColShape = null;
        private ColShape Shop9ColShape = null;
        private ColShape Shop10ColShape = null;
        private ColShape Shop11ColShape = null;
        private ColShape Shop12ColShape = null;
        private ColShape Shop13ColShape = null;
        private ColShape Shop14ColShape = null;
        private ColShape Shop15ColShape = null;
        private ColShape Shop16ColShape = null;
        private ColShape Shop17ColShape = null;
        private ColShape Shop18ColShape = null;
        private ColShape Shop19ColShape = null;

        //private Timer Shop1Timer;
        //private Timer Shop2Timer;
        //private Timer Shop3Timer;
        //private Timer Shop4Timer;
        //private Timer Shop5Timer;
        //private Timer Shop6Timer;
        //private Timer Shop7Timer;
        //private Timer Shop8Timer;
        //private Timer Shop9Timer;
        //private Timer Shop10Timer;
        //private Timer Shop11Timer;
        //private Timer Shop12Timer;
        //private Timer Shop13Timer;
        //private Timer Shop14Timer;
        //private Timer Shop15Timer;
        //private Timer Shop16Timer;
        //private Timer Shop17Timer;
        //private Timer Shop18Timer;
        //private Timer Shop19Timer;


        public void OnStart()
        {
            ///
            Shop1ColShape = API.createCylinderColShape(new Vector3(373.8327, 326.2473, 103.5664), 4, 10);
            Shop2ColShape = API.createCylinderColShape(new Vector3(-1487.329, -378.9935, 40.1634), 4, 10);
            Shop3ColShape = API.createCylinderColShape(new Vector3(-1222.853, -907.1208, 12.32635), 4, 10);
            Shop4ColShape = API.createCylinderColShape(new Vector3(-47.77902, -1757.163, 29.42101), 4, 10);
            Shop5ColShape = API.createCylinderColShape(new Vector3(1135.689, -982.5187, 46.41581), 4, 10);
            Shop6ColShape = API.createCylinderColShape(new Vector3(1163.354, -322.7556, 69.20509), 4, 10);
            Shop7ColShape = API.createCylinderColShape(new Vector3(25.71709, -1347.987, 29.49703), 4, 10);
            Shop8ColShape = API.createCylinderColShape(new Vector3(-707.4104, -913.54, 19.21559), 4, 10);
            Shop9ColShape = API.createCylinderColShape(new Vector3(-1771.866, -1572.237, 4.663626), 4, 10);
            Shop10ColShape = API.createCylinderColShape(new Vector3(-2967.837, 39.6269, 15.04331), 4, 10);
            Shop11ColShape = API.createCylinderColShape(new Vector3(-3039.178, 585.8055, 7.90893), 4, 10);
            Shop12ColShape = API.createCylinderColShape(new Vector3(-3241.737, 1001.218, 12.83071), 4, 10);
            Shop13ColShape = API.createCylinderColShape(new Vector3(547.793, 2671.213, 42.5165), 4, 10);
            Shop14ColShape = API.createCylinderColShape(new Vector3(1165.44, 2709.346, 38.1577), 4, 10);
            Shop15ColShape = API.createCylinderColShape(new Vector3(1961.318, 3740.439, 32.34375), 4, 10);
            Shop16ColShape = API.createCylinderColShape(new Vector3(1698.751, 4924, 42.06367), 4, 10);
            Shop17ColShape = API.createCylinderColShape(new Vector3(1729.075, 6414.805, 35.03723), 4, 10);
            Shop18ColShape = API.createCylinderColShape(new Vector3(2678.501, 3280.611, 55.24114), 4, 10);
            Shop19ColShape = API.createCylinderColShape(new Vector3(2557.365, 382.0884, 108.623), 4, 10);
        }


        public void OnEntityExitColShapeHandler(ColShape shape, NetHandle entity)
        {
            if (API.getEntityType(entity) == EntityType.Player)
            {
                Client client = API.getPlayerFromHandle(entity);

                if (shape == Shop1ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 1); }
                if (shape == Shop2ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 2); }
                if (shape == Shop3ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 3); }
                if (shape == Shop4ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 4); }
                if (shape == Shop5ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 5); }
                if (shape == Shop6ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 6); }
                if (shape == Shop7ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 7); }
                if (shape == Shop8ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 8); }
                if (shape == Shop9ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 9); }
                if (shape == Shop10ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 10); }
                if (shape == Shop11ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 11); }
                if (shape == Shop12ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 12); }
                if (shape == Shop13ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 13); }
                if (shape == Shop14ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 14); }
                if (shape == Shop15ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 15); }
                if (shape == Shop16ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 16); }
                if (shape == Shop17ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 17); }
                if (shape == Shop18ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 18); }
                if (shape == Shop19ColShape) { API.sendNotificationToPlayer(client, "~r~ Der Raubüberfall ist fehlgeschlagen!"); StopRaub(client, 19); }

                API.setEntitySyncedData(client, "ShopRob", false);

            }

        }

        public void StopRaub(Client client, int shopid)
        {
            
                Shop1ColShape.onEntityEnterColShape += (shape, entity) =>
                {
                    Client player;
                    Vector3 bank1 = new Vector3(258.1556, 220.7626, 106.2845);
                    if ((player = API.getPlayerFromHandle(entity)) != null)
                    {
                        API.sendNotificationToPlayer(client, "~r~Der Raubüberfall wurde abgebrochen!");

                        if (shopid == 1) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Vinewood Mitte!"); }
                        if (shopid == 2) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Morningwood!"); }
                        if (shopid == 3) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Vespucci-Kanäle!"); }
                        if (shopid == 4) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Davis!"); }
                        if (shopid == 5) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Murrieta Heights!"); }
                        if (shopid == 6) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Mirrir Park!"); }
                        if (shopid == 7) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Strawberry!"); }
                        if (shopid == 8) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Little Seoul!"); }
                        if (shopid == 9) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Vespucci Beach!"); }
                        if (shopid == 10) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Banham Canyon 1!"); }
                        if (shopid == 11) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Banham Canyon 2!"); }
                        if (shopid == 12) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Chumash!"); }
                        if (shopid == 13) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Harmony!"); }
                        if (shopid == 14) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Grand-Senora-Wüste!"); }
                        if (shopid == 15) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Sandy Shores!"); }
                        if (shopid == 16) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Grapeseed!"); }
                        if (shopid == 17) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Mount Chiliad!"); }
                        if (shopid == 18) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Grand-Senora-Wüste Senora Fwy!"); }
                        if (shopid == 19) { FactionHandler.FraUtil.BroadcastToLSPD("~r~ Der Raubüberfall wurde erfolgreich verhindert Tataviam Bergkette!"); }

                       
                        API.sendNotificationToPlayer(client, "~r~Der Raubüberfall wurde abgebrochen!");

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
            if (shopid == 5) { API.startTimer(time, true, () => { EndRobbing(client, 5); }); }
            if (shopid == 6) { API.startTimer(time, true, () => { EndRobbing(client, 6); }); }
            if (shopid == 7) { API.startTimer(time, true, () => { EndRobbing(client, 7); }); }
            if (shopid == 8) { API.startTimer(time, true, () => { EndRobbing(client, 8); }); }
            if (shopid == 9) { API.startTimer(time, true, () => { EndRobbing(client, 9); }); }
            if (shopid == 10) { API.startTimer(time, true, () => { EndRobbing(client, 10); }); }
            if (shopid == 11) { API.startTimer(time, true, () => { EndRobbing(client, 11); }); }
            if (shopid == 12) { API.startTimer(time, true, () => { EndRobbing(client, 12); }); }
            if (shopid == 13) { API.startTimer(time, true, () => { EndRobbing(client, 13); }); }
            if (shopid == 14) { API.startTimer(time, true, () => { EndRobbing(client, 14); }); }
            if (shopid == 15) { API.startTimer(time, true, () => { EndRobbing(client, 15); }); }
            if (shopid == 16) { API.startTimer(time, true, () => { EndRobbing(client, 16); }); }
            if (shopid == 17) { API.startTimer(time, true, () => { EndRobbing(client, 17); }); }
            if (shopid == 18) { API.startTimer(time, true, () => { EndRobbing(client, 18); }); }
            if (shopid == 19) { API.startTimer(time, true, () => { EndRobbing(client, 19); }); }

        }



        public void EndRobbing(Client client, int shopid)
        {
            if ((shopid == 1) && (!IsInRangeOf(client.position, Shop1, 10))) { return; }
            if ((shopid == 2) && (!IsInRangeOf(client.position, Shop2, 10))) { return; }
            if ((shopid == 3) && (!IsInRangeOf(client.position, Shop3, 10))) { return; }
            if ((shopid == 4) && (!IsInRangeOf(client.position, Shop4, 10))) { return; }
            if ((shopid == 5) && (!IsInRangeOf(client.position, Shop5, 10))) { return; }
            if ((shopid == 6) && (!IsInRangeOf(client.position, Shop6, 10))) { return; }
            if ((shopid == 7) && (!IsInRangeOf(client.position, Shop7, 10))) { return; }
            if ((shopid == 8) && (!IsInRangeOf(client.position, Shop8, 10))) { return; }
            if ((shopid == 9) && (!IsInRangeOf(client.position, Shop9, 10))) { return; }
            if ((shopid == 10) && (!IsInRangeOf(client.position, Shop10, 10))) { return; }
            if ((shopid == 11) && (!IsInRangeOf(client.position, Shop11, 10))) { return; }
            if ((shopid == 12) && (!IsInRangeOf(client.position, Shop12, 10))) { return; }
            if ((shopid == 13) && (!IsInRangeOf(client.position, Shop13, 10))) { return; }
            if ((shopid == 14) && (!IsInRangeOf(client.position, Shop14, 10))) { return; }
            if ((shopid == 15) && (!IsInRangeOf(client.position, Shop15, 10))) { return; }
            if ((shopid == 16) && (!IsInRangeOf(client.position, Shop16, 10))) { return; }
            if ((shopid == 17) && (!IsInRangeOf(client.position, Shop17, 10))) { return; }
            if ((shopid == 18) && (!IsInRangeOf(client.position, Shop18, 10))) { return; }
            if ((shopid == 19) && (!IsInRangeOf(client.position, Shop19, 10))) { return; }



            if (API.getEntitySyncedData(client, "ShopRob") == true)
            {


                Player player = client.getData("player");

                Random rnd = new Random();
                double amount = rnd.Next(500, 1000);

                MoneyService.MoneyService.AddPlayerCash(client, amount);

                API.sendNotificationToPlayer(client, "~g~Raubüberfall abgeschlossen!");

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
                            API.triggerClientEvent(client, "PoliceNavi", "S1", Shop1.X, Shop1.Y);
                            break;
                        case 2:
                            API.triggerClientEvent(client, "PoliceNavi", "S2", Shop2.X, Shop2.Y);
                            break;
                        case 3:
                            API.triggerClientEvent(client, "PoliceNavi", "S3", Shop3.X, Shop3.Y);
                            break;
                        case 4:
                            API.triggerClientEvent(client, "PoliceNavi", "S4", Shop4.X, Shop4.Y);
                            break;
                        case 5:
                            API.triggerClientEvent(client, "PoliceNavi", "S5", Shop5.X, Shop5.Y);
                            break;
                        case 6:
                            API.triggerClientEvent(client, "PoliceNavi", "S6", Shop6.X, Shop6.Y);
                            break;
                        case 7:
                            API.triggerClientEvent(client, "PoliceNavi", "S7", Shop7.X, Shop7.Y);
                            break;
                        case 8:
                            API.triggerClientEvent(client, "PoliceNavi", "S8", Shop8.X, Shop8.Y);
                            break;
                        case 9:
                            API.triggerClientEvent(client, "PoliceNavi", "S9", Shop9.X, Shop9.Y);
                            break;
                        case 10:
                            API.triggerClientEvent(client, "PoliceNavi", "S10", Shop10.X, Shop10.Y);
                            break;
                        case 11:
                            API.triggerClientEvent(client, "PoliceNavi", "S11", Shop11.X, Shop11.Y);
                            break;
                        case 12:
                            API.triggerClientEvent(client, "PoliceNavi", "S12", Shop12.X, Shop12.Y);
                            break;
                        case 13:
                            API.triggerClientEvent(client, "PoliceNavi", "S13", Shop13.X, Shop13.Y);
                            break;
                        case 14:
                            API.triggerClientEvent(client, "PoliceNavi", "S14", Shop14.X, Shop14.Y);
                            break;
                        case 15:
                            API.triggerClientEvent(client, "PoliceNavi", "S15", Shop15.X, Shop15.Y);
                            break;
                        case 16:
                            API.triggerClientEvent(client, "PoliceNavi", "S16", Shop16.X, Shop16.Y);
                            break;
                        case 17:
                            API.triggerClientEvent(client, "PoliceNavi", "S17", Shop17.X, Shop17.Y);
                            break;
                        case 18:
                            API.triggerClientEvent(client, "PoliceNavi", "S18", Shop18.X, Shop18.Y);
                            break;
                        case 19:
                            API.triggerClientEvent(client, "PoliceNavi", "S19", Shop19.X, Shop19.Y);
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

        [Command("robshop", GreedyArg = true)]
        public void Robshop(Client client)
        {


            if (API.getEntitySyncedData(client, "ShopRob") == true) { return; }

            else if (IsInRangeOf(client.position, Shop1, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Vinewood Mitte!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Vinewood Mitte!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Vinewood Mitte!");
                SetPoliceNav(1);
                StartShopTimer(client, 1);
            }

            else if (IsInRangeOf(client.position, Shop2, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Morningwood!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Morningwood!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Morningwood!");
                SetPoliceNav(2);
                StartShopTimer(client, 2);
            }

            else if (IsInRangeOf(client.position, Shop3, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Vespucci-Kanäle!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Vespucci-Kanäle!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Vespucci-Kanäle!");
                SetPoliceNav(3);
                StartShopTimer(client, 3);
            }

            else if (IsInRangeOf(client.position, Shop4, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Davis!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Davis!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~Ein Raubüberfall wurde gemeldet Davis!");
                SetPoliceNav(4);
                StartShopTimer(client, 4);
            }

            else if (IsInRangeOf(client.position, Shop5, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Murrieta Heights!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Murrieta Heights!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Murrieta Heights!");
                SetPoliceNav(5);
                StartShopTimer(client, 5);
            }

            else if (IsInRangeOf(client.position, Shop6, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Mirrir Park!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Mirrir Park!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Mirrir Park!");
                SetPoliceNav(6);
                StartShopTimer(client, 6);
            }

            else if (IsInRangeOf(client.position, Shop7, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Strawberry!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Strawberry!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Strawberry!");
                SetPoliceNav(7);
                StartShopTimer(client, 7);
            }

            else if (IsInRangeOf(client.position, Shop8, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Little Seoul!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Little Seoul!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Little Seoul!");
                SetPoliceNav(8);
                StartShopTimer(client, 8);
            }

            else if (IsInRangeOf(client.position, Shop9, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Vespucci-Kanäle!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Vespucci-Kanäle!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Vespucci-Kanäle!");
                SetPoliceNav(9);
                StartShopTimer(client, 9);
            }

            else if (IsInRangeOf(client.position, Shop10, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Banham Canyon!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Banham Canyon!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Banham Canyon!");
                SetPoliceNav(10);
                StartShopTimer(client, 10);
            }

            else if (IsInRangeOf(client.position, Shop11, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Banham Canyon!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Banham Canyon!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Banham Canyon!");
                SetPoliceNav(11);
                StartShopTimer(client, 11);
            }

            else if (IsInRangeOf(client.position, Shop12, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Chumash!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Chumash!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Chumash!");
                SetPoliceNav(12);
                StartShopTimer(client, 12);
            }


            else if (IsInRangeOf(client.position, Shop13, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Harmony!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Harmony!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~Ein Raubüberfall wurde gemeldet Harmony!");
                SetPoliceNav(13);
                StartShopTimer(client, 13);
            }

            else if (IsInRangeOf(client.position, Shop14, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Grand-Senora-Wüste!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Grand-Senora-Wüste!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~  Ein Raubüberfall wurde gemeldet Grand-Senora-Wüste!");
                SetPoliceNav(14);
                StartShopTimer(client, 14);
            }

            else if (IsInRangeOf(client.position, Shop15, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Sandy Shores!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Sandy Shores!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Sandy Shores!");
                SetPoliceNav(15);
                StartShopTimer(client, 15);
            }

            else if (IsInRangeOf(client.position, Shop16, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Grapeseed!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Grapeseed!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Grapeseed!");
                SetPoliceNav(16);
                StartShopTimer(client, 16);
            }

            else if (IsInRangeOf(client.position, Shop17, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Mount Chiliad!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Mount Chiliad!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Mount Chiliad!");
                SetPoliceNav(17);
                StartShopTimer(client, 17);
            }

            else if (IsInRangeOf(client.position, Shop18, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Grand-Senora-Wüste Senora Fwy!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Grand-Senora-Wüste Senora Fwy");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Grand-Senora-Wüste Senora Fwy!");
                SetPoliceNav(18);
                StartShopTimer(client, 18);
            }

            else if (IsInRangeOf(client.position, Shop19, 10))
            {
                API.setEntitySyncedData(client, "ShopRob", true);
                API.sendNotificationToPlayer(client, "~g~Sie beginnen den Shop auszurauben!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~ Ein Raubüberfall wurde gemeldet Tataviam Bergkette!");
                FactionHandler.FraUtil.BroadcastToFIB("~r~ Ein Raubüberfall wurde gemeldet Tataviam Bergkette!");
                FactionHandler.FraUtil.BroadcastToSWAT("~r~ Ein Raubüberfall wurde gemeldet Tataviam Bergkette!");
                SetPoliceNav(19);
                StartShopTimer(client, 19);
            }

        }

    }

}