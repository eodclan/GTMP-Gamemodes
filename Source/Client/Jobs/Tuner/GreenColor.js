"use strict";
var TunerNeonsGreenJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "TunerNeonsGreen_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    TunerNeonsGreenJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    TunerNeonsGreen.Visible = true;
                }
            }
            break;
        
    }
});
var TunerNeonsGreen = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    TunerNeonsGreen.Clear();
    for (var i = 0; i < TunerNeonsGreenJson.length; i++) {
        var MenuObj = TunerNeonsGreenJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        TunerNeonsGreen.AddItem(NewItem);
    }
}

TunerNeonsGreen.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("TunerNeonsGreen_ItemSelected", TunerNeonsGreenJson[index]["Value1"]);
});


