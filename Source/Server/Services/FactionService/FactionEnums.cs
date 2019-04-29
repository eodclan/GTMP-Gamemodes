namespace FactionLife.Server.Services.FactionService
{
	public enum FactionType
	{
		Citizen = 0,
        Police = 1,
		EMS = 2,
		State = 3,
		ACLS = 4,
        Grove = 5,
        Aztecas = 6,
        FIB = 7,
        Army = 8,
        Vagos = 9,
        LostMC = 10,
        Sheriff = 11,
        Fahrschule = 12,
        JVA = 13,
        Bloods = 14,
        Yakuza = 15,
        SWAT = 16,
        Camorra = 17,
        LosSantosLocos = 18
    }

	public enum AnimationFlags
	{
		Loop = 1 << 0,
		StopOnLastFrame = 1 << 1,
		OnlyAnimateUpperBody = 1 << 4,
		AllowPlayerControl = 1 << 5,
		Cancellable = 1 << 7
	}
}
