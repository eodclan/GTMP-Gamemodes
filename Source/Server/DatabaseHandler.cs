using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using System.Data;
using System.Threading;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.BlipService;
using FactionLife.Server.Services.ClothingService;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.ShopService;
using FactionLife.Server.Services.BurgerShotService;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Services.DoorService;
using FactionLife.Server.Services.LicenseService;
using FactionLife.Server.Services.DealerService;
using FactionLife.Server.Services.SellerService;
using FactionLife.Server.Services.EisenService;
using FactionLife.Server.Services.EisenSellerService;
using FactionLife.Server.Services.GoldService;
using FactionLife.Server.Services.GoldSellerService;
using FactionLife.Server.Services.HolzService;
using FactionLife.Server.Services.HolzSellerService;
using FactionLife.Server.Services.WeaponLicenseService;
using FactionLife.Server.Services.FisherSellService;
using FactionLife.Server.Services.PayNLoudService;
using FactionLife.Server.Services.TequiLaLaService;
using FactionLife.Server.Services.WeaponShopService;

namespace FactionLife.Server
{
	 
	class DatabaseHandler
		: Script
	{
		private static MySqlConnection _connection;
		private string _server;
        private string _port;
        private string _database;
		private string _uid;
		private string _password;
        private static string _connstring;

		// Server Start Password
		private string oldServerPassword;

        public static object MultipleActiveResultSets { get; private set; }

        //Constructor
        public DatabaseHandler()
		{
			API.onResourceStop += OnResourceStopHandler;
			API.onResourceStart += OnResourceStartHandler;
		}

		public void OnResourceStartHandler()
		{
			oldServerPassword = API.getServerPassword();
			Random rand = new Random();
			API.shared.setServerPassword(rand.Next().ToString());
			API.consoleOutput("Gesperrter Server mit Passwort: " + API.getServerPassword());
			Initialize();
		}

		public void OnResourceStopHandler()
		{
			if (CloseConnection())
			{
				API.consoleOutput(LogCat.Info, "Geschlossene Datenbankverbindung ..");
			}
			else
			{
				API.consoleOutput(LogCat.Error, "Datenbankverbindung konnte nicht geschlossen werden.");
			}
		}

        private void WaitForEndOfRead()
        {
            int count = 0;
            while (_connection.State == ConnectionState.Fetching)
            {
                API.sleep(1000);
                count++;
                if (count == 10)
                {
                    break;
                }
            }
        }

        //Initialize values
        private void Initialize()
		{
			if (API.hasSetting("mysql_server")) _server = API.getSetting<string>("mysql_server");
            if (API.hasSetting("mysql_port")) _port = API.getSetting<string>("mysql_port");
            if (API.hasSetting("mysql_user")) _uid = API.getSetting<string>("mysql_user");
			if (API.hasSetting("mysql_password")) _password = API.getSetting<string>("mysql_password");
			if (API.hasSetting("mysql_database")) _database = API.getSetting<string>("mysql_database");
			if (_server == null || _port == null || _database == null || _uid == null || _password == null)
			{
				API.consoleOutput(LogCat.Fatal, "Einige MySQL Informationen fehlen!");
			}
			else
			{
				var connectionString = "SERVER=" + _server + "; PORT= " + _port + "; " + "DATABASE=" + _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";SslMode=none; convert zero datetime=True";
                _connstring = connectionString;
				_connection = new MySqlConnection(connectionString);

				if (OpenConnection())
				{
					if(_connection.State != ConnectionState.Open) { API.consoleOutput(LogCat.Fatal, "Connection State: " + _connection.State); Initialize(); return; }
					API.consoleOutput(LogCat.Info, "Erfolgreich mit der Datenbank verbunden.");
					CloseConnection();
					Dictionary<string, string> parameters = new Dictionary<string, string>();
					#region Startup Data Count Info
					DataTable result = ExecutePreparedStatement("SELECT COUNT(*) AS accountcount FROM accounts", parameters);
					if (result.Rows.Count != 0)
					{
						// Datenbank Eintrag gefunden
						foreach (DataRow row in result.Rows)
						{
							API.consoleOutput(LogCat.Info, "Found " + row["accountcount"] + " Accounts.");
						}
					}
					#endregion
					if (CheckDatabaseStructure())
					{
						LoadAllMySQLInformations();
					}
				}
				else
				{
					API.consoleOutput(LogCat.Fatal, "Keine Verbindung zur Datenbank möglich!");
				}
			}

		}

		//open connection to database
		private bool OpenConnection()
		{
			try
			{
				_connection.Open();
				return true;
			}
			catch (MySqlException ex)
			{
				//When handling errors, you can your application's response based 
				//on the error number.
				switch (ex.Number)
				{
					case 0:
						API.consoleOutput(LogCat.Error, "Kann nicht mit dem Server verbinden..");
						API.consoleOutput(LogCat.Error, ex.Message);
						break;
					case 1044:
						API.consoleOutput(LogCat.Error, $"Zugang für Benutzer verweigert '{_uid}'@'{_password}' to database '{_database}' (ErrCode: {ex.Number})");
						break;
					case 1045:
						API.consoleOutput(LogCat.Error, $"Benutzername / Kennwort ungültig.. (ErrCode: {ex.Number})");
						break;
					case 1051:
						API.consoleOutput(LogCat.Error, " Unbekannte Tabelle '" + _database + $"' (ErrCode: {ex.Number})");
						break;
				}
				return false;
			}
		}


		//Close connection
		private bool CloseConnection()
		{
			try
			{
				_connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
				API.consoleOutput(LogCat.Error, ex.Message);
				return false;
			}
		}

		public static DataTable ExecutePreparedStatement(string sql, Dictionary<string, string> parameters)
		{
            MySqlConnection conn =  new MySqlConnection(_connstring);

            using (conn)
            {
                
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    { 
                        MultipleActiveResultSets = true;

                    if (conn.State != ConnectionState.Open && conn.State != ConnectionState.Connecting)
                    {
                        conn.Open();
                    }

                    foreach (KeyValuePair<string, string> entry in parameters)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    { 
                        API.shared.consoleOutput(LogCat.Info, "DBR: Reader gestartet");
                        DataTable results = new DataTable();
                        results.Load(rdr);
                        API.shared.consoleOutput(LogCat.Info, "DBR: Daten in DT übergeben");
                        API.shared.consoleOutput(LogCat.Info, "API delay gestartet");
                        API.shared.consoleOutput(LogCat.Info, "DBR + conn Closed. Daten werden returned");
                        return results;
                        }
                    }
             
                }
                catch (Exception ex)
                {
                    if (conn != null)
                    {
                        API.shared.consoleOutput(LogCat.Error, "DATABASE: [ERROR] " + ex.Message);
                        conn.Dispose();
                        if (parameters != null | parameters.Count > 0)
                        {
                            parameters.Clear();
                        }
                    }
                    throw;
                }
            }           
        }
		/*
			DataTable result = Database.executeQueryWithResult(query);
			if (result.Rows.Count != 0)
			{
				// Datenbank Eintrag gefunden
				foreach (DataRow row in result.Rows)
				{
					row[""];
				}
			}
		*/

		private bool CheckDatabaseStructure()
		{
			bool returnvar = false;
			Dictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "@database", _database }
			};
			DataTable result = ExecutePreparedStatement("SELECT COUNT(*) AS count FROM information_schema.tables WHERE table_schema = @database AND " +
				"(table_name = 'accounts' OR " +		// 1
				"table_name = 'atms' OR " +             // 2
				"table_name = 'characters' OR " +       // 3
				"table_name = 'clothes_feets' OR " +    // 4
				"table_name = 'clothes_legs' OR " +     // 5
				"table_name = 'clothes_tops' OR " +     // 6
				"table_name = 'clothingshops' OR " +    // 7
				"table_name = 'custom_blips' OR " +     // 8
				"table_name = 'doors' OR " +            // 9
				"table_name = 'garages' OR " +          // 10
				"table_name = 'gasstations' OR " +      // 11
				"table_name = 'items' OR " +            // 12
                "table_name = 'ownedvehicles' OR " +    // 13
				"table_name = 'shops' OR " +            // 14
                "table_name = 'licenses' OR " +            // 14
                "table_name = 'wlicenses' OR " +            // 14
                "table_name = 'dealers' OR " +            // 14
                "table_name = 'sellers' OR " +            // 14
                "table_name = 'eisens' OR " +            // 14
                "table_name = 'eisens_seller' OR " +            // 14
                "table_name = 'golds' OR " +            // 14
                "table_name = 'golds_seller' OR " +            // 14
                "table_name = 'holz_job' OR " +            // 14
                "table_name = 'holz_job_seller' OR " +            // 14
                "table_name = 'burgershots' OR " +       // 3
                "table_name = 'tequilala' OR " +       // 3
                "table_name = 'vehicle_paynloud' OR " +       // 3
                "table_name = 'fisher_restaurant' OR " +            // 14
                "table_name = 'weaponshops' OR " +      // 15
                "table_name = 'vehicleinfo' OR " +      // 15
				"table_name = 'vehicleshops' OR " +     // 16
				"table_name = 'whitelist')"             // 17
				, parameters);
			if (result.Rows.Count != 0)
			{ 
				foreach (DataRow row in result.Rows)
				{
					if(Convert.ToInt32(row["count"]) >= 17)
					{
						API.consoleOutput(LogCat.Info, "Database structure ist okay..");
						returnvar = true;
					}
					else
					{
						API.consoleOutput(LogCat.Fatal, "Einige Datenbanktabellen fehlen! (Überprüfen Sie Ihre Datenbanktabellen)");
						API.consoleOutput(LogCat.Warn, "Startvorgang wurde gestoppt.");
						returnvar = false;
					}
				}
			}
			else
			{
				returnvar = false;
			}
			return returnvar;
		}

        public void LoadAllMySQLInformations()
		{
			ATMService.LoadAllATMs();
			BlipService.LoadCustomBlipsFromDatabase();
			ClothingService.LoadAllClothing();
			ItemService.LoadItemsFromDB();
            ShopService.LoadShopsFromDB();
            BurgerShotService.LoadBurgerShotFromDB();
            TequiLaLaService.LoadTequiLaLaFromDB();
            LicenseService.LoadLicensesFromDB();
            WeaponLicenseService.LoadWeaponLicenseFromDB();
            DealerService.LoadDealerFromDB();
			EisenService.LoadEisenFromDB();
            EisenSellerService.LoadEisenSellerFromDB();
			GoldService.LoadGoldFromDB();
			HolzService.LoadHolzFromDB();
			HolzSellerService.LoadHolzSellerFromDB();			
            GoldSellerService.LoadGoldSellerFromDB();			
            SellerService.LoadSellerFromDB();
            FisherSellService.LoadFisherSellFromDB();
            PayNLoudService.LoadPayNLoudFromDB();
            GarageService.LoadAllGarageFromDB();
			VehicleService.ResetAllVehicles();
			GasStationService.LoadAllGasStationsFromDB();
			VehicleService.LoadVehicleInformationsFromDB();
			VehicleShopService.LoadAllVehicleShopsFromDB();
			DoorService.LoadAllDoorsFromDB();
			ClothingShopService.LoadAllClothingShopsFromDB();
            WeaponShopService.LoadWeaponShopsFromDB();

            API.delay(5000, true, () =>
			{
				API.setServerPassword(oldServerPassword);
				API.consoleOutput("Der Server ist jetzt entsperrt.");
			});
		}
	}
}
