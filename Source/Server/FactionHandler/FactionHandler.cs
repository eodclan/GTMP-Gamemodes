using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.ClothingService;
using FactionLife.Server.Services.DoorService;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.FactionShopHandler
{
	class FactionShopHandler 
		: Script
	{
        private void BroadcastToLSPD(Client client, bool onDuty)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            player.Character.OnDuty = onDuty;

            API.shared.getAllPlayers().ForEach(otherclient => {
                if (otherclient.hasData("player"))
                {
                    Player otherplayer = otherclient.getData("player");
                    if (otherplayer.Character.Faction == FactionType.Police && otherplayer.Character.OnDuty)
                    {
                        if (onDuty)
                        {
                            API.shared.sendNotificationToPlayer(otherclient, $"~y~Police Information:~n~~r~Der Raubüberfall wurde abgeschlossen! Die Täter flüchten!");
                        }
                        else
                        {
                            API.shared.sendNotificationToPlayer(client, $"~y~Shop Raub:~n~~r~Du hast Glück, es sind keine Cops im Dienst!");
                        }
                    }
                }
            });
        }


        private void BroadcastToSheriff(Client client, bool onDuty)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");

            player.Character.OnDuty = onDuty;

            API.shared.getAllPlayers().ForEach(otherclient => {
                if (otherclient.hasData("player"))
                {
                    Player otherplayer = otherclient.getData("player");
                    if (otherplayer.Character.Faction == FactionType.Sheriff && otherplayer.Character.OnDuty)
                    {
                        if (onDuty)
                        {
                            API.shared.sendNotificationToPlayer(otherclient, $"~y~Sheriff Information:~n~~r~Der Raubüberfall wurde abgeschlossen! Die Täter flüchten!");
                        }
                        else
                        {
                            API.shared.sendNotificationToPlayer(client, $"~y~Shop Raub:~n~~r~Du hast Glück, es sind keine Cops im Dienst!");
                        }
                    }
                }
            });
        }

    }
}