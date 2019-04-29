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
    public class Justiz : Script
    {
        public Justiz()
        {

            API.onResourceStart += onResourceStart;

        }

        public void onResourceStart()
        {
            //API.createMarker(1, new Vector3(-1157.92078, -2039.11609, 13.1873159), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
            Blip Justiz = API.createBlip(new Vector3(-539.2943, -214.9799f, 37.64978f));
            API.setBlipSprite(Justiz, 409);
            API.setBlipColor(Justiz, 4);
            API.setBlipName(Justiz, "Justiz");
        }
    } 
}