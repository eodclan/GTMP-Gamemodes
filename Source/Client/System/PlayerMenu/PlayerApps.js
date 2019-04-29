"use strict";
var PlayerAppsJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PlayerApps_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    PlayerAppsJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    PlayerApps.Visible = true;
                }
            }
            break;
        
    }
});
var PlayerApps = API.createMenu("Apps", "Unsere Apps für dich", 0, 0, 6);
function FillPlayerMenu() {
    PlayerApps.Clear();
    for (var i = 0; i < PlayerAppsJson.length; i++) {
        var MenuObj = PlayerAppsJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        PlayerApps.AddItem(NewItem);
    }
}

PlayerApps.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("PlayerApps_ItemSelected", PlayerAppsJson[index]["Value1"]);
});


