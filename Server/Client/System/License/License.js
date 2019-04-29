"use strict";
var LizenzMenuJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Lizenz_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    API.triggerServerEvent("Inventory_Request");
                    LizenzMenuJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    LizenzMenu.Visible = true;
                }
            }
            break;

    }
});
var LizenzMenu = API.createMenu("Lizenzen", "", 0, 0, 6);
function FillPlayerMenu() {
    LizenzMenu.Clear();
    for (var i = 0; i < LizenzMenuJson.length; i++) {
        var MenuObj = LizenzMenuJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        LizenzMenu.AddItem(NewItem);
    }
}

LizenzMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("LizenzMenu_ItemSelected", LizenzMenuJson[index]["Value1"]);
});


