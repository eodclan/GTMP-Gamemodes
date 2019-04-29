"use strict";
var PlayerGPSJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PlayerGPS_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    PlayerGPSJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    PlayerGPS.Visible = true;
                }
            }
            break;
        
    }
});
var PlayerGPS = API.createMenu("Admin", "Admin Menü für die Blips", 0, 0, 6);
function FillPlayerMenu() {
    PlayerGPS.Clear();
    for (var i = 0; i < PlayerGPSJson.length; i++) {
        var MenuObj = PlayerGPSJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        PlayerGPS.AddItem(NewItem);
    }
}

PlayerGPS.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("PlayerGPS_ItemSelected", PlayerGPSJson[index]["Value1"]);
});


