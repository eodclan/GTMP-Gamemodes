﻿using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Model
{
	class Dispatches
	{
		public Client Client { get; set; }
		public Player Player { get; set; }
		public Blip Blip { get; set; }
        public DateTime Time { get; set; }
    }
}
