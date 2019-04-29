using System.Linq;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace FactionLife.Server.Services.CrouchService
{
    public class CrouchService : Script
    {
        const string EntityDataKey = "IsCrouched";
        const float SyncRange = 350f;

        public CrouchService()
        {
            API.onClientEventTrigger += Crouch_EventTrigger;
            API.onPlayerModelChange += Crouch_ModelChange;
        }

        public void TriggerRangedCrouchEvent(string eventName, Client originalPlayer)
        {
            var playersInRange = API.getAllPlayers().Where(p => p.position.DistanceTo(originalPlayer.position) <= SyncRange && p.dimension == originalPlayer.dimension);
            foreach (Client player in playersInRange) player.triggerEvent(eventName, originalPlayer.handle);
        }

        #region Events
        public void Crouch_EventTrigger(Client player, string eventName, params object[] args)
        {
            if (eventName == "ToggleCrouch")
            {
                if (!player.hasSyncedData(EntityDataKey))
                {
                    TriggerRangedCrouchEvent("EnableCrouch", player);
                    player.setSyncedData(EntityDataKey, true);
                }
                else
                {
                    TriggerRangedCrouchEvent("DisableCrouch", player);
                    player.resetSyncedData(EntityDataKey);
                }
            }
        }

        public void Crouch_ModelChange(Client player, int oldModel)
        {
            if (player.hasSyncedData(EntityDataKey)) TriggerRangedCrouchEvent("EnableCrouch", player);
        }
        #endregion
    }
}
