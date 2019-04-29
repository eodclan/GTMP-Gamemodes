"use strict";
var DispatchMenuJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "DispatchMenu_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    DispatchMenuJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    DispatchMenu.Visible = true;
                }
            }
            break;
        
    }
});
var DispatchMenu = API.createMenu("Dispatch", "", 0, 0, 6);
function FillPlayerMenu() {
    DispatchMenu.Clear();
    for (var i = 0; i < DispatchMenuJson.length; i++) {
        var MenuObj = DispatchMenuJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        DispatchMenu.AddItem(NewItem);
    }
}

DispatchMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("DispatchMenu_ItemSelected", DispatchMenuJson[index]["Value1"]);
});



