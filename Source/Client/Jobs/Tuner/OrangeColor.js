"use strict";
var TunerNeonsOrangeJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "TunerNeonsOrange_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    TunerNeonsOrangeJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    TunerNeonsOrange.Visible = true;
                }
            }
            break;
        
    }
});
var TunerNeonsOrange = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    TunerNeonsOrange.Clear();
    for (var i = 0; i < TunerNeonsOrangeJson.length; i++) {
        var MenuObj = TunerNeonsOrangeJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        TunerNeonsOrange.AddItem(NewItem);
    }
}

TunerNeonsOrange.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("TunerNeonsOrange_ItemSelected", TunerNeonsOrangeJson[index]["Value1"]);
});


