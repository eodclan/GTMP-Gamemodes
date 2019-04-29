using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.FactionService;
using System;

namespace FactionLife.TeamspeakServer
{
    public class Teamspeak
        : Script
    {
    private Client Player { get; set; }

    public Teamspeak()
    {
        API.onClientEventTrigger += OnClientEventTriggerHandler;
    }

    public Teamspeak(Client player)
    {
        this.Player = player;
    }

    private void OnClientEventTriggerHandler(Client player, string eventName, params object[] arguments)
    {
        if (eventName == "ChangeVoiceRange")
        {
            this.ChangeVoiceRange(player);
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
                break;
            case "Weit":
                voiceRange = "Kurz";
                break;
            case "Kurz":
                voiceRange = "Normal";
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
        new Teamspeak(player).Connect(characterName);
    }

    }
}