using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Services.FactionService
{
	class FactionService
	{
		public static void SetPlayerFaction(Client client, FactionType factionType, int rank = 1)
		{
			if (!client.hasData("player")) { return; }
			Player player = client.getData("player");
			player.Character.Faction = factionType;
			player.Character.FactionRank = rank;
			CharacterService.CharacterService.UpdateCharacter(player.Character);
		}

		public static void SetPlayerRank(Client client, int newRank)
		{
			if (!client.hasData("player")) { return; }
			Player player = client.getData("player");
			player.Character.FactionRank = newRank;
			CharacterService.CharacterService.UpdateCharacter(player.Character);
		}

        public static void RemoveAllOfFaction(Client client, int factionid, int rank = 0)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Faction", factionid.ToString() },
                {"@FactionNew", 0.ToString() },
                {"@FactionRankNew", 0.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Faction = @FactionNew, FactionRank = @FactionRankNew WHERE Faction = @Faction", parameters);
        }
    }
}
