using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Threading;
using FactionLife.Server.Services.MoneyService;


public class Ammunation : Script
{
    private readonly Vector3 spawnloc = new Vector3(842.7685 ,-1024.539 ,28.34478);

    public Ammunation()
    {
        API.onResourceStart += OnResourceStart;
        API.onPlayerConnected += spawn;
        API.onClientEventTrigger += OnClientEvent;
    }
    Client sen;
    public ColShape Infoammunation;
    Blip ammu;

    public void spawn(Client sender)
    {
        sen = sender;
        sender.position = spawnloc;
    }

    public void OnResourceStart()
    {
        API.createMarker(1, new Vector3(842.6454, -1033.193, 27.19486), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
        Infoammunation = API.createCylinderColShape(new Vector3(842.6454, -1033.193, 27.19486), 2f, 5f);
        ammu = API.createBlip(new Vector3(842.5272, -1032.0923,28.19486));
        API.setBlipSprite(ammu, 110);
        API.setBlipColor(ammu, 5);

        Infoammunation.onEntityEnterColShape += (shape, entity) =>
        {
            Client player;
            if ((player = API.getPlayerFromHandle(entity)) != null)
            {
                API.triggerClientEvent(player, "openmenu");

            }


        };

        Infoammunation.onEntityExitColShape += (shape, entity) =>
        {
            Client player;
            if ((player = API.getPlayerFromHandle(entity)) != null)
            {
                API.triggerClientEvent(player, "closemenu");

            }


        };
    }

    public void OnClientEvent(Client player, string eventName, params object[] args)
    {
        if (eventName == "clickeditem")
        {
            if ((string)args[0] == "Pistol")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 5000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 12 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 5000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Pistol");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "CombatPistol")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 7500))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 12 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 7500);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Combat Pistol");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "HeavyPistol")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 10000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 9 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 10000);
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "AssaultRifle")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 250000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 250000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Assault Rifle");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "CarbineRifle")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 275000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 275000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Carbine Rifle");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "BullpupRifle")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 265000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 265000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Bullpup Rifle");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "CompactRifle")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 350000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 350000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Compact Rifle");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "Knife")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 3500))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 3500);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Knife");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "BattleAxe")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 35000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 35000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Battle Axe");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "Machete")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 35000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 35000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Machete");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "Dagger")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 25000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 25000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Dagger");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "SawnOffShotgun")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 250000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 250000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Sawn Off Shotgun");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "Musket")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 200000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 200000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Musket");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "PumpShotgunMk2")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 390000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 390000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Pump Shotgun Mk2");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
            else if ((string)args[0] == "PumpShotgun")
            {
                if (MoneyService.HasPlayerEnoughBank(player, 280000))
                {
                    player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true);
                    API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
                    MoneyService.RemovePlayerBank(player, 280000);
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> Gekauft ~r~1x~s~ Pump Shotgun");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(player, "<C>Ammu-Nation:</C> ~r~Sie haben nicht genug Geld.");
                }
            }
        }
    }
}



