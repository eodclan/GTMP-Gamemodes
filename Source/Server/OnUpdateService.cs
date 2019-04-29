using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.ItemService;
using System.Timers;

namespace FactionLife.Server
{
    internal class OnUpdateService
        : Script
    {
        public OnUpdateService()
        {
            API.onPlayerArmorChange += OnPlayerArmorChangeHandler;
        }

        private void OnPlayerArmorChangeHandler(Client entity, int Valuezero)
        {
            Player player = entity.getData("player");

            if (entity.armor == 0)
            {
                int state = 0;
                Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                        { "@Id", player.Character.Id.ToString() },
                        { "@Status", state.ToString() }
                    };

                DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Armorid = @Status  WHERE Id = @Id", parameters);

                CharacterService.UpdateHealthandArmor(entity, player.Character);


                API.shared.setPlayerClothes(entity, 9, 0, 0);
            }

            else { return; }

        }
    }
}