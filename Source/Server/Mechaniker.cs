using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using FactionLife.Server;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.IO;
using FactionLife.Server.Services.MoneyService;
using System.Data;
using GrandTheftMultiplayer.Shared;

namespace FactionLife.Server
{
    public class Mechaniker
        : Script
    {



        public Mechaniker()
        {
  
        }


        [Command("speed")]
        public void SetSpeedMulti(Client player, int idd)
        {
            Player admin = player.getData("player");
            if (admin.Account.AdminLvl != 8) { return; }
            NetHandle vehicle = API.getPlayerVehicle(player);

            float newid = idd;

            API.setVehicleEnginePowerMultiplier(vehicle, idd);
        }

        [Command("checkspeed")]
        public void CheckSpeedMulti(Client player)
        {
            Player admin = player.getData("player");
            if (admin.Account.AdminLvl != 8) { return; }
            NetHandle vehicle = API.getPlayerVehicle(player);


            API.sendChatMessageToPlayer(player, API.getVehicleEnginePowerMultiplier(vehicle).ToString());
        }


        [Command("vehcolor", Group = "Mechaniker Commands")]
        public void VehicleColor(Client client, int color1, int color2)
        {
            if (!client.isInVehicle) { return; }

            OwnedVehicle ownedVehicle = null;
            ownedVehicle = client.vehicle.getData("vehicle");


            string colora = color1.ToString();
            string colorb = color2.ToString();

            client.vehicle.primaryColor = color1;
            client.vehicle.secondaryColor = color2;

            ownedVehicle.PrimaryColor = color1;
            ownedVehicle.SecondaryColor = color2;
            

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() },
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET PrimaryColor = " + colora + ", SecondaryColor = " + colorb + " WHERE Id = @Id LIMIT 1", parameters);

            Services.VehicleService.VehicleService.UpdateVehicle(ownedVehicle);

            API.sendNotificationToPlayer(client, "~r~Erfolg, Farbe gespeichert!");
        }


        [Command("whellcolor")]
        public void TestTyreSmoke(Client client, int r, int g, int b)
        {

            if (!client.isInVehicle) { return; }

            OwnedVehicle ownedVehicle = null;
            ownedVehicle = client.vehicle.getData("vehicle");


            client.vehicle.setMod(20, 1);
            client.vehicle.tyreSmokeColor = new Color(r, g, b);

            string colorr = r.ToString();
            string colorg = g.ToString();
            string colorb = b.ToString();
            int state = 1;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() },
                { "@State", state.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET TyreSmokeState = @State, tyreSmokeColorR = " + colorr + ", tyreSmokeColorG = " + colorg + ", tyreSmokeColorB = " + colorb + " WHERE Id = @Id LIMIT 1", parameters);

            API.sendNotificationToPlayer(client, "~r~Erfolg, Reifenrauch gespeichert!");
        }

        [Command("wheelColor")]
        public void WheelColor(Client client, int id)
        {

            if (!client.isInVehicle) { return; }

            OwnedVehicle ownedVehicle = null;
            ownedVehicle = client.vehicle.getData("vehicle");


            client.vehicle.setMod(16, 1);
            client.vehicle.wheelColor = id;

            string colorr = id.ToString();

            ownedVehicle.wheelColor = id;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET wheelColor = " + colorr + " WHERE Id = @Id LIMIT 1", parameters);

            API.sendNotificationToPlayer(client, "~r~Erfolg, Reifenrauch gespeichert!");
        }



        [Command("delwhellcolor")]
        public void DeleteTyreSmoke(Client client)
        {

            if (!client.isInVehicle) { return; }

            OwnedVehicle ownedVehicle = null;
            ownedVehicle = client.vehicle.getData("vehicle");


            client.vehicle.setMod(20, 1);
            client.vehicle.tyreSmokeColor = new Color(150, 150, 150);

            string colorr = 0.ToString();
            string colorg = 0.ToString();
            string colorb = 0.ToString();
            int state = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() },
                { "@State", state.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET TyreSmokeState = @State, tyreSmokeColorR = " + colorr + ", tyreSmokeColorG = " + colorg + ", tyreSmokeColorB = " + colorb + " WHERE Id = @Id LIMIT 1", parameters);

            API.sendNotificationToPlayer(client, "~r~Erfolg, Reifenrauch gelöscht!");
            //API.shared.sendChatMessageToPlayer(client, "Erfolg, Reifenrauch gelöscht!");

        }

        [Command("neon")]
        public void NeonVehicleColor(Client client, int r, int g, int b)
        {
            

            if (!client.isInVehicle) { return; }

            OwnedVehicle ownedVehicle = null;
            ownedVehicle = client.vehicle.getData("vehicle");
            

            for (int slot = 0; slot <= 3; slot++)
            {
                API.setVehicleNeonState(client.vehicle, slot, true);
            }

            API.setVehicleNeonColor(client.vehicle, r, g, b);


            string colorr = r.ToString();
            string colorg = g.ToString();
            string colorb = b.ToString();

            int state = 1;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() },
                { "@State", state.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET NeonCarState = @State, neonCarColorR = " + colorr + ", neonCarColorG = " + colorg + ", neonCarColorB = " + colorb + " WHERE Id = @Id LIMIT 1", parameters);

            API.sendNotificationToPlayer(client, "~r~Erfolg, Neonlichter gespeichert!");
        }

        [Command("delneon")]
        public void DeleteNeonVehicleColor(Client client)
        {


            if (!client.isInVehicle) { return; }

            OwnedVehicle ownedVehicle = null;
            ownedVehicle = client.vehicle.getData("vehicle");


            for (int slot = 0; slot <= 3; slot++)
            {
                API.setVehicleNeonState(client.vehicle, slot, true);
            }

            API.setVehicleNeonColor(client.vehicle, 0, 0, 0);


            string colorr = 0.ToString();
            string colorg = 0.ToString();
            string colorb = 0.ToString();

            int state = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() },
                { "@State", state.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET NeonCarState = @State, neonCarColorR = " + colorr + ", neonCarColorG = " + colorg + ", neonCarColorB = " + colorb + " WHERE Id = @Id LIMIT 1", parameters);

            API.sendNotificationToPlayer(client, "~r~Erfolg, Neonlichter gelöscht!");
        }

    }
}
