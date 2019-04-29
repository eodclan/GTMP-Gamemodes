using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace FactionLife.Server.Services.VehicleService
{
    class VehicleService
    {
        public static readonly List<OwnedVehicle> OwnedVehicleList = new List<OwnedVehicle>();

        #region Vehicle Informations
        public static readonly List<VehicleInfo> VehicleData = new List<VehicleInfo>();

        public static void LoadVehicleInformationsFromDB()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM vehicleinfo", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    VehicleInfo vehicleInfo = new VehicleInfo
                    {
                        Id = (int)row["Id"],
                        Model = (string)row["Model"],
                        DisplayName = (string)row["DisplayName"],
                        MaxFuel = (int)row["MaxFuel"],
                        Fuel = (FuelType)row["Fuel"],
                        MaxStorage = (int)row["MaxStorage"],
                        Type = (VehicleType)row["Type"]
                    };
                    VehicleData.Add(vehicleInfo);
                }
                API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " Vehicle Informations Loaded..");
            }
            else
            {
                API.shared.consoleOutput(LogCat.Info, "No Vehicle Informations Loaded..");
            }
        }

        public static VehicleInfo GetVehicleInfo(string modelName)
        {
            return VehicleData.FirstOrDefault(info => info.Model.ToLower() == modelName.ToLower());
        }
        #endregion Vehicle Informations

        public static void CreateNewVehicle(OwnedVehicle ownedVehicle)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Owner", ownedVehicle.Owner },
                { "@OwnerCharId", ownedVehicle.OwnerCharId.ToString() },
                { "@Model", ownedVehicle.ModelName },
                { "@EngineHealth", ownedVehicle.EngineHealth.ToString() },
                { "@Fuel", ownedVehicle.Fuel.ToString() },
                { "@Faction", ((int)ownedVehicle.Faction).ToString() },
                { "@NumberPlate", ownedVehicle.NumberPlate },
                { "@PrimaryColor", ownedVehicle.PrimaryColor.ToString() },
                { "@SecondaryColor", ownedVehicle.SecondaryColor.ToString() },
                { "@Livery", ownedVehicle.Livery.ToString() },
                { "@Inventory", JsonConvert.SerializeObject(ownedVehicle.Inventory) }
            };
            DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO ownedvehicles (Owner, OwnerCharId, Model, EngineHealth, Fuel, Faction, NumberPlate, PrimaryColor, SecondaryColor," +
                " Inventory, Livery) " +
                "VALUES (@Owner, @OwnerCharId, @Model, @EngineHealth, @Fuel, @Faction, @NumberPlate, @PrimaryColor, @SecondaryColor, @Inventory, @Livery)", parameters);
        }

        public static void UpdateVehicle(OwnedVehicle ownedVehicle)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() },
                { "@Owner", ownedVehicle.Owner },
                { "@Model", ownedVehicle.Model.ToString() },
                { "@EngineHealth", ownedVehicle.ActiveHandle.engineHealth.ToString() },
                { "@Fuel", ownedVehicle.Fuel.ToString() },
                { "@Faction", ownedVehicle.Faction.ToString() },
                { "@NumberPlate", ownedVehicle.NumberPlate },
                { "@PrimaryColor", ownedVehicle.PrimaryColor.ToString() },
                { "@SecondaryColor", ownedVehicle.SecondaryColor.ToString() },
                { "@Livery", ownedVehicle.Livery.ToString() },
                { "@Inventory", JsonConvert.SerializeObject(ownedVehicle.Inventory) }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET Owner = @Owner, EngineHealth = @EngineHealth, Fuel = @Fuel, NumberPlate = @NumberPlate," +
                " PrimaryColor = @PrimaryColor, SecondaryColor = @SecondaryColor, Inventory = @Inventory, Livery = @Livery WHERE Id = @Id LIMIT 1", parameters);
        }

        public static OwnedVehicle LoadFromDatabase(int id)
        {
            OwnedVehicle ownedVehicle = null;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"@Id", id.ToString()}
            };
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM ownedvehicles WHERE Id = @Id LIMIT 1", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    ownedVehicle = new OwnedVehicle
                    {
                        Id = (int)row["Id"],
                        Owner = (string)row["Owner"],
                        OwnerCharId = (int)row["OwnerCharId"],
                        Model = API.shared.getHashKey((string)row["Model"]),
                        ModelName = (string)row["Model"],
                        EngineHealth = (int)row["EngineHealth"],
                        Fuel = (int)row["Fuel"],
                        Faction = (FactionType)((int)row["Faction"]),
                        NumberPlate = (string)row["NumberPlate"],
                        PrimaryColor = (int)row["PrimaryColor"],
                        SecondaryColor = (int)row["SecondaryColor"],
                        TyreSmokeState = (int)row["TyreSmokeState"],
                        tyreSmokeColorR = (int)row["tyreSmokeColorR"],
                        tyreSmokeColorG = (int)row["tyreSmokeColorG"],
                        tyreSmokeColorB = (int)row["tyreSmokeColorB"],
                        NeonCarState = (int)row["NeonCarState"],
                        neonCarColorR = (int)row["neonCarColorR"],
                        neonCarColorG = (int)row["neonCarColorG"],
                        neonCarColorB = (int)row["neonCarColorB"],

                        wheelColor = (int)row["wheelColor"],

                        Livery = (int)row["Livery"],
                        InUse = Convert.ToBoolean((int)row["InUse"]),
                        Inventory = JsonConvert.DeserializeObject<List<InventoryItem>>((string)row["Inventory"])
                    };
                }
            }
            return ownedVehicle;
        }

        public static void SetInUse(OwnedVehicle ownedVehicle, bool inUse)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", ownedVehicle.Id.ToString() },
                { "@InUse", Convert.ToInt32(inUse).ToString() }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET InUse = @InUse WHERE Id = @Id LIMIT 1", parameters);
        }

        public static void ResetAllVehicles()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE ownedvehicles SET InUse = 0", parameters);
        }

        public static List<OwnedVehicle> GetUserVehicles(Client client, bool inUse = false)
        {
            if (!client.hasData("player"))
                return null;
            Player player = client.getData("player");

            string keystring1 = Convert.ToString(player.Character.Key1);
            string keystring2 = Convert.ToString(player.Character.Key2);
            string keystring3 = Convert.ToString(player.Character.Key3);
            string keystring4 = Convert.ToString(player.Character.Key4);
            string keystring5 = Convert.ToString(player.Character.Key5);


            List<OwnedVehicle> vehicles = new List<OwnedVehicle>();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"@Owner", client.socialClubName},
                {"@OwnerCharId", player.Character.Id.ToString()},
                {"@InUse", Convert.ToInt32(inUse).ToString()}
            };

            if (keystring1 != "0")
            {
                parameters.Add("@KeyID1", keystring1);
            }
            if (keystring2 != "0")
            {
                parameters.Add("@KeyID2", keystring2);
            }
            if (keystring3 != "0")
            {
                parameters.Add("@KeyID3", keystring3);
            }
            if (keystring4 != "0")
            {
                parameters.Add("@KeyID4", keystring4);
            }
            if (keystring5 != "0")
            {
                parameters.Add("@KeyID5", keystring5);
            }

            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM ownedvehicles WHERE Owner = @Owner AND OwnerCharId = @OwnerCharId AND InUse = @InUse", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    vehicles.Add(new OwnedVehicle
                    {
                        Id = (int)row["Id"],
                        Owner = (string)row["Owner"],
                        OwnerCharId = (int)row["OwnerCharId"],
                        Model = API.shared.getHashKey((string)row["Model"]),
                        ModelName = (string)row["Model"],
                        EngineHealth = (int)row["EngineHealth"],
                        Fuel = (int)row["Fuel"],
                        Faction = (FactionType)((int)row["Faction"]),
                        NumberPlate = (string)row["NumberPlate"],
                        PrimaryColor = (int)row["PrimaryColor"],
                        SecondaryColor = (int)row["SecondaryColor"],
                        Livery = (int)row["Livery"],
                        InUse = Convert.ToBoolean((int)row["InUse"])
                    });
                }
            }

            if (player.Character.Key1 != 0)
            {
                DataTable result2 = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM ownedvehicles WHERE ID = @KeyID1 AND InUse = @InUse", parameters);
                if (result2.Rows.Count != 0)
                {
                    foreach (DataRow row in result2.Rows)
                    {
                        vehicles.Add(new OwnedVehicle
                        {
                            Id = (int)row["Id"],
                            Owner = (string)row["Owner"],
                            OwnerCharId = (int)row["OwnerCharId"],
                            Model = API.shared.getHashKey((string)row["Model"]),
                            ModelName = (string)row["Model"],
                            EngineHealth = (int)row["EngineHealth"],
                            Fuel = (int)row["Fuel"],
                            Faction = (FactionType)((int)row["Faction"]),
                            NumberPlate = (string)row["NumberPlate"],
                            PrimaryColor = (int)row["PrimaryColor"],
                            SecondaryColor = (int)row["SecondaryColor"],
                            Livery = (int)row["Livery"],
                            InUse = Convert.ToBoolean((int)row["InUse"])
                        });
                    }
                }
            }

            if (player.Character.Key2 != 0)
            {
                DataTable result3 = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM ownedvehicles WHERE ID = @KeyID2 AND InUse = @InUse", parameters);
                if (result3.Rows.Count != 0)
                {
                    foreach (DataRow row in result3.Rows)
                    {
                        vehicles.Add(new OwnedVehicle
                        {
                            Id = (int)row["Id"],
                            Owner = (string)row["Owner"],
                            OwnerCharId = (int)row["OwnerCharId"],
                            Model = API.shared.getHashKey((string)row["Model"]),
                            ModelName = (string)row["Model"],
                            EngineHealth = (int)row["EngineHealth"],
                            Fuel = (int)row["Fuel"],
                            Faction = (FactionType)((int)row["Faction"]),
                            NumberPlate = (string)row["NumberPlate"],
                            PrimaryColor = (int)row["PrimaryColor"],
                            SecondaryColor = (int)row["SecondaryColor"],
                            Livery = (int)row["Livery"],
                            InUse = Convert.ToBoolean((int)row["InUse"])
                        });
                    }
                }
            }

            if (player.Character.Key3 != 0)
            {
                DataTable result4 = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM ownedvehicles WHERE ID = @KeyID3 AND InUse = @InUse", parameters);
                if (result4.Rows.Count != 0)
                {
                    foreach (DataRow row in result4.Rows)
                    {
                        vehicles.Add(new OwnedVehicle
                        {
                            Id = (int)row["Id"],
                            Owner = (string)row["Owner"],
                            OwnerCharId = (int)row["OwnerCharId"],
                            Model = API.shared.getHashKey((string)row["Model"]),
                            ModelName = (string)row["Model"],
                            EngineHealth = (int)row["EngineHealth"],
                            Fuel = (int)row["Fuel"],
                            Faction = (FactionType)((int)row["Faction"]),
                            NumberPlate = (string)row["NumberPlate"],
                            PrimaryColor = (int)row["PrimaryColor"],
                            SecondaryColor = (int)row["SecondaryColor"],
                            Livery = (int)row["Livery"],
                            InUse = Convert.ToBoolean((int)row["InUse"])
                        });
                    }
                }
            }

            if (player.Character.Key4 != 0)
            {
                DataTable result5 = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM ownedvehicles WHERE ID = @KeyID4 AND InUse = @InUse", parameters);
                if (result5.Rows.Count != 0)
                {
                    foreach (DataRow row in result5.Rows)
                    {
                        vehicles.Add(new OwnedVehicle
                        {
                            Id = (int)row["Id"],
                            Owner = (string)row["Owner"],
                            OwnerCharId = (int)row["OwnerCharId"],
                            Model = API.shared.getHashKey((string)row["Model"]),
                            ModelName = (string)row["Model"],
                            EngineHealth = (int)row["EngineHealth"],
                            Fuel = (int)row["Fuel"],
                            Faction = (FactionType)((int)row["Faction"]),
                            NumberPlate = (string)row["NumberPlate"],
                            PrimaryColor = (int)row["PrimaryColor"],
                            SecondaryColor = (int)row["SecondaryColor"],
                            Livery = (int)row["Livery"],
                            InUse = Convert.ToBoolean((int)row["InUse"])
                        });
                    }
                }
            }


            if (player.Character.Key5 != 0)
            {
                DataTable result6 = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM ownedvehicles WHERE ID = @KeyID5 AND InUse = @InUse", parameters);
                if (result6.Rows.Count != 0)
                {
                    foreach (DataRow row in result6.Rows)
                    {
                        vehicles.Add(new OwnedVehicle
                        {
                            Id = (int)row["Id"],
                            Owner = (string)row["Owner"],
                            OwnerCharId = (int)row["OwnerCharId"],
                            Model = API.shared.getHashKey((string)row["Model"]),
                            ModelName = (string)row["Model"],
                            EngineHealth = (int)row["EngineHealth"],
                            Fuel = (int)row["Fuel"],
                            Faction = (FactionType)((int)row["Faction"]),
                            NumberPlate = (string)row["NumberPlate"],
                            PrimaryColor = (int)row["PrimaryColor"],
                            SecondaryColor = (int)row["SecondaryColor"],
                            Livery = (int)row["Livery"],
                            InUse = Convert.ToBoolean((int)row["InUse"])
                        });
                    }
                }
            }

            return vehicles;
        }

        #region Vehicle Inventory
        public static void RequestVehicleInventory(Client client, OwnedVehicle ownedVehicle)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            List<InventoryMenuItem> VehicleInventoryMenuItems = new List<InventoryMenuItem>();
            ownedVehicle.Inventory.ForEach(invItem => {
                Item item = ItemService.ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
                if (item != null)
                {
                    VehicleInventoryMenuItems.Add(new InventoryMenuItem
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Count = invItem.Count
                    });
                }
            });
            API.shared.triggerClientEvent(client, "Vehicle_UpdateInventory", JsonConvert.SerializeObject(VehicleInventoryMenuItems));
        }

        public static void RequestPlayerInventory(Client client)
        {
            List<InventoryMenuItem> inventoryMenuList = new List<InventoryMenuItem>();
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            if (player.Character.Inventory.Count != 0)
            {
                player.Character.Inventory.ForEach(inventoryItem =>
                {
                    Item item = ItemService.ItemService.ItemList.FirstOrDefault(x => x.Id == inventoryItem.ItemID);
                    if (item != null)
                    {
                        inventoryMenuList.Add(new InventoryMenuItem
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            Count = inventoryItem.Count
                        });
                    }
                });
            }
            API.shared.triggerClientEvent(client, "Vehicle_UpdatePlayerInventory", JsonConvert.SerializeObject(inventoryMenuList));
        }
        #endregion Vehicle Inventory

        #region Menu Builder
        public static List<MenuItem> BuildVehicleMenu(Player player, OwnedVehicle ownedVehicle)
        {
            List<MenuItem> menuItemList = new List<MenuItem>();

            bool hasAccess = false;
            if (ownedVehicle.Owner == player.Character.Player.socialClubName && ownedVehicle.OwnerCharId == player.Character.Id) { hasAccess = true; }
            if ((ownedVehicle.Id == player.Character.Key1) || (ownedVehicle.Id == player.Character.Key2) || (ownedVehicle.Id == player.Character.Key3) || (ownedVehicle.Id == player.Character.Key4) || (ownedVehicle.Id == player.Character.Key5)) { hasAccess = true; }
            if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return null; }
            if (player.Character.Player.isInVehicle)
            {
                if (player.Character.Player.vehicleSeat == -1)
                {
                    if (ownedVehicle.ActiveHandle.engineStatus)
                    {
                        menuItemList.Add(new MenuItem
                        {
                            Title = "Motor ausschalten",
                            Value1 = "enginestatus"
                        });
                        menuItemList.Add(new MenuItem
                        {
                            Title = "Fahrzeuginformation",
                            Value1 = "vehinfo"
                        });
                    }
                    else
                    {
                        menuItemList.Add(new MenuItem
                        {
                            Title = "Motor einschalten",
                            Value1 = "enginestatus"
                        });
                        menuItemList.Add(new MenuItem
                        {
                            Title = "Fahrzeuginformation",
                            Value1 = "vehinfo"
                        });
                    }
                }
            }
            if (player.Character.Player.isInVehicle)
            {
                if (player.Character.Player.vehicleSeat == -1)
                {
                    if (ownedVehicle.ActiveHandle.engineStatus)
                    {
                        menuItemList.Add(new MenuItem
                        {
                            Title = "Handbremse lösen ",
                            Value1 = "vehhandbreake"
                        });
                    }
                    else
                    {
                        menuItemList.Add(new MenuItem
                        {
                            Title = "Handbremse Ziehen",
                            Value1 = "vehhandbreake"
                        });
                    }
                }
            }
            if (hasAccess)
            {
                if (ownedVehicle.ActiveHandle.locked)
                {
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Fahrzeug aufschließen",
                        Value1 = "lockstatus"
                    });
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Türen öffnen",
                        Value1 = "vehdooropen"
                    });
                }
                else
                {
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Fahrzeug zuschließen",
                        Value1 = "lockstatus"
                    });
                    menuItemList.Add(new MenuItem
                    {
                        Title = "Türen schließen",
                        Value1 = "vehdoorclose"
                    });
                }
            }
            if (!player.Character.Player.isInVehicle)
            {
                menuItemList.Add(new MenuItem
                {
                    Title = "Aus Den Kofferraum nehmen",
                    Value1 = "trunkputout"
                });
                menuItemList.Add(new MenuItem
                {
                    Title = "In Den Kofferraum legen",
                    Value1 = "trunkputin"
                });
            }
            return menuItemList;
        }

        public static void ProcessVehicleMenuReturn(Client client, string menuValue)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            OwnedVehicle ownedVehicle = null;
            if (client.isInVehicle)
            {
                if (!client.vehicle.hasData("vehicle")) { return; }
                ownedVehicle = client.vehicle.getData("vehicle");
            }
            else
            {
                ownedVehicle = OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 4f);
            }
            if (ownedVehicle == null)
                return;
            bool hasAccess = false;
            if (ownedVehicle.Owner == player.Character.Player.socialClubName && ownedVehicle.OwnerCharId == player.Character.Id) { hasAccess = true; }
            if ((ownedVehicle.Id == player.Character.Key1) || (ownedVehicle.Id == player.Character.Key2) || (ownedVehicle.Id == player.Character.Key3) || (ownedVehicle.Id == player.Character.Key4) || (ownedVehicle.Id == player.Character.Key5)) { hasAccess = true; }
            switch (menuValue)
            {
                case "enginestatus":
                    if (!client.isInVehicle) { return; }
                    if (client.vehicleSeat != -1) { return; }
                    if (!hasAccess) { return; }
                    if (client.vehicle.engineStatus)
                    {
                        client.vehicle.engineStatus = false;
                        API.shared.sendNotificationToPlayer(client, "Motor wurde ~r~ausgeschaltet!");

                    }
                    else
                    {
                        if (ownedVehicle.Fuel <= 0)
                        {
                            ownedVehicle.ActiveHandle.engineStatus = false;
                            return;
                        }
                        client.vehicle.engineStatus = true;
                        API.shared.sendNotificationToPlayer(client, "Motor wurde ~g~eingeschaltet!");
                        API.shared.setEntitySyncedData(ownedVehicle.ActiveHandle, "currentfuel", ownedVehicle.Fuel);

                        API.shared.setEntitySyncedData(ownedVehicle.ActiveHandle, "maxfuel", VehicleService.GetVehicleInfo(ownedVehicle.ModelName).MaxFuel);
                    }
                    CloseVehicleMenu(client);
                    break;
                case "lockstatus":
                    if (!hasAccess) { return; }
                    if (ownedVehicle.ActiveHandle.locked)
                    {
                        ownedVehicle.ActiveHandle.locked = false;
                        //API.shared.sendNotificationToPlayer(client, "The vehicle doors of your ~b~" + ownedVehicle.ModelName + " ~w~are now ~g~unlocked");
                        VehicleHandler.PlayUnlockSound(client, ownedVehicle, false);
                    }
                    else
                    {
                        ownedVehicle.ActiveHandle.locked = true;
                        //API.shared.sendNotificationToPlayer(client, "The vehicle doors of your ~b~" + ownedVehicle.ModelName + "~w~ are now ~r~locked");
                        VehicleHandler.PlayUnlockSound(client, ownedVehicle, true);
                    }
                    CloseVehicleMenu(client);
                    break;
                case "trunkputout":
                    if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return; }
                    if (client.isInVehicle) return;
                    RequestVehicleInventory(client, ownedVehicle);
                    API.shared.triggerClientEvent(client, "Vehicle_OpenInventory");
                    ownedVehicle.ActiveHandle.openDoor(5);
                    break;
                case "trunkputin":
                    if (client.isInVehicle) return;
                    if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return; }
                    RequestPlayerInventory(client);
                    API.shared.triggerClientEvent(client, "Vehicle_OpenPlayerInventory");
                    ownedVehicle.ActiveHandle.openDoor(5);
                    break;
                case "vehdoorclose":
                    if (client.isInVehicle) return;
                    if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return; }
                    ownedVehicle.ActiveHandle.closeDoor(0);
                    ownedVehicle.ActiveHandle.closeDoor(1);
                    ownedVehicle.ActiveHandle.closeDoor(2);
                    ownedVehicle.ActiveHandle.closeDoor(3);
                    ownedVehicle.ActiveHandle.closeDoor(4);
                    ownedVehicle.ActiveHandle.closeDoor(5);
                    break;
                case "vehdooropen":
                    if (client.isInVehicle) return;
                    if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return; }
                    ownedVehicle.ActiveHandle.openDoor(0);
                    ownedVehicle.ActiveHandle.openDoor(1);
                    ownedVehicle.ActiveHandle.openDoor(2);
                    ownedVehicle.ActiveHandle.openDoor(3);
                    ownedVehicle.ActiveHandle.openDoor(4);
                    ownedVehicle.ActiveHandle.openDoor(5);
                    break;
                case "vehhandbreake":
                    if (!client.isInVehicle) { return; }
                    if (client.vehicleSeat != -1) { return; }
                    if (!hasAccess) { return; }
                    if (ownedVehicle.ActiveHandle.freezePosition)
                    {
                        ownedVehicle.ActiveHandle.freezePosition = false;
                    }
                    else
                    {
                        ownedVehicle.ActiveHandle.freezePosition = true;
                    }
                    break;
                case "vehinfo":
                    if (!client.isInVehicle) { return; }
                    if (client.vehicleSeat != -1) { return; }
                    if (!hasAccess) { return; }
                    API.shared.sendPictureNotificationToPlayer(client, "Fahrzeug: " + ownedVehicle.ModelName + "\nID: " + ownedVehicle.Id + "\nTank: " + ownedVehicle.Fuel + "L", "CHAR_CARSITE", 0, 8, "Fahrzeug", "Fahrzeuginformation");
                    break;


            }
        }

        public static void CloseVehicleMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "Vehicle_CloseAllMenus");
        }
        #endregion Menu Builder

        public static void PutOutOfVehicleInventory(Client client, int itemId, int count)
        {
            if (client.isInVehicle) return;
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            OwnedVehicle ownedVehicle = null;
            if (client.isInVehicle)
            {
                if (!client.vehicle.hasData("vehicle")) { return; }
                ownedVehicle = client.vehicle.getData("vehicle");
            }
            else
            {
                ownedVehicle = OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 4f);
            }
            if (ownedVehicle == null)
                return;
            bool hasAccess = false;
            if (ownedVehicle.Owner == player.Character.Player.socialClubName && ownedVehicle.OwnerCharId == player.Character.Id) { hasAccess = true; }
            if ((ownedVehicle.Id == player.Character.Key1) || (ownedVehicle.Id == player.Character.Key2) || (ownedVehicle.Id == player.Character.Key3) || (ownedVehicle.Id == player.Character.Key4) || (ownedVehicle.Id == player.Character.Key5)) { hasAccess = true; }
            if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return; }
            Item item = ItemService.ItemService.ItemList.FirstOrDefault(x => x.Id == itemId);
            if (item == null) { return; }
            InventoryItem invitem = ownedVehicle.Inventory.FirstOrDefault(x => x.ItemID == itemId);
            if (invitem == null || invitem.Count <= 0 || count <= 0) { return; }
            if (invitem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genug Artikel dieser Art in Ihrem Fahrzeug!"); return; }
            InventoryItem plrinvitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == itemId);
            if (plrinvitem == null)
            {
                player.Character.Inventory.Add(new InventoryItem
                {
                    ItemID = itemId,
                    Count = count
                });
            }
            else
            {
                plrinvitem.Count += count;
            }
            invitem.Count -= count;
            if (invitem.Count <= 0)
            {
                ownedVehicle.Inventory.Remove(invitem);
            }
            UpdateVehicle(ownedVehicle);
            CharacterService.CharacterService.UpdateCharacter(player.Character);
            API.shared.sendNotificationToPlayer(client, "Du hast ~b~" + count + "~w~x ~g~" + item.Name + "~w~ aus dem Fahrzeugkofferraum genommen.");
            RequestVehicleInventory(client, ownedVehicle);

        }

        public static void PutIntoVehicleInventory(Client client, int itemId, int count)
        {
            if (client.isInVehicle) return;
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            OwnedVehicle ownedVehicle = null;
            if (client.isInVehicle)
            {
                if (!client.vehicle.hasData("vehicle")) { return; }
                ownedVehicle = client.vehicle.getData("vehicle");
            }
            else
            {
                ownedVehicle = OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 4f);
            }
            if (ownedVehicle == null)
                return;
            bool hasAccess = false;
            if (ownedVehicle.Owner == player.Character.Player.socialClubName && ownedVehicle.OwnerCharId == player.Character.Id) { hasAccess = true; }
            if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return; }
            Item item = ItemService.ItemService.ItemList.FirstOrDefault(x => x.Id == itemId);
            if (item == null) { return; }
            InventoryItem plrinvitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == itemId);
            if (plrinvitem == null || plrinvitem.Count <= 0 || count <= 0) { return; }
            if (plrinvitem.Count < count) { API.shared.sendNotificationToPlayer(client, "~r~Sie haben nicht genügend Artikel dieser Art in Ihrem Inventar!"); return; }


            InventoryItem invitem = ownedVehicle.Inventory.FirstOrDefault(x => x.ItemID == itemId);
            if (invitem == null)
            {
                ownedVehicle.Inventory.Add(new InventoryItem
                {
                    ItemID = itemId,
                    Count = count
                });
            }
            else
            {
                invitem.Count += count;
            }
            plrinvitem.Count -= count;
            if (plrinvitem.Count <= 0)
            {
                player.Character.Inventory.Remove(plrinvitem);
            }
            UpdateVehicle(ownedVehicle);
            CharacterService.CharacterService.UpdateCharacter(player.Character);
            API.shared.sendNotificationToPlayer(client, "Du legst ~b~" + count + "~w~x ~g~" + item.Name + "~w~ in den Fahrzeugkofferraum.");
            RequestPlayerInventory(client);
        }

        public static void ChangeDoorState(Client client, int door, bool doorOpen)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            OwnedVehicle ownedVehicle = null;
            if (client.isInVehicle)
            {
                if (!client.vehicle.hasData("vehicle")) { return; }
                ownedVehicle = client.vehicle.getData("vehicle");
            }
            else
            {
                ownedVehicle = OwnedVehicleList.FirstOrDefault(x => x.ActiveHandle.position.DistanceTo(client.position) <= 4f);
            }
            if (ownedVehicle == null)
                return;
            bool hasAccess = false;
            if (ownedVehicle.Owner == player.Character.Player.socialClubName && ownedVehicle.OwnerCharId == player.Character.Id) { hasAccess = true; }
            if ((ownedVehicle.Id == player.Character.Key1) || (ownedVehicle.Id == player.Character.Key2) || (ownedVehicle.Id == player.Character.Key3) || (ownedVehicle.Id == player.Character.Key4) || (ownedVehicle.Id == player.Character.Key5)) { hasAccess = true; }
            if (ownedVehicle.ActiveHandle.locked == true && hasAccess == false) { return; }
            if (doorOpen)
            {
                ownedVehicle.ActiveHandle.openDoor(door);


            }
            else
            {
                ownedVehicle.ActiveHandle.closeDoor(door);
            }
        }
    }
}
