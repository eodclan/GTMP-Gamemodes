"use strict";
var TunerNeonsRedJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "TunerNeonsRed_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    TunerNeonsRedJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    TunerNeonsRed.Visible = true;
                }
            }
            break;
        
    }
});
var TunerNeonsRed = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    TunerNeonsRed.Clear();
    for (var i = 0; i < TunerNeonsRedJson.length; i++) {
        var MenuObj = TunerNeonsRedJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        TunerNeonsRed.AddItem(NewItem);
    }
}

TunerNeonsRed.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("TunerNeonsRed_ItemSelected", TunerNeonsRedJson[index]["Value1"]);
});


