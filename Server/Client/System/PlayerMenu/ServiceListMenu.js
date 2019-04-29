"use strict";
var ServiceMenuJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "ServiceMenu_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    API.triggerServerEvent("Inventory_Request");
                    ServiceMenuJson = JSON.parse(args[0]);
                    FillServiceMenu();
                    ServiceMenu.Visible = true;
                }
            }
            break;

    }
});
var ServiceMenu = API.createMenu("Service-Anfragen", "", 0, 0, 6);
function FillServiceMenu() {
    ServiceMenu.Clear();
    for (var i = 0; i < ServiceMenuJson.length; i++) {
        var MenuObj = ServiceMenuJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        ServiceMenu.AddItem(NewItem);
    }
}
ServiceMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("ServiceMenu_Trigger", ServiceMenuJson[index]["Value1"], ServiceMenuJson[index]["Value2"]);
});
