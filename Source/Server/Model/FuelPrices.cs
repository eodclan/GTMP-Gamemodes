namespace FactionLife.Server.Model
{
	class FuelPrices
	{
		public double Benzin { get; set; }
		public double Diesel { get; set; }
		public double Gas { get; set; }
		public double Electricity { get; set; }
		public double Kerosene { get; set; }
		public FuelPrices()
		{
            Benzin = 0;
			Diesel = 0;
			Gas = 0;
			Electricity = 0;
			Kerosene = 0;
		}
	}
}
