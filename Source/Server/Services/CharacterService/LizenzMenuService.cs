using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Services.CharacterService
{
    class LizensMenuService
    {

        public static void OpenLizenzMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "PlayerMenu_Open", JsonConvert.SerializeObject(BuildLizenzMenu(player, client)));
        }

        #region Player Lizenz Menu
        public static List<MenuItem> BuildLizenzMenu(Player player, Client player123)
        {

            Player otherplayer1;

            List<MenuItem> menuItemList = new List<MenuItem>();

            Client nextplayer = player.Character.Player;
            otherplayer1 = CharacterService.GetNextPlayerInNearOfPlayer(player);

            #region Lizenzen

            foreach (License license in player.Character.Licenses)
            {
                switch (license.Id)
                {
                    case 1:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ PKW-Führerschein",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 2:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Motorrad-Führerschein",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 3:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ LKW-Führerschein",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 4:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Flugschein A",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 5:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Flugschein B",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 6:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Waffenschein A",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 7:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Waffenschein B",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 8:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Taxi License",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 9:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Boats License",
                            Value1 = "nonevaluegiven"
                        });
                        break;

                    case 10:
                        menuItemList.Add(new MenuItem
                        {
                            Title = "~g~ Flugschein B",
                            Value1 = "nonevaluegiven"
                        });
                        break;
                }
            
                
                
            }

            #endregion Lizenzen

            #region Lizenzen zeigen
            menuItemList.Add(new MenuItem
            {
                Title = "Lizenzen zeigen",
                Value1 = "showlic"
            });
            #endregion Lizenzen zeigen

            #region Schließen


            menuItemList.Add(new MenuItem
            {
                    Title = "Schließen",
                    Value1 = "nonevaluegiven"
            });
            
            #endregion Schließen

            return menuItemList;
        }
        #endregion Player Lizenz Menu


        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
        }

        #region Lizenz Menu Processing
        public static void ProcessLizenzMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");


            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
            switch (itemvalue)
            {

                #region nonevaluegiven

                case "nonevaluegiven":
                    break;

                #endregion nonevaluegiven

                #region showlic
                case "showlic":
                    List<Player> playersaround = new List<Player>();
                    API.shared.getPlayersInRadiusOfPlayer(1, client).ForEach(x => {
                        if (x.hasData("player"))
                        {
                            playersaround.Add((Player)x.getData("player"));
                        }
                    });
                    if (playersaround.Any(x => x == player))
                    {
                        playersaround.Remove(player);
                    }
                    if (playersaround.Count > 1)
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Es befinden sich zu viele Personen in deiner Nähe.");
                        return;
                    }
                    if (playersaround.Count < 1)
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Niemand befindet sich in deiner Nähe");
                        return;
                    }
                    Player target = playersaround.FirstOrDefault();
                    if (target == null) { API.shared.sendNotificationToPlayer(client, "~r~Hoppla, etwas ist schiefgelaufen.."); return; }

                    Client ggshopwing = API.shared.getAllPlayers().FirstOrDefault(x => x.socialClubName.ToLower() == target.Account.SocialClubName.ToLower());

                    List<License> player_licenses = player.Character.Licenses;

                    API.shared.sendPictureNotificationToPlayer(ggshopwing, "~y~Lizenzen von ~g~" + player.Character.FirstName + "\n" + player.Character.LastName + "\n", "CHAR_MP_FM_CONTACT", 0, 2, "Los Santos", "License");
                    foreach (License lic in player_licenses)
                    {
                        //API.shared.sendChatMessageToPlayer(ggshopwing, "-> " + lic.Name);
                        API.shared.sendNotificationToPlayer(ggshopwing, "-> " + lic.Name);
                    }
                    API.shared.sendNotificationToPlayer(client, "Sie haben Ihre Lizenzen gezeigt!");


                    break;
                    #endregion showlic




            }
        }
        #endregion Lizenz Menu Processing



        
        
    }
}