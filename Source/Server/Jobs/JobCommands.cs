﻿using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;

namespace FactionLife.Server.Services.FactionService
{
    class JobCommands : Script
    {

        /// <summary>
        /// Handles the startjob command
        /// </summary>
        /// <param name="player">Player who sends the command</param>
        [Command("startjob")]
        public void StartJob(Client player)
        {
            if (PlayerManager.Instance().IsClientUsingCharacter(player))
            {
                Character c = PlayerManager.Instance().GetActiveCharacterForClient(player);
                JobManager.Instance().StartJobForCharacter(c);
            }
        }
    }
}
