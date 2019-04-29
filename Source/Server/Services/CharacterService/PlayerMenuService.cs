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
    class PlayerMenuService
    {

        public object InventoryMaxCapacity;

        public static void OpenMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "PlayerMenu_Open", JsonConvert.SerializeObject(BuildInteractionMenu(player, client)));
        }



        #region Player Interaction Menu
        public static List<MenuItem> BuildInteractionMenu(Player player, Client player123)
        {
            Player otherplayer1;

            List<MenuItem> menuItemList = new List<MenuItem>();

            Client nextplayer = player.Character.Player;
            otherplayer1 = CharacterService.GetNextPlayerInNearOfPlayer(player);

            #region Name

            menuItemList.Add(new MenuItem
            {
                Title = "~g~Name : ~y~" + player.Character.FirstName + " " + player.Character.LastName + "",
                Value1 = "nonevaluegiven"
            });

            #endregion Name

            #region Phone

            menuItemList.Add(new MenuItem
            {
                Title = "~g~Phone : ~y~" + player.Character.Phone + "",
                Value1 = "nonevaluegiven"
            });

            #endregion Phone

            #region Level

            menuItemList.Add(new MenuItem
            {
                Title = "~g~Level : ~y~" + player.Character.Level.ToString(),
                Description = "Zeit bis Levelaufstieg: " + (120 - player.Character.TimeOnServer).ToString() + " Minuten",
                Value1 = "novaluegiven"
            });

            #endregion Level

            #region Fraktion

            menuItemList.Add(new MenuItem
            {
                Title = "~g~Fraktion : ~y~" + (Convert.ToString(player.Character.Faction)) + "",
                Description = "~g~Payday in : ~y~" + (60 - player.Character.TimeSincePayday).ToString() + " Minuten",
                Value1 = "nonevaluegiven"
            });

            #endregion Fraktion

            #region Inventar

            menuItemList.Add(new MenuItem
            {
                Title = "Tasche",
                Description = "Du hast " + ItemService.ItemService.ItemList.Count + " KG von Max. " + ItemService.ItemService.InventoryMaxCapacity + " KG in deiner Tasche",
                Value1 = "openinventory"
            });

            #endregion Inventar

            #region Dispatch
            menuItemList.Add(new MenuItem
            {
                Title = "Dispatch",
                Description = "Hiermit kannst du ein Dispatch abschicken",
                Value1 = "opendispatchmenu"
            });
            #endregion Dispatch

            #region Apps
            menuItemList.Add(new MenuItem
            {
                Title = "Apps",
                Description = "Hiermit kannst du die Apps benutzen",
                Value1 = "openplayerappsmenu"
            });
            #endregion Apps

            #region Geldbörse

            menuItemList.Add(new MenuItem
            {
                Title = "Geldbörse",
                Value1 = "opengeldmenu"
            });

            #endregion Geldbörse

            #region Player Animation

            menuItemList.Add(new MenuItem
            {
                Title = "Animationen",
                Value1 = "openplayeranimation"
            });

            #endregion Player Animation

            #region Player Clothes

            menuItemList.Add(new MenuItem
            {
                Title = "Klamotten",
                Value1 = "openplayerclothes"
            });

            #endregion Player Clothes

            #region Schlüssel

            menuItemList.Add(new MenuItem
            {
                Title = "Fahrzeugschlüssel",
                Description = "/givekey [Player name] [Schlüssel ID]",
                Value1 = "openschlüsselmenu"
            });

            #endregion Schlüssel

            #region Handsup
            if (!player.Character.Player.isInVehicle)
            {
                if (player.Character.HasHandsup)
                {
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Hände Runter",
                        Value1 = "togglehandsup"
                    });
                }
                else
                {
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Hände Hoch",
                        Value1 = "togglehandsup"
                    });
                }
            }
            #endregion Handsup

            #region Seatbelt Menu Item
            if (player.Character.Player.isInVehicle)
            {
                if (player.Character.Player.seatbelt)
                {
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Sicherheitsgurt öffnen",
                        Value1 = "toggleseatbelt"
                    });
                }
                else
                {
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Sicherheitsgurt schließen",
                        Value1 = "toggleseatbelt"
                    });
                }
            }
            #endregion Seatbelt Menu Item

            #region Faction Menu Item
            if (player.Character.OnDuty && player.Character.Faction != FactionType.Citizen)
            {
                switch (player.Character.Faction)
                {
                    case FactionType.Police:
                        menuItemList.AddRange(PoliceService.BuildInteractionMenu(player)); // Add Police Menu Items
                        break;
                    case FactionType.FIB:
                        menuItemList.AddRange(PoliceService.BuildInteractionMenu(player)); // Add FIB Menu Items
                        break;
                    case FactionType.EMS:
                        menuItemList.AddRange(EMSService.BuildInteractionMenu(player)); // Add EMS Menu Items
                        break;
                    case FactionType.SWAT:
                        menuItemList.AddRange(PoliceService.BuildInteractionMenu(player)); // Add FIB Menu Items
                        break;
                    case FactionType.Sheriff:
                        menuItemList.AddRange(SheriffService.BuildInteractionMenu(player)); // Add FIB Menu Items
                        break;
                    case FactionType.ACLS:
                        menuItemList.AddRange(ACLSService.BuildInteractionMenu(player)); // Add FIB Menu Items
                        break;
                }
            }
            #endregion Faction Menu Item

            return menuItemList;
        }
        #endregion Player Interaction Menu


        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
        }

        #region PlayerInteraction Menu Processing
        public static void ProcessInteractionMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            if (player.Character.Faction != FactionType.Citizen)
            {
                switch (player.Character.Faction)
                {
                    case FactionType.Police:
                        PoliceService.ProcessInteractionMenu(client, itemvalue);
                        break;
                    case FactionType.EMS:
                        EMSService.ProcessInteractionMenu(client, itemvalue);
                        break;
                    case FactionType.FIB:
                        PoliceService.ProcessInteractionMenu(client, itemvalue);
                        break;
                    case FactionType.SWAT:
                        PoliceService.ProcessInteractionMenu(client, itemvalue);
                        break;
                    case FactionType.JVA:
                        PoliceService.ProcessInteractionMenu(client, itemvalue);
                        break;
                    case FactionType.Sheriff:
                        SheriffService.ProcessInteractionMenu(client, itemvalue);
                        break;
                    case FactionType.ACLS:
                        ACLSService.ProcessInteractionMenu(client, itemvalue);
                        break;
                }
            }

            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
            switch (itemvalue)
            {
                #region Seatbelt Menu Item
                case "toggleseatbelt":
                    if (!client.isInVehicle) { return; }
                    if (client.seatbelt)
                    {
                        client.seatbelt = false;
                        API.shared.sendNotificationToPlayer(client, "Sicherheitsgurt ~r~abgelegt");
                    }
                    else
                    {
                        client.seatbelt = true;
                        API.shared.sendNotificationToPlayer(client, "Sicherheitsgurt ~g~angelegt");
                    }
                    break;
                #endregion Seatbelt Menu Item

                case "togglehandsup":
                    PoliceService.TogglePlayerHandsup(player);
                    break;

                #region Inventar
                case "openinventory":
                    if (!client.hasData("player")) { return; }
                    player = client.getData("player");
                    if (player.Character.IsDeath || player.Character.IsCuffed) { return; }
                    API.shared.triggerClientEvent(client, "Inventory_Open", JsonConvert.SerializeObject(CharacterService.GetInventoryMenuItems(client)));
                    break;

                #endregion Inventar

                #region Geldbörse
                case "opengeldmenu":
                    API.shared.triggerClientEvent(client, "Geldbörse_Open", JsonConvert.SerializeObject(GeldbörseMenuService.BuildGeldbörsenMenu(player, client)));
                    break;
                #endregion Geldbörse

                #region Dispatch
                case "opendispatchmenu":
                    API.shared.triggerClientEvent(client, "DispatchMenu_Open", JsonConvert.SerializeObject(DispatchMenuService.BuildDispatchMenu(player, client)));
                    break;
                #endregion Dispatch

                #region Apps
                case "openplayerappsmenu":
                    API.shared.triggerClientEvent(client, "PlayerApps_Open", JsonConvert.SerializeObject(PlayerAppsMenuService.BuildPlayerAppsMenu(player, client)));
                    break;
                #endregion Apps

                #region Player Animation
                case "openplayeranimation":
                    API.shared.triggerClientEvent(client, "PlayerAnimation_Open", JsonConvert.SerializeObject(PlayerAnimationMenuService.BuildPlayerAnimationMenu(player, client)));
                    break;
                #endregion Player Animation

                #region Player Clothes
                case "openplayerclothes":
                    API.shared.triggerClientEvent(client, "PlayerClothes_Open", JsonConvert.SerializeObject(PlayerClothesMenuService.BuildPlayerClothesMenu(player, client)));
                    break;
                #endregion Player Clothes

                #region Schlüssel
                case "openschlüsselmenu":

                    API.shared.triggerClientEvent(client, "Schlüssel_Open", JsonConvert.SerializeObject(SchlüsselMenuService.BuildSchlüsselMenu(player, client)));
                    break;

                #endregion Schlüssel


            }
        }
        #endregion PlayerInteraction Menu Processing
    }
}