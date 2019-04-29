using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Base;
using FactionLife.Server.Model;

namespace FactionLife.Server
{
    internal class BlipManagerHandler
        : Script
    {
    public class BlipManager : Script
    {

        public static List<GrandTheftMultiplayer.Server.Elements.Blip> BlipsOnMap = new List<GrandTheftMultiplayer.Server.Elements.Blip>();
        public BlipManager()
        {
            
            db_Blips dbBlips = new Database.db_Blips();
            dbBlips.GetAll();

            foreach (var item in db_Blips.currentBlips.Items)
            {
                BlipsOnMap.Add(API.createBlip(item.Position, item.Range, item.Dimension));
                BlipsOnMap.LastOrDefault().color = item.Color;
                BlipsOnMap.LastOrDefault().name = item.Name;
                BlipsOnMap.LastOrDefault().sprite = item.ModelId;
            }
        }



        
    }
}
