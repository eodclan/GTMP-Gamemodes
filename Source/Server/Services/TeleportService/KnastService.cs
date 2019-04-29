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

namespace FactionLife.Server.Services.KnastService
{
    public class Knast : Script
    {
        public Knast()
        {
            API.onResourceStart += onStart;

        }
        public ColShape Infoammunation;


        public void onStart()
        {
            {
                API.createMarker(25, new Vector3(462.2706, -1009.808, 24.7), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 255); //Krankenhaus innen
                Infoammunation = API.createCylinderColShape(new Vector3(462.2706, -1009.808, 24.7), 1, 1);
                Infoammunation.onEntityEnterColShape += (shape, entity) =>
                {
                    Client client;
                    Vector3 Knast1 = new Vector3(1815.577, 2594.355, 45.3);
                    if ((client = API.getPlayerFromHandle(entity)) != null)
                    {
                        API.setEntityPosition(client, Knast1);
                    }
                };

                API.createMarker(25, new Vector3(1818.603, 2594.392, 45), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 255); //Krankenhaus aussen
                Infoammunation = API.createCylinderColShape(new Vector3(1818.603, 2594.392, 45), 1, 1);
                Infoammunation.onEntityEnterColShape += (shape, entity) =>
                {
                    Client client;
                    Vector3 Knast2 = new Vector3(463.6082, -1009.43, 25);
                    if ((client = API.getPlayerFromHandle(entity)) != null)
                    {
                        API.setEntityPosition(client, Knast2);
                    }
                };
            }
        }
    }
}