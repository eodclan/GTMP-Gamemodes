"use strict";
var TunerNeonsYellowJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "TunerNeonsYellow_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    TunerNeonsYellowJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    TunerNeonsYellow.Visible = true;
                }
            }
            break;
        
    }
});
var TunerNeonsYellow = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    TunerNeonsYellow.Clear();
    for (var i = 0; i < TunerNeonsYellowJson.length; i++) {
        var MenuObj = TunerNeonsYellowJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        TunerNeonsYellow.AddItem(NewItem);
    }
}

TunerNeonsYellow.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("TunerNeonsYellow_ItemSelected", TunerNeonsYellowJson[index]["Value1"]);
});


