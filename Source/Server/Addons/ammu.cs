using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;


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
   // int door1, door2;
    public void spawn(Client sender)
    {
        sen = sender;
        sender.position = spawnloc;
        //API.exported.doormanager.refreshDoorState(door1);
        //API.exported.doormanager.refreshDoorState(door2);
    }

    public void OnResourceStart()
    {
        // door1 = API.exported.doormanager.registerDoor(-8873588, new Vector3(18.572, -1115.495, 29.94694));
        //door2 = API.exported.doormanager.registerDoor(97297972, new Vector3(16.12787, -1114.606, 29.94694));
        //API.exported.doormanager.setDoorState(door1, true, 1);
       // API.exported.doormanager.setDoorState(door2, true, 1);
        API.createMarker(1, new Vector3(842.6454, -1033.193, 27.19486), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
        Infoammunation = API.createCylinderColShape(new Vector3(842.6454, -1033.193, 27.19486), 2f, 5f);
        ammu = API.createBlip(new Vector3(842.5272, -1032.0923,28.19486));
        API.setBlipSprite(ammu, 110);
        API.setBlipColor(ammu, 5);

       // API.sendNativeToAllPlayers(0x38C951A4, -2119023917, 18.572f, -1115.495f, 29.9469f, false, 0,0);

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
                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 12 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
            }
            else if((string)args[0] == "CombatPistol") {

                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 12 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));

            }
            else if ((string)args[0] == "HeavyPistol")
            {

                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 9 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));

            }
            else if ((string)args[0] == "Revolver")
            {

                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 6 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));

            }
            else if ((string)args[0] == "AssaultRifle")
            {

                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));

            }
             else if ((string)args[0] == "CarbineRifle")
            {

                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));

            }
            else if ((string)args[0] == "BullpupRifle")
            {

                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
            }
            else if ((string)args[0] == "CompactRifle")
            {

                player.giveWeapon(API.weaponNameToModel((string)args[0]), 0 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")), true, false);
                API.setPlayerWeaponAmmo(player, API.weaponNameToModel((string)args[0]), 30 + API.getPlayerWeaponAmmo(player, API.weaponNameToModel("Pistol")));
            }


        }

    }

}



