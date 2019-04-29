using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;


namespace GTG_World_RP.Server.Jobs.Job
{
    public class BusFahrer : Script
    {
        public List<Vehicle> Vehicles = new List<Vehicle>();
        public BusFahrer()
        {
            API.onResourceStart += OnResourceStart;
           // API.onPlayerExitVehicle += OnPlayerExitVehicleHandler;
           // API.onPlayerEnterVehicle += OnPlayerEnterVehicleHandler;
        }

        private void OnResourceStart()
        {
            int[] RanCol = new int[] { 1, 2, 3, 7, 8, 9, 18, 40, 42, 56, 62, 97, 105, 107, 114, 121 };

            Random rnd = new Random();

            Vehicles.Add(API.createVehicle((VehicleHash)(-713569950), new Vector3(466.072, -654.1628, 27.8785), new Vector3(0, 0, 171.29), 111, 27));
            Vehicles.Add(API.createVehicle((VehicleHash)(-713569950), new Vector3(474.3908, -591.524, 28.4945), new Vector3(0, 0, 174.8065), 111, 64));
            Vehicles.Add(API.createVehicle((VehicleHash)(65402552), new Vector3(393.9163, -641.663, 28.00065), new Vector3(0, 0, -89.13536), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(444171386), new Vector3(422.4557, -587.1438, 28.42794), new Vector3(0, 0, 142.0633), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(723973206), new Vector3(432.8897, -610.8251, 28.07017), new Vector3(0, 0, -92.44521), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(-2072933068), new Vector3(470.8587, -616.1309, 29.32965), new Vector3(0, 0, 172.1459), 0, 0));
            Vehicles.Add(API.createVehicle((VehicleHash)(886934177), new Vector3(415.8954, -644.1381, 28.08931), new Vector3(0, 0, 88.48782), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(1039032026), new Vector3(409.1006, -649.4869, 27.99963), new Vector3(0, 0, -85.61617), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(1221512915), new Vector3(433.5679, -603.1773, 27.96392), new Vector3(0, 0, 83.96123), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(1337041428), new Vector3(416.1511, -641.1852, 27.99767), new Vector3(0, 0, 93.00301), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(1723137093), new Vector3(408.4015, -636.0602, 27.89916), new Vector3(0, 0, 79.23195), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(-713569950), new Vector3(429.8405, -640.86, 28.49508), new Vector3(0, 0, 0.7692581), 111, 27));
            Vehicles.Add(API.createVehicle((VehicleHash)(-2072933068), new Vector3(408.5434, -624.2587, 29.42403), new Vector3(0, 0, 87.54694), 0, 0));
            Vehicles.Add(API.createVehicle((VehicleHash)(-1137532101), new Vector3(393.9472, -655.1495, 28.13573), new Vector3(0, 0, 84.39259), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(1723137093), new Vector3(393.6889, -649.6031, 27.89835), new Vector3(0, 0, -88.52162), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(-1177863319), new Vector3(408.9158, -641.6357, 28.17505), new Vector3(0, 0, -96.61128), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(1039032026), new Vector3(431.8293, -623.8922, 27.99941), new Vector3(0, 0, -99.88419), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(499169875), new Vector3(432.6457, -618.955, 27.77766), new Vector3(0, 0, 89.21114), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(-1883002148), new Vector3(433.2809, -613.6787, 28.00048), new Vector3(0, 0, -91.33807), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));
            Vehicles.Add(API.createVehicle((VehicleHash)(-1130810103), new Vector3(434.0393, -608.0451, 27.98985), new Vector3(0, 0, -95.35843), RanCol[rnd.Next(0, RanCol.Length - 1)], RanCol[rnd.Next(0, RanCol.Length - 1)]));

            API.createObject(868148414, new Vector3(449.3575, -666.3087, 27.2993), new Vector3(-4.461952E-05, 3.50003, 1.000003));
            API.createObject(868148414, new Vector3(449.4, -667.4321, 27.31672), new Vector3(-4.461284E-05, 3.50003, 1.000003));
            API.createObject(868148414, new Vector3(449.24, -668.28, 27.58), new Vector3(-53.25006, 3.390037, 1.250002));
            API.createObject(238110203, new Vector3(450.8749, -668.9719, 27.23237), new Vector3(0, 0, 17.99901));
            API.createObject(1603241576, new Vector3(424.0487, -670.225, 27.24), new Vector3(9.542027E-15, 0.5000057, 5.008955E-06));
            API.createObject(1603241576, new Vector3(423.9966, -667.6838, 27.21), new Vector3(0, 0, 0));
            API.createObject(1603241576, new Vector3(423.9447, -665.16, 27.18), new Vector3(0, 0, 0));
            API.createObject(1603241576, new Vector3(423.8884, -662.4147, 27.04), new Vector3(0, 0, 0));
            API.createObject(1603241576, new Vector3(459.1097, -668.8286, 25.84), new Vector3(1.00179E-05, 4.249997, -5.008956E-06));
            API.createObject(1603241576, new Vector3(459.0637, -666.61, 25.84), new Vector3(1.00179E-05, 4.249997, -5.008956E-06));
            API.createObject(1603241576, new Vector3(458.9534, -664.72, 25.87), new Vector3(1.001788E-05, 3.500002, 5.008956E-06));
            API.createObject(-1901227524, new Vector3(451.5363, -553.7686, 27.49981), new Vector3(0, 0, 24.99991));
            API.createObject(160663734, new Vector3(451.8625, -572.7619, 27.49981), new Vector3(1.069211E-05, -3.500003, 125.9988));
            API.createObject(729253480, new Vector3(475.1102, -575.8093, 27.49976), new Vector3(0, 0, -8.975495));
            API.createObject(729253480, new Vector3(470.8779, -632.5427, 27.49978), new Vector3(0, 0, -97.97574));
            API.createObject(-1022684418, new Vector3(411.0536, -670.3841, 28.28974), new Vector3(0, 0, 0));
            API.createObject(-1022684418, new Vector3(444.8453, -683.7726, 27.58893), new Vector3(0, 0, 179.999));
            API.createObject(2142033519, new Vector3(413.347, -669.185, 28.27083), new Vector3(0, 0, 0));
            API.createObject(1888204845, new Vector3(442.4565, -685.233, 27.71027), new Vector3(0, 0, -174.0009));
            API.createObject(729253480, new Vector3(440.1435, -595.4543, 27.71517), new Vector3(0, 0, 174.024));
            API.createObject(729253480, new Vector3(426.3918, -580.5024, 27.53382), new Vector3(0, 0, 48.02322));
            API.createObject(729253480, new Vector3(476.7946, -605.9749, 27.49952), new Vector3(0, 0, 176.0221));
            API.createObject(1821241621, new Vector3(389.9014, -662.6171, 27.79809), new Vector3(0, 0, 137.9997));
            API.createObject(-845118873, new Vector3(443.1718, -661.1084, 31.33418), new Vector3(-89.99988, 6.223893E-08, -0.998759));
            API.createObject(-1686309583, new Vector3(417.4601, -666.3728, 28.2443), new Vector3(0, 0, 35.99995));

            API.createPed((PedHash)(349680864), new Vector3(458.6499, -574.6008, 28.49981), 155.0682f);
            API.createPed((PedHash)(1358380044), new Vector3(394.0978, -639.5235, 28.50037), -26.89476f);
            API.createPed((PedHash)(813893651), new Vector3(393.7617, -638.4045, 28.50041), -102.1077f);
            API.createPed((PedHash)(1358380044), new Vector3(395.272, -638.5978, 28.50035), 111.9992f);
            API.createPed((PedHash)(-1386944600), new Vector3(449.9078, -650.5392, 28.47753), -82.68468f);
            API.createPed((PedHash)(1640504453), new Vector3(452.0094, -633.2776, 28.52544), 96.56088f);
            API.createPed((PedHash)(891398354), new Vector3(443.8945, -685.416, 28.63767), 38.07472f);
            API.createPed((PedHash)(-1736970383), new Vector3(435.4257, -642.9184, 28.73556), 140.2522f);
            API.createPed((PedHash)(330231874), new Vector3(439.6191, -685.6738, 28.84847), -5.755019f);


            //Marker Blibs
            Blip Bus = API.createBlip(new Vector3(437.6204, -625.9449, 28.1));
            API.setBlipSprite(Bus, 513);
            API.setBlipColor(Bus, 5);
            API.setBlipName(Bus, "Bus Station.");
            API.createMarker(0, new Vector3(437.6204, -625.9449, 28.1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.75, 0.75, 0.75), 255, 0, 0, 255);
            API.createMarker(0, new Vector3(435.9425, -647.543, 28.1), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0.75, 0.75, 0.75), 255, 0, 0, 255);
            API.createMarker(0, new Vector3(454.4912, -571.7501, 27.84), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.75, 0.75, 0.75), 255, 0, 0, 255);

            foreach (Vehicle Vehicle in Vehicles)
            {
                Vehicle.engineStatus = false;
                Vehicle.numberPlate = "GTG-Bus";
                int AutoGas = (int)75;
                Vehicle.setData("AutoGas", AutoGas);
            }
        }

        private void OnPlayerEnterVehicleHandler(Client player, NetHandle vehicle)
        {
            foreach (Vehicle Vehicle in Vehicles)
            {
                if (Vehicle.handle == vehicle)
                {
                    Vehicle.engineStatus = true;
                    API.shared.triggerClientEvent(player, "update_gas", (int)player.vehicle.getData("AutoGas"));
                    break;
                }
            }
        }
        private void OnPlayerExitVehicleHandler(Client player, NetHandle vehicle)
        {
            foreach (Vehicle Vehicle in Vehicles)
            {
                if (Vehicle.handle == vehicle)
                {
                    Vehicle.engineStatus = false;
                    break;
                }
            }
        }
    }
}
