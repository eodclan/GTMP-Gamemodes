using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Managers;
using System.Threading;
using System.Timers;

namespace FactionLife.Server.Services.TeleportService
{
    public class KrankenhausService : Script
    {
        public KrankenhausService()
        {
            API.onResourceStart += onStart;
        }
        public ColShape Infoammunation;


        public void onStart()
        {
            {
                API.createMarker(25, new Vector3(275.7025, -1361.49, 24.03781), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 255); //Krankenhaus innen
                Infoammunation = API.createCylinderColShape(new Vector3(275.7025, -1361.49, 24.53781), 1, 1);
                Infoammunation.onEntityEnterColShape += (shape, entity) =>
                {
                    Client client;
                    Vector3 khaussen = new Vector3(-450.208, -349.8097, 34.50173);
                    if ((client = API.getPlayerFromHandle(entity)) != null)
                    {
                        API.setEntityPosition(client, khaussen);
                    }
                };

                API.createMarker(25, new Vector3(-449.6167, -342.2872, 34.50172), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 255); //Krankenhaus aussen
                Infoammunation = API.createCylinderColShape(new Vector3(-449.6167, -342.2872, 34.50172), 1, 1);
                Infoammunation.onEntityEnterColShape += (shape, entity) =>
                {
                    Client client;
                    Vector3 khinnen = new Vector3(274.0519, -1359.839, 24.53781);
                    if ((client = API.getPlayerFromHandle(entity)) != null)
                    {
                        API.setEntityPosition(client, khinnen);
                    }
                };


            }

        }
    }
}