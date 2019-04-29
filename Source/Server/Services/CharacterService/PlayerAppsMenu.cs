using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using FactionLife.Server.Base;
using FactionLife.Server.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FactionLife.Server;
using FactionLife.Server.Services.AdminService;
using FactionLife.Server.Services.CharacterService;
using GrandTheftMultiplayer.Shared.Math;
using System.Data;
using System.Collections;
using FactionLife.Server.Services.FactionService;

namespace FactionLife.Server.Services.CharacterService
{
    class PlayerAppsMenuService
    {

        public static void OpenMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "PlayerApps_Open");
        }



        #region Player Interaction Menu
        public static List<MenuItem> BuildPlayerAppsMenu(Player player, Client player123)
        {
            List<MenuItem> menuItemList = new List<MenuItem>();

            #region Smartphone
            menuItemList.Add(new MenuItem
            {
                Title = "Smartphone",
                Value1 = "opensmartphone"
            });
            #endregion Smartphone

            #region Tablet
            menuItemList.Add(new MenuItem
            {
                Title = "Tablet",
                Value1 = "opentablet"
            });
            #endregion Tablet

            #region Tastaturbelegung
            menuItemList.Add(new MenuItem
            {
                Title = "Tastaturbelegung",
                Value1 = "opentastaturbelegung"
            });
            #endregion Tastaturbelegung


            return menuItemList;
        }
        #endregion Player Interaction Menu

        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PlayerApps_Close");
        }

        #region PlayerInteraction Menu Processing
        public static void ProcessPlayerAppsMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            API.shared.triggerClientEvent(client, "PlayerApps_Close");
            
            switch (itemvalue)
            {

                case "opentastaturbelegung":
                    API.shared.triggerClientEvent(client, "Tastaturbelegung_OpenMenu");
                    API.shared.delay(26000, true, () => {
                        API.shared.triggerClientEvent(client, "closeTastaturbelegung_OpenMenu");
                    });
                    break;

                case "opensmartphone":
                    API.shared.triggerClientEvent(client, "Smartphone_OpenMenu");
                    API.shared.delay(30000, true, () => {
                        API.shared.triggerClientEvent(client, "closeSmartphone_OpenMenu");
                    });
                    break;

                case "opentablet":
                    API.shared.triggerClientEvent(client, "Tablet_OpenMenu");
                    API.shared.delay(25000, true, () => {
                        API.shared.triggerClientEvent(client, "closeTablet_OpenMenu");
                    });
                    break;
            }
        }
        #endregion PlayerInteraction Menu Processing
    }
}