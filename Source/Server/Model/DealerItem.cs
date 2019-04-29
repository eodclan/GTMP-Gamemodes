namespace FactionLife.Server.Model
{
	public class DealerItem
	{
		public int Id { get; set; }
		public double BuyPrice { get; set; }
		public int Count { get; set; }
        public int MaxStorage { get; internal set; }
    }
}
