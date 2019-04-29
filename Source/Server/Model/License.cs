using FactionLife.Server.Services.LicenseService;

namespace FactionLife.Server.Model
{
    class License
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public LicenseType Type { get; set; }
    }
}
