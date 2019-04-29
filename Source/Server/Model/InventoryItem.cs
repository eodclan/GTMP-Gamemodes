namespace FactionLife.Server.Model
{
	public class InventoryItem
	{
        public static int invitem { get; internal set; }
        public int ItemID { get; set; }
		public int Count { get; set; }
        public int MaxStorage { get; set; }
    }
}
