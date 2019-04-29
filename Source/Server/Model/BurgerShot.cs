﻿using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;

namespace FactionLife.Server.Model
{
	public class BurgerShot
	{
		public int Id { get; set; }
		public string Owner { get; set; }
		public List<BurgerShotItem> Storage { get; set; }
		public double MoneyStorage { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 PedPosition { get; set; }
		public Vector3 PedRotation { get; set; }
		public string MenuImage { get; set; }
		public Ped Ped { get; set; }
		public Blip Blip { get; set; }
		public BurgerShot()
		{
			Storage = new List<BurgerShotItem>();
			MoneyStorage = 0;
			Position = new Vector3();
			PedPosition = new Vector3();
			PedRotation = new Vector3();
			MenuImage = "";
		}
	}
}