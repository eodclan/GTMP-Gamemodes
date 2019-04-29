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
    public class Knast : Script
    {
        public Knast()
        {

            API.onResourceStart += onResourceStart;

        }

        public void onResourceStart()
        {
            //API.createMarker(1, new Vector3(98.65886, 6620.395, 32.43531), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
            Blip Knast = API.createBlip(new Vector3(1845.3117f, 2608.921f, 45.59389f));
            API.setBlipSprite(Knast, 188);
            API.setBlipColor(Knast, 4);
            API.setBlipScale(Knast, 0.7f);
			API.setBlipName(Knast, "Los Santos Penitentiary");
        }       
    } 

}