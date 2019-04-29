using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Services.FactionService;
using System.Collections.Generic;

namespace FactionLife.Server.Model
{
	public class OwnedVehicle
	{
        public int Id { get; set; }
		public string Owner { get; set; }
		public int OwnerCharId { get; set; }
		public int Model { get; set; }
		public string ModelName { get; set; }
		public int EngineHealth { get; set; }
		public int Fuel { get; set; }
		public FactionType Faction { get; set; }
		public bool InUse { get; set; }

		public string NumberPlate { get; set; }
		public int PrimaryColor { get; set; }
		public int SecondaryColor { get; set; }
		public int Livery { get; set; }

        public int TyreSmokeState { get; set; }
        public int tyreSmokeColorR { get; set; }
        public int tyreSmokeColorG { get; set; }
        public int tyreSmokeColorB { get; set; }

        public int NeonCarState { get; set; }
        public int neonCarColorR { get; set; }
        public int neonCarColorG { get; set; }
        public int neonCarColorB { get; set; }

        public int wheelColor { get; set; }

        public List<InventoryItem> Inventory { get; set; }

		public Vehicle ActiveHandle { get; set; }
        public int BackWheels { get; internal set; }
        public int FrontWheels { get; internal set; }
        public int Exhaust { get; internal set; }
        public int Fender { get; internal set; }
        public int Hood { get; internal set; }
        public int Horns { get; internal set; }
        public int MaxStorage { get; internal set; }
        public int RightFender { get; internal set; }
        public int Spoilers { get; internal set; }
        public int Turbo { get; internal set; }
        public int Xenon { get; internal set; }

        public OwnedVehicle()
		{
			Inventory = new List<InventoryItem>();
			Livery = 0;
		}
	}
}
