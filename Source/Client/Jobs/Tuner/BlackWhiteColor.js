"use strict";
var TunerNeonsBlackWhiteJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "TunerNeonsBlackWhite_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    TunerNeonsBlackWhiteJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    TunerNeonsBlackWhite.Visible = true;
                }
            }
            break;
        
    }
});
var TunerNeonsBlackWhite = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    TunerNeonsBlackWhite.Clear();
    for (var i = 0; i < TunerNeonsBlackWhiteJson.length; i++) {
        var MenuObj = TunerNeonsBlackWhiteJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        TunerNeonsBlackWhite.AddItem(NewItem);
    }
}

TunerNeonsBlackWhite.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("TunerNeonsBlackWhite_ItemSelected", TunerNeonsBlackWhiteJson[index]["Value1"]);
});


