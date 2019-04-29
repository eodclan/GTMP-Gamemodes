"use strict";
var PlayerClothesJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PlayerClothes_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    PlayerClothesJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    PlayerClothes.Visible = true;
                }
            }
            break;
        
    }
});
var PlayerClothes = API.createMenu("Klamotten", "An und Ausziehen", 0, 0, 6);
function FillPlayerMenu() {
    PlayerClothes.Clear();
    for (var i = 0; i < PlayerClothesJson.length; i++) {
        var MenuObj = PlayerClothesJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        PlayerClothes.AddItem(NewItem);
    }
}

PlayerClothes.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("PlayerClothes_ItemSelected", PlayerClothesJson[index]["Value1"]);
});


