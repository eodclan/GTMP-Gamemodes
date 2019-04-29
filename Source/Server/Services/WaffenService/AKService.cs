using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using GrandTheftMultiplayer.Server.Constant;
using FactionLife.Server.Services.FactionService;
using System.Data;
using System.Timers;
using GrandTheftMultiplayer.Shared.Math;

namespace FactionLife.Server.Services.WaffenService

{
    class AkService : Script
    {
        public AkService()
        {
            API.onClientEventTrigger += OnClientEvent;
            API.onResourceStart += OnStart;
        }

        private Ped WaffenPed = null;
        public object Waffentitem { get; private set; }

        public void OnStart()
        {
           WaffenPed = API.createPed(PedHash.Factory01SMY, new Vector3(55.80976, 3689.763, 39.92129), 52);  //Bitte aufpassen kevin da drauf xD
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            Player player = client.getData("player");

            switch (eventName)
            {
                case "KeyboardKey_E_Pressed":

                    if (IsInRangeOf(client.position, new Vector3(55.80976, 3689.763, 39.92129), 2f))
                    {
                        if (client.isInVehicle) { return; }
                        if (API.getEntityData(client, "in_action") == true) { return; }

                        API.setEntityData(client, "in_action", true);

                        InventoryItem EisenbarrenItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 22);
                        InventoryItem AssaultRifleItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 50);

                        if ((EisenbarrenItem == null))
                        {
                            API.sendPictureNotificationToPlayer(client, "Sie Brauchen mindestens 250 Eisenbarren...", "CHAR_BLOCKED", 0, 3, "Privat", "Waffen"); SetStateFalse(client); return;
                        }
                      
                        addAssaultRifle(client);
                    }
                    break;
            }
        }

        //nein bin ich net traurig egal besser wen man das nicht weiß da ich so schlechte erfarung da mit habe wen ich mit jmd drüber reden tuhe ...........
        // und wie ok -_- 
        //
        //
        // OMG DEINE STMME VOLL SCHÖN *.* ne mom mus testen eben
        //
        //
        //CUTTET

        public void RemoveContents(Client client)
        { }

        public void addAssaultRifle(Client client)
        {
            Player player = client.getData("player");
			
            InventoryItem EisenbarrenItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 22);
            InventoryItem AssaultRifleItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == 50);
          
            API.delay(5000, true, () =>
            {
                Random rnd = new Random();
                int AssaultRiflecount = rnd.Next(1, 1);
				
                EisenbarrenItem.Count = EisenbarrenItem.Count -25;
                if (EisenbarrenItem.Count <= 0)
                {
                    player.Character.Inventory.Remove(EisenbarrenItem);
                }

                if (AssaultRifleItem == null)
                {
                    player.Character.Inventory.Add(new InventoryItem
                    {
                        ItemID = 50,
                        Count = AssaultRiflecount
                    });
                }
                else
                {
                    if (AssaultRiflecount == 1)
                    {
                        AssaultRifleItem.Count = AssaultRifleItem.Count + 1;
                    }
                }

                API.sendPictureNotificationToPlayer(client, "Sie haben ~g~" + AssaultRifleItem + " ~w~ AssaultRifle erhalten", "CHAR_BLOCKED", 0, 3, "Privat", "Waffe");
                API.setEntityData(client, "in_action", false);
                return;
            }); 
        }

        public void SetStateFalse(Client client)
        {
            Player player = client.getData("player");

            API.delay(1500, true, () =>
            {
                API.setEntityData(client, "in_action", false);
            });
        }

        public static bool IsInRangeOf(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }
    }
}