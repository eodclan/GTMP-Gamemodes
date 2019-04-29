using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FactionLife.Server.Services.TequiLaLaService
{
    public class TequiLaLaService
        : Script
    {
        public static readonly List<TequiLaLa> TequiLaLaList = new List<TequiLaLa>();

        public TequiLaLaService()
        {
            API.onResourceStop += OnResourceStopHandler;
        }

        public void OnResourceStopHandler()
        {
            TequiLaLaList.ForEach(TequiLaLa =>
            {
                if (TequiLaLa.Blip != null)
                    API.deleteEntity(TequiLaLa.Blip);
                if (TequiLaLa.Ped != null)
                    API.deleteEntity(TequiLaLa.Ped);
                SaveTequiLaLa(TequiLaLa);
            });
            TequiLaLaList.Clear();
        }

        public static void LoadTequiLaLaFromDB()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM tequilala", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    TequiLaLa tequilala = new TequiLaLa
                    {
                        Id = (int)row["Id"],
                        Owner = (string)row["Owner"],
                        Storage = JsonConvert.DeserializeObject<List<TequiLaLaItem>>((string)row["Storage"]),
                        MoneyStorage = (double)row["MoneyStorage"],
                        Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
                        PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
                        PedRotation = new Vector3(0, 0, (float)row["PedRot"]),
                        MenuImage = (string)row["MenuImage"],
                        Blip = API.shared.createBlip(new Vector3((float) row["PosX"], (float) row["PosY"], (float) row["PosZ"]))
                    };

                    tequilala.Blip.sprite = 51;
                    tequilala.Blip.name = "Tequi-La-La";
                    tequilala.Blip.color = 75;
                    tequilala.Blip.scale = 0.8f;
                    tequilala.Blip.shortRange = true;
                    BlipService.BlipService.BlipList.Add(tequilala.Blip);

                    tequilala.Ped = API.shared.createPed(PedHash.AmmuCountrySMM, tequilala.PedPosition, tequilala.PedRotation.Z);

                    TequiLaLaList.Add(tequilala);
                }

                API.shared.consoleOutput(LogCat.Info, result.Rows.Count + " TequiLaLas Loaded..");
            }
            else
            {
                API.shared.consoleOutput(LogCat.Info, "Keine TequiLaLa geladen ..");
            }
        }

        public static TequiLaLa LoadTequiLaLaFromDB(int id)
        {
            TequiLaLa tequilala = null;
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"@Id", id.ToString()}
            };
            DataTable result = DatabaseHandler.ExecutePreparedStatement("SELECT * FROM tequilala WHERE Id = @Id LIMIT 1", parameters);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    tequilala = new TequiLaLa
                    {
                        Id = (int)row["Id"],
                        Owner = (string)row["Owner"],
                        Storage = JsonConvert.DeserializeObject<List<TequiLaLaItem>>((string)row["Storage"]),
                        MoneyStorage = (double)row["MoneyStorage"],
                        Position = new Vector3((float)row["PosX"], (float)row["PosY"], (float)row["PosZ"]),
                        PedPosition = new Vector3((float)row["PedPosX"], (float)row["PedPosY"], (float)row["PedPosZ"]),
                        PedRotation = new Vector3(0, 0, (float)row["PedRot"])
                    };
                }
            }
            return tequilala;
        }

        public static void SaveTequiLaLa(TequiLaLa tequilala)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Id", tequilala.Id.ToString() },
                { "@Owner", tequilala.Owner },
                { "@Storage", JsonConvert.SerializeObject(tequilala.Storage)},
                { "@MoneyStorage", tequilala.MoneyStorage.ToString() },
                { "@PosX", tequilala.Position.X.ToString().Replace(",", ".") },
                { "@PosY", tequilala.Position.Y.ToString().Replace(",", ".") },
                { "@PosZ", tequilala.Position.Z.ToString().Replace(",", ".") },
                { "@PedPosX", tequilala.PedPosition.X.ToString().Replace(",", ".") },
                { "@PedPosY", tequilala.PedPosition.Y.ToString().Replace(",", ".") },
                { "@PedPosZ", tequilala.PedPosition.Z.ToString().Replace(",", ".") },
                { "@PedRot", tequilala.PedRotation.Z.ToString().Replace(",", ".") }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE tequilala SET Owner = @Owner, Storage = @Storage, MoneyStorage = @MoneyStorage," +
                "PosX = @PosX, PosY = @PosY, PosZ = @PosZ, PedPosX = @PedPosX, PedPosY = @PedPosY, PedPosZ = @PedPosZ, PedRot = @PedRot WHERE Id = @Id LIMIT 1", parameters);
        }

        public static TequiLaLa AddTequiLaLa(Vector3 position)
        {
            TequiLaLa tequilala = new TequiLaLa();
            tequilala.Position = position;
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "@Storage", JsonConvert.SerializeObject(tequilala.Storage)},
                { "@MoneyStorage", tequilala.MoneyStorage.ToString() },
                { "@PosX", tequilala.Position.X.ToString().Replace(",", ".") },
                { "@PosY", tequilala.Position.Y.ToString().Replace(",", ".") },
                { "@PosZ", tequilala.Position.Z.ToString().Replace(",", ".") },
                { "@PedPosX", tequilala.PedPosition.X.ToString().Replace(",", ".") },
                { "@PedPosY", tequilala.PedPosition.Y.ToString().Replace(",", ".") },
                { "@PedPosZ", tequilala.PedPosition.Z.ToString().Replace(",", ".") },
                { "@PedRot", tequilala.PedRotation.Z.ToString().Replace(",", ".") }
            };

            DataTable result = DatabaseHandler.ExecutePreparedStatement("INSERT INTO tequilala (Storage, MoneyStorage, PosX, PosY, PosZ, PedPosX, PedPosY, PedPosZ, PedRot) " +
                "VALUES (@Storage, @MoneyStorage, @PosX, @PosY, @PosZ, @PedPosX, @PedPosY, @PedPosZ, @PedRot)", parameters);

            tequilala.Blip = API.shared.createBlip(tequilala.Position);
            tequilala.Blip.sprite = 51;
            tequilala.Blip.scale = 0.8f;
            tequilala.Blip.name = "Tequi-La-La";
            tequilala.Blip.shortRange = true;

            BlipService.BlipService.BlipList.Add(tequilala.Blip);
            TequiLaLaList.Add(tequilala);
            return tequilala;
        }

        public static void OpenTequiLaLaMenu(Client client, TequiLaLa tequilala)
        {
            List<TequiLaLaMenuItem> TequiLaLaItems = new List<TequiLaLaMenuItem>();
            tequilala.Storage.ForEach(item =>
            {
                TequiLaLaItems.Add(new TequiLaLaMenuItem
                {
                    Id = item.Id,
                    Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
                    BuyPrice = item.BuyPrice,
                    Count = item.Count
                });
            });

            List<InventoryMenuItem> TequiLaLaInventoryItems = new List<InventoryMenuItem>();
            Player player = client.getData("player");
            player.Character.Inventory.ForEach(item =>
            {
                Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
                if (invitem == null)
                    return;
                if (invitem.Sellable)
                {
                    TequiLaLaInventoryItems.Add(new InventoryMenuItem
                    {
                        Id = item.ItemID,
                        Name = invitem.Name,
                        Description = invitem.DefaultSellPrice.ToString(),
                        Count = item.Count
                    });
                }
            });

            SaveTequiLaLa(tequilala);

            API.shared.triggerClientEvent(client, "TequiLaLa_OpenMenu", JsonConvert.SerializeObject(TequiLaLaItems), JsonConvert.SerializeObject(TequiLaLaInventoryItems), tequilala.MenuImage);
        }

        public static void RefreshTequiLaLaBuyMenu(Client client, TequiLaLa tequilala)
        {
            List<TequiLaLaMenuItem> TequiLaLaItems = new List<TequiLaLaMenuItem>();
            tequilala.Storage.ForEach(item =>
            {
                TequiLaLaItems.Add(new TequiLaLaMenuItem
                {
                    Id = item.Id,
                    Name = ItemService.ItemService.ItemList.First(x => x.Id == item.Id).Name,
                    BuyPrice = item.BuyPrice,
                    Count = item.Count
                });
            });

            List<InventoryMenuItem> TequiLaLaInventoryItems = new List<InventoryMenuItem>();
            Player player = client.getData("player");
            player.Character.Inventory.ForEach(item =>
            {
                Item invitem = ItemService.ItemService.ItemList.First(x => x.Id == item.ItemID);
                if (invitem == null)
                    return;
                if (invitem.Sellable)
                {
                    TequiLaLaInventoryItems.Add(new InventoryMenuItem
                    {
                        Id = item.ItemID,
                        Name = invitem.Name,
                        Description = invitem.DefaultSellPrice.ToString(),
                        Count = item.Count
                    });
                }
            });

            API.shared.triggerClientEvent(client, "TequiLaLa_RefreshTequiLaLaMenu", JsonConvert.SerializeObject(TequiLaLaItems), JsonConvert.SerializeObject(TequiLaLaInventoryItems));
        }

        public static void CloseTequiLaLaMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "TequiLaLa_CloseMenu");
        }
    }
}

