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

namespace SimpleRoleplay.Server.Services.VehicleService

{
    public class GaragenMarkerService : Script
    {
        public GaragenMarkerService()
        {
            API.onResourceStart += onStart;

        }
        public ColShape Infoammunation;


        public void onStart()
        {
            {
             
                ///LSPD Garage
                API.createMarker(1, new Vector3(458.7997, -1007.881, 27.3), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //LSPD Garage auto garage
                ///LSPD Helii garege
                API.createMarker(1, new Vector3(465.0272, -982.606, 42.73), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //LSPD Helii Garege
                ///FIB garage
                API.createMarker(1, new Vector3(135.1701, -718.345, 32.1), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //FIB garage
                ///Vagos garage
                API.createMarker(1, new Vector3(-1077.957, -1675.243, 3), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Vagos garage
                ///YakuZa garage
                API.createMarker(1, new Vector3(-1784.754, 453.5848, 127), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //YakuZa garage
                ///Army garage
                API.createMarker(1, new Vector3(-2432.87, 3303.46, 32), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Army garage
                ///Atzen garage
                API.createMarker(1, new Vector3(1407.375, 3619.955, 33.9), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Atzen garage
                ///Lost MC garage
                API.createMarker(1, new Vector3(967.2532, -115.8964, 73.3), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Lost MC garage
                ///Ballas garage
                API.createMarker(1, new Vector3(99.97647, -1959.27, 19.8), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Ballas garage
                ///EMS garage
                API.createMarker(1, new Vector3(334.0869, -561.4349, 27.7), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //EMS garage
                ///Knast garage
                API.createMarker(1, new Vector3(1728.746, 2446.424, 44.5), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Knast garage
                ///Chumash garage
                API.createMarker(1, new Vector3(-3133.298, 1105.2, 19.5), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Chumash garage
                ///Rockford garage
                API.createMarker(1, new Vector3(-779.8875, -189.9742, 36.3), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Rockford garage
                ///Hawick garage
                API.createMarker(1, new Vector3(508.203, -75.7713, 87.9), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Hawick garage
                ///Pillbox Hill garage
                API.createMarker(1, new Vector3(44.4237, -842.871, 30), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Pillbox Hill garage
                ///Vespucci-Kanäle garage
                API.createMarker(1, new Vector3(-1112.74, -848.465, 12.4), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Vespucci-Kanäle garage
                ///Airport garage
                API.createMarker(1, new Vector3(-957.549, -2704.72, 12.6), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Airport garage
                ///Airport garage
                API.createMarker(1, new Vector3(-1660.24, -3156.41, 12.9), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Airport garage
                ///Feuerwache garage
                API.createMarker(1, new Vector3(1172.02, -1527.45, 34), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Feuerwache garage
                ///Harmony garage
                API.createMarker(1, new Vector3(643.82, 2733.16, 40.9), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Harmony garage
                ///Sandy garage
                API.createMarker(1, new Vector3(1952.92, 3841.97, 31.1), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0); //Sandy garage
                ///Paleto bay garage
                API.createMarker(1, new Vector3(-70.0137, 6536.38, 30.4), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
                ///ARMY
                API.createMarker(1, new Vector3(-2109.623, 3274.321, 32.1), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
                ///Ballas
                API.createMarker(1, new Vector3(1159.775, -1644.387, 35.9), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
                //Fahrschule
                API.createMarker(1, new Vector3(-962.154, -2070.47, 8.4), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
                //Bloods
                API.createMarker(1, new Vector3(-217.5319, -1693.016, 33), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
                //swat
                API.createMarker(1, new Vector3(1024.735, -2508.354, 27.4), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
                // xD 1
                API.createMarker(1, new Vector3(-941.4518, -2954.703, 13.1), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
                //camora
                API.createMarker(1, new Vector3(1408.123, 1114.405, 113.5), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 0, 0);
            }

        }
    }
}

