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
using System.Data;
using System.Linq;
using System.Timers;
using System.ComponentModel;


namespace FactionLife.Server.Services.ItemService
{
    class ItemService
    {
        public static readonly List<Item> ItemList = new List<Item>();

        public static int PrimaryColor { get; private set; }
        public static int SecondaryColor { get; private set; }
        public static int WheelColor { get; private set; }
        public static int WheelType { get; private set; }
        public static bool cuffed { get; private set; }
        public static NetHandle HVehicle { get; private set; }

        public int InventoryMaxCapacity { get; set; } = 15;

        public static void LoadItemsFromDB()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM items", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    ItemList.Add(new Item
                    {
                        Id = (int)row["Id"],
                        Name = (string)row["Name"],
                        Description = (string)row["Description"],
                        Type = (ItemType)((int)row["Type"]),
                        Weight = (int)row["Weight"],
                        DefaultPrice = (double)row["DefaultPrice"],
                        DefaultSellPrice = (double)row["DefaultSellPrice"],
                        Value1 = (int)row["Value1"],
                        Value2 = (int)row["Value2"],
                        Sellable = Convert.ToBoolean((int)row["Sellable"]),
                        Weapon = (string)row["Weapon"]
                    });
                }
                API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Items Loaded..");
            }
            else
            {
                API.shared.consoleOutput(LogCat.Info, "No Items Loaded..");
            }
        }

        public static void SetPlayerCuffed(Client client, bool cuffed)
        {
            API.shared.stopPlayerAnimation(client);
            API.shared.freezePlayer(client, false);
            if (cuffed) API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl), "mp_arresting", "idle");
            API.shared.triggerClientEvent(client, "Client_cuffed", cuffed);
            client.setSyncedData("cuffed", cuffed);
            if (!cuffed) { client.resetSyncedData("cuffed"); }
            API.shared.triggerClientEvent(client, "Client_Cuffed", cuffed);
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            player.Character.IsCuffed = cuffed;
            player.Character.HasHandsup = false;
        }

        public static void ItemAction(Client client, Player player, Item item, bool cuffed)
        {
            OwnedVehicle ownedVehicle = null;
            Player otherPlayer = null;

            switch (item.Type)
            {
                case ItemType.Special:
                    break;
                case ItemType.Food: // Value 1 = Hunger | Value 2 = Thirst
                    player.Character.Hunger += item.Value1;
                    player.Character.Thirst += item.Value2;
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.OnlyAnimateUpperBody), "mp_player_inteat@burger", "mp_player_int_eat_burger");
                    API.shared.delay(2500, true, () => {
                        API.shared.stopPlayerAnimation(client);
                    });
                    break;
                case ItemType.Drink: // Value 1 = Hunger | Value 2 = Thirst
                    player.Character.Hunger += item.Value1;
                    player.Character.Thirst += item.Value2;
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.OnlyAnimateUpperBody), "mp_player_intdrink", "loop");
                    API.shared.delay(2500, true, () => {
                        API.shared.stopPlayerAnimation(client);
                    });
                    break;
                case ItemType.PlayerBinding:
                    otherPlayer = CharacterService.CharacterService.GetNextPlayerInNearOfPlayer(player);
                    if (otherPlayer != null)
                    {
                        SetPlayerCuffed(otherPlayer.Character.Player, !otherPlayer.Character.IsCuffed);
                    }
                    API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Du hast deine Geisel festgenommen!");
                    break;
                case ItemType.PlayerUnbinding:
                    otherPlayer = CharacterService.CharacterService.GetNextPlayerInNearOfPlayer(player);
                    if (otherPlayer != null)
                    {
                        SetPlayerCuffed(otherPlayer.Character.Player, !otherPlayer.Character.IsCuffed);
                    }
                    API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Du hast deine Geisel frei gelassen!");
                    break;
                case ItemType.Medic: // Value 1 = Heal Amount | Value 2 = none
                    client.health += item.Value1;
                    break;
                case ItemType.Repair: // Value 1 = Repair Amount | Value 2 = none
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.ActiveHandle.repair();
                    ownedVehicle.EngineHealth = 4000;
                    ownedVehicle.MaxStorage = 35;
                    break;
                case ItemType.FuelCanister:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Fuel += item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("fuel", ownedVehicle.Fuel);
                    break;
                case ItemType.WeaponItem:
                    API.shared.delay(1500, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "<C>Ammu-Nation:</C>~r~Eine Waffe ist nicht immer der Richtige Weg.");
                    });
                    //API.shared.givePlayerWeapon(client, API.shared.weaponNameToModel(Convert.ToString(weapon)), 120, true);
                    API.shared.givePlayerWeapon(client, API.shared.weaponNameToModel(item.Weapon), 120, true);
                    API.shared.setPlayerWeaponAmmo(client, API.shared.weaponNameToModel(item.Weapon), 120 + API.shared.getPlayerWeaponAmmo(client, API.shared.weaponNameToModel("Pistol")));
                    break;
                case ItemType.VehicleColor:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.PrimaryColor = 0;
                    ownedVehicle.SecondaryColor = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("color1", ownedVehicle.PrimaryColor);
                    ownedVehicle.ActiveHandle.setSyncedData("color2", ownedVehicle.SecondaryColor);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.PrimaryColor = item.Value1;
                    ownedVehicle.SecondaryColor = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("color1", ownedVehicle.PrimaryColor);
                    ownedVehicle.ActiveHandle.setSyncedData("color2", ownedVehicle.SecondaryColor);
                    break;
                case ItemType.VehicleReifenFarbe:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.FrontWheels = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("FrontWheels", ownedVehicle.FrontWheels);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Reifen Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.FrontWheels = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("23", ownedVehicle.FrontWheels);
                    break;
                case ItemType.VehicleReifenTyp:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.BackWheels = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("22", ownedVehicle.BackWheels);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Reifen Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.BackWheels = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("22", ownedVehicle.BackWheels);
                    break;
                case ItemType.VehicleSpoilers:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Spoilers = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("MOD_SPOILER", ownedVehicle.Spoilers);
                    API.shared.delay(1200, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Spoiler wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.Spoilers = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("MOD_SPOILER", ownedVehicle.Spoilers);
                    break;
                case ItemType.VehicleXenonHeadlights:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Xenon = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("xenon", ownedVehicle.Xenon);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.Xenon = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("xenon", ownedVehicle.Xenon);
                    break;
                case ItemType.VehicleExhaust:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Exhaust = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("exhaust", ownedVehicle.Exhaust);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.Exhaust = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("exhaust", ownedVehicle.Exhaust);
                    break;
                case ItemType.VehicleFender:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Fender = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("fender", ownedVehicle.Fender);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.Fender = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("fender", ownedVehicle.Fender);
                    break;
                case ItemType.VehicleRightFender:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.RightFender = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("rightfender", ownedVehicle.RightFender);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.RightFender = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("rightfender", ownedVehicle.RightFender);
                    break;
                case ItemType.VehicleHorns:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Horns = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("horns", ownedVehicle.Horns);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.Horns = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("horns", ownedVehicle.Horns);
                    break;
                case ItemType.VehicleHood:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Hood = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("hood", ownedVehicle.Hood);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.Hood = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("hood", ownedVehicle.Hood);
                    break;
                case ItemType.VehicleTurbo:
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return;
                    }
                    ownedVehicle.Turbo = 0;
                    ownedVehicle.ActiveHandle.setSyncedData("turbo", ownedVehicle.Turbo);
                    API.shared.delay(1000, true, () => {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug Farbe wird nun geändert!");
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Fahrzeug nun einparken und wieder ausparken!");
                        API.shared.stopPlayerAnimation(client);
                    });
                    ownedVehicle.Turbo = item.Value1;
                    ownedVehicle.ActiveHandle.setSyncedData("turbo", ownedVehicle.Turbo);
                    break;
                case ItemType.Drug: // Value 1 = DrugType | Value 2 = Intense
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missfbi3_party_d", "walk_from_a_to_b_male2");
                    InterfaceService.ScreenService.PlayScreenEffect(client, "DrugsMichaelAliensFightIn", 1, true);
                    API.shared.delay(60000, true, () => {
                        InterfaceService.ScreenService.FadeScreenIn(client, 2000);
                        InterfaceService.ScreenService.StopAllScreenEffects(client);
                        InterfaceService.ScreenService.SetHudVisible(client, true);
                        API.shared.stopPlayerAnimation(client);
                    });
                    break;
                case ItemType.License: // Value 1 = LicenseType | Value 7 = Intense
                    API.shared.sendNotificationToPlayer(player.Character.Player, "<C>State Center</C> ~r~Du kannst diese Lizenz nicht weg geben!");
                    break;
                case ItemType.Geschenk: // Value 1 = Repair Amount | Value 2 = none
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "anim@heists@heist_corona@single_team", "des_floor_root");
                    break;
                case ItemType.Farmer: // Value 1 = DrugType | Value 2 = Intense
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    break;
                case ItemType.Organe: // Value 1 = DrugType | Value 2 = Intense
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Magazin:
                    break;



            }

            InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == item.Id);

            invitem.Count--;
            if (invitem.Count <= 0)
                player.Character.Inventory.Remove(invitem);
            CharacterService.CharacterService.UpdateHUD(client);
            UpdateInventar(client);
            API.shared.sendNotificationToPlayer(player.Character.Player, "Sie verwenden ein ~g~" + item.Name);
        }

        public static bool UseItem(Client client, int id)
        {
            if (!client.hasData("player"))
                return false;
            if (client.hasData("usagetimer"))
            {
                API.shared.sendNotificationToPlayer(client, "~r~Du hast bereits eine Aktion gestartet..");
                return false;
            }
            Player player = client.getData("player");
            InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == id);
            InventoryItem.invitem = 15;

            if (client.hasData("invitem"))
            {
                API.shared.sendNotificationToPlayer(client, "~r~Du kannst nicht mehr davon tragen.");
                return false;
            }

            Player otherPlayer = null;

            if (invitem == null)
                return false;
            Item item = ItemList.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return false;
            OwnedVehicle ownedVehicle = null;
            switch (item.Type)
            {
                case ItemType.Special:
                    StartUsageTimer(client, player, item, 0);
                    break;
                case ItemType.Food: // Value 1 = Hunger | Value 2 = Thirst
                    StartUsageTimer(client, player, item, 0);
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.OnlyAnimateUpperBody), "mp_player_inteat@burger", "loop");
                    API.shared.delay(1500, true, () => {
                        API.shared.stopPlayerAnimation(client);
                    });
                    break;
                case ItemType.Drink: // Value 1 = Hunger | Value 2 = Thirst
                    StartUsageTimer(client, player, item, 0);
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.OnlyAnimateUpperBody), "mp_player_intdrink", "mp_player_intdrink");
                    API.shared.delay(1500, true, () => {
                        API.shared.stopPlayerAnimation(client);
                    });
                    break;
                case ItemType.PlayerBinding:
                    otherPlayer = CharacterService.CharacterService.GetNextPlayerInNearOfPlayer(player);
                    if (otherPlayer != null)
                    {
                        SetPlayerCuffed(otherPlayer.Character.Player, !otherPlayer.Character.IsCuffed);
                    }
                    API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Du hast deine Geisel festgenommen!");
                    break;
                case ItemType.PlayerUnbinding:
                    otherPlayer = CharacterService.CharacterService.GetNextPlayerInNearOfPlayer(player);
                    if (otherPlayer != null)
                    {
                        SetPlayerCuffed(otherPlayer.Character.Player, !otherPlayer.Character.IsCuffed);
                    }
                    API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Du hast deine Geisel frei gelassen!");
                    break;
                case ItemType.Medic: // Value 1 = Heal Amount | Value 2 = none
                    StartUsageTimer(client, player, item, 10);
                    break;
                case ItemType.Repair: // Value 1 = Repair Amount | Value 2 = none
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "anim@amb@garage@chassis_repair@", "base_amy_skater_01");
                    StartUsageTimer(client, player, item, 10);
                    break;
                case ItemType.FuelCanister:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "anim@amb@garage@chassis_repair@", "base_amy_skater_01");
                    StartUsageTimer(client, player, item, 5);
                    break;
                case ItemType.WeaponItem:
                    API.shared.givePlayerWeapon(client, API.shared.weaponNameToModel(item.Weapon), 120, true);
                    API.shared.setPlayerWeaponAmmo(client, API.shared.weaponNameToModel(item.Weapon), 120 + API.shared.getPlayerWeaponAmmo(client, API.shared.weaponNameToModel("Pistol")));
                    break;
                case ItemType.VehicleColor:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleReifenFarbe:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleReifenTyp:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleSpoilers:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleXenonHeadlights:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleExhaust:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleFender:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleRightFender:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleHorns:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleHood:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.VehicleTurbo:
                    ownedVehicle = VehicleService.VehicleService.OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 2);
                    if (ownedVehicle == null)
                    {
                        API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Kann kein Fahrzeug finden ..");
                        return false;
                    }
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "missheistdockssetup1clipboard@idle_a", "idle_b");
                    StartUsageTimer(client, player, item, 5);
                    API.shared.stopPlayerAnimation(client);
                    break;
                case ItemType.Drug: // Value 1 = DrugType | Value 2 = Intense
                    StartUsageTimer(client, player, item, 0);
                    break;
                case ItemType.License: // Value 7 = LicenseType | Value 1 = Verkaufen
                    API.shared.sendNotificationToPlayer(player.Character.Player, "~r~Du kannst diese Lizenz nicht weg geben!");
                    break;
                case ItemType.Geschenk: // Value 1 = Repair Amount | Value 2 = none
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "anim@heists@heist_corona@single_team", "des_floor_root");
                    StartUsageTimer(client, player, item, 10);
                    break;
                case ItemType.Farmer: // Value 1 = DrugType | Value 2 = Intense
                    API.shared.playPlayerAnimation(client, (int)(AnimationFlags.Loop), "anim@heists@heist_corona@single_team", "sarcastic_right_facial");
                    StartUsageTimer(client, player, item, 10);
                    break;
                case ItemType.Organe: // Value 1 = DrugType | Value 2 = Intense
                    break;
                case ItemType.Weapon:
                    if ((item.Name) == "PDW")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.CombatPDW, 10, true);
                    }
                    else if ((item.Name) == "AssaultRifle")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.AssaultRifle, 10, true);
                    }
                    else if ((item.Name) == "Knife")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Knife, 10, true);
                    }
                    else if ((item.Name) == "Hammer")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Hammer, 10, true);
                    }
                    else if ((item.Name) == "Bat")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Bat, 10, true);
                    }
                    else if ((item.Name) == "Crowbar")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Crowbar, 10, true);
                    }
                    else if ((item.Name) == "GolfClub")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.GolfClub, 10, true);
                    }
                    else if ((item.Name) == "Bottle")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Bottle, 10, true);
                    }
                    else if ((item.Name) == "Dagger")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Dagger, 10, true);
                    }
                    else if ((item.Name) == "Hatchet")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Hatchet, 10, true);
                    }
                    else if ((item.Name) == "KnuckleDuster")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.KnuckleDuster, 10, true);
                    }
                    else if ((item.Name) == "Machete")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Machete, 10, true);
                    }
                    else if ((item.Name) == "Flashlight")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Flashlight, 10, true);
                    }
                    else if ((item.Name) == "SwitchBlade")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.SwitchBlade, 10, true);
                    }
                    else if ((item.Name) == "PoolCue")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.PoolCue, 10, true);
                    }
                    else if ((item.Name) == "Wrench")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Wrench, 10, true);
                    }
                    else if ((item.Name) == "BattleAxe")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.BattleAxe, 10, true);
                    }
                    else if ((item.Name) == "Pistol")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Pistol, 10, true);
                    }
                    else if ((item.Name) == "CombatPistol")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.CombatPistol, 10, true);
                    }
                    else if ((item.Name) == "Pistol50")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Pistol50, 10, true);
                    }
                    else if ((item.Name) == "SNSPistol")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.SNSPistol, 10, true);
                    }
                    else if ((item.Name) == "HeavyPistol")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.HeavyPistol, 10, true);
                    }
                    else if ((item.Name) == "VintagePistol")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.VintagePistol, 10, true);
                    }
                    else if ((item.Name) == "APPistol")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.APPistol, 10, true);
                    }
                    else if ((item.Name) == "Gusenberg")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Gusenberg, 10, true);
                    }
                    else if ((item.Name) == "DoubleBarrelShotgun")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.DoubleBarrelShotgun, 10, true);
                    }
                    else if ((item.Name) == "SniperRifle")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.SniperRifle, 10, true);
                    }
                    else if ((item.Name) == "Molotov")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Molotov, 10, true);
                    }
                    else if ((item.Name) == "Flare")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.Flare, 10, true);
                    }
                    else if ((item.Name) == "MicroSMG")
                    {
                        API.shared.givePlayerWeapon(client, WeaponHash.MicroSMG, 10, true);
                    }

                    StartUsageTimer(client, player, item, 0);
                    CharacterService.CharacterService.UpdateCharacter(player.Character);
                    break;
                case ItemType.Magazin:

                    int currentAmmo;

                    WeaponHash currentWeapon = API.shared.getPlayerCurrentWeapon(client);

                    if (((item.Name) == "PDWMagazin") && (currentWeapon == WeaponHash.CombatPDW)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.CombatPDW);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.CombatPDW, currentAmmo + 90);
                    }
                    else if (((item.Name) == "AKMagazin") && (currentWeapon == WeaponHash.AssaultRifle)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.AssaultRifle);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.AssaultRifle, currentAmmo + 90);
                    }
                    else if (((item.Name) == "PistolMagazin") && (currentWeapon == WeaponHash.Pistol)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.Pistol);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.Pistol, currentAmmo + 90);
                    }
                    else if (((item.Name) == "CombatMagazin") && (currentWeapon == WeaponHash.CombatPistol)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.CombatPistol);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.CombatPistol, currentAmmo + 90);
                    }
                    else if (((item.Name) == "Pistol50Magazin") && (currentWeapon == WeaponHash.Pistol50)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.Pistol50);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.Pistol50, currentAmmo + 90);
                    }
                    else if (((item.Name) == "SNSPMagazin") && (currentWeapon == WeaponHash.SNSPistol)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.SNSPistol);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.SNSPistol, currentAmmo + 90);
                    }
                    else if (((item.Name) == "HeavyMagazin") && (currentWeapon == WeaponHash.HeavyPistol)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.HeavyPistol);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.HeavyPistol, currentAmmo + 90);
                    }
                    else if (((item.Name) == "VintagMagazin") && (currentWeapon == WeaponHash.VintagePistol)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.VintagePistol);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.VintagePistol, currentAmmo + 90);
                    }
                    else if (((item.Name) == "APPMagazin") && (currentWeapon == WeaponHash.APPistol)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.APPistol);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.APPistol, currentAmmo + 90);
                    }
                    else if (((item.Name) == "GusenbergMagazin") && (currentWeapon == WeaponHash.Gusenberg)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.Gusenberg);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.Gusenberg, currentAmmo + 90);
                    }
                    else if (((item.Name) == "DoubleBarrelShotgunMagazin") && (currentWeapon == WeaponHash.DoubleBarrelShotgun)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.DoubleBarrelShotgun);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.DoubleBarrelShotgun, currentAmmo + 90);
                    }
                    else if (((item.Name) == "SniperRifleMagazin") && (currentWeapon == WeaponHash.SniperRifle)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.SniperRifle);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.SniperRifle, currentAmmo + 90);
                    }
                    else if (((item.Name) == "MicroSMGMagazin") && (currentWeapon == WeaponHash.MicroSMG)) {
                        currentAmmo = API.shared.getPlayerWeaponAmmo(client, WeaponHash.MicroSMG);
                        API.shared.setPlayerWeaponAmmo(client, WeaponHash.MicroSMG, currentAmmo + 90);
                    }

                    else {
                            API.shared.sendNotificationToPlayer(client, "~b~Sie müssen die Waffe in der Hand halten");
                            break;
                    }
                    StartUsageTimer(client, player, item, 0);
                    CharacterService.CharacterService.UpdateCharacter(player.Character);
                    break;
            }

            API.shared.triggerClientEvent(client, "Inventory_Success");
            return true;
        }

        public static void GiveItem(Client client, int itemId, int count)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
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
                API.shared.sendNotificationToPlayer(client, "~r~Zu viele Leute um dich herum..");
                return;
            }
            if (playersaround.Count < 1)
            {
                API.shared.sendNotificationToPlayer(client, "~r~Niemand ist um dich herum..");
                return;
            }
            Player target = playersaround.FirstOrDefault();
            if (target == null) { API.shared.sendNotificationToPlayer(client, "~r~Ooops something went wrong.."); return; }
            Item item = ItemList.FirstOrDefault(x => x.Id == itemId);
            if (item == null) { return; }
            InventoryItem plrinvitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == itemId);
            if (plrinvitem == null || plrinvitem.Count <= 0 || count <= 0) { return; }
            if (plrinvitem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Du hast nicht genügend Gegenstände dieser Art in deinem Inventar!"); return; }

            InventoryItem targetinvitem = target.Character.Inventory.FirstOrDefault(x => x.ItemID == itemId);
            if (targetinvitem == null)
            {
                target.Character.Inventory.Add(new InventoryItem
                {
                    ItemID = itemId,
                    Count = count
                });
            }
            else
            {
                targetinvitem.Count += count;
            }
            API.shared.sendNotificationToPlayer(player.Character.Player, "Du gibts ~b~" + count + "x ~g~" + item.Name + "~w~ weg");
            API.shared.sendNotificationToPlayer(target.Character.Player, "Du erhältst ~b~" + count + "x ~g~" + item.Name);

            plrinvitem.Count -= count;
            if (plrinvitem.Count <= 0)
            {
                player.Character.Inventory.Remove(plrinvitem);
            }

            CharacterService.CharacterService.UpdateCharacter(player.Character);
            CharacterService.CharacterService.UpdateCharacter(target.Character);
            UpdateInventar(client);
            API.shared.triggerClientEvent(client, "Inventory_Success");
        }

        public static void ThrowAway(Client client, int itemId, int count)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            Item item = ItemList.FirstOrDefault(x => x.Id == itemId);
            if (item == null) { return; }
            InventoryItem plrinvitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == itemId);
            if (plrinvitem == null || plrinvitem.Count <= 0 || count <= 0) { return; }
            if (plrinvitem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Du hast nicht genügend Gegenstände dieser Art in deinem Inventar!"); return; }
            plrinvitem.Count -= count;
            if (plrinvitem.Count <= 0)
            {
                player.Character.Inventory.Remove(plrinvitem);
            }
            API.shared.sendNotificationToPlayer(player.Character.Player, "Du wirfst ~b~" + count + "x ~g~" + item.Name + " weg");
            UpdateInventar(client);
            API.shared.triggerClientEvent(client, "Inventory_Success");
        }

        public static void UpdateInventar(Client client)
        {
            API.shared.triggerClientEvent(client, "Inventory_Data", JsonConvert.SerializeObject(CharacterService.CharacterService.GetInventoryMenuItems(client)));
        }

        public static void StartUsageTimer(Client client, Player player, Item item, int time)
        {
            if (time <= 0)
            {
                ItemAction(client, player, item, cuffed);
                return;
            }
            int count = 0;
            InterfaceService.ProgressBarService.ShowBar(client, 0, time, "Benutze " + item.Name);
            client.setData("usagetimer", API.shared.startTimer(1000, false, () =>
            {
                InterfaceService.ProgressBarService.ChangeProgress(client, count);
                if (count >= time)
                {
                    InterfaceService.ProgressBarService.HideBar(client);
                    ItemAction(client, player, item, cuffed);
                    API.shared.stopTimer(client.getData("usagetimer"));
                    client.resetData("usagetimer");
                    API.shared.stopPlayerAnimation(client);
                    return;
                }

                count++;
            }));
        }
    }
}
