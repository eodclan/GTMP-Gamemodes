using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using FactionLife.Server.Base;
using FactionLife.Server.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FactionLife.Server;
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
using System.Collections;

namespace FactionLife.Server.Services.CharacterService
{
    class IDCardService : Script
    {
        public IDCardService()
        {
            API.onClientEventTrigger += OnClientEvent;
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            Player player = client.getData("player");

            Blip misBlip = API.createBlip(new Vector3(238.0202f, -412.1451f, 48.11195f)); // blip
            API.setBlipSprite(misBlip, 498);
            API.setBlipName(misBlip, "Einwohnermeldeamt");
            API.setBlipColor(misBlip, 498);
            API.setBlipScale(misBlip, 0.8f);

            API.shared.createPed(PedHash.AmmuCountrySMM, new Vector3(238.0202, -412.1451, 48.11195), 5f);

            switch (eventName)
            {
                case "SetIDCardTrueInDatabase":
                    double price = Convert.ToDouble(500);
                    if (player.Character.HasIDCard == 1) {
                        API.sendNotificationToPlayer(client, "Sie besitzen bereits einen Personalausweis!");
                        return;
                    }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, price)) {
                        API.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld dabei!");
                        return;
                    }
                    player.Character.HasIDCard = 1;
                    MoneyService.MoneyService.RemovePlayerCash(client, price);
                    CharacterService.UpdateCharacter(player.Character);
                    API.sendNotificationToPlayer(client, "~g~Sie haben einen Personalausweis erhalten!");
                    break;

                case "KeyboardKey_E_Pressed":
                    if (IsInRangeOf(client.position, new Vector3(238.0202, -412.1451, 48.11195), 5f))
                    {
                        API.triggerClientEvent(client, "OpenEinburgerungsMenu");
                    }
                    break;
            };
        }


        public static bool IsInRangeOf(Vector3 playerPos, Vector3 target, float range)
        {
            var direct = new Vector3(target.X - playerPos.X, target.Y - playerPos.Y, target.Z - playerPos.Z);
            var len = direct.X * direct.X + direct.Y * direct.Y + direct.Z * direct.Z;
            return range * range > len;
        }

    }
}