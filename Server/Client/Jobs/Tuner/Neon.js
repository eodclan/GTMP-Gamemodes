"use strict";
var IllegalTunerVehNeonJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "IllegalTunerVehNeon_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    IllegalTunerVehNeonJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    IllegalTunerVehNeon.Visible = true;
                }
            }
            break;
        
    }
});
var IllegalTunerVehNeon = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    IllegalTunerVehNeon.Clear();
    for (var i = 0; i < IllegalTunerVehNeonJson.length; i++) {
        var MenuObj = IllegalTunerVehNeonJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        IllegalTunerVehNeon.AddItem(NewItem);
    }
}

IllegalTunerVehNeon.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("IllegalTunerVehNeon_ItemSelected", IllegalTunerVehNeonJson[index]["Value1"]);
});


