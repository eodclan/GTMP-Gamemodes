using FactionLife.Server.Base;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Data;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Model;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.ItemService;
using System.Linq;
using FactionLife.Server.Services.FactionService;

public class DiamantMine : Script
{
    public DiamantMine()
    {
        API.onResourceStart += onResourceStart;
        API.onClientEventTrigger += OnClientEvent;
    }

    public ColShape misStartColshape;
    public ColShape misEndColshape;

    public Marker diamantmarker;
    public Blip desdiamantblip;
    public int taskdiamant;


    public void onResourceStart()
    {
        API.createMarker(0, new Vector3(-602.5063, 2090.948, 131.4611), new Vector3(), new Vector3(), new Vector3(2, 2, 2), 155, 0, 0, 155); // marker to start the job
	
        misStartColshape = API.createCylinderColShape(new Vector3(-602.5063, 2090.948, 131.4611), 3F, 2F); // colshap for mission start point

        misStartColshape.onEntityEnterColShape += (shape, entity) =>
        {
            Client player;

            if ((player = API.getPlayerFromHandle(entity)) != null)
            {
                API.sendNotificationToPlayer(player, "<C>Diamant Mine:</C> ~n~Drücken Sie Y, um zu Farmen");
            }
        };

    }

    public static String GetTimestamp(DateTime value)
    {
        return value.ToString("mmss");
    }

    public void OnClientEvent(Client player, string eventName, params object[] arguments) //arguments param can contain multiple params
    {
        if (eventName == "StartDiamantMine") //an eventname with no params that was triggered from the Client-side script
        {

            if (misStartColshape.containsEntity(player.handle))
            {
                API.setEntityData(player, "DiamantMine", true);
                API.sendNotificationToPlayer(player, "<C>Diamant Mine:</C> ~n~Sie sind jetzt ein Diamant Minen Arbeiter.");
                nextdesdiamant(player);
            }

        }

        if (eventName == "DiamantMineComplete") //an eventname with no params that was triggered from the Client-side script
        {

            if (misEndColshape.containsEntity(player.handle))
            {
                taskdiamant++;

                if (taskdiamant == 20)
                {
                    API.sendNotificationToPlayer(player, "<C>Diamant Mine:</C> ~n~Du hast ~g~1x Diamant ~r~Tour abgeschlossen!");
                    API.deleteColShape(misEndColshape);
                    API.deleteEntity(diamantmarker);
                    taskdiamant = 0;
                }
                else
                {
                    API.sendNotificationToPlayer(player, "<C>Diamant Mine:</C> ~n~Du hast 1x Diamant erhalten.");
                    Player client = player.getData("player");
                    client.Character.Inventory.Add(new InventoryItem { ItemID = 265, Count = 1 });
                    API.playPlayerAnimation(player, (int)(AnimationFlags.Loop), "random@burial", "a_burial");
                    API.delay(4500, true, () => {
                        API.stopPlayerAnimation(player);
                    });
                   
                    API.deleteColShape(misEndColshape);
                    API.deleteEntity(diamantmarker);
                }
                nextdesdiamant(player);
            }
          
        }
    }

    public void nextdesdiamant(Client player)
    {
        Random random = new Random();

        List<Vector3> desdiamant = new List<Vector3>();
        desdiamant.Add(new Vector3(-582.3063, 2037.865, 128.9012));
        desdiamant.Add(new Vector3(-576.8021, 2030.045, 128.2452));
        desdiamant.Add(new Vector3(-568.8348, 2020.37, 127.6122));	
        desdiamant.Add(new Vector3(-562.454, 2012.345, 127.3012));
        desdiamant.Add(new Vector3(-556.0303, 2004.183, 127.1435));
        desdiamant.Add(new Vector3(-552.3634, 1997.748, 127.0667));

        Vector3 desdiamantpoint = desdiamant[random.Next(desdiamant.Count)];
        
        misEndColshape = API.createCylinderColShape(desdiamantpoint, 3F, 2F);

        API.triggerClientEvent(player, "nextDiamantMine", desdiamantpoint, desdiamantpoint.X, desdiamantpoint.Y, diamantmarker);
    }
  
}