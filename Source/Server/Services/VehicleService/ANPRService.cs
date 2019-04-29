using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System;

namespace FactionLife.Server.Services.VehicleService
{
    public class ANPR : Script
    {
        #region Instance
        public static ANPR instance;

        public ANPR()
        {
            API.onResourceStart += OnResourceStartHandler;
        }

        public void OnResourceStartHandler()
        {
            if (instance != null)
                API.consoleOutput("WARNING: Eine Instanz von 'ANPR' wurde bereits deklariert!");
            else
                instance = this;
        }
        #endregion

        public void CreateANPR(Vehicle newVehicle)
        {
            Tuple<Vector3, Vector3> Dimensions = API.getVehicleDimensions(API.vehicleNameToModel(newVehicle.displayName));
            ANPRData anprData = new ANPRData()
            {
                FrontLeftPos = (-(Dimensions.Item1.X - Dimensions.Item2.X)),
                FrontRightPos = (-(Dimensions.Item1.Z - Dimensions.Item2.Z))
            };
            API.setEntityData(newVehicle, "ANPRData", anprData);
        }

        public ANPRData GetANPRData(Vehicle newVehicle)
        {
            return newVehicle.hasData("ANPRData") ? newVehicle.getData("ANPRData") : null;
        }
    }

    public class ANPRData
    {
        public float FrontLeftPos { get; set; }
        public float FrontRightPos { get; set; }
    }

}