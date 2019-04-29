using GrandTheftMultiplayer.Server.API;
using System;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;

namespace FactionLife.Server.Services.MoneyService
{
    class GiveMoneyService
    : Script
    {
        public GiveMoneyService()
        {
            API.onClientEventTrigger += OnClientEvent;
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            switch (eventName)
            {
                case "GiveMoneyValued":

                    GiveMoney(client, (int)arguments[0]);

                    break;
            }
        }

        public static void GiveMoney(Client client, int amount)
        {

            Player player = client.getData("player");
            Player otherPlayer = null;

            double moneygiven = Convert.ToDouble(amount);

            otherPlayer = CharacterService.CharacterService.GetNextPlayerInNearOfPlayer(player);

            if (amount == 0) { return; }
            if (moneygiven == 0) { return; }

            if (otherPlayer != null)
            {
                Client nextPlayer = otherPlayer.Character.Player;

                if (!MoneyService.HasPlayerEnoughCash(client, moneygiven)) {
                    API.shared.sendPictureNotificationToPlayer(client, "Sie haben nicht so viel Geld dabei!.", "CHAR_BANK_MAZE", 0, 3, "MAZE BANK", "MAZE BANK");
                    return;
                }

                MoneyService.RemovePlayerCash(client, moneygiven);
                MoneyService.AddPlayerCash(nextPlayer, moneygiven);

                API.shared.sendPictureNotificationToPlayer(client, "~y~Sie haben ~g~" + otherPlayer.Character.FirstName + ", " + otherPlayer.Character.LastName + " " + moneygiven + " $ ~y~gegeben.", "CHAR_BANK_MAZE", 0, 3, "MAZE BANK", "MAZE BANK");
                API.shared.sendPictureNotificationToPlayer(nextPlayer, "~y~Sie haben ~g~" + moneygiven + " $ ~y~von ~g~" + player.Character.FirstName + ", " + player.Character.LastName + " ~y~bekommen.", "CHAR_BANK_MAZE", 0, 3, "MAZE BANK", "MAZE BANK");

            }


            else API.shared.sendPictureNotificationToPlayer(client, "Keine Person in der Nähe.", "CHAR_BANK_MAZE", 0, 3, "MAZE BANK", "MAZE BANK");
        }
    }

}



