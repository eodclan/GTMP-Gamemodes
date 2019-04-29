using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Model
{
	class BoughtClothing
	{
		public List<int> Tops { get; set; }
		public List<int> Legs { get; set; }
		public List<int> Feets { get; set; }
        public List<int> Torso { get; set; }

        public List<int> Masks { get; set; }
        public List<int> Undershirt { get; set; }
        public List<int> Decals { get; set; }
        public List<int> Hair { get; set; }
        public List<int> Backpacks { get; set; }
        public List<int> Accessories { get; set; }

		public List<int> Hats { get; set; }
		public List<int> Glasses { get; set; }
		public List<int> Ears { get; set; }
		public List<int> Watches { get; set; }
		public List<int> Bracelets { get; set; }

		public BoughtClothing()
		{

			Tops = new List<int>();
            Torso = new List<int>();
            Legs = new List<int>();
			Feets = new List<int>();
			Masks = new List<int>();
            Undershirt = new List<int>();
            Decals = new List<int>();
            Hair = new List<int>();
            Backpacks = new List<int>();
            Accessories = new List<int>();
			Hats = new List<int>();
			Glasses = new List<int>();
			Ears = new List<int>();
			Watches = new List<int>();
			Bracelets = new List<int>();
		}
	}
}
