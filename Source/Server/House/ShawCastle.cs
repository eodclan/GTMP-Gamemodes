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
    public class ShawCastle : Script
    {
        public ShawCastle()
        {

            API.onResourceStart += onResourceStart;

        }

        public void onResourceStart()
        {
            //API.createMarker(1, new Vector3(98.65886, 6620.395, 32.43531), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
            Blip ShawCastle = API.createBlip(new Vector3(-3018.081f, 87.69326f, 11.60847f));
            API.setBlipSprite(ShawCastle, 350);
            API.setBlipColor(ShawCastle, 4);
            API.setBlipScale(ShawCastle, 0.7f);
        }       
    } 

}