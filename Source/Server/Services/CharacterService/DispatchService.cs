using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
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
    class DispatchMenuService
    {

        public static void OpenDispatchMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "DispatchMenu_Open");
        }



        #region Player Interaction Menu
        public static List<MenuItem> BuildDispatchMenu(Player player, Client player123)
        {
            List<MenuItem> menuItemList = new List<MenuItem>();

            #region Taxi

            menuItemList.Add(new MenuItem
            {
                Title = "Taxi rufen",
                Description = "Hiermit kannst du ein Taxi rufen",
                Value1 = "dispatchtaxi"
            });

            #endregion Taxi
			
            #region EMS

            menuItemList.Add(new MenuItem
            {
                Title = "EMS rufen",
                Description = "Hiermit kannst du das EMS rufen",
                Value1 = "dispatchems"
            });

            #endregion EMS

            #region ACLS

            menuItemList.Add(new MenuItem
            {
                Title = "ACLS rufen",
                Description = "Hiermit kannst du das ACLS rufen",
                Value1 = "dispatchacls"
            });

            #endregion ACLS 

            #region Police

            menuItemList.Add(new MenuItem
            {
                Title = "Police rufen",
                Description = "Hiermit kannst du ein Police rufen",
                Value1 = "dispatchpolice"
            });

            #endregion Police			

            return menuItemList;
        }
        #endregion Player Interaction Menu

        #region Dispatches
        public static readonly List<Dispatch> Dispatches = new List<Dispatch>();
        #endregion Dispatches

        public static bool IsMedicOnline()
        {
            bool isOnline = false;
            API.shared.getAllPlayers().ForEach(otherclient => {
                if (otherclient.hasData("player"))
                {
                    Player otherplayer = otherclient.getData("player");
                    if (otherplayer.Character.Faction == FactionType.EMS && otherplayer.Character.OnDuty)
                    {
                        isOnline = true;
                    }
                }
            });
            return isOnline;
        }

        public static bool IsACLSOnline()
        {
            bool isOnline = false;
            API.shared.getAllPlayers().ForEach(otherclient => {
                if (otherclient.hasData("player"))
                {
                    Player otherplayer = otherclient.getData("player");
                    if (otherplayer.Character.Faction == FactionType.ACLS && otherplayer.Character.OnDuty)
                    {
                        isOnline = true;
                    }
                }
            });
            return isOnline;
        }

        public static bool IsPoliceOnline()
        {
            bool isOnline = false;
            API.shared.getAllPlayers().ForEach(otherclient => {
                if (otherclient.hasData("player"))
                {
                    Player otherplayer = otherclient.getData("player");
                    if (otherplayer.Character.Faction == FactionType.Police && otherplayer.Character.OnDuty)
                    {
                        isOnline = true;
                    }
                }
            });
            return isOnline;
        }


        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "DispatchMenu_Close");
        }

        #region PlayerInteraction Menu Processing
        public static void ProcessDispatchMenu(Client client, string itemvalue)
        {		
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            API.shared.triggerClientEvent(client, "DispatchMenu_Close");
            Client sender = null;
            
            switch (itemvalue)
            {
                case "dispatchtaxi":
                    API.shared.triggerClientEvent(sender, "useTaxis");
                    API.shared.call("Taxi", "useTaxis", sender);
                    API.shared.sendNotificationToPlayer(client, "~g~Der Dispatch wurde verschickt werden!");
                    break;

                case "dispatchems":
                    if (IsMedicOnline())
                    {
                        API.shared.getAllPlayers().ForEach(otherclient => {
                            if (otherclient.hasData("player"))
                            {
                                Player otherplayer = otherclient.getData("player");
                                if (otherplayer.Character.Faction == FactionType.EMS)
                                {
                                    Blip blip = API.shared.createBlip(client);
                                    blip.sprite = 305;
                                    blip.color = 1;
                                    blip.name = $"Dispatch: Er braucht Hilfe! | {player.Character.FirstName} {player.Character.LastName}";
                                    Dispatches.Add(
                                        new Dispatch
                                        {
                                            Client = client,
                                            Player = player,
                                            Time = DateTime.Now,
                                            Blip = blip
                                        });
                                    BlipService.BlipService.BlipList.Add(blip);
                                    API.shared.delay(300, true, () => {
                                        Dispatch dispatch = Dispatches.FirstOrDefault(x => x.Client == client);
                                        Dispatches.Remove(dispatch);
                                        API.shared.deleteEntity(dispatch.Blip);
                                    });
                                    API.shared.sendPictureNotificationToPlayer(otherclient, "Sie haben ein Dispatch auf ihr Navigation bekommen!", "CHAR_CASTRO", 0, 2, "Dienststelle", "EMS Information");
                                }
                            }
                        });
                    }
                    else
                    {
                        API.shared.sendNotificationToPlayer(client, "~g~Bei den EMS ist momentan keiner im Dienst!");
                    }
                    break;

                case "dispatchacls":
                    if (IsACLSOnline())
                    {
                        API.shared.getAllPlayers().ForEach(otherclient => {
                            if (otherclient.hasData("player"))
                            {
                                Player otherplayer = otherclient.getData("player");
                                if (otherplayer.Character.Faction == FactionType.ACLS)
                                {
                                    Blip blip = API.shared.createBlip(client);
                                    blip.sprite = 560;
                                    blip.color = 1;
                                    blip.name = $"Dispatch: Er braucht Hilfe! | {player.Character.FirstName} {player.Character.LastName}";
                                    Dispatches.Add(
                                        new Dispatch
                                        {
                                            Client = client,
                                            Player = player,
                                            Time = DateTime.Now,
                                            Blip = blip
                                        });
                                    BlipService.BlipService.BlipList.Add(blip);
                                    API.shared.delay(300, true, () => {
                                        Dispatch dispatch = Dispatches.FirstOrDefault(x => x.Client == client);
                                        Dispatches.Remove(dispatch);
                                        API.shared.deleteEntity(dispatch.Blip);
                                    });
                                    API.shared.sendPictureNotificationToPlayer(otherclient, "Sie haben ein Dispatch auf ihr Navigation bekommen!", "CHAR_ASHLEY", 0, 2, "Dienststelle", "ACLS Information");
                                }
                            }
                        });
                    }
                    else
                    {
                        API.shared.sendNotificationToPlayer(client, "~g~Bei den ACLS ist momentan keiner im Dienst!");
                    }
                    break;

                case "dispatchpolice":
                    if (IsPoliceOnline())
                    {
                        API.shared.getAllPlayers().ForEach(otherclient => {
                            if (otherclient.hasData("player"))
                            {
                                Player otherplayer = otherclient.getData("player");
                                if (otherplayer.Character.Faction == FactionType.Police)
                                {
                                    Blip blip = API.shared.createBlip(client);
                                    blip.sprite = 60;
                                    blip.color = 4;
                                    blip.name = $"Dispatch: Er braucht Hilfe! | {player.Character.FirstName} {player.Character.LastName}";
                                    Dispatches.Add(
                                        new Dispatch
                                        {
                                            Client = client,
                                            Player = player,
                                            Time = DateTime.Now,
                                            Blip = blip
                                        });
                                    BlipService.BlipService.BlipList.Add(blip);
                                    API.shared.delay(300, true, () => {
                                        Dispatch dispatch = Dispatches.FirstOrDefault(x => x.Client == client);
                                        Dispatches.Remove(dispatch);
                                        API.shared.deleteEntity(dispatch.Blip);
                                    });
                                    API.shared.sendPictureNotificationToPlayer(otherclient, "Sie haben ein Dispatch auf ihr Navigation bekommen!", "CHAR_CALL911", 0, 2, "Dienststelle", "Police Information");
                                }
                            }
                        });
                    }
                    else
                    {
                        API.shared.sendNotificationToPlayer(client, "~g~Bei den LSPD ist momentan keiner im Dienst!");
                    }
                    break;
            }
        }
        #endregion PlayerInteraction Menu Processing
    }
}