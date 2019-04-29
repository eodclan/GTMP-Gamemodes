using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Base
{
    public static class Constants
    {
        public readonly static Vector3 EmptyVector = new Vector3(0, 0, 0);
        public readonly static Vector3 LoginRegisterCamera = new Vector3(418.9948, 747.476, 192);
        public readonly static Vector3 LoginRegisterCameraLookAt = new Vector3(-428.2029, 1050.306, 320.3225);
        public readonly static Vector3 ConnectPosition = new Vector3(425.0599, 745.7097, 192.64);
        public readonly static Vector3 DefaultSpawnPosition = new Vector3(-256.5307, -295.8158, 21.62641);
        public readonly static double DefaultSpawnRotation = -160.6727;
    }
}
