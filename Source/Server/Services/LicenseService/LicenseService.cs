using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;

namespace FactionLife.Server.Services.LicenseService
{
    class LicenseService
    {
        public static readonly List<License> LicenseList = new List<License>();


        public static void LoadLicensesFromDB()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM license", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    LicenseList.Add(new License
                    {
                        Id = (int)row["Id"],
                        Name = (string)row["Name"],
                        Description = (string)row["Description"],
                        Type = (LicenseType)((int)row["Type"])
                    });
                }
                API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Lizenzen geladen..");
            }
            else
            {
                API.shared.consoleOutput(LogCat.Info, "Lizenzen nicht geladen..");
            }
        }


        





    }







}