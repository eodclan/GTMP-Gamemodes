"use strict";
var TunerNeonsBluemJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "TunerNeonsBluem_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    TunerNeonsBluemJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    TunerNeonsBluem.Visible = true;
                }
            }
            break;
        
    }
});
var TunerNeonsBluem = API.createMenu("Illegaler Tuner", "", 0, 0, 6);
function FillPlayerMenu() {
    TunerNeonsBluem.Clear();
    for (var i = 0; i < TunerNeonsBluemJson.length; i++) {
        var MenuObj = TunerNeonsBluemJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        TunerNeonsBluem.AddItem(NewItem);
    }
}

TunerNeonsBluem.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("TunerNeonsBluem_ItemSelected", TunerNeonsBluemJson[index]["Value1"]);
});


