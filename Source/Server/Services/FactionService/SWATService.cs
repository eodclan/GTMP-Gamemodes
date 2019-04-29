using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FactionLife.Server.Services.FactionService
{
    class SWATService
        : Script
    {
        public SWATService()
        {
            API.onClientEventTrigger += OnClientEvent;
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            switch (eventName)
            {
                case "Police_ApplyCuffedAnimation":
                    API.playPlayerAnimation(client, (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl), "mp_arresting", "idle");
                    break;
            }
        }


        public static void ChangeClothing(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");


            if ((API.shared.getEntitySyncedData(client, "has_clothes") == false) || (API.shared.getEntitySyncedData(client, "has_clothes") == null) || (!API.shared.hasEntitySyncedData(client, "has_clothes")))
            {
                if (player.Character.Gender == Gender.Male)
                {
                    ClothingService.ClothingService.ApplyOutfit(client, 7); // Male SWAT Outfit
                    API.shared.setEntitySyncedData(client, "has_clothes", true);
                }
                else
                {
                    ClothingService.ClothingService.ApplyOutfit(client, 8); // Female SWAT Outfit
                    API.shared.setEntitySyncedData(client, "has_clothes", true);
                }
            }

            else { CharacterService.CharacterService.ApplyAppearance(client); API.shared.setEntitySyncedData(client, "has_clothes", false); }


        }


        #region Army Weapon Menu
        public static List<MenuItem> BuildWeaponMenu(Player player)
        {
            List<MenuItem> menuItemList = new List<MenuItem>();

            // SWAT Rank 0 Weapons
            menuItemList.Add(new MenuItem
            {
                Title = "Schutzweste v2",
                Value1 = "item35"
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.StunGun.ToString(),
                Value1 = WeaponHash.StunGun.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.SpecialCarbine.ToString(),
                Value1 = WeaponHash.SpecialCarbine.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.HeavySniper.ToString(),
                Value1 = WeaponHash.HeavySniper.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.Knife.ToString(),
                Value1 = WeaponHash.Knife.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.BZGas.ToString(),
                Value1 = WeaponHash.BZGas.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.Pistol50.ToString(),
                Value1 = WeaponHash.Pistol50.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.CombatMG.ToString(),
                Value1 = WeaponHash.CombatMG.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.PumpShotgun.ToString(),
                Value1 = WeaponHash.PumpShotgun.ToString()
            });


            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.AssaultShotgun.ToString(),
                Value1 = WeaponHash.AssaultShotgun.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.APPistol.ToString(),
                Value1 = WeaponHash.APPistol.ToString()
            });

            menuItemList.Add(new MenuItem
            {
                Title = WeaponHash.Parachute.ToString(),
                Value1 = WeaponHash.Parachute.ToString()
            });


            return menuItemList;
        }

        public static void ProcessWeaponMenu(Client client, string itemvalue)


        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            if (itemvalue == "item35")
            {
                InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 35);
                if (invitem == null)
                {
                    player.Character.Inventory.Add(new InventoryItem
                    {
                        ItemID = 35,
                        Count = 1
                    });
                }
                else
                {
                   invitem.Count++;
                    
                }
                API.shared.sendChatMessageToPlayer(client, "~y~Sie erhielten eine Schutzweste!");
                return;
            }

            if (player.Character.Faction != FactionType.SWAT || !player.Character.OnDuty) { return; }
            API.shared.givePlayerWeapon(client, API.shared.weaponNameToModel(itemvalue), 120, true);
            CharacterService.CharacterService.UpdatePlayerWeapons(player.Character);
            API.shared.sendNotificationToPlayer(client, "~y~ARMORY~w~~n~Sie erhielten ein ~b~" + itemvalue + ".");
            API.shared.triggerClientEvent(client, "Army_CloseWeaponMenu");
        }



        public static void OpenWeaponMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "Army_OpenWeaponMenu", JsonConvert.SerializeObject(BuildWeaponMenu(player)));
        }
        #endregion Army Weapon Menu

    }
}