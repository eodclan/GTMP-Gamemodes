using FactionLife.Server.Services.ClothingService;
using FactionLife.Server.Services.CharacterService;

namespace FactionLife.Server.Model
{
    class Clothing
    {
        public int Id { get; set; }
        public int Slot { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public int Torso { get; set; }
        public int Undershirt { get; set; }
        public double Price { get; set; }
        public bool AlreadyBought { get; set; }
        public Gender Gender { get; set; }
        public ClothingStoreType StoreType { get; set; }
        public Clothing()
        {
            AlreadyBought = false;
        }
    }
}
