using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Services.VehicleService;
using System.Collections.Generic;

namespace FactionLife.Server.Model
{
	class Garage
	{
		public int Id { get; set; }
		public Vector3 Position { get; set; }
		public float PedRotation { get; set; }
		public FactionType FactionType { get; set; }
		public List<GarageSpawn> Spawnpoints { get; set; }
		public NetHandle Ped { get; set; }
		public Blip MapMarker { get; set; }
		public GarageType Type { get; set; }
		public Garage()
		{
			Position = new Vector3();
			PedRotation = 0;
			Spawnpoints = new List<GarageSpawn>();
			FactionType = FactionType.Citizen;
			Type = GarageType.GroundVehicle;
		}
	}
}
