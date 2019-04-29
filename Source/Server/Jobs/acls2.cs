using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using System;

namespace FactionLife.Server.Services.FactionService
{
    public class Acls2 : Script
    {
        public Acls2()
        {

            API.onResourceStart += onResourceStart;

        }

        public void onResourceStart()
        {
            //API.createMarker(1, new Vector3(-1157.92078, -2039.11609, 13.1873159), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
            Blip Acls2 = API.createBlip(new Vector3(-1129.83972, -1990.36133f, 12.7434874f));
            API.setBlipSprite(Acls2, 560);
            API.setBlipColor(Acls2, 5);
            API.setBlipName(Acls2, "Acls Center South");
        }
    } 
}