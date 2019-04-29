using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Base;
using FactionLife.Server.Model;
using FactionLife.Server.Services.AccountService;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.VoiceService;

namespace FactionLife.Server
{
    internal class ConnectionHandler
        : Script
    {
        public ConnectionHandler()
        {
            API.onPlayerConnected += OnPlayerConnectedHandler;
            API.onPlayerDisconnected += OnPlayerDisconnectedHandler;
        }

        #region OnPlayerConnectedHandler

        private void OnPlayerConnectedHandler(Client client)
        {
            if (Settings.WhitelistEnabled)
            {
                if (!WhiteListService.IsClientWhitelisted(client.socialClubName))
                {
                    API.kickPlayer(client, "Nicht gewhitelisted. Bitte Whitelisten lassen unter http://rp-deluxe.de !");
                    return;
                }
            }
            Account account = AccountService.LoadAccount(client.socialClubName);
            if (account != null)
            {
                // User has an account
                if (account.Locked == 1)
                {
                    API.kickPlayer(client, "Ihr Konto ist gesperrt!");
                    return;
                }
                client.setData("preaccount", account);
            }
            client.dimension = -10 - API.getAllPlayers().Count;
        }

        #endregion OnPlayerConnectedHandler

        #region OnPlayerDisconnectedHandler

        private void OnPlayerDisconnectedHandler(Client client, string reason)
        {
            if (client.hasData("player"))
            {
                Player player = (Player)client.getData("player");
                CharacterService.UpdateHealthandArmor(client, player.Character);
                CharacterService.UpdateCharacter(player.Character);
                AccountService.SaveAndLogoutPlayer(player.Account);
            }
        }

        #endregion OnPlayerDisconnectedHandler
    }
}