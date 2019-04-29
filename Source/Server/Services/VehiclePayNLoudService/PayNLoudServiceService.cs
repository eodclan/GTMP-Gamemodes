using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.PayNLoudService
{
    public class PayNLoudService
        : Script
    {
        public static readonly List<PayNLoud> PayNLoudList = new List<PayNLoud>();

        public PayNLoudService()
        {
            API.onResourceStop += OnResourceStopHandler;
        }

        public void OnResourceStopHandler()
        {
            PayNLoudList.ForEach(PayNLoud =>
            {
                if (PayNLoud.Blip != null)
                    API.deleteEntity(PayNLoud.Blip);
                if (PayNLoud.Ped != null)
                    API.deleteEntity(PayNLoud.Ped);
                SavePayNLoud(PayNLoud);
            });
            PayNLoudList.Clear();
        }

        public static void LoadPayNLoudFromDB()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM vehicle_paynloud", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    PayNLoud PayNLoud = new PayNLoud
                    {
                        Id = (int)row["Id"],
                        Owner = (string)row["Owner"],
                        Storage = JsonConvert.DeserializeObject<List<PayNLoudItem>>((string)row["Storage"]),
                        MoneyStorage = (double)row["MoneyStorage"],
                        Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
                        PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
                        PedRotation = new Vector3(0, 0, (float)row["PedRot"]),
                        MenuImage = (string)row["MenuImage"],
                        Blip = API.shared.createBlip(new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]))
                    };

                    PayNLoud.Blip.sprite = 545;
                    PayNLoud.Blip.scale = 0.7f;
                    PayNLoud.Blip.name = "Pay N Loud";
                    PayNLoud.Blip.shortRange = true;
                    BlipService.BlipService.BlipList.Add(PayNLoud.Blip);

                    PayNLoud.Ped = API.shared.createPed(PedHash.ArmGoon02GMY, PayNLoud.PedPosition, PayNLoud.PedRotation.Z);

                    PayNLoudList.Add(PayNLoud);
                }
                API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " PayNLouds Loaded..");
            }
            else
            {
                API.shared.consoleOutput(LogCat.Info, "Keine PayNLoud geladen ..");
            }
        }

        public static PayNLoud LoadPayNLoudFromDB(int id)
        {
            PayNLoud PayNLoud = null;
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"@Id", id.ToString()}
            };
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM vehicle_paynloud WHERE Id = @Id LIMIT 1", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    PayNLoud = new PayNLoud
                    {
                        Id = (int)row["Id"],
                        Owner = (string)row["Owner"],
                        Storage = JsonConvert.DeserializeObject<List<PayNLoudItem>>((string)row["Storage"]),
                        MoneyStorage = (double)row["MoneyStorage"],
                        Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
                        PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
                        PedRotation = new Vector3(0, 0, (float)row["PedRot"])
                    };
                }
            }
            return PayNLoud;
        }

        public static void SavePayNLoud(PayNLoud PayNLoud)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", PayNLoud.Id.ToString() },
                { "@Owner", PayNLoud.Owner },
                { "@Storage", JsonConvert.SerializeObject(PayNLoud.Storage)},
                { "@MoneyStorage", PayNLoud.MoneyStorage.ToString() },
                { "@PosX", PayNLoud.Position.X.ToString().Replace(",", ".") },
                { "@PosY", PayNLoud.Position.Y.ToString().Replace(",", ".") },
                { "@PosZ", PayNLoud.Position.Z.ToString().Replace(",", ".") },
                { "@PedPosX", PayNLoud.PedPosition.X.ToString().Replace(",", ".") },
                { "@PedPosY", PayNLoud.PedPosition.Y.ToString().Replace(",", ".") },
                { "@PedPosZ", PayNLoud.PedPosition.Z.ToString().Replace(",", ".") },
                { "@PedRot", PayNLoud.PedRotation.Z.ToString().Replace(",", ".") }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE vehicle_paynloud SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
                "PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
        }

        public static PayNLoud AddPayNLoud(Vector3 position)
        {
            PayNLoud PayNLoud = new PayNLoud();
            PayNLoud.Position = position;
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Storage", JsonConvert.SerializeObject(PayNLoud.Storage)},
                { "@MoneyStorage", PayNLoud.MoneyStorage.ToString() },
                { "@PosX", PayNLoud.Position.X.ToString().Replace(",", ".") },
                { "@PosY", PayNLoud.Position.Y.ToString().Replace(",", ".") },
                { "@PosZ", PayNLoud.Position.Z.ToString().Replace(",", ".") },
                { "@PedPosX", PayNLoud.PedPosition.X.ToString().Replace(",", ".") },
                { "@PedPosY", PayNLoud.PedPosition.Y.ToString().Replace(",", ".") },
                { "@PedPosZ", PayNLoud.PedPosition.Z.ToString().Replace(",", ".") },
                { "@PedRot", PayNLoud.PedRotation.Z.ToString().Replace(",", ".") }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO vehicle_paynloud (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
                "VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

            PayNLoud.Blip = API.shared.createBlip(PayNLoud.Position);
            PayNLoud.Blip.sprite = 545;
            PayNLoud.Blip.scale = 0.7f;
            PayNLoud.Blip.name = "Pay N Loud";
            PayNLoud.Blip.shortRange = true;

            BlipService.BlipService.BlipList.Add(PayNLoud.Blip);
            PayNLoudList.Add(PayNLoud);
            return PayNLoud;
        }

        public static void OpenPayNLoudMenu(Client client, PayNLoud PayNLoud)
        {
            List<PayNLoudMenuItem> PayNLoudItems = new List<PayNLoudMenuItem>();
            PayNLoud.Storage.ForEach(item =>
            {
                PayNLoudItems.Add(new PayNLoudMenuItem
                {
                    Id = item.Id,
                    Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
                    BuyPrice = item.BuyPrice,
                    Count = item.Count
                });
            });

            List<InventoryMenuItem> PayNLoudInventoryItems = new List<InventoryMenuItem>();
            Player player = client.getData("player");
            player.Character.Inventory.ForEach(item =>
            {
                Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
                if (invitem == null)
                    return;
                if (invitem.Sellable)
                {
                    PayNLoudInventoryItems.Add(new InventoryMenuItem
                    {
                        Id = item.ItemID,
                        Name = invitem.Name,
                        Description = invitem.DefaultSellPrice.ToString(),
                        Count = item.Count
                    });
                }
            });

            SavePayNLoud(PayNLoud);

            API.shared.triggerClientEvent(client, "PayNLoud_OpenMenu", JsonConvert.SerializeObject(PayNLoudItems), JsonConvert.SerializeObject(PayNLoudInventoryItems), PayNLoud.MenuImage);
        }

        public static void RefreshPayNLoudBuyMenu(Client client, PayNLoud PayNLoud)
        {
            List<PayNLoudMenuItem> PayNLoudItems = new List<PayNLoudMenuItem>();
            PayNLoud.Storage.ForEach(item =>
            {
                PayNLoudItems.Add(new PayNLoudMenuItem
                {
                    Id = item.Id,
                    Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
                    BuyPrice = item.BuyPrice,
                    Count = item.Count
                });
            });

            List<InventoryMenuItem> PayNLoudInventoryItems = new List<InventoryMenuItem>();
            Player player = client.getData("player");
            player.Character.Inventory.ForEach(item =>
            {
                Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
                if (invitem == null)
                    return;
                if (invitem.Sellable)
                {
                    PayNLoudInventoryItems.Add(new InventoryMenuItem
                    {
                        Id = item.ItemID,
                        Name = invitem.Name,
                        Description = invitem.DefaultSellPrice.ToString(),
                        Count = item.Count
                    });
                }
            });

            API.shared.triggerClientEvent(client, "PayNLoud_RefreshPayNLoudMenu", JsonConvert.SerializeObject(PayNLoudItems), JsonConvert.SerializeObject(PayNLoudInventoryItems));
        }

        public static void ClosePayNLoudMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PayNLoud_CloseMenu");
        }
    }
}
