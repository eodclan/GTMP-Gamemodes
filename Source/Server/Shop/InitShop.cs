using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Shared;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;

namespace FactionLife.Server.Services.AmmuShop
{
    class InitAmmuShop : Script
    {
        public InitAmmuShop()
        {
            API.consoleOutput("[Shop][INFO] Initialisation des shops!");
            API.onEntityEnterColShape += colShapeEnterEvent;
            API.onEntityExitColShape += colShapeExitEvent;
        }

        public const int ITEM_ID_SNIPERRIFLE = 1;
        public const int ITEM_ID_FIREEXTINGUISHER = 2;
        public const int ITEM_ID_VINTAGEPISTOL = 3;
        public const int ITEM_ID_COMBATPDW = 4;
        public const int ITEM_ID_HEAVYSNIPER = 5;
        public const int ITEM_ID_MICROSMG = 6;
        public const int ITEM_ID_PISTOL = 7;
        public const int ITEM_ID_PUMPSHOTGUN = 8;
        public const int ITEM_ID_MOLOTOV = 9;
        public const int ITEM_ID_SMG = 10;
        public const int ITEM_ID_PETROLCAN = 11;
        public const int ITEM_ID_STUNGUN = 12;
        public const int ITEM_ID_DOUBLEBARRELSHOTGUN = 13;
        public const int ITEM_ID_GOLFCLUB = 15;
        public const int ITEM_ID_HAMMER = 16;
        public const int ITEM_ID_COMBATPISTOL = 17;
        public const int ITEM_ID_GUSENBERG = 18;
        public const int ITEM_ID_NIGHTSTICK = 19;
        public const int ITEM_ID_SAWNOFFSHOTGUN = 20;
        public const int ITEM_ID_CARBINERIFLE = 21;
        public const int ITEM_ID_CROWBAR = 22;
        public const int ITEM_ID_FLASHLIGHT = 23;
        public const int ITEM_ID_DAGGER = 24;
        public const int ITEM_ID_BAT = 25;
        public const int ITEM_ID_KNIFE = 26;
        public const int ITEM_ID_BZGAS = 27;
        public const int ITEM_ID_MUSKET = 28;
        public const int ITEM_ID_SNSPISTOL = 29;
        public const int ITEM_ID_ASSUALTRIFLE = 30;
        public const int ITEM_ID_REVOLVER = 31;
        public const int ITEM_ID_HEAVYPISTOL = 32;
        public const int ITEM_ID_KNUCKLEDUSTER = 33;
        public const int ITEM_ID_MARKSMANPISTOL = 34;
        public const int ITEM_ID_MACHETE = 35;
        public const int ITEM_ID_SWITCHBLADE = 36;
        public const int ITEM_ID_HATCHET = 37;
        public const int ITEM_ID_BOTTLE = 38;
        public const int ITEM_ID_SMOKEGRENADE = 39;
        public const int ITEM_ID_PARACHUTE = 40;
        public const int ITEM_ID_MORPHINE = 41;
        public const int ITEM_ID_HEROIN = 42;
        public const int ITEM_ID_SMALLBANDAGE = 43;
        public const int ITEM_ID_LARGEBANDAGE = 44;
        public const int ITEM_ID_COPBADGE = 45;
        public const int ITEM_ID_WEED28 = 46;
        public const int ITEM_ID_WEED = 47;
        public const int ITEM_ID_PLICENSE = 48;
        public const int ITEM_ID_FLICENSE = 49;
        public const int ITEM_ID_AMMOPISTOL = 49;
        public const int ITEM_ID_AMMOSMG = 50;
        public const int ITEM_ID_AMMOASSAULT = 51;
        public const int ITEM_ID_AMMOSNIPER = 52;
        public const int ITEM_ID_AMMOSHOTGUN = 53;
        public const int ITEM_ID_SPIKESTRIP = 54;
        public const int ITEM_ID_SPARETIRE = 55;
        public const int ITEM_ID_SPRUNKPACK = 56;
        public const int ITEM_ID_COLAPACK = 57;
        public const int ITEM_ID_WATERPACK = 58;
        public const int ITEM_ID_COPKEY = 60;
        public const int ITEM_ID_MILJETKEY = 61;
        public const int ITEM_ID_MILHELIKEY = 62;
        public const int ITEM_ID_SPRUNK = 63;
        public const int ITEM_ID_ECOLA = 64;
        public const int ITEM_ID_EWATER = 65;
        public const int ITEM_ID_REPAIRKIT = 66;
        public const int ITEM_ID_COMP_SNIPERBARREL = 67;
        public const int ITEM_ID_COMP_SNIPERSTOCK = 68;
        public const int ITEM_ID_COMP_SNIPERRECIEVER = 69;
        public const int ITEM_ID_COMP_MICROSMGBARREL = 70;
        public const int ITEM_ID_COMP_MICROSMGSTOCK = 71;
        public const int ITEM_ID_COMP_MICROSMGRECIEVER = 72;
        public const int ITEM_ID_COMP_SMGBARREL = 73;
        public const int ITEM_ID_COMP_SMGSTOCK = 74;
        public const int ITEM_ID_COMP_SMGRECIEVER = 75;
        public const int ITEM_ID_COMP_DOUBLEBARRELSHOTGUNBARREL = 76;
        public const int ITEM_ID_COMP_DOUBLEBARRELSHOTGUNSTOCK = 77;
        public const int ITEM_ID_COMP_DOUBLEBARRELSHOTGUNRECIEVER = 78;
        public const int ITEM_ID_COMP_AKBARREL = 79;
        public const int ITEM_ID_COMP_AKSTOCK = 80;
        public const int ITEM_ID_COMP_AKRECIEVER = 81;
        public const int ITEM_ID_COMP_SAWNOFFBARREL = 82;
        public const int ITEM_ID_COMP_SAWNOFFSTOCK = 83;
        public const int ITEM_ID_COMP_SAWNOFFRECIEVER = 84;
        public const int ITEM_ID_COMP_HEAVYPISTOLBARREL = 82;
        public const int ITEM_ID_COMP_HEAVYPISTOLSTOCK = 83;
        public const int ITEM_ID_COMP_HEAVYPISTOLRECIEVER = 84;
        public const int ITEM_ID_COMP_50CALBARREL = 82;
        public const int ITEM_ID_COMP_50CALSTOCK = 83;
        public const int ITEM_ID_COMP_50CALRECIEVER = 84;
        public const int ITEM_ID_MineraiOreCopper = 85;
        public const int ITEM_ID_OreCopper = 86;
        public const int ITEM_ID_BUNCH_OF_GRAPES = 87;
        public const int ITEM_ID_GRAPES = 88;
        public const int ITEM_ID_EATCHICKEN = 89;

        public const int ITEM_ID_WEEDNONTRAITER = 90;
        public const int ITEM_ID_WEEDTRAITER = 91;

        public void colShapeEnterEvent(ColShape shape, NetHandle entity)
        {
            var player = API.getPlayerFromHandle(entity);
            if (player == null) return;
            if (shape.hasData("AmmuShop"))
            {
                var ShopObject = shape.getData("AmmuShop");
                {
                    ShopObject.Show(player);
                }
            }
        }

        public void colShapeExitEvent(ColShape shape, NetHandle entity)
        {
            var player = API.getPlayerFromHandle(entity);
            if (player == null) return;
            if (shape.hasData("AmmuShop"))
            {
                API.triggerClientEvent(player, "menu_handler_close_menu");
            }
        }
    }

    class Shop : Script
    {
        public string Name;
        public double X;
        public double Y;
        public double Z;
        public int ModelHash;
        public float PedHeading;
        public List<KeyValuePair<int, int>> Products;
        public int Dimension;
        //   public int RequiredItem;

        public Shop()
        {

        }

        public Shop(string name, double x, double y, double z, int modelhash, float pedheading, List<KeyValuePair<int, int>> products, int dimension)
        {
            Name = name;
            X = x; Y = y; Z = z;
            ModelHash = modelhash;
            PedHeading = pedheading;
            Products = products;
            Dimension = dimension;
            Init();
        }

        public virtual void Init()
        {
            //API.shared.createMarker(1, (new Vector3(X, Y, Z) - (new Vector3(0f, 0f, 1f))), new Vector3(), new Vector3(), new Vector3(2f, 2f, 1f), 255, 255, 255, 0, 0);
        }

        public virtual void Show(Client sender)
        {

        }

        public virtual void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {

        }
    }
}
