using GrandTheftMultiplayer.Server.API;

namespace FactionLife.Server.Model
{
    class IplService
    : Script
    {
        public IplService()
        {
            API.onResourceStart += OnResourceStartHandler;
        }

        public void OnResourceStartHandler()
        {
            PrepareMap();
        }

        public static void PrepareMap()
        {
            API.shared.requestIpl("FIBlobby");
            API.shared.removeIpl("FIBlobbyfake");
            API.shared.requestIpl("hei_carrier");
            API.shared.requestIpl("hei_carrier_DistantLights");
            API.shared.requestIpl("hei_Carrier_int1");
            API.shared.requestIpl("hei_Carrier_int2");
            API.shared.requestIpl("hei_Carrier_int3");
            API.shared.requestIpl("hei_Carrier_int4");
            API.shared.requestIpl("hei_Carrier_int5");
            API.shared.requestIpl("hei_Carrier_int6");
            API.shared.requestIpl("hei_carrier_LODLights");
            API.shared.requestIpl("hei_dt1_05_interior_v_fib01_milo_");
            API.shared.requestIpl("hei_dt1_05_interior_v_fib02_milo_");

            API.shared.requestIpl("apa_v_mp_h_04_b");
            API.shared.requestIpl("coronertrash");
            API.shared.requestIpl("coroner_int_on");
            API.shared.requestIpl("bkr_bi_hw1_13_int");

            API.shared.removeIpl("jewel2fake");
            API.shared.removeIpl("bh1_16_refurb");
            API.shared.requestIpl("post_hiest_unload");
            
            API.shared.requestIpl("linvader");
            API.shared.removeIpl("backpackslobbyfake"); // Entfernt die Fake Türen beim Lifeinvader
            API.shared.requestIpl("backpackslobby"); // Fügt richtige Life Infaver Türen hinzu


            API.shared.consoleOutput("IPL geladen!");

        }
    }
}