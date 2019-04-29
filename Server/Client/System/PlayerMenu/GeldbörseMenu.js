"use strict";
var GeldbörseMenuJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Geldbörse_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    API.triggerServerEvent("Inventory_Request");
                    GeldbörseMenuJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    GeldbörseMenu.Visible = true;
                }
            }
            break;
        
    }
});
var GeldbörseMenu = API.createMenu("Geldbörse", "", 0, 0, 6);
function FillPlayerMenu() {
    GeldbörseMenu.Clear();
    for (var i = 0; i < GeldbörseMenuJson.length; i++) {
        var MenuObj = GeldbörseMenuJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        GeldbörseMenu.AddItem(NewItem);
    }
}

GeldbörseMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("GeldbörsenMenu_ItemSelected", GeldbörseMenuJson[index]["Value1"]);
});


