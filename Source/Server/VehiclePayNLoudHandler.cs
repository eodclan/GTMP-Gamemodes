using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.ItemService;
using FactionLife.Server.Services.MoneyService;
using FactionLife.Server.Services.PayNLoudService;
using System.Linq;
using System;

namespace FactionLife.Server
{
    class PayNLoudHandler
        : Script
    {

        public PayNLoudHandler()
        {
            API.onClientEventTrigger += OnClientEvent;
        }

        public void OnClientEvent(Client client, string eventName, params object[] arguments)
        {
            PayNLoud PayNLoud = null;
            Player player = null;
            switch (eventName)
            {
                case "KeyboardKey_E_Pressed":
                    if (client.isInVehicle)
                        return;
                    if (!client.hasData("player"))
                        return;
                    PayNLoud = PayNLoudService.PayNLoudList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
                    if (PayNLoud == null)
                        return;
                    if (PayNLoud.Storage.Count == 0)
                    {
                        API.sendPictureNotificationToPlayer(client, "Entschuldigung, es ist kein PayNLoud da.", "CHAR_BLOCKED", 0, 0, "ID: ~b~" + PayNLoud.Id, "PayNLoud");
                        return;
                    }
                    PayNLoudService.OpenPayNLoudMenu(client, PayNLoud);
                    break;

                case "PayNLoudBuyItem":
                    if (!client.hasData("player"))
                        return;
                    player = client.getData("player");
                    PayNLoud = PayNLoudService.PayNLoudList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
                    if (PayNLoud == null)
                    {
                        // No PayNLoud nearby
                        API.sendNotificationToPlayer(client, "~r~Kein PayNLoud in der Nähe!!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                        return;
                    }

                    PayNLoudItem item = PayNLoud.Storage.FirstOrDefault(x => x.Id == (int)arguments[0]);
                    if (item == null)
                    {
                        // Not in the assortment
                        API.sendNotificationToPlayer(client, "~r~Nicht im Sortiment!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                        return;
                    }

                    if (!MoneyService.HasPlayerEnoughCash(client, item.BuyPrice))
                    {
                        // Not enough Cash
                        API.sendNotificationToPlayer(client, "~r~Nicht genug Geld!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                        return;
                    }

                    if (item.Count <= 0)
                    {
                        // Sold Out
                        API.sendNotificationToPlayer(client, "~r~Für diese Lizenz ist nicht genug Papier da!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                        return;
                    }

                    Item itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == item.Id);
                    if (itemInfo == null)
                    {
                        // Item Doen't exist
                        API.sendNotificationToPlayer(client, "~r~Diese Lizenz existiert nicht!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                        return;
                    }

                    InventoryItem invitem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == item.Id);
                    if (invitem == null)
                    {
                        player.Character.Inventory.Add(new InventoryItem
                        {
                            ItemID = item.Id,
                            Count = 1
                        });
                    }
                    else
                    {
                        if (player.Character.Inventory.Count >= ItemService.MaxInventorySlots)
                        {
                            API.sendNotificationToPlayer(client, "Deine Tasche ist leider zu voll...");
                        }
                        invitem.Count++;
                    }

                    item.Count--;
                    MoneyService.RemovePlayerCash(client, item.BuyPrice);
                    PayNLoud.MoneyStorage += item.BuyPrice;
                    API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + item.BuyPrice + " $ erhalten!");
                    PayNLoudService.RefreshPayNLoudBuyMenu(client, PayNLoud);
                    PayNLoudService.ClosePayNLoudMenu(client);
                    break;

                case "PayNLoudSellItem":
                    if (!client.hasData("player"))
                        return;
                    player = client.getData("player");
                    PayNLoud = PayNLoudService.PayNLoudList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 2);
                    if (PayNLoud == null)
                    {
                        // No PayNLoud nearby
                        API.sendNotificationToPlayer(client, "~r~Kein PayNLoud in der Nähe!!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                        return;
                    }
                    InventoryItem invItem = player.Character.Inventory.FirstOrDefault(x => x.ItemID == (int)arguments[0]);
                    if (invItem == null)
                    {
                        // Doesn't own this item
                        API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                    }
                    itemInfo = ItemService.ItemList.FirstOrDefault(x => x.Id == invItem.ItemID);
                    if (itemInfo == null)
                    {
                        // Item Doen't exist
                        API.sendNotificationToPlayer(client, "~r~Dieser Artikel existiert nicht!");
                        PayNLoudService.ClosePayNLoudMenu(client);
                        return;
                    }
                    if (invItem.Count <= 0)
                    {
                        // You does not own this item!
                        API.sendNotificationToPlayer(client, "~r~Du besitzt diesen Artikel nicht!");
                        player.Character.Inventory.Remove(invItem);
                    }
                    invItem.Count--;
                    if (invItem.Count <= 0)
                        player.Character.Inventory.Remove(invItem);
                    item = PayNLoud.Storage.FirstOrDefault(x => x.Id == invItem.ItemID);
                    if (item != null)
                        item.Count++;
                    MoneyService.AddPlayerCash(client, itemInfo.DefaultSellPrice);
                    API.sendNotificationToPlayer(client, "~g~Erfolgreich: ~w~" + itemInfo.Name + " ~g~für ~w~" + itemInfo.DefaultSellPrice + " $ erhalten!");
                    PayNLoudService.RefreshPayNLoudBuyMenu(client, PayNLoud);
                    PayNLoudService.ClosePayNLoudMenu(client);
                    break;
            }
        }

    }
}
