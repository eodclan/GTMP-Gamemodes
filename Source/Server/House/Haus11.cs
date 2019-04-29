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
    public class Haus11 : Script
    {
        public Haus11()
        {

            API.onResourceStart += onResourceStart;

        }

        public void onResourceStart()
        {
            //API.createMarker(1, new Vector3(98.65886, 6620.395, 32.43531), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
            Blip Haus11 = API.createBlip(new Vector3(149.7864f, -1900.436f, 23.15737f));
            API.setBlipSprite(Haus11, 350);
            API.setBlipColor(Haus11, 4);
            API.setBlipScale(Haus11, 0.7f);
        }       
    } 

}