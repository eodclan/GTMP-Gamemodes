using System;
using System.Collections.Generic;
using System.Linq;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server;
using FactionLife.Server.Model;
using FactionLife.Server.Services.AdminService;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.ClothingService;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.ShopService;
using FactionLife.Server.Services.VehicleService;
using GrandTheftMultiplayer.Shared.Math;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.JobService
{
    public class Fahrschule : Script
    {
        [Command("selllicense", Alias = "givelic")]
        public void GiveLicAsInstructor(Client client, Client schüler, int FührerscheinID)
        {
            Player lehrer = client.getData("player");
            Player schuler = schüler.getData("player");

            int price;
            if (lehrer.Character.Faction != FactionType.Police) { return; }

            foreach (License lic in schuler.Character.Licenses)
            {
                if (lic.Id == FührerscheinID) { return; }
            }

            switch (FührerscheinID)
            {
                case 1:
                    price = 3500;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 1);
                    RemoveFakedLic(schüler, 1);
                    break;
                case 2:
                    price = 1500;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 2);
                    RemoveFakedLic(schüler, 2);
                    break;
                case 3:
                    price = 4000;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 3);
                    RemoveFakedLic(schüler, 3);
                    break;
                case 4:
                    price = 12000;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 4);
                    RemoveFakedLic(schüler, 4);
                    break;
                case 5:
                    price = 28000;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 5);
                    RemoveFakedLic(schüler, 5);
                    break;
                case 6:
                    price = 10000;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 6);
                    RemoveFakedLic(schüler, 6);
                    break;
                case 7:
                    price = 20000;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 7);
                    RemoveFakedLic(schüler, 7);
                    break;
                case 8:
                    price = 5000;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 8);
                    RemoveFakedLic(schüler, 8);
                    break;
                case 9:
                    price = 3000;
                    if (!MoneyService.HasPlayerEnoughBank(schüler, Convert.ToDouble(price))) { return; }
                    RemoveMoney(client, schüler, Convert.ToDouble(price));
                    AddLicense(client, schüler, 9);
                    RemoveFakedLic(schüler, 9);
                    break;
            }

        }

        public void RemoveMoney(Client lehrer, Client client, double price)
        {
            MoneyService.RemovePlayerBank(client, price);
            API.shared.sendNotificationToPlayer(client, "~g~Der Staat hat Ihnen " + price + " $ für Ihre Lizenz abgebucht!");
            API.shared.sendNotificationToPlayer(lehrer, "~g~Die Prüfungsgebühr wurde dem Prüfling abgebucht!");
        }

        public void AddLicense(Client lehrer, Client client, int FührerscheinID)
        {
            Player schuler = client.getData("player");

            switch (FührerscheinID)
            {
                case 1:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "PKW-Führerschein",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 2:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "Motorad-Führerschein",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 3:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "LKW-Führerschein",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 4:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "Flugschein A",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 5:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "Flugschein B",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 6:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "Waffenschein A",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 7:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "Waffenschein B",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 8:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "Taxi License",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
                case 9:
                    schuler.Character.Licenses.Add(new License
                    {
                        Id = FührerscheinID,
                        Name = "Boats License",
                        Description = "Legal",
                        Type = FactionLife.Server.Services.LicenseService.LicenseType.Real
                    });
                    break;
            }

            CharacterService.UpdateCharacter(schuler.Character);

            API.shared.sendNotificationToPlayer(client, "~g~Sie haben einen Führerschein erhalten!");
            API.shared.sendNotificationToPlayer(lehrer, "~g~Sie haben einen Führerschein erhalten!");
        }

        public void RemoveFakedLic(Client client, int LicID)
        {

            Player schuler = client.getData("player");

            License lice1 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 1);
            License lice2 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 2);
            License lice3 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 3);
            License lice4 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 4);
            License lice5 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 5);
            License lice6 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 6);
            License lice7 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 7);
            License lice8 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 8);
            License lice9 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 9);
            License lice10 = schuler.Character.Licenses.FirstOrDefault(x => x.Id == 10);

            foreach (License lic in schuler.Character.Licenses)
            {

                if ((LicID == 1) && (lic.Id == 6)) { schuler.Character.Licenses.Remove(lice6); }

                if ((LicID == 2) && (lic.Id == 7)) { schuler.Character.Licenses.Remove(lice7); }

                if ((LicID == 3) && (lic.Id == 8)) { schuler.Character.Licenses.Remove(lice8); }

                if ((LicID == 4) && (lic.Id == 9)) { schuler.Character.Licenses.Remove(lice9); }

                if ((LicID == 5) && (lic.Id == 10)) { schuler.Character.Licenses.Remove(lice10); }
            }

            CharacterService.UpdateCharacter(schuler.Character);
        }  
    }
}
