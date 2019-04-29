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
    class PlayerAnimationMenuService
    {

        public static void OpenMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "PlayerAnimation_Open");
        }



        #region Player Interaction Menu
        public static List<MenuItem> BuildPlayerAnimationMenu(Player player, Client player123)
        {
            List<MenuItem> menuItemList = new List<MenuItem>();

            #region Sitzen

            menuItemList.Add(new MenuItem
            {
                Title = "Sitzen",
                Value1 = "animationsitzen"
            });

            #endregion Sitzen
			
            #region Grüßen

            menuItemList.Add(new MenuItem
            {
                Title = "Grüßen",
                Value1 = "animationgruessen"
            });

            #endregion Grüßen

            #region Kniehen

            menuItemList.Add(new MenuItem
            {
                Title = "Kniehen",
                Value1 = "animationkniehen"
            });

            #endregion Kniehen 

            #region Schmerzen

            menuItemList.Add(new MenuItem
            {
                Title = "Schmerzen",
                Value1 = "animationschmerzen"
            });

            #endregion Schmerzen

            #region Schmerzen

            menuItemList.Add(new MenuItem
            {
                Title = "Abbrechen",
                Value1 = "animationabbrechen"
            });

            #endregion Schmerzen	

            return menuItemList;
        }
        #endregion Player Interaction Menu

        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PlayerAnimation_Close");
        }

        #region PlayerInteraction Menu Processing
        public static void ProcessPlayerAnimationMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            API.shared.triggerClientEvent(client, "PlayerAnimation_Close");
            
            switch (itemvalue)
            {
                case "animationsitzen":
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missexile1_cargoplaneleadinoutexile_1_int", "_leadout_michael");
                    break;

                case "animationgruessen":
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.OnlyAnimateUpperBody), "get_up@directional@transition@prone_to_seated@standard", "back_hipl");
                    break;

                case "animationkniehen":
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "misschinese2_crystalmaze", "2int_loop_a_taotranslator");
                    break;

                case "animationschmerzen":
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1ig_13@kick_idle", "guard_beatup_kickidle_dockworker");
                    break;

                case "animationabbrechen":
                    API.shared.stopPlayerAnimation(client);
                    break;
            }
        }
        #endregion PlayerInteraction Menu Processing
    }
}