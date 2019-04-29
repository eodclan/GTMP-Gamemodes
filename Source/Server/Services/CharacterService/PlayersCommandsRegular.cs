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
using FactionLife.Server.Services.ClothingService;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.ShopService;
using FactionLife.Server.Services.VehicleService;
using GrandTheftMultiplayer.Shared.Math;
using System.Data;
using System.Collections;

namespace FactionLife.Server.Services.CharacterService
{
    class PlayerCommandsRegular : Script
    {
        public PlayerCommandsRegular()
        {
            API.onClientEventTrigger += OnClientEvent;
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            Player player = client.getData("player");

            switch (eventName)
            {
                case "SetPersoDataNow":
                    API.delay(2000, true, () =>
                    {

                        string firstname = player.Character.FirstName;
                        string lastname = player.Character.LastName;
                        string firstvisit = player.Account.CreatedAt.Date.ToString();

                        API.triggerClientEvent(client, "ChangeSetting", firstname, lastname, firstvisit);

                    });
                    break;
            };
        }


        public List<Message> ServiceListPolice = new List<Message>();
        public List<Message> ServiceListMedic = new List<Message>();
        public List<Message> ServiceListFahrschule = new List<Message>();

        //public List<MenuItem> menuitems = new List<MenuItem>();

        [Command("remallitems")]
        public void RemALllITEMS(Client client)
        {
            Player player = client.getData("player");
            player.Character.Inventory = JsonConvert.DeserializeObject<List<InventoryItem>>("[]");
            CharacterService.UpdateCharacter(player.Character);



        }

        [Command("givekey")]
        public void GiveKey(Client client, Client PlayerToGiveKeyTo, int IDofKeyToGiveAway)
        {
            Player playertarget = PlayerToGiveKeyTo.getData("player");

            VehicleService.VehicleService.GetUserVehicles(client).ForEach(ownedVehicle =>
            {
                if ((ownedVehicle.Id != IDofKeyToGiveAway) || (ownedVehicle.OwnerCharId == playertarget.Character.Id)) { return; }
                else { KeyGiveAway(client, PlayerToGiveKeyTo, IDofKeyToGiveAway); }
            });

        }


        [Command("givekeyadmin")]
        public void AdminGiveKey(Client client, Client PlayerToGiveKeyTo, int IDofKeyToGiveAway)
        {

            Player admin = client.getData("player");
            Player playertarget = PlayerToGiveKeyTo.getData("player");

            if (admin.Account.AdminLvl == 8)
            {



                int targetid = playertarget.Character.Id;

                int keydata1 = playertarget.Character.Key1;
                int keydata2 = playertarget.Character.Key2;
                int keydata3 = playertarget.Character.Key3;
                int keydata4 = playertarget.Character.Key4;
                int keydata5 = playertarget.Character.Key5;


                if (keydata1 == IDofKeyToGiveAway) { playertarget.Character.Key1 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
                if (keydata2 == IDofKeyToGiveAway) { playertarget.Character.Key2 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
                if (keydata3 == IDofKeyToGiveAway) { playertarget.Character.Key3 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
                if (keydata4 == IDofKeyToGiveAway) { playertarget.Character.Key4 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
                if (keydata5 == IDofKeyToGiveAway) { playertarget.Character.Key5 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }

                //if (keydata1 == 0)



                if (keydata1 == 0) { playertarget.Character.Key1 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 1); return; }
                if (keydata2 == 0) { playertarget.Character.Key2 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 2); return; }
                if (keydata3 == 0) { playertarget.Character.Key3 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 3); return; }
                if (keydata4 == 0) { playertarget.Character.Key4 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 4); return; }
                if (keydata5 == 0) { playertarget.Character.Key5 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 5); return; }

                if ((keydata1 != 0) && (keydata2 != 0) && (keydata3 != 0) && (keydata4 != 0) && (keydata5 != 0)) { API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "~r~Sie besitzen bereits die maximale Anzahl Schlüssel!"); API.sendChatMessageToPlayer(client, "~r~Die Person besitzt bereits zu viele Zweitschlüssel!"); }

            }

        }


        public void KeyGiveAway(Client client, Client PlayerToGiveKeyTo, int IDofKeyToGiveAway)
        {

            Player playertarget = PlayerToGiveKeyTo.getData("player");




            int targetid = playertarget.Character.Id;

            int keydata1 = playertarget.Character.Key1;
            int keydata2 = playertarget.Character.Key2;
            int keydata3 = playertarget.Character.Key3;
            int keydata4 = playertarget.Character.Key4;
            int keydata5 = playertarget.Character.Key5;


            if (keydata1 == IDofKeyToGiveAway) { playertarget.Character.Key1 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
            if (keydata2 == IDofKeyToGiveAway) { playertarget.Character.Key2 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
            if (keydata3 == IDofKeyToGiveAway) { playertarget.Character.Key3 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
            if (keydata4 == IDofKeyToGiveAway) { playertarget.Character.Key4 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }
            if (keydata5 == IDofKeyToGiveAway) { playertarget.Character.Key5 = IDofKeyToGiveAway; API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "Sie besitzen bereits einen Schlüssel für dieses Fahrzeug"); return; }

            //if (keydata1 == 0)



            if (keydata1 == 0) { playertarget.Character.Key1 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 1); return; }
            if (keydata2 == 0) { playertarget.Character.Key2 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 2); return; }
            if (keydata3 == 0) { playertarget.Character.Key3 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 3); return; }
            if (keydata4 == 0) { playertarget.Character.Key4 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 4); return; }
            if (keydata5 == 0) { playertarget.Character.Key5 = IDofKeyToGiveAway; RunCommandForKeys(client, PlayerToGiveKeyTo, targetid, IDofKeyToGiveAway, 5); return; }

            if ((keydata1 != 0) && (keydata2 != 0) && (keydata3 != 0) && (keydata4 != 0) && (keydata5 != 0)) { API.sendChatMessageToPlayer(PlayerToGiveKeyTo, "~r~Sie besitzen bereits die maximale Anzahl Schlüssel!"); API.sendChatMessageToPlayer(client, "~r~Die Person besitzt bereits zu viele Zweitschlüssel!"); }


        }


        [Command("remkey")]
        public void RemoveKey(Client client, int IDofKeyToGiveAway)
        {
            Player player = client.getData("player");

            int keydata1 = player.Character.Key1;
            int keydata2 = player.Character.Key2;
            int keydata3 = player.Character.Key3;
            int keydata4 = player.Character.Key4;
            int keydata5 = player.Character.Key5;

            if (keydata1 == IDofKeyToGiveAway) { player.Character.Key1 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 1); API.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); }
            if (keydata2 == IDofKeyToGiveAway) { player.Character.Key2 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 2); API.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); }
            if (keydata3 == IDofKeyToGiveAway) { player.Character.Key3 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 3); API.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); }
            if (keydata4 == IDofKeyToGiveAway) { player.Character.Key4 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 4); API.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); }
            if (keydata5 == IDofKeyToGiveAway) { player.Character.Key5 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 5); API.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); }

        }


        public void RunCommandForKeys(Client client, Client target, int targetid, int KeyID, int IntoKeyRow)
        {
            Player player = target.getData("player");

            switch (IntoKeyRow)
            {
                case 1:

                    Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key1 = @Key WHERE Id = @Id LIMIT 1", parameters);
                    if (KeyID != 0) { API.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;

                case 2:

                    Dictionary<string, string> parameters2 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result2 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key2 = @Key WHERE Id = @Id LIMIT 1", parameters2);
                    if (KeyID != 0) { API.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
                case 3:

                    Dictionary<string, string> parameters3 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result3 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key3 = @Key WHERE Id = @Id LIMIT 1", parameters3);
                    if (KeyID != 0) { API.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
                case 4:

                    Dictionary<string, string> parameters4 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result4 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key4 = @Key WHERE Id = @Id LIMIT 1", parameters4);
                    if (KeyID != 0) { API.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
                case 5:

                    Dictionary<string, string> parameters5 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result5 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key5 = @Key WHERE Id = @Id LIMIT 1", parameters5);
                    if (KeyID != 0) { API.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
            }

            API.delay(1000, true, () =>
            {
                CharacterService.UpdateCharacter(player.Character);

            });
        }


        [Command("packgun")]
        public void PackGun(Client client)
        {

            Player player = client.getData("player");

            WeaponHash currentWeapon = API.shared.getPlayerCurrentWeapon(client);

            if (currentWeapon == WeaponHash.Unarmed) { return; }

            switch (currentWeapon)
            {
                case WeaponHash.CombatPDW:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine PDW in Ihre Tasche verstaut");
                    break;
                case WeaponHash.AssaultRifle:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine AssaultRifle in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Hammer:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Hammer in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Knife:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Knife in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Bat:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Bat in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Crowbar:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Crowbar in Ihre Tasche verstaut");
                    break;
                case WeaponHash.GolfClub:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein GolfClub in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Bottle:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Bottle in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Dagger:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Dagger in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Hatchet:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Hatchet in Ihre Tasche verstaut");
                    break;
                case WeaponHash.KnuckleDuster:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein KnuckleDuster in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Machete:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Machete in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Flashlight:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Flashlight in Ihre Tasche verstaut");
                    break;
                case WeaponHash.SwitchBlade:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein SwitchBlade in Ihre Tasche verstaut");
                    break;
                case WeaponHash.PoolCue:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein PoolCue in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Wrench:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben ein Wrench in Ihre Tasche verstaut");
                    break;
                case WeaponHash.BattleAxe:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine BattleAxe in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Pistol:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine Pistol in Ihre Tasche verstaut");
                    break;
                case WeaponHash.CombatPistol:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine CombatPistol in Ihre Tasche verstaut");
                    break;
                case WeaponHash.Pistol50:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine Pistol50 in Ihre Tasche verstaut");
                    break;
                case WeaponHash.SNSPistol:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine SNSPistol in Ihre Tasche verstaut");
                    break;
                case WeaponHash.HeavyPistol:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine HeavyPistol in Ihre Tasche verstaut");
                    break;
                case WeaponHash.VintagePistol:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine VintagePistol in Ihre Tasche verstaut");
                    break;
                case WeaponHash.APPistol:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine APPistol in Ihre Tasche verstaut");
                    break;
                ///FRAK WAFFEH
                case WeaponHash.Gusenberg:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine Gusenberg in Ihre Tasche verstaut");
                    break;
                case WeaponHash.DoubleBarrelShotgun:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine DoubleBarrelShotgun in Ihre Tasche verstaut");
                    break;
                case WeaponHash.SniperRifle:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine SniperRifle in Ihre Tasche verstaut");
                    break;
                case WeaponHash.MicroSMG:
                    API.shared.sendChatMessageToPlayer(client, "Sie haben eine MicroSMG in Ihre Tasche verstaut");
                    break;
            }


            //int ammoactual = API.getPlayerWeaponAmmo(client, currentWeapon);

            API.shared.removePlayerWeapon(client, currentWeapon);


            //AddMagazineToInv(client, WeaponHash.CombatPDW, ammoactual);


            if (currentWeapon == WeaponHash.CombatPDW) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 49); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 49, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.AssaultRifle) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 50); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 50, Count = 1 }); } else { invitem.Count++; } }
            //Shop AMU hand waffen 
            else if (currentWeapon == WeaponHash.Hammer) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 54); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 54, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Knife) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 53); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 53, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Bat) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 55); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 55, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Crowbar) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 56); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 56, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.GolfClub) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 57); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 57, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Bottle) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 58); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 58, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Dagger) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 59); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 59, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Hatchet) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 60); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 60, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.KnuckleDuster) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 61); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 61, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Machete) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 62); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 62, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Flashlight) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 63); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 63, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.SwitchBlade) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 64); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 64, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.PoolCue) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 65); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 65, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Wrench) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 66); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 66, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.BattleAxe) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 67); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 67, Count = 1 }); } else { invitem.Count++; } }
            //shops AMU Waffen 
            else if (currentWeapon == WeaponHash.Pistol) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 68); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 68, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.CombatPistol) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 70); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 70, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.Pistol50) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 72); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 72, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.SNSPistol) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 74); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 74, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.HeavyPistol) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 76); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 76, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.VintagePistol) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 78); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 78, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.APPistol) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 80); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 80, Count = 1 }); } else { invitem.Count++; } }
            ///FRAK WAFFEH
            else if (currentWeapon == WeaponHash.Gusenberg) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 82); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 82, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.DoubleBarrelShotgun) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 84); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 84, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.SniperRifle) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 86); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 86, Count = 1 }); } else { invitem.Count++; } }
            else if (currentWeapon == WeaponHash.MicroSMG) { InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 90); if (invitem == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 90, Count = 1 }); } else { invitem.Count++; } }


        }

        //public void AddMagazineToInv(Client client, WeaponHash weapon, int ammo)
        //{
        //Player player = client.getData("player");
        //
        //int magazinecount;
        //   
        //
        //
        // switch (weapon)
        //{
        //   case WeaponHash.CombatPDW:
        //magazinecount = (ammo / 30);
        //InventoryItem invitemmag = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 51); if (invitemmag == null) { player.Character.Inventory.Add(new InventoryItem { ItemID = 51, Count = 1 }); } else { invitemmag.Count++;  }
        //           break;
        //}
        //}


            //ServiceDienste Achtung: Fahrschule benötigt Funk!!!!!



        #region cancelServices


        [Command("cancel", GreedyArg = true)]
        public void ServiceCancel(Client client, string Dienst)
        {
        
            Player player = client.getData("player");
           // Player target = sender.getData("player");
            
            string idstring = player.Character.Id.ToString();

            if (Dienst == "police")
            {
                if (API.getEntitySyncedData(client, "HasPoliceService") == false) { return; }

                API.setEntitySyncedData(client, "HasPoliceService", false);
                API.setEntitySyncedData(client, "PoliceService", null);

                API.sendChatMessageToPlayer(client, "~g~Ihr Notruf wurde abgebrochen!");
                FactionHandler.FraUtil.BroadcastToLSPD("~r~Der Notruf von " + player.Character.FirstName + "_" + player.Character.LastName + " wurde abgebrochen!");


                foreach (Message message in ServiceListPolice)
                {
                    if (message.Value2 == idstring)
                    {
                        ServiceListPolice.Remove(message);
                        return;
                    }


                }

                return;
            }
            if (Dienst == "medic")
            {
                if (API.getEntitySyncedData(client, "HasMedicService") == false) { return; }

                API.setEntitySyncedData(client, "HasMedicService", false);
                API.setEntitySyncedData(client, "MedicService", null);

                API.sendChatMessageToPlayer(client, "~g~Ihr Notruf wurde abgebrochen!");
                FactionHandler.FraUtil.BroadcastToEMS("~r~Der Notruf von " + player.Character.FirstName + "_" + player.Character.LastName + " wurde abgebrochen!");


                foreach (Message message in ServiceListMedic)
                {
                    if (message.Value2 == idstring)
                    {
                        ServiceListMedic.Remove(message);
                        return;
                    }


                }
                return;
            }
            if (Dienst == "fahrschule")
            {
                if (API.getEntitySyncedData(client, "HasFahrschuleService") == false) { return; }

                API.setEntitySyncedData(client, "HasFahrschuleService", false);
                API.setEntitySyncedData(client, "FahrschuleService", null);

                API.sendChatMessageToPlayer(client, "~g~Ihr Auftrag wurde abgebrochen!");
                FactionHandler.FraUtil.BroadcastToFahrschule("~r~Der Auftrag von " + player.Character.FirstName + "_" + player.Character.LastName + " wurde abgebrochen!");


                foreach (Message message in ServiceListFahrschule)
                {
                    if (message.Value2 == idstring)
                    {
                        ServiceListFahrschule.Remove(message);
                        return;
                    }


                }
                return;
            }

            else { API.sendChatMessageToPlayer(client, "~y~Hilfe: ~w~Nutzen sie /cancel [police | medic | fahrschule] Nachricht"); return; }

            

        }
#endregion cancelServices

        #region callservices

        [Command("service", GreedyArg = true)]
        public void ServiceCall(Client client, string Dienst, string Nachricht)
        {
            

            Player player = client.getData("player");

            var time = API.getTime();
            string timestringHours = time.Hours.ToString();
            string timestringMinutes = time.Minutes.ToString();

            
            switch (Dienst)
            {
                case "police":

                    Message policenotification = (new Message
                    {
                        Title = Nachricht,
                        Value2 = player.Character.Id.ToString(),
                        RightLabel = timestringHours + ":" + timestringMinutes,
                        Sender = client

                    });




                    if (API.getEntitySyncedData(client, "HasPoliceService") == true) { API.sendChatMessageToPlayer(client, "~r~Sie haben bereits eine offene Anfrage"); return; }

                    ServiceListPolice.Add(policenotification);

                    API.sendChatMessageToPlayer(client, "~g~ Ihr Anfrage wurde abgesendet!");
                    FactionHandler.FraUtil.BroadcastToLSPD("Notruf von " + player.Character.FirstName + "_" + player.Character.LastName + " ist eingegangen!");
                    FactionHandler.FraUtil.BroadcastToLSPD("Nutze /show zum anzeigen der Notrufe!");
                    API.setEntitySyncedData(client, "HasPoliceService", true);
                    API.setEntitySyncedData(client, "PoliceService", policenotification);
                    break;


                case "medic":

                    Message medicnotification = (new Message
                    {
                        Title = Nachricht,
                        Value2 = player.Character.Id.ToString(),
                        RightLabel = timestringHours + ":" + timestringMinutes,
                        Sender = client

                    });

                    if (API.getEntitySyncedData(client, "HasMedicService") == true) { API.sendChatMessageToPlayer(client, "~r~Sie haben bereits eine offene Anfrage"); return; }

                    ServiceListMedic.Add(medicnotification);

                    API.sendChatMessageToPlayer(client, "~g~ Ihr Anfrage wurde abgesendet!");
                    FactionHandler.FraUtil.BroadcastToEMS("Notruf von " + player.Character.FirstName + player.Character.LastName + " ist eingegangen!");
                    FactionHandler.FraUtil.BroadcastToEMS("Nutze /show zum anzeigen der Notrufe!");

                    API.setEntitySyncedData(client, "HasMedicService", true);
                    API.setEntitySyncedData(client, "MedicService", medicnotification);
                    break;


                case "fahrschule":

                    Message fahrschulenotification = (new Message
                    {
                        Title = Nachricht,
                        Value2 = player.Character.Id.ToString(),
                        RightLabel = timestringHours + ":" + timestringMinutes,
                        Sender = client

                    });

                    if (API.getEntitySyncedData(client, "HasFahrschuleService") == true) { API.sendChatMessageToPlayer(client, "~r~Sie haben bereits eine offene Anfrage"); return; }
                    ServiceListFahrschule.Add(fahrschulenotification);

                    API.sendChatMessageToPlayer(client, "~g~ Ihr Anfrage wurde abgesendet!");
                    FactionHandler.FraUtil.BroadcastToFahrschule("Auftrag von " + player.Character.FirstName + player.Character.LastName + " ist eingegangen!");
                    FactionHandler.FraUtil.BroadcastToFahrschule("Nutze /show zum anzeigen der Aufträge!");

                    API.setEntitySyncedData(client, "HasFahrschuleService", true);
                    API.setEntitySyncedData(client, "FahrschuleService", fahrschulenotification);
                    break;
            }

            if ((Dienst != "police") && (Dienst != "medic") && (Dienst != "fahrschule")) { API.sendChatMessageToPlayer(client, "~y~Hilfe: ~w~Nutzen sie /service [police | medic | fahrschule] Nachricht"); return; }



        }
#endregion callservices

        #region showServices

        [Command("show")]
        public void ServiceCallShow(Client client)
        {
            Player player = client.getData("player");

            if (player.Character.Faction == FactionType.Police)
            {
                List<MenuItem> menuitems = new List<MenuItem>();

                foreach (Message message in ServiceListPolice)
                {
                    Client second = message.Sender;
                    Player seecnd = second.getData("player");

                    menuitems.Add(new MenuItem
                    {
                        Title = "~r~" + message.Title,
                        Value1 = "nonevaluegiven",
                        Value2 = seecnd.Character.Id.ToString(),
                        RightLabel = message.RightLabel,
                        Description = "Von: " + seecnd.Character.FirstName + "_" + seecnd.Character.LastName + "; ID: " + seecnd.Character.Id.ToString()
                    });

                }

                API.shared.triggerClientEvent(client, "ServiceMenu_Open", JsonConvert.SerializeObject(menuitems));
                return;
            }

            //

            if (player.Character.Faction == FactionType.EMS)
            {
                List<MenuItem> menuitems = new List<MenuItem>();

                foreach (Message message in ServiceListMedic)
                {
                    Client second = message.Sender;
                    Player seecnd = second.getData("player");

                    menuitems.Add(new MenuItem
                    {
                        Title = "~r~" + message.Title,
                        Value1 = "nonevaluegiven",
                        Value2 = seecnd.Character.Id.ToString(),
                        RightLabel = message.RightLabel,
                        Description = "Von: " + seecnd.Character.FirstName + "_" + seecnd.Character.LastName + "; ID: " + seecnd.Character.Id.ToString()
                    });

                }

                API.shared.triggerClientEvent(client, "ServiceMenu_Open", JsonConvert.SerializeObject(menuitems));
                return;
            }

            //

            if (player.Character.Faction == FactionType.Fahrschule)
            {

                List<MenuItem> menuitems = new List<MenuItem>();

                foreach (Message message in ServiceListFahrschule)
                {
                    Client second = message.Sender;
                    Player seecnd = second.getData("player");

                    menuitems.Add(new MenuItem
                    {
                        Title = "~r~" + message.Title,
                        Value1 = "nonevaluegiven",
                        Value2 = seecnd.Character.Id.ToString(),
                        RightLabel = message.RightLabel,
                        Description = "Von: " + seecnd.Character.FirstName + "_" + seecnd.Character.LastName + "; ID: " + seecnd.Character.Id.ToString()
                    });

                }

                API.shared.triggerClientEvent(client, "ServiceMenu_Open", JsonConvert.SerializeObject(menuitems));
                return;


            }
        }
# endregion showServices

        #region acceptservice
        [Command("acceptservice")]
        public void AcceptService(Client client, Client sender)
        {
            Player player = client.getData("player");
            Player target = sender.getData("player");
            string idstring = target.Character.Id.ToString();

            Vector3 positionofsender = API.getEntityPosition(sender);

            if (player.Character.Faction == FactionType.Police)
            {
                if (API.getEntitySyncedData(sender, "HasPoliceService") == false) { return; }

                API.setEntitySyncedData(sender, "HasPoliceService", false);
                API.setEntitySyncedData(sender, "PoliceService", null);

                API.sendChatMessageToPlayer(sender, "~g~Ihr Notruf wurde angenommen!");
                FactionHandler.FraUtil.BroadcastToLSPD(player.Character.FirstName + "_" + player.Character.LastName + " hat den Notruf von " + target.Character.FirstName + "_" + target.Character.LastName + " angenommen!");

                API.triggerClientEvent(client, "PoliceNavi", "S1", positionofsender.X, positionofsender.Y);

                foreach (Message message in ServiceListPolice)
                {
                    if (message.Value2 == idstring)
                    {
                        ServiceListPolice.Remove(message);
                        return;
                        
                    }
                    

                }

                return;

                
            }

            //

            if (player.Character.Faction == FactionType.EMS)
            {
                if (API.getEntitySyncedData(sender, "HasMedicService") == false) { return; }

                API.setEntitySyncedData(sender, "HasMedicService", false);
                API.setEntitySyncedData(sender, "MedicService", null);

                API.sendChatMessageToPlayer(sender, "~g~Ihr Notruf wurde angenommen!");
                FactionHandler.FraUtil.BroadcastToEMS(player.Character.FirstName + "_" + player.Character.LastName + " hat den Notruf von " + target.Character.FirstName + "_" + target.Character.LastName + " angenommen!");

                API.triggerClientEvent(client, "PoliceNavi", "S1", positionofsender.X, positionofsender.Y);

                foreach (Message message in ServiceListMedic)
                {
                    if (message.Value2 == idstring)
                    {
                        ServiceListMedic.Remove(message);
                        return;
                    }
                    ;

                }

                return;
            }

            //

            if (player.Character.Faction == FactionType.Fahrschule)
            {
                if (API.getEntitySyncedData(sender, "HasFahrschuleService") == false) { return; }

                API.setEntitySyncedData(sender, "HasFahrschuleService", false);
                API.setEntitySyncedData(sender, "FahrschuleService", null);

                API.sendChatMessageToPlayer(sender, "~g~Ihr Auftrag wurde angenommen!");
                FactionHandler.FraUtil.BroadcastToFahrschule(player.Character.FirstName + "_" + player.Character.LastName + " hat den Auftrag von " + target.Character.FirstName + "_" + target.Character.LastName + " angenommen!");

                API.triggerClientEvent(client, "PoliceNavi", "S1", positionofsender.X, positionofsender.Y);

                foreach (Message message in ServiceListFahrschule)
                {
                    if (message.Value2 == idstring)
                    {
                        ServiceListFahrschule.Remove(message);
                        return;
                    }

                    ;
                }

                return;
            }





        }
        #endregion 



        [Command("ooc", GreedyArg = true)]
        public void OutOfCharacterMessaging(Client client, string message)
        {
            Player admin = client.getData("player");

            List<Client> playerList = API.getPlayersInRadiusOfPlayer(20, client);
            foreach (Client playerItem in playerList)
            {
                Player target = playerItem.getData("player");

                API.sendChatMessageToPlayer(playerItem, "~g~OOC [" + admin.Character.FirstName + "_" + admin.Character.LastName + "] : ~y~" + message);

            }
        }









        #region ProcessServiceMenu Processing
        public static void ProcessServiceMenu(Client client, string itemvalue1, string itemvalue2)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            
            
            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
            switch (itemvalue1)
            {
                case "novaluegiven":

                    break;
                    

            }
        }
        #endregion ProcessServiceMenu Processing

    }
}
