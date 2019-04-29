"use strict";
var IllegalTunerMenuJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "IllegalTunerMenu_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    IllegalTunerMenuJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    IllegalTunerMenu.Visible = true;
                }
            }
            break;
        
    }
});
var IllegalTunerMenu = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    IllegalTunerMenu.Clear();
    for (var i = 0; i < IllegalTunerMenuJson.length; i++) {
        var MenuObj = IllegalTunerMenuJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        IllegalTunerMenu.AddItem(NewItem);
    }
}

IllegalTunerMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("IllegalTunerMenu_ItemSelected", IllegalTunerMenuJson[index]["Value1"]);
});


