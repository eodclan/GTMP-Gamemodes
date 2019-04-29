using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Services.CharacterService
{
    class GeldbörseMenuService
    {



        public static void OpenGeldbörseMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "PlayerMenu_Open", JsonConvert.SerializeObject(BuildGeldbörsenMenu(player, client)));
        }



        #region Player Geldbörse Menu
        public static List<MenuItem> BuildGeldbörsenMenu(Player player, Client player123)
        {
            Player otherplayer1;

            List<MenuItem> menuItemList = new List<MenuItem>();

            Client nextplayer = player.Character.Player;
            otherplayer1 = CharacterService.GetNextPlayerInNearOfPlayer(player);

            #region BargeldStand

            menuItemList.Add(new MenuItem
            {
                Title = "~g~Bargeld : ~y~" + player.Character.Cash + " ~g~$",
                Value1 = "nonevaluegiven"
            });

            #endregion BargeldStand

            #region Kontostand

            menuItemList.Add(new MenuItem
            {
                Title = "~g~Kontostand : ~y~" + player.Character.Bank + " ~g~$",
                Value1 = "nonevaluegiven"
            });

            #endregion Kontostand

            #region Personalausweis
            if (player.Character.HasIDCard == 1)
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "~g~Ausweis zeigen",
                    Value1 = "showpersotrue"
                });
            }

            if (player.Character.HasIDCard == 0)
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "~r~Kein Ausweis vorhanden!",
                    Value1 = "nonevaluegiven"
                });
            }

            #endregion Personalausweis

            #region Lizenzen

            menuItemList.Add(new MenuItem
            {
                Title = "Lizenzen",
                Value1 = "openlicensesmenu"
            });

            #endregion Lizenzen

            #region Geld geben


            if (otherplayer1 != null)
            {

                menuItemList.Add(new MenuItem
                {
                    Title = "Geld geben",
                    Value1 = "givemoney"
                });
            }
            else
            {

            }

            #endregion Geld geben

            return menuItemList;
        }
        #endregion Player Geldbörse Menu


        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
        }

        #region Geldbörsen Menu Processing
        public static void ProcessGeldbörsenMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");


            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
            switch (itemvalue)
            {

                #region nonevaluegiven(Alles anderexD)

                case "nonevaluegiven":
                    break;

                #endregion nonevaluegiven(Alles anderexD)

                #region geld geben

                case "givemoney":
                    API.shared.triggerClientEvent(client, "GiveMoney");
                    break;

                #endregion geld geben

                case "openlicensesmenu":
                    API.shared.triggerClientEvent(client, "Lizenz_Open", JsonConvert.SerializeObject(LizensMenuService.BuildLizenzMenu(player, client)));
                    break;

                case "showpersotrue":
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

                    string firstname = player.Character.FirstName;
                    string lastname = player.Character.LastName;
                    string day = player.Account.CreatedAt.Day.ToString();
                    string month = player.Account.CreatedAt.Month.ToString();
                    string year = player.Account.CreatedAt.Year.ToString();

                    string job = Convert.ToString(player.Character.Faction);


                    string firstvisit = day + "." + month + "." + year;


                    //API.shared.triggerClientEvent(ggshopwing, "setIDVisible", true, firstname, lastname, firstvisit);
                    API.shared.sendPictureNotificationToPlayer(ggshopwing, "Vorname: " + firstname + "\nNachname: " + lastname + "\nEinreise: " + firstvisit + "\n~w~Job: " + job + "", "CHAR_MP_FM_CONTACT", 0, 2, "Los Santos", "Personalausweis");

                    API.shared.sendNotificationToPlayer(client, "Sie haben Ihren Ausweis gezeigt!");

                    API.shared.delay(8000, true, () =>
                    {
                        API.shared.triggerClientEvent(ggshopwing, "setIDInvisible");
                    });
                    break;



            }
        }
        #endregion PlayerInteraction Menu Processing
    }
}