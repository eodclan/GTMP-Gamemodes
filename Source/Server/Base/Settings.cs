using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared.Math;

namespace FactionLife.Server.Base
{
	public class StartUp : Script
	{
		public StartUp()
		{
			API.onResourceStart += OnResourceStartHandler;
		}

		public void OnResourceStartHandler()
		{
			LoadSettings();
		}

		public void LoadSettings()
		{
			if (API.hasSetting("server_name"))
				Settings.ServerName = API.getSetting<string>("server_name");
			if (API.hasSetting("gamemode_name"))
				Settings.GamemodeName = API.getSetting<string>("gamemode_name");
			if (API.hasSetting("whitelist_enabled"))
				Settings.WhitelistEnabled = API.getSetting<bool>("whitelist_enabled");
            if (API.hasSetting("server_version"))
                Settings.ServerVersion = API.getSetting<string>("server_version");
            API.setServerName(Settings.ServerName + " " + Settings.ServerVersion);
			API.setGamemodeName(Settings.GamemodeName);
			
            //IPL Loading
            API.requestIpl("coronertrash");
            API.requestIpl("Coroner_Int_On");//Krankenhaus
            API.requestIpl("apa_v_mp_h_01_a");//Modern Apartment 1
            API.requestIpl("apa_v_mp_h_01_c");//Modern Apartment 2
            API.requestIpl("apa_v_mp_h_01_b");//Modern Apartment 3
            API.requestIpl("apa_v_mp_h_02_a");//Mody Apartment 1
            API.requestIpl("apa_v_mp_h_02_c");//Mody Apartment 2
            API.requestIpl("apa_v_mp_h_02_b");//Mody Apartment 3
            API.requestIpl("apa_v_mp_h_03_a");//Vibrant Apartment 1
            API.requestIpl("apa_v_mp_h_03_c");//Vibrant Apartment 2
            API.requestIpl("apa_v_mp_h_03_b");//Vibrant Apartment 3
            API.requestIpl("apa_v_mp_h_04_a");//Sharp Apartment 1
            API.requestIpl("apa_v_mp_h_04_c");//Sharp Apartment 2
            API.requestIpl("apa_v_mp_h_04_b");//Sharp Apartment 3
            API.requestIpl("apa_v_mp_h_05_a");//Monochrome Apartment 1
            API.requestIpl("apa_v_mp_h_05_c");//Monochrome Apartment 2
            API.requestIpl("apa_v_mp_h_05_b");//Monochrome Apartment 3
            API.requestIpl("apa_v_mp_h_06_a");//Seductive Apartment 1
            API.requestIpl("apa_v_mp_h_06_c");//Seductive Apartment 2
            API.requestIpl("apa_v_mp_h_06_b");//Seductive Apartment 3
            API.requestIpl("apa_v_mp_h_07_a");//Regal Apartment 1
            API.requestIpl("apa_v_mp_h_07_c");//Regal Apartment 2
            API.requestIpl("apa_v_mp_h_07_b");//Regal Apartment 3
            API.requestIpl("apa_v_mp_h_08_a");//Aqua Apartment 1
            API.requestIpl("apa_v_mp_h_08_c");//Aqua Apartment 2
            API.requestIpl("apa_v_mp_h_08_b");//Aqua Apartment 3			
		}
	}
	public class Settings
	{
		// Account

		#region Account

		public static int MinPasswordLength = 4;
		public static bool AllowNewRegistrations = true;

		#endregion Account

		#region Cameras

		public static readonly Vector3 RegisterCameraPosition = new Vector3(-2029.775, 5273.782, 19.75231);
		public static readonly Vector3 RegisterCameraLookAt = new Vector3(-2030.775, 5273.782, 19.75231);
		public static readonly Vector3 RegisterPedPosition = new Vector3(-2030.775, 5273.782, 19.75231);
		public static readonly Vector3 LoginCameraPosition = new Vector3(-2029.775, 5273.782, 19.75231);
		public static readonly Vector3 LoginCameraLookAt = new Vector3(-2029.775, 5273.782, 19.75231);
		public static readonly Vector3 LoginPedPosition = new Vector3(-2030.775, 5273.782, 19.75231);
		public static readonly Vector3 CharSelectCameraPosition = new Vector3(377.2062, -993.7937, -98.00002);
		public static readonly Vector3 CharSelectCameraLookAt = new Vector3(376.9842, -991.884, -98.60493);
		public static readonly Vector3 CharSelectCameraPedPosition = new Vector3(376.9842, -991.884, -98.60493);
		public static readonly Vector3 CharSelectCameraPedRotation = new Vector3(0, 0, -176.8093);

		#endregion Cameras

		#region Character
		public static double StartMoneyCash = 500;
		public static double StartMoneyBank = 2500;
		#endregion

		// General Settings
		#region General Settings
		public static string ServerName = "RP Deluxe";
		public static string GamemodeName = "RP Deluxe Beta";
        public static string ServerVersion = "0.4";		
		public static bool WhitelistEnabled = false;
        #endregion
    }
}