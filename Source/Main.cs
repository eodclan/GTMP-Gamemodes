using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server;
using FactionLife.Server.Model;
using FactionLife.Server.Services.AdminService;
using FactionLife.Server.Services.CharacterService;
using FactionLife.Server.Services.ClothingService;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.ShopService;
using FactionLife.Server.Services.LicenseService;
using FactionLife.Server.Services.VehicleService;
using FactionLife.Server.Services.DealerService;
using FactionLife.Server.Services.SellerService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FactionLife.Server.Services.PayNLoudService;
using FactionLife.Server.Services.BurgerShotService;
using FactionLife.Server.Services.AccountService;

namespace FactionLife
{
	public class Main 
		: Script
	{
        public Main()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

        public void BanPlayerByIDExe(Account account)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@SocialClubName", account.SocialClubName },
                { "@Locked", 1.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE accounts SET Locked = @Locked WHERE SocialClubName = @SocialClubName LIMIT 1", parameters);

        }

        [Command("acloth")]
		public void OpenCloth(Client sender)
		{
            if (!sender.hasData("player"))
                return;
            Player player = sender.getData("player");
            if (player.Account.AdminLvl >= 8)
            {
                API.triggerClientEvent(sender, "open_ClothingCreator");
                API.sendPictureNotificationToPlayer(sender, "Du hast den Clothing Creator gestartet!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
            }
            else
            {
                API.sendNotificationToPlayer(sender, "Du hast nicht die erforderlichen Rechte");
            }
		}

		public void OnClientEvent(Client player, string eventName, params object[] arguments)
		{
			switch (eventName)
			{
				case "save_clothing":
					Clothing cloth = new Clothing();
					cloth.Slot = 11;
					cloth.Drawable = (int)arguments[0];
					cloth.Texture = (int)arguments[1];
					cloth.Torso = (int)arguments[2];
					cloth.Undershirt = (int)arguments[3];
					cloth.Gender = ((Player)player.getData("player")).Character.Gender;


					Dictionary<string, string> parameters = new Dictionary<string, string>
					{
						{ "@Slot", cloth.Slot.ToString() },
						{ "@Drawable", cloth.Drawable.ToString() },
						{ "@Texture", cloth.Texture.ToString() },
						{ "@Torso", cloth.Torso.ToString() },
						{ "@Undershirt", cloth.Undershirt.ToString() },
						{ "@Gender", ((int)cloth.Gender).ToString() }
					};
					DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO clothes_tops (Slot, Drawable, Texture, Torso, Undershirt, Gender) " +
						"VALUES (@Slot, @Drawable, @Texture, @Torso, @Undershirt, @Gender)", parameters);
					break;
			}
		}

        [Command("restart")]
        public void RestartServer(Client client)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Account.AdminLvl >= 8)
            {
                OnChatMessageHandler(client, "Die Sonnenwende beginnt nun...", "Sonnenwende!");
                API.stopResource("FL-RP-V0.0.X");
                API.startResource("FL-RP-V0.0.X");
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }
        }

        [Command("invisible")]
        public void Invisible_Command(Client client)
        {
            Player player = client.getData("player");
            if (player.Account.AdminLvl != 8) { return; }

            API.setEntityTransparency(client.handle, 0);
            API.sendPictureNotificationToPlayer(client, "Sie sind ~r~unsichtbar", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
        }

        [Command("visible")]
        public void Visible_Command(Client client)
        {
            Player player = client.getData("player");
            if (player.Account.AdminLvl != 8) { return; }

            API.setEntityTransparency(client.handle, 255);
            API.sendPictureNotificationToPlayer(client, "Sie sind ~g~sichtbar", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
        }

        [Command("dimension")]
        public void SetEntityDimension(Client player, Client target, int Dimension)
        {
            Player admin = player.getData("player");
            if (admin.Account.AdminLvl != 8) { return; }

            API.setEntityDimension(target, Dimension);

            API.sendPictureNotificationToPlayer(player, "Du hast die Dimension des Spielers geändert.", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
        }

        [Command("id")]
        public void GetPlayerByID(Client player, int id)
        {
            Player admin = player.getData("player");
            if (admin.Account.AdminLvl == 0) { return; }

            List<Client> playerList = API.getAllPlayers();
            foreach (Client playerItem in playerList)
            {
                Player target = playerItem.getData("player");

                if (target.Character.Id == id) {
                    API.sendPictureNotificationToPlayer(player, "Die ID gehört zu ~y~" + target.Character.FirstName + "_" + target.Character.LastName +"", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
                    API.sendPictureNotificationToPlayer(player, "Der Server-Name lautet ~y~" + API.getPlayerName(playerItem) + "", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
                }
            }
        }

        [Command("ban")]
        public void BanPlayerByID(Client player, int id)
        {
            Player admin = player.getData("player");
            if (admin.Account.AdminLvl != 7) { return; }

            List<Client> playerList = API.getPlayersInRadiusOfPlayer(10, player);
            foreach (Client playerItem in playerList)
            {
                Player target = playerItem.getData("player");

                if (target.Character.Id == id)
                {
                    Account account = AccountService.LoadAccount(playerItem.socialClubName);

                    BanPlayerByIDExe(account);

                    API.delay(5000, true, () =>
                    {
                        API.kickPlayer(playerItem, "Dein Account wurde gesperrt! Melde dich im ~r~Support!");
                    });
                }
            }
        }

        [Command("unban", GreedyArg = true)]
        public void UnBanPlayerBySCN(Client player, string SocialClubName)
        {
            Player admin = player.getData("player");
            if (admin.Account.AdminLvl != 8) { return; }

            Account account = AccountService.LoadAccount(SocialClubName);

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@SocialClubName", account.SocialClubName },
                { "@Locked", 0.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE accounts SET Locked = @Locked WHERE SocialClubName = @SocialClubName LIMIT 1", parameters);

            API.sendPictureNotificationToPlayer(player, "Spieler wurde entbannt!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
        }



        [Command("banSC", GreedyArg = true)]
        public void BanPlayerBySCN(Client player, string SocialClubName)
        {
            Player admin = player.getData("player");
            if (admin.Account.AdminLvl != 8) { return; }

            Account account = AccountService.LoadAccount(SocialClubName);

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@SocialClubName", account.SocialClubName },
                { "@Locked", 1.ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE accounts SET Locked = @Locked WHERE SocialClubName = @SocialClubName LIMIT 1", parameters);

            API.sendPictureNotificationToPlayer(player, "Spieler wurde gebannt!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
        }

        [Command("addwhite", Alias = "aw", Group = "Admin Commands", GreedyArg = true)]
        public void WhitelistAddClient(Client client, string socialclubname)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Account.AdminLvl >= 2)
            {
                Server.Services.AccountService.WhiteListService.AddClientToWhitelist(socialclubname);
                API.sendPictureNotificationToPlayer(client, "Spieler mit " + socialclubname + " der Whitelist ~g~hinzugefügt.", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }
        }


        [Command("removewhite", Alias = "rw", Group = "Admin Commands", GreedyArg = true)]
        public void WhitelistRemClient(Client client, string socialclubname)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Account.AdminLvl >= 2)
            {
                Server.Services.AccountService.WhiteListService.RemoveClientFromWhitelist(socialclubname);
                API.sendPictureNotificationToPlayer(client, "Spieler mit " + socialclubname + " der Whitelist ~g~entfernt.", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }			
        }		

        [Command("dropfps", "", Alias = "fps", Group = "FPS Drops")]
        public void DropFPS(Client sender, int drop)
        {
            API.delay(3000, true, () =>
            {
                for (int i = 0; i < drop; ++i)
                    API.triggerClientEvent(sender, "dummy");
            });
            API.sendNotificationToPlayer(sender, "Du hast erfolgreich den FPS Drop Fix ausgeführt!");
        }

        [Command("addoutfit", GreedyArg = true)]
		public void AddOutfit(Client client, Gender gender, string name)
		{
			Outfit outfit = new Outfit
			{
				Name = name,
				Gender = gender
			};

			Dictionary<string, string> parameters = new Dictionary<string, string>
					{
						{ "@Outfit", JsonConvert.SerializeObject(outfit) }
					};
			DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO clothes_outfits (Outfit) " +
				"VALUES (@Outfit)", parameters);
		}

		[Command("addatm")]
		public void AddATMCmd(Client player)
		{
			ATMService.AddATM(player.position);
            API.sendPictureNotificationToPlayer(player, "ATM ~w~added..", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
		}

        [Command("geldgeben")]
        public static void GeldGeben(Client client, Client targetclient, double amount)
        {
            if (MoneyService.HasPlayerEnoughCash(client, amount))
            {
                if (targetclient == null)
                {
                    return;
                }
                if (!client.hasData("player") || !targetclient.hasData("player"))
                {
                    return;
                }
                Player player = client.getData("player");
                Player target = targetclient.getData("player");
                player.Character.Cash -= amount;
                target.Character.Cash += amount;
                player.Account.Player.setSyncedData("cash", player.Character.Cash);
                CharacterService.UpdateCharacter(player.Character);
                target.Account.Player.setSyncedData("cash", target.Character.Cash);
                CharacterService.UpdateCharacter(target.Character);
                API.shared.sendNotificationToPlayer(client, "<C>Maze Bank:</C> ~r~Du hast gerade ein Geld Betrag weitergegeben!");
            }
            else
            {
                API.shared.sendNotificationToPlayer(client, "<C>Maze Bank:</C> ~r~Sie haben nicht genug Geld in Ihrem Geldbeutel.");
            }
        }

        [Command("anim")]
		public void Animation(Client client, string dict, string anim)
		{
			API.playPlayerAnimation(client, (int)(AnimationFlags.StopOnLastFrame), dict, anim);
		}

        [Command("coords")]
        public void getCoords(Client client)
        {
            API.sendPictureNotificationToPlayer(client, "X: " + client.position.X.ToString() + " Y: " + client.position.Y.ToString() + " Z: " + client.position.Z.ToString() + "", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
        }
    
        [Command("msgtoall", Alias ="mta")]
        public void OnChatMessageHandler(Client player, string msg, string headline)
        {
        
            if (msg.Contains("_"))
            {
                msg = msg.Replace("_", " ");
            }
            if (headline.Contains("_"))
            {
                headline = headline.Replace("_", " ");
            }
           
            API.sendPictureNotificationToAll(msg, "CHAR_SOCIAL_CLUB", 0, 3, headline, "Eilmeldung");
        }

        [Command("testinventory")]
		public void TestInventory(Client client)
		{
			Player player = client.getData("player");
			API.sendChatMessageToPlayer(client, "Inventar:");
			if (player.Character.Inventory.Count != 0)
			{
				player.Character.Inventory.ForEach(delegate (InventoryItem item)
				{
					Item dbitem = ItemService.ItemList.Find(x => x.Id == item.ItemID);
					API.sendChatMessageToPlayer(client, "- " + dbitem.Name + "(" + item.ItemID + ")");
				});
			}
		}

        // Give Money
        [Command("gvmoney")]
        public void givemoney(Client client, double cashAmount)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Account.AdminLvl >= 5)
            {
                player.Character.Cash += cashAmount;
                CharacterService.UpdateCharacter(player.Character);
                player.Account.Player.setSyncedData("cash", player.Character.Cash);
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }
        }

        [Command("port")]
        public void teleport(Client client)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Account.AdminLvl >= 2)
            {
                API.triggerClientEvent(client, "mapmarker_getposition");
                API.sendPictureNotificationToPlayer(client, "Du hast dich geported!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }
        }

        [Command("kick")]
        public void kickChar(Client client, Client target, string reason)
        {
            if (!client.hasData("player"))
                return;
            if (!target.hasData("player"))
                return;

            Player player = client.getData("player");
           
            if (player.Account.AdminLvl >= 5)
            {
                AdminService.KickPlayer(client, target, reason);
                API.sendPictureNotificationToPlayer(client, "Du hast den Spieler gekickt!", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }
        }

        [Command("reboot")]
        public void rebootMSG(Client client, int minutes)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Account.AdminLvl >= 2)
            {
                OnChatMessageHandler(client, "Wir bitten alle Bewohner sich innerhalb von " + minutes + " Minuten in Sicherheit zu begeben!", "Sonnenwende!");
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }
        }

        [Command("weapon", Alias = "wp")]
        public void giveWeapon(Client client, String weapon)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Account.AdminLvl >= 6)
            {
                client.giveWeapon(API.weaponNameToModel(weapon), 120, true);
            }
            else
            {
                API.sendNotificationToPlayer(client, "Du hast nicht die erforderlichen Rechte");
            }         
        }

        [Command("livery")]
		public void ChangeLivery(Client client, int id)
		{
			if (client.isInVehicle)
			{
				client.vehicle.livery = id;
			}
		}

		[Command("additem")]
		public void AddItem(Client client, int id)
		{
			Player player = client.getData("player");
			player.Character.Inventory.Add(new InventoryItem {
                ItemID = id, Count = 1 }
            );
            API.sendPictureNotificationToPlayer(client, "Added Item ID " + id + " to Player Inventory", "CHAR_BUGSTARS", 0, 3, "RP-Deluxe", "Admin Service");
		}

		[Command("tyresmoke")]
		public void TestTyreSmoke(Client client, int r, int g, int b)
		{
			if (!client.isInVehicle) { return; }
			client.vehicle.setMod(20, 1);
			client.vehicle.tyreSmokeColor = new Color(r, g, b);
		}

		[Command("addshop")]
		public void AddShop(Client client)
		{
			ShopService.AddShop(client.position);
			API.sendChatMessageToPlayer(client, "Shop Created");
		}

        [Command("addshopped")]
		public void AddShopPed(Client client, int shopId)
		{
			Shop shop = ShopService.LoadShopFromDB(shopId);
			shop.PedPosition = client.position;
			shop.PedRotation = client.rotation;
			ShopService.SaveShop(shop);
			shop.Ped = API.createPed(PedHash.AmmuCountrySMM, shop.PedPosition, shop.PedRotation.Z);
		}

        [Command("adddealer")]
        public void AddDealer(Client client)
        {
            DealerService.AddDealer(client.position);
            API.sendChatMessageToPlayer(client, "Dealer Created");
        }

        [Command("adddealerped")]
        public void AddDealerPed(Client client, int dealerId)
        {
            Dealer dealer = DealerService.LoadDealerFromDB(dealerId);
            dealer.PedPosition = client.position;
            dealer.PedRotation = client.rotation;
            DealerService.SaveDealer(dealer);
            dealer.Ped = API.createPed(PedHash.AmmuCountrySMM, dealer.PedPosition, dealer.PedRotation.Z);
        }

        [Command("addseller")]
        public void AddSeller(Client client)
        {
            SellerService.AddSeller(client.position);
            API.sendChatMessageToPlayer(client, "Seller Created");
        }

        [Command("addsellerped")]
        public void AddSellerPed(Client client, int sellerId)
        {
            Seller seller = SellerService.LoadSellerFromDB(sellerId);
            seller.PedPosition = client.position;
            seller.PedRotation = client.rotation;
            SellerService.SaveSeller(seller);
            seller.Ped = API.createPed(PedHash.AmmuCountrySMM, seller.PedPosition, seller.PedRotation.Z);
        }

        [Command("addpaynloud")]
        public void AddPayNLoud(Client client)
        {
            PayNLoudService.AddPayNLoud(client.position);
            API.sendChatMessageToPlayer(client, "AddPayNLoud Created");
        }

        [Command("addpaynloudped")]
        public void AddPayNLoudPed(Client client, int PayNLoudId)
        {
            PayNLoud PayNLoud = PayNLoudService.LoadPayNLoudFromDB(PayNLoudId);
            PayNLoud.PedPosition = client.position;
            PayNLoud.PedRotation = client.rotation;
            PayNLoudService.SavePayNLoud(PayNLoud);
            PayNLoud.Ped = API.createPed(PedHash.ArmGoon02GMY, PayNLoud.PedPosition, PayNLoud.PedRotation.Z);
        }

        [Command("addburgershot")]
        public void AddBurgerShot(Client client)
        {
            BurgerShotService.AddBurgerShot(client.position);
            API.sendChatMessageToPlayer(client, "BurgerShot Created");
        }

        [Command("addburgershotped")]
        public void AddBurgerShotPed(Client client, int BurgerShotId)
        {
            BurgerShot BurgerShot = BurgerShotService.LoadBurgerShotFromDB(BurgerShotId);
            BurgerShot.PedPosition = client.position;
            BurgerShot.PedRotation = client.rotation;
            BurgerShotService.SaveBurgerShot(BurgerShot);
            BurgerShot.Ped = API.createPed(PedHash.AmmuCountrySMM, BurgerShot.PedPosition, BurgerShot.PedRotation.Z);
        }

        [Command("addshopitem")]
		public void AddShopItem(Client client, int shopId, int itemId, double price, int count)
		{
			Shop shop = ShopService.ShopList.Find(x => x.Id == shopId);
			shop.Storage.Add(new ShopItem {
				Id = itemId,
				BuyPrice = price,
				SellPrice = price / 2,
				Count = count
			});
			API.sendChatMessageToPlayer(client, "Added");
		}

        [Command("stats")]
		public void StatsCmd(Client client)
		{
			Player player = client.getData("player");
			API.sendChatMessageToPlayer(client, "~y~PLAYER~w~| Hunger: ~o~" + player.Character.Hunger + " ~w~| Durst: ~b~" + player.Character.Thirst);
			API.sendChatMessageToPlayer(client, "~y~SYNCED~w~| Hunger: ~o~" + client.getSyncedData("hunger") + " ~w~| Durst: ~b~" + client.getSyncedData("thirst"));
		}

		[Command("avehicle", Alias ="aveh")]
		public void AdminVehicle(Client client, string vehicle)
		{
			AdminService.SpawnAdminVehicle(client, vehicle);
        }

		[Command("addgarage")]
		public void AddGarageCmd(Client client)
		{
			GarageService.AddGarage(client);
			API.sendChatMessageToPlayer(client, "Garage added");
		}

		[Command("addgaragespawn")]
		public void AddGarageSpawnCmd(Client client, int garageId)
		{
			GarageService.AddSpawnpoint(garageId, client);
			API.sendChatMessageToPlayer(client, "Garage spawn added");
		}
		
		[Command("reloadgarages")]
		public void ReloadGarages(Client client)
		{
			GarageService.ReloadGarages();
			API.sendChatMessageToPlayer(client, "Garages reloading");
		}

		[Command("addgasstation")]
		public void AddGasStationCmd(Client client)
		{
			GasStationService.AddGasStation(client);
			API.sendChatMessageToPlayer(client, "Gas Station added");
			GasStationService.ReloadGasStations();
		}

		[Command("addgasstationpump")]
		public void AddGasStationPumpCmd(Client client, int gasstationid)
		{
			GasStationService.AddGasPump(gasstationid, client);
			API.sendChatMessageToPlayer(client, "Gas Station Pump added");
		}

		[Command("reloadgasstations")]
		public void ReloadGasStations(Client client)
		{
			GasStationService.ReloadGasStations();
			API.sendChatMessageToPlayer(client, "Gas Stations reloading");
		}

		[Command("forceweather")]
		public void AdminForceWeather(Client client, int weatherId)
		{
			AdminService.ForceWeather(client, weatherId);
		}
		
		[Command("addvehicleshop")]
		public void AddVehicleShop(Client client, string shopName)
		{
			VehicleShopService.AddVehicleShop(client, shopName);
			API.sendChatMessageToPlayer(client, "Vehicle Shop added");
		}

		[Command("changevehicleshoppreview")]
		public void ChangeVehShopPreview(Client client, int shopId)
		{
			VehicleShopService.ChangePreviewPosition(shopId, client);
			API.sendChatMessageToPlayer(client, "Vehicle Shop Preview Changed");
		}

		[Command("changevehicleshopcamera")]
		public void ChangeVehShopCamera(Client client, int shopId)
		{
			VehicleShopService.ChangePreviewCamera(shopId, client);
			API.sendChatMessageToPlayer(client, "Vehicle Shop Camera Changed");
		}

		[Command("addvehicleshopitem")]
		public void AddVehicleShopItem(Client client, int shopId, string modelName, double price)
		{
			VehicleShopService.AddShopItem(shopId, modelName, price);
			API.sendChatMessageToPlayer(client, modelName + " added to ShopID " + shopId + " for " + price + "$");
		}

		[Command("addclothingshop")]
		public void AddClothingShop(Client client)
		{
			ClothingShopService.AddClothingShop(client.position);
		}

		[Command("addclothingshopped")]
		public void AddClothingShopPed(Client client, int clothingShopId)
		{
			ClothingShopService.ChangePedPosition(clothingShopId, client.position, client.rotation.Z);
		}

        [Command("addclothingshopitem")]
        public void AddClothingShopItem(Client client, int clothingShopId, string type, int id)
        {
            ClothingShop clothingShop = ClothingShopService.ClothingShopList.FirstOrDefault(x => x.Id == clothingShopId);
            Clothing clothing;
            if (clothingShop == null) { return; }
            switch (type)
            {
                case "top":
                    clothing = ClothingService.TopList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableTops.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableTops.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Top ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "leg":
                    clothing = ClothingService.LegList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableLegs.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableLegs.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Leg ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "feet":
                    clothing = ClothingService.FeetList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableFeets.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableFeets.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Feet ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "masks":
                    clothing = ClothingService.MasksList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableMasks.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableMasks.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Mask ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "hair":
                    clothing = ClothingService.HairList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableHair.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableHair.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Hair ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "backpacks":
                    clothing = ClothingService.BackpacksList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableBackpacks.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableBackpacks.Add(clothing);
                    API.sendChatMessageToPlayer(client, "backpacks ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "accessories":
                    clothing = ClothingService.AccessoriesList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableAccessories.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableAccessories.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Accessories ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "decals":
                    clothing = ClothingService.DecalsList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableDecals.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableDecals.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Decals  ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "undershirt ":
                    clothing = ClothingService.UndershirtList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableUndershirt.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableUndershirt.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Undershirt   ID: " + id + " added to Shop " + clothingShopId);
                    break;
                case "torso ":
                    clothing = ClothingService.TorsoList.FirstOrDefault(x => x.Id == id);
                    if (clothing == null) { return; }
                    if (clothingShop.AvailableTorso.FirstOrDefault(x => x.Id == id) != null) { return; }
                    clothingShop.AvailableTorso.Add(clothing);
                    API.sendChatMessageToPlayer(client, "Torso   ID: " + id + " added to Shop " + clothingShopId);
                    break;
            }
            ClothingShopService.SaveClothingShop(clothingShopId);
        }
    }
}
