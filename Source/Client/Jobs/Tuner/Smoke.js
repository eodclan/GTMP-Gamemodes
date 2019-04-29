"use strict";
var IllegalTunerTyreSmokeJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "IllegalTunerTyreSmoke_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    IllegalTunerTyreSmokeJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    IllegalTunerTyreSmoke.Visible = true;
                }
            }
            break;
        
    }
});
var IllegalTunerTyreSmoke = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    IllegalTunerTyreSmoke.Clear();
    for (var i = 0; i < IllegalTunerTyreSmokeJson.length; i++) {
        var MenuObj = IllegalTunerTyreSmokeJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        IllegalTunerTyreSmoke.AddItem(NewItem);
    }
}

IllegalTunerTyreSmoke.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("IllegalTunerTyreSmoke_ItemSelected", IllegalTunerTyreSmokeJson[index]["Value1"]);
});


