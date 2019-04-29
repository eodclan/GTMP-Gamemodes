using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VehicleService;
using System;

namespace FactionLife.Server.Services.FactionService
{
    public class Fib : Script
    {
        public Fib()
        {

            API.onResourceStart += onResourceStart;

        }

        public void onResourceStart()
        {
            //API.createMarker(1, new Vector3(-1157.92078, -2039.11609, 13.1873159), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 255, 255, 0);
            Blip Fib = API.createBlip(new Vector3(146.8701, -716.3765f, 33.13222f));
            API.setBlipSprite(Fib, 468);
            API.setBlipColor(Fib, 4);
            API.setBlipName(Fib, "Fib");
        }

        public static void SetOnDuty(Client client, bool onDuty)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            player.Character.OnDuty = onDuty;

            if (onDuty)
            {
                if (player.Character.Gender == Gender.Male)
                {
                    ClothingService.ClothingService.ApplyOutfit(client, 7); // Male Cop Outfit
                }
                else
                {
                    ClothingService.ClothingService.ApplyOutfit(client, 8); // Female Cop Outfit
                }
            }
            else
            {
                CharacterService.CharacterService.ApplyAppearance(client);
            }

            client.setSyncedData("onduty", onDuty);
            client.setSyncedData("faction", (int)player.Character.Faction);
            if (!onDuty)
            {
                client.resetSyncedData("onduty");
                client.resetSyncedData("faction");
                API.shared.removeAllPlayerWeapons(client);
            }


            API.shared.getAllPlayers().ForEach(otherclient => {
                if (otherclient.hasData("player"))
                {
                    Player otherplayer = otherclient.getData("player");
                    if (otherplayer.Character.Faction == FactionType.FIB && otherplayer.Character.OnDuty)
                    {
                        if (onDuty)
                        {
                            API.shared.sendNotificationToPlayer(otherclient, $"~y~FIB Information:~n~~b~{player.Character.FirstName} {player.Character.LastName} ~w~ist jetzt ~g~im Dienst");
                        }
                        else
                        {
                            API.shared.sendNotificationToPlayer(otherclient, $"~y~FIB Information:~n~~b~{player.Character.FirstName} {player.Character.LastName} ~w~ist jetzt ~g~ausser Dienst");
                        }
                    }
                }
            });
        }
    } 
}