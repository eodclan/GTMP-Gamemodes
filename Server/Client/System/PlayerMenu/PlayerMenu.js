"use strict";
var PlayerMenuJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PlayerMenu_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    API.triggerServerEvent("Inventory_Request");
                    PlayerMenuJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    PlayerMenu.Visible = true;
                }
            }
            break;
        case "PlayerMenu_Close":
            API.closeAllMenus();
            break;
        case "Inventory_Open":
            API.closeAllMenus();
            InventoryJson = JSON.parse(args[0]);
            FillInventoryMenu();
            InventoryMenu.Visible = true;
            break;
        case "Inventory_Data":
            InventoryJson = JSON.parse(args[0]);
            FillInventoryMenu();
            break;
        case "Inventory_Success":
            API.closeAllMenus();
            InventoryMenu.Visible = true;
            break;
        case "GiveMoney":
            var usercountinputmoney = API.getUserInput("0", 4);
            var usercountmoney = parseInt(usercountinputmoney);
            if (!isNaN(usercountmoney)) {
                API.triggerServerEvent("GiveMoneyValued", usercountmoney);
            }
            break;

    }
});
var PlayerMenu = API.createMenu("Interaction Menu", "", 0, 0, 6);
function FillPlayerMenu() {
    PlayerMenu.Clear();
    for (var i = 0; i < PlayerMenuJson.length; i++) {
        var MenuObj = PlayerMenuJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        PlayerMenu.AddItem(NewItem);
    }
}
PlayerMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("PlayerMenu_ItemSelected", PlayerMenuJson[index]["Value1"]);
});
var InventoryMenu = API.createMenu("Tasche", "", 0, 0, 6);
var InventoryJson;
var InventoryActionMenu = API.createMenu("Gegenstand Aktion", "Mögliche Aktionen:", 0, 0, 6);
var InventoryActionUse = API.createMenuItem("Benutzen", "");
var InventoryActionGive = API.createMenuItem("Geben", "");
var InventoryActionTrash = API.createMenuItem("Wegwerfen", "");
InventoryActionMenu.AddItem(InventoryActionUse);
InventoryActionMenu.AddItem(InventoryActionGive);
InventoryActionMenu.AddItem(InventoryActionTrash);
function FillInventoryMenu() {
    InventoryMenu.Clear();
    for (var i = 0; i < InventoryJson.length; i++) {
        var invObj = InventoryJson[i];
        var NewItem = API.createMenuItem(invObj["Name"], invObj["Description"]);
        NewItem.SetRightLabel(invObj["Count"] + "x");
        InventoryMenu.AddItem(NewItem);
    }
}
var selecteditemindex = 0;
InventoryMenu.OnItemSelect.connect(function (menu, item, index) {
    selecteditemindex = index;
    InventoryActionMenu.Visible = true;
    InventoryMenu.Visible = false;
});
InventoryActionMenu.OnItemSelect.connect(function (menu, item, index) {
    if (item == InventoryActionUse) {
        API.triggerServerEvent("Inventory_SelectItemUse", InventoryJson[selecteditemindex]["Id"]);
    }
    else {
        var usercountinput = API.getUserInput("0", 4);
        var usercount = parseInt(usercountinput);
        if (!isNaN(usercount)) {
            switch (item) {
                case InventoryActionGive:
                    API.triggerServerEvent("Inventory_SelectItemGive", InventoryJson[selecteditemindex]["Id"], usercount);
                    break;
                case InventoryActionTrash:
                    API.triggerServerEvent("Inventory_SelectItemTrash", InventoryJson[selecteditemindex]["Id"], usercount);
                    break;
            }
        }
        else {
            API.sendNotification("~r~Bitte geben Sie eine gültige Nummer ein!");
        }
    }
    API.triggerServerEvent("Inventory_Request");
});
