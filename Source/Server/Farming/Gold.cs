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

public class GoldMine : Script
{
    public GoldMine()
    {
        API.onResourceStart += onResourceStart;
        API.onClientEventTrigger += OnClientEvent;
    }

    public ColShape misStartColshape;
    public ColShape misEndColshape;

    public Marker goldmarker;
    public Blip desgoldblip;
    public int taskgold;


    public void onResourceStart()
    {
        API.createMarker(30, new Vector3(-592.6141, 2093.039, 131.8903), new Vector3(), new Vector3(), new Vector3(2, 2, 2), 155, 0, 0, 155); // marker to start the job
	
        misStartColshape = API.createCylinderColShape(new Vector3(-592.6141, 2093.039, 131.8903), 3F, 2F); // colshap for mission start point

        misStartColshape.onEntityEnterColShape += (shape, entity) =>
        {
            Client player;

            if ((player = API.getPlayerFromHandle(entity)) != null)
            {
                API.sendNotificationToPlayer(player, "<C>Gold Mine:</C> ~n~Drücken Sie Y, um zu Farmen");
            }
        };

    }

    public static String GetTimestamp(DateTime value)
    {
        return value.ToString("mmss");
    }

    public void OnClientEvent(Client player, string eventName, params object[] arguments) //arguments param can contain multiple params
    {
        if (eventName == "StartGoldMine") //an eventname with no params that was triggered from the Client-side script
        {

            if (misStartColshape.containsEntity(player.handle))
            {
                API.setEntityData(player, "GoldMine", true);
                API.sendNotificationToPlayer(player, "<C>Gold Mine:</C> ~n~Sie sind jetzt ein Gold Minen Arbeiter.");
                nextdesgold(player);
            }

        }

        if (eventName == "GoldMineComplete") //an eventname with no params that was triggered from the Client-side script
        {

            if (misEndColshape.containsEntity(player.handle))
            {
                taskgold++;

                if (taskgold == 20)
                {
                    API.sendNotificationToPlayer(player, "<C>Gold Mine:</C> ~n~Du hast ~g~1x Gold ~r~Tour abgeschlossen!");
                    API.deleteColShape(misEndColshape);
                    API.deleteEntity(goldmarker);
                    taskgold = 0;
                }
                else
                {
                    Random rnd = new Random();
                    int amount = rnd.Next(3, 5);

                    API.sendNotificationToPlayer(player, "<C>Gold Mine:</C> ~n~Du hast " + amount + "x Gold erhalten.");
                    Player client = player.getData("player");
                    client.Character.Inventory.Add(new InventoryItem { ItemID = 263, Count = amount });
                    API.playPlayerAnimation(player, (int)(AnimationFlags.Loop), "random@burial", "a_burial");
                    API.delay(4500, true, () => {
                        API.stopPlayerAnimation(player);
                    });
                   
                    API.deleteColShape(misEndColshape);
                    API.deleteEntity(goldmarker);
                }
                nextdesgold(player);
            }
          
        }
    }

    public void nextdesgold(Client player)
    {
            
        
        Random random = new Random();

        List<Vector3> desgold = new List<Vector3>();
        desgold.Add(new Vector3(-582.3063, 2037.865, 128.9012));
        desgold.Add(new Vector3(-576.8021, 2030.045, 128.2452));
        desgold.Add(new Vector3(-568.8348, 2020.37, 127.6122));	
        desgold.Add(new Vector3(-562.454, 2012.345, 127.3012));
        desgold.Add(new Vector3(-556.0303, 2004.183, 127.1435));
        desgold.Add(new Vector3(-552.3634, 1997.748, 127.0667));

        Vector3 desgoldpoint = desgold[random.Next(desgold.Count)];
        
        misEndColshape = API.createCylinderColShape(desgoldpoint, 3F, 2F);

        API.triggerClientEvent(player, "nextGoldMine", desgoldpoint, desgoldpoint.X, desgoldpoint.Y, goldmarker);
    }
  
}