using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Managers;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using System.Collections.Generic;
using System;

namespace FactionLife.Server.Services.VoiceService
{
    public class VoiceService : Script
    {
        private Client Player { get; set; }

	    public VoiceService()
        {
            API.onClientEventTrigger += OnClientEventTriggerHandler;
        }

        public VoiceService(Client player)
        {
            this.Player = player;
        }

        private void OnClientEventTriggerHandler(Client player, string eventName, params object[] arguments)
        {
            switch (eventName)
            {

                case "ChangeVoiceRange":
                    this.ChangeVoiceRange(player);
                    break;
                case "KeyboardKey_F2_Pressed":
                    ChangeVoiceChange(player);
                    break;
            }
        }

        private void ChangeVoiceRange(Client player)
        {
            String voiceRange = "Normal";
            if (API.hasEntitySyncedData(player, "VOICE_RANGE"))
            {
                voiceRange = API.getEntitySyncedData(player, "VOICE_RANGE");
            }
            switch (voiceRange)
            {
                case "Normal":
                    voiceRange = "Weit";
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Du sprichst nun sehr laut", "CHAR_BLANK_ENTRY", 0, 8, "Voice System", "Deine Stimme");
                    break;
                case "Weit":
                    voiceRange = "Kurz";
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Du sprichst nun sehr leise", "CHAR_BLANK_ENTRY", 0, 8, "Voice System", "Deine Stimme");
                    break;
                case "Kurz":
                    voiceRange = "Normal";
                    API.shared.sendPictureNotificationToPlayer(player, "~g~Du sprichst nun mit einer normalen Lautstärke", "CHAR_BLANK_ENTRY", 0, 8, "Voice System", "Deine Stimme");
                    break;
            }
            API.setEntitySyncedData(player, "VOICE_RANGE", voiceRange);
        }

        public void Connect(String characterName)
	    {
		    API.triggerClientEvent(this.Player, "ConnectTeamspeak");
        }

        public static void Connect(Client player, String characterName)
        {
            new VoiceService(player).Connect(characterName);
        }

        public void Range(String characterName)
        {
            API.triggerClientEvent(this.Player, "ChangeVoiceRange");
        }

        public static void Range(Client player, String characterName)
        {
            new VoiceService(player).Range(characterName);
        }

        [Command("voicer")]
        public void ChangeVoiceChange(Client client)
        {
            ChangeVoiceRange(client);

            API.triggerClientEvent(client, "UpdateVoiceImage");

        }
    }
}