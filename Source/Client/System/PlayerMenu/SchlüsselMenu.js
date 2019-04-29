"use strict";
var SchlüsselMenuJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Schlüssel_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    API.triggerServerEvent("Inventory_Request");
                    SchlüsselMenuJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    SchlüsselMenu.Visible = true;
                }
            }
            break;
        
    }
});
var SchlüsselMenu = API.createMenu("Schlüssel", "", 0, 0, 6);
function FillPlayerMenu() {
    SchlüsselMenu.Clear();
    for (var i = 0; i < SchlüsselMenuJson.length; i++) {
        var MenuObj = SchlüsselMenuJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        SchlüsselMenu.AddItem(NewItem);
    }
}

SchlüsselMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("SchlüsselMenu_ItemSelected", SchlüsselMenuJson[index]["Value1"]);
});


