namespace FactionLife.Server.Model
{
	public class WeaponShopMenuItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double BuyPrice { get; set; }
		public double SellPrice { get; set; }
		public int Count { get; set; }
        public string Title { get; internal set; }
        public string Value1 { get; internal set; }
    }
}
