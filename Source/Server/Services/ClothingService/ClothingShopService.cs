using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using FactionLife.Server.Services.MoneyService;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Services.ClothingService
{
    class ClothingShopService
    {
        public static List<ClothingShop> ClothingShopList = new List<ClothingShop>();

        public static void LoadAllClothingShopsFromDB()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM clothingshops", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    ClothingShop clothingShop = new ClothingShop
                    {
                        Id = (int)row["Id"],
                        Name = (string)row["Name"],
                        Blip = (int)row["Blip"],
                        Position = JsonConvert.DeserializeObject<Vector3>((string)row["Position"]),
                        PedPosition = JsonConvert.DeserializeObject<Vector3>((string)row["PedPosition"]),
                        PedRotation = JsonConvert.DeserializeObject<Vector3>((string)row["PedRotation"]),
                        AvailableTops = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Tops"]),
                        AvailableLegs = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Legs"]),
                        AvailableFeets = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Feets"]),
                        AvailableDecals = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Decals"]),
                        AvailableMasks = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Masks"]),
                        AvailableTorso = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Torso"]),
                        AvailableBackpacks = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Backpacks"]),
                        AvailableHair = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Hair"]),
                        AvailableAccessories = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Accessories"]),
                        AvailableHats = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Hats"]),
                        AvailableGlasses = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Glasses"]),
                        AvailableEars = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Ears"]),
                        AvailableWatches = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Watches"]),
                        AvailableBracelets = JsonConvert.DeserializeObject<List<Clothing>>((string)row["Bracelets"])
                    };
                    clothingShop.MapMarker = API.shared.createBlip(clothingShop.Position);
                    clothingShop.MapMarker.shortRange = true;
                    clothingShop.MapMarker.sprite = clothingShop.Blip;
                    clothingShop.MapMarker.scale = 0.75f;
                    if (clothingShop.Name == "") clothingShop.MapMarker.name = "Clothing Shop";
                    else clothingShop.MapMarker.name = clothingShop.Name;
                    clothingShop.Ped = API.shared.createPed(PedHash.Bevhills02AFM, clothingShop.PedPosition, clothingShop.PedRotation.Z);
                    ClothingShopList.Add(clothingShop);
                    BlipService.BlipService.BlipList.Add(clothingShop.MapMarker);
                }
                API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Clothing Shops Loaded..");
            }
            else
            {
                API.shared.consoleOutput(LogCat.Info, "No Clothing Shops Loaded..");
            }
        }

        public static void AddClothingShop(Vector3 position)
        {
            ClothingShop clothingShop = new ClothingShop
            {
                Position = position
            };

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Position", JsonConvert.SerializeObject(clothingShop.Position)},
                { "@PedPosition", JsonConvert.SerializeObject(clothingShop.PedPosition)},
                { "@PedRotation", JsonConvert.SerializeObject(clothingShop.PedRotation)},
                { "@Tops", JsonConvert.SerializeObject(clothingShop.AvailableTops)},
                { "@Legs", JsonConvert.SerializeObject(clothingShop.AvailableLegs)},
                { "@Feets", JsonConvert.SerializeObject(clothingShop.AvailableFeets)},
                { "@Masks", JsonConvert.SerializeObject(clothingShop.AvailableMasks)},
                { "@Torso", JsonConvert.SerializeObject(clothingShop.AvailableTorso)},
                { "@Decals ", JsonConvert.SerializeObject(clothingShop.AvailableDecals )},
                { "@Backpacks", JsonConvert.SerializeObject(clothingShop.AvailableBackpacks)},
                { "@Hair", JsonConvert.SerializeObject(clothingShop.AvailableHair)},
                { "@Accessories", JsonConvert.SerializeObject(clothingShop.AvailableAccessories)},
                { "@Hats", JsonConvert.SerializeObject(clothingShop.AvailableHats)},
                { "@Glasses", JsonConvert.SerializeObject(clothingShop.AvailableGlasses)},
                { "@Ears", JsonConvert.SerializeObject(clothingShop.AvailableEars)},
                { "@Watches", JsonConvert.SerializeObject(clothingShop.AvailableWatches)},
                { "@Bracelets", JsonConvert.SerializeObject(clothingShop.AvailableBracelets)}
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO clothingshops (Position, PedPosition, PedRotation, Tops, Legs, Feets, Masks, Torso, Decals, Backpacks, Hair," +
                " Accessories, Hats, Glasses, Ears, Watches, Bracelets) " +
                "VALUES (@Position, @PedPosition, @PedRotation, @Tops, @Legs, @Feets, @Masks, @Torso, @Decals, @Backpacks, @Hair, @Accessories, @Hats, @Glasses, @Ears, @Watches, @Bracelets)", parameters);
            ReloadClothingShops();
        }

        public static void ReloadClothingShops()
        {
            ClothingShopList.ForEach(clothingShop =>
            {
                if (clothingShop.MapMarker != null)
                    API.shared.deleteEntity(clothingShop.MapMarker);
                if (clothingShop.Ped != null)
                    API.shared.deleteEntity(clothingShop.Ped);
            });
            ClothingShopList.Clear();
            LoadAllClothingShopsFromDB();
        }

        public static void ChangePedPosition(int clothingShopId, Vector3 position, double rotation)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", clothingShopId.ToString() },
                { "@PedPosition", JsonConvert.SerializeObject(position) },
                { "@PedRotation", JsonConvert.SerializeObject(new Vector3(0, 0, rotation)) }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE clothingshops SET PedPosition = @PedPosition, " +
                "PedRotation = @PedRotation WHERE Id = @Id LIMIT 1", parameters);
            ReloadClothingShops();
        }

        public static void OpenShopMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            ClothingShop clothingShop = ClothingShopList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2f);
            if (clothingShop == null) { return; }
            if (clothingShop.AvailableAccessories.Count == 0 && clothingShop.AvailableBracelets.Count == 0 && clothingShop.AvailableEars.Count == 0 &&
                clothingShop.AvailableFeets.Count == 0 && clothingShop.AvailableGlasses.Count == 0 && clothingShop.AvailableHats.Count == 0 &&
                clothingShop.AvailableLegs.Count == 0 && clothingShop.AvailableMasks.Count == 0 && clothingShop.AvailableTorso.Count == 0 && clothingShop.AvailableDecals.Count == 0 && clothingShop.AvailableBackpacks.Count == 0 && clothingShop.AvailableHair.Count == 0 && clothingShop.AvailableTops.Count == 0 &&
                clothingShop.AvailableWatches.Count == 0)
            {
                API.shared.sendPictureNotificationToPlayer(client, "Wir verkaufen momentan nix..", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + clothingShop.Id, "Clothing Shop");
                return;
            }
            ClothingShopJson clothingShopJson = new ClothingShopJson(clothingShop, player.Character.Gender);

            clothingShopJson.AvailableTops.ForEach(x => { x.AlreadyBought = false; });
            clothingShopJson.AvailableLegs.ForEach(x => { x.AlreadyBought = false; });
            clothingShopJson.AvailableFeets.ForEach(x => { x.AlreadyBought = false; });
			clothingShopJson.AvailableMasks.ForEach(x => { x.AlreadyBought = false; });
            clothingShopJson.AvailableTorso.ForEach(x => { x.AlreadyBought = false; });
            clothingShopJson.AvailableDecals.ForEach(x => { x.AlreadyBought = false; });
            clothingShopJson.AvailableHair.ForEach(x => { x.AlreadyBought = false; });
            clothingShopJson.AvailableBackpacks.ForEach(x => { x.AlreadyBought = false; });
            clothingShopJson.AvailableAccessories.ForEach(x => { x.AlreadyBought = false; });

            player.Character.BoughtClothing.Tops.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableTops.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });

            player.Character.BoughtClothing.Legs.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableLegs.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });

            player.Character.BoughtClothing.Masks.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableMasks.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });

            player.Character.BoughtClothing.Torso.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableTorso.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });

            player.Character.BoughtClothing.Decals.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableDecals.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });

            player.Character.BoughtClothing.Accessories.ForEach(id => {
				Clothing cloth = clothingShopJson.AvailableAccessories.FirstOrDefault(x => x.Id == id);
				if (cloth != null) { cloth.AlreadyBought = true; }
			});

            player.Character.BoughtClothing.Backpacks.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableBackpacks.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });

            player.Character.BoughtClothing.Hair.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableHair.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });

            player.Character.BoughtClothing.Feets.ForEach(id => {
                Clothing cloth = clothingShopJson.AvailableFeets.FirstOrDefault(x => x.Id == id);
                if (cloth != null) { cloth.AlreadyBought = true; }
            });
            API.shared.triggerClientEvent(client, "ClothingShop_OpenMenu", JsonConvert.SerializeObject(clothingShopJson));
            CharacterService.CharacterService.UpdatePlayerWeapons(player.Character);
        }

        public static void PreviewClothing(Client client, string type, int id)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            ClothingShop clothingShop = ClothingShopList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 3f);
            if (clothingShop == null) { return; }
            Clothing clothing = null;
            switch (type)
            {
                case "top":
                    clothing = ClothingService.TopList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Tops, clothing.Drawable, clothing.Texture);
                    client.setClothes((int)ClothingSlot.Torso, clothing.Torso, 0);
                    client.setClothes((int)ClothingSlot.Undershirt, clothing.Undershirt, 0);
                    break;
                case "leg":
                    clothing = ClothingService.LegList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Legs, clothing.Drawable, clothing.Texture);
                    break;
                case "feet":
                    clothing = ClothingService.FeetList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Feet, clothing.Drawable, clothing.Texture);
                    break;
				case "masks":
					clothing = ClothingService.MasksList.FirstOrDefault(x => x.Id == id);
					if (clothing == null) { return; }
					client.setClothes((int)ClothingSlot.Mask, clothing.Drawable, clothing.Texture);
                    break;
                case "torso":
                    clothing = ClothingService.TorsoList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Torso, clothing.Drawable, clothing.Texture);
                    break;
                case "decals":
                    clothing = ClothingService.DecalsList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Decals, clothing.Drawable, clothing.Texture);
                    break;
                case "hair":
                    clothing = ClothingService.HairList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Hair, clothing.Drawable, clothing.Texture);
                    break;
                case "backpacks":
                    clothing = ClothingService.BackpacksList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Backpacks, clothing.Drawable, clothing.Texture);
                    break;
                case "accessories":
                    clothing = ClothingService.AccessoriesList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    client.setClothes((int)ClothingSlot.Accessories, clothing.Drawable, clothing.Texture);
                    break;
             

            }
        }

        public static void BuyClothing(Client client, string type, int id)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            ClothingShop clothingShop = ClothingShopList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 3f);
            if (clothingShop == null) { return; }
            Clothing clothing = null;



            switch (type)
            {
                case "top":
                    clothing = ClothingService.TopList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Tops.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert.");
                        client.setClothes((int)ClothingSlot.Tops, clothing.Drawable, clothing.Texture);
                        client.setClothes((int)ClothingSlot.Torso, clothing.Torso, 0);
                        client.setClothes((int)ClothingSlot.Undershirt, clothing.Undershirt, 0);
                        player.Character.ClothesTop = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Tops, clothing.Drawable, clothing.Texture);
                    client.setClothes((int)ClothingSlot.Torso, clothing.Torso, 0);
                    client.setClothes((int)ClothingSlot.Undershirt, clothing.Undershirt, 0);
                    player.Character.ClothesTop = id;
                    player.Character.BoughtClothing.Tops.Add(id);
                    break;
                case "leg":
                    clothing = ClothingService.LegList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Legs.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
                        client.setClothes((int)ClothingSlot.Legs, clothing.Drawable, clothing.Texture);
                        player.Character.ClothesLegs = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Legs, clothing.Drawable, clothing.Texture);
                    player.Character.ClothesLegs = id;
                    player.Character.BoughtClothing.Legs.Add(id);
                    break;
                case "feet":
                    clothing = ClothingService.FeetList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Feets.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
                        client.setClothes((int)ClothingSlot.Feet, clothing.Drawable, clothing.Texture);
                        player.Character.ClothesFeets = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Feet, clothing.Drawable, clothing.Texture);
                    player.Character.ClothesFeets = id;
                    player.Character.BoughtClothing.Feets.Add(id);
                    break;
				case "masks":
					clothing = ClothingService.MasksList.FirstOrDefault(x => x.Id == id);
					if (clothing == null) { return; }
					if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
					{
						API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
						return;
					}
					if (player.Character.BoughtClothing.Masks.IndexOf(id) != -1)
					{
						API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
						client.setClothes((int)ClothingSlot.Mask, clothing.Drawable, clothing.Texture);
						player.Character.ClothesMasks = id;
						CharacterService.CharacterService.UpdateCharacter(player.Character);
						return;
					}
					MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
					client.setClothes((int)ClothingSlot.Mask, clothing.Drawable, clothing.Texture);
					player.Character.ClothesMasks = id;
					player.Character.BoughtClothing.Masks.Add(id);
					break;
                case "torso":
                    clothing = ClothingService.TorsoList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Torso.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
                        client.setClothes((int)ClothingSlot.Torso, clothing.Drawable, clothing.Texture);
                        player.Character.ClothesTorso = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Torso, clothing.Drawable, clothing.Texture);
                    player.Character.ClothesTorso = id;
                    player.Character.BoughtClothing.Torso.Add(id);
                    break;
                case "decals":
                    clothing = ClothingService.DecalsList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Decals.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
                        client.setClothes((int)ClothingSlot.Decals, clothing.Drawable, clothing.Texture);
                        player.Character.ClothesDecals = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Decals, clothing.Drawable, clothing.Texture);
                    player.Character.ClothesDecals = id;
                    player.Character.BoughtClothing.Decals.Add(id);
                    break;
                case "accessories":
                    clothing = ClothingService.AccessoriesList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Accessories.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
                        client.setClothes((int)ClothingSlot.Accessories, clothing.Drawable, clothing.Texture);
                        player.Character.ClothesAccessories = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Accessories, clothing.Drawable, clothing.Texture);
                    player.Character.ClothesAccessories = id;
                    player.Character.BoughtClothing.Accessories.Add(id);
                    break;
                case "hair":
                    clothing = ClothingService.HairList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Hair.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
                        client.setClothes((int)ClothingSlot.Hair, clothing.Drawable, clothing.Texture);
                        player.Character.ClothesHair = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Hair, clothing.Drawable, clothing.Texture);
                    player.Character.ClothesHair = id;
                    player.Character.BoughtClothing.Hair.Add(id);
                    break;
                case "backpacks":
                    clothing = ClothingService.BackpacksList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (!MoneyService.MoneyService.HasPlayerEnoughCash(client, clothing.Price))
                    {
                        API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Geld!");
                        return;
                    }
                    if (player.Character.BoughtClothing.Backpacks.IndexOf(id) != -1)
                    {
                        API.shared.sendNotificationToPlayer(client, "Kleidung gespeichert");
                        client.setClothes((int)ClothingSlot.Backpacks, clothing.Drawable, clothing.Texture);
                        player.Character.ClothesBackpacks = id;
                        CharacterService.CharacterService.UpdateCharacter(player.Character);
                        return;
                    }
                    MoneyService.MoneyService.RemovePlayerCash(client, clothing.Price);
                    client.setClothes((int)ClothingSlot.Backpacks, clothing.Drawable, clothing.Texture);
                    player.Character.ClothesBackpacks = id;
                    player.Character.BoughtClothing.Backpacks.Add(id);
                    break;
            }
            API.shared.sendNotificationToPlayer(client, "~g~Kleidung für " + clothing.Price + " gekauft!");
            CharacterService.CharacterService.UpdateCharacter(player.Character);
            API.shared.triggerClientEvent(client, "ClothingShop_CloseMenu");
        }

        public static void ResetPreview(Client client)
        {
            CharacterService.CharacterService.ApplyAppearance(client);
            CharacterService.CharacterService.GivePlayerWeapons(client);
        }

        public static void SaveClothingShop(int clothingShopId)
        {
            ClothingShop clothingShop = ClothingShopList.FirstOrDefault(x => x.Id == clothingShopId);
            if (clothingShop == null) { return; }

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", clothingShopId.ToString() },
                { "@Tops", JsonConvert.SerializeObject(clothingShop.AvailableTops) },
                { "@Legs", JsonConvert.SerializeObject(clothingShop.AvailableLegs) },
                { "@Feets", JsonConvert.SerializeObject(clothingShop.AvailableFeets) },
                { "@Masks", JsonConvert.SerializeObject(clothingShop.AvailableMasks) },
                { "@Torso", JsonConvert.SerializeObject(clothingShop.AvailableTorso) },
                { "@Decals", JsonConvert.SerializeObject(clothingShop.AvailableDecals) },
                { "@Backpacks", JsonConvert.SerializeObject(clothingShop.AvailableBackpacks) },
                { "@Hair", JsonConvert.SerializeObject(clothingShop.AvailableHair) },
                { "@Accessories", JsonConvert.SerializeObject(clothingShop.AvailableAccessories) },
                { "@Hats", JsonConvert.SerializeObject(clothingShop.AvailableHats) },
                { "@Glasses", JsonConvert.SerializeObject(clothingShop.AvailableGlasses) },
                { "@Ears", JsonConvert.SerializeObject(clothingShop.AvailableEars) },
                { "@Watches", JsonConvert.SerializeObject(clothingShop.AvailableWatches) },
                { "@Bracelets", JsonConvert.SerializeObject(clothingShop.AvailableBracelets) },
                { "@PedPosition", JsonConvert.SerializeObject(clothingShop.PedPosition) },
                { "@PedRotation", JsonConvert.SerializeObject(clothingShop.PedRotation) }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE clothingshops SET PedPosition = @PedPosition, " +
                "PedRotation = @PedRotation, Tops = @Tops, Legs = @Legs, Feets = @Feets, Masks = @Masks, Torso = @Torso, Decals = @Decals, Backpacks = @Backpacks, Hair = @Hair, Accessories = @Accessories," +
                " Hats = @Hats, Glasses = @Glasses, Ears = @Ears, Watches = @Watches, Bracelets = @Bracelets WHERE Id = @Id LIMIT 1", parameters);
        }
    }
}
