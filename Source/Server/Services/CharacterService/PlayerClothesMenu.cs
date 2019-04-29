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
    class PlayerClothesMenuService
    {

        public static void OpenMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "PlayerClothes_Open");
        }



        #region Player Interaction Menu
        public static List<MenuItem> BuildPlayerClothesMenu(Player player, Client player123)
        {
            Player otherplayer1;

            List<MenuItem> menuItemList = new List<MenuItem>();

            Client nextplayer = player.Character.Player;
            otherplayer1 = CharacterService.GetNextPlayerInNearOfPlayer(player);

            #region Accessoires
            if (player.Character.ClothesAccessories == 1)
            {

            }
            else
                if (player123.getClothesDrawable(7) > 1)
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "Accessoires",
                    Value1 = "accessoriesevent"
                });
            }
            else
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "Accessoires",
                    Value1 = "accessoriesevent1"
                });
            }

            #endregion Backpacks

            #region Maske
            if (player.Character.ClothesBackpacks == 1)
            {

            }
            else
                if (player123.getClothesDrawable(5) > 1)
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "Rucksack",
                    Value1 = "backpacksevent"
                });
            }
            else
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "Rucksack",
                    Value1 = "backpacksevent1"
                });
            }

            #endregion Backpacks

            #region Maske
            if (player.Character.ClothesMasks == 1)
            {

            }
            else
                if (player123.getClothesDrawable(1) > 1)
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "Maske",
                    Value1 = "maskeevent"
                });
            }
            else
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "Maske",
                    Value1 = "maskeevent1"
                });
            }

            #endregion Maske	

            return menuItemList;
        }
        #endregion Player Interaction Menu

        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PlayerClothes_Close");
        }

        #region PlayerInteraction Menu Processing
        public static void ProcessPlayerClothesMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            API.shared.triggerClientEvent(client, "PlayerClothes_Close");
            
            switch (itemvalue)
            {
                case "accessoriesevent":
                    int state2 = 0;
                    Dictionary<string, string> parameters2 = new Dictionary<string, string>
                    {
                        { "@Id", player.Character.Id.ToString() },
                        { "@Statusid", state2.ToString() }
                    };

                    DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Accessoriesstate = @Statusid  WHERE Id = @Id", parameters2);
                    client.setClothes(7, 0, 0);
                    break;
                case "accessoriesevent1":
                    int state3 = 1;
                    Dictionary<string, string> parameters3 = new Dictionary<string, string>
                    {
                        { "@Id", player.Character.Id.ToString() },
                        { "@Statusid", state3.ToString() }
                    };

                    DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Accessoriesstate = @Statusid  WHERE Id = @Id", parameters3);
                    Clothing accessories = ClothingService.ClothingService.AccessoriesList.FirstOrDefault(x => x.Id == player.Character.ClothesAccessories);
                    if (accessories == null)
                    {
                        return;
                    }
                    else
                        client.setClothes((int)CharacterComponents.Accessories, accessories.Drawable, accessories.Texture);
                    break;

                case "backpacksevent":
                    int state4 = 0;
                    Dictionary<string, string> parameters6 = new Dictionary<string, string>
                    {
                        { "@Id", player.Character.Id.ToString() },
                        { "@Statusid", state4.ToString() }
                    };

                    DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Backpackstate = @Statusid  WHERE Id = @Id", parameters6);
                    client.setClothes(5, 0, 0);
                    break;
                case "backpacksevent1":
                    int state5 = 1;
                    Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                        { "@Id", player.Character.Id.ToString() },
                        { "@Statusid", state5.ToString() }
                    };

                    DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Backpackstate = @Statusid  WHERE Id = @Id", parameters);
                    Clothing backpacks = ClothingService.ClothingService.BackpacksList.FirstOrDefault(x => x.Id == player.Character.ClothesBackpacks);
                    if (backpacks == null)
                    {
                        return;
                    }
                    else
                        client.setClothes((int)CharacterComponents.Backpack, backpacks.Drawable, backpacks.Texture);
                    break;

                case "maskeevent":
                    int state6 = 0;
                    Dictionary<string, string> parameters5 = new Dictionary<string, string>
                    {
                        { "@Id", player.Character.Id.ToString() },
                        { "@Statusid", state6.ToString() }
                    };

                    DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Maskstate = @Statusid  WHERE Id = @Id", parameters5);
                    client.setClothes(1, 0, 0);
                    break;
                case "maskeevent1":
                    int state7 = 1;
                    Dictionary<string, string> parameters7 = new Dictionary<string, string>
                    {
                        { "@Id", player.Character.Id.ToString() },
                        { "@Statusid", state7.ToString() }
                    };

                    DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Maskstate = @Statusid  WHERE Id = @Id", parameters7);
                    Clothing masks = ClothingService.ClothingService.MasksList.FirstOrDefault(x => x.Id == player.Character.ClothesMasks);
                    if (masks == null)
                    {
                        return;
                    }
                    else
                        client.setClothes((int)CharacterComponents.Mask, masks.Drawable, masks.Texture);
                    break;
            }
        }
        #endregion PlayerInteraction Menu Processing
    }
}