﻿using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Services.CharacterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Model
{
	class ClothingShopJson
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public List<Clothing> AvailableTops { get; set; }
		public List<Clothing> AvailableLegs { get; set; }
		public List<Clothing> AvailableFeets { get; set; }
		public List<Clothing> AvailableMasks { get; set; }
        public List<Clothing> AvailableTorso { get; set; }
        public List<Clothing> AvailableUndershirt { get; set; }
        public List<Clothing> AvailableDecals { get; set; }
        public List<Clothing> AvailableBackpacks { get; set; }
        public List<Clothing> AvailableHair { get; set; }
        public List<Clothing> AvailableAccessories { get; set; }

		public List<Clothing> AvailableHats { get; set; }
		public List<Clothing> AvailableGlasses { get; set; }
		public List<Clothing> AvailableEars { get; set; }
		public List<Clothing> AvailableWatches { get; set; }
		public List<Clothing> AvailableBracelets { get; set; }
		public ClothingShopJson(ClothingShop clothingShop, Gender gender)
		{
			Id = 0;
			Name = "Clothing Shop";
			AvailableTops = clothingShop.AvailableTops.Where(x => x.Gender == gender).ToList();
			AvailableLegs = clothingShop.AvailableLegs.Where(x => x.Gender == gender).ToList();
			AvailableFeets = clothingShop.AvailableFeets.Where(x => x.Gender == gender).ToList();
			AvailableMasks = clothingShop.AvailableMasks.Where(x => x.Gender == gender).ToList();
            AvailableTorso = clothingShop.AvailableTorso.Where(x => x.Gender == gender).ToList();
            AvailableUndershirt = clothingShop.AvailableUndershirt.Where(x => x.Gender == gender).ToList();
            AvailableDecals = clothingShop.AvailableDecals.Where(x => x.Gender == gender).ToList();
            AvailableHair = clothingShop.AvailableHair.Where(x => x.Gender == gender).ToList();
            AvailableBackpacks = clothingShop.AvailableBackpacks.Where(x => x.Gender == gender).ToList();
            AvailableAccessories = clothingShop.AvailableAccessories.Where(x => x.Gender == gender).ToList();
			AvailableHats = clothingShop.AvailableHats.Where(x => x.Gender == gender).ToList();
			AvailableGlasses = clothingShop.AvailableGlasses.Where(x => x.Gender == gender).ToList();
			AvailableEars = clothingShop.AvailableEars.Where(x => x.Gender == gender).ToList();
			AvailableWatches = clothingShop.AvailableWatches.Where(x => x.Gender == gender).ToList();
			AvailableBracelets = clothingShop.AvailableBracelets.Where(x => x.Gender == gender).ToList();
		}
	}
}
