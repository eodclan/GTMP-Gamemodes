﻿namespace FactionLife.Server.Model
{
	class InventoryMenuItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Count { get; set; }
        public int MaxStorage { get; set; }
    }
}
