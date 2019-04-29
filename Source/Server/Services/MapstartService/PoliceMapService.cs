using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Model;

namespace FactionLife.Server.Services.PoliceStationMapService
{
    class PoliceStationMapHandler
        : Script
    {
        public PoliceStationMapHandler()
        {
            API.onClientEventTrigger += OnClientEvent;
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            API.consoleOutput("Police Station System geladen");
            // Objects (Total: 454)
            API.createObject(-994492850, new Vector3(405.379669, -1017.36005, 28.40012), new Vector3(0, 0, -2.499523), 0);
            API.createObject(-994492850, new Vector3(407.590057, -1017.27283, 28.3782463), new Vector3(0, 0, -2.499523), 0);
            API.createObject(-994492850, new Vector3(409.158325, -1017.241, 28.3625069), new Vector3(0, 0, -2.499523), 0);
            API.createObject(-994492850, new Vector3(410.758331, -1017.22943, 28.3496857), new Vector3(0, 0, -2.499523), 0);
            API.createObject(1172303719, new Vector3(405.070831, -1024.56567, 28.3276863), new Vector3(0, 0, 89.99986), 0);
            API.createObject(1172303719, new Vector3(405.065582, -1028.53638, 28.3276024), new Vector3(0, 0, 89.999855), 0);
            API.createObject(1230099731, new Vector3(405.168884, -1017.54077, 29.2460384), new Vector3(0, 0, -90.17078), 0);
            API.createObject(-994492850, new Vector3(405.316925, -1016.77075, 28.3986549), new Vector3(0, 0, -2.499523), 0);
            API.createObject(307771752, new Vector3(405.386169, -1017.35437, 28.4000454), new Vector3(0, 0, 88.82864), 0);

        }
    }
}