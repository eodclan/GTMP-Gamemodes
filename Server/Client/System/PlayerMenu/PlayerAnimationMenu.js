"use strict";
var PlayerAnimationJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PlayerAnimation_Open":
            var player = API.getLocalPlayer();
            if (API.hasEntitySyncedData(player, "hud")) {
                if (API.getEntitySyncedData(player, "hud")) {
                    API.closeAllMenus();
                    PlayerAnimationJson = JSON.parse(args[0]);
                    FillPlayerMenu();
                    PlayerAnimation.Visible = true;
                }
            }
            break;
        
    }
});
var PlayerAnimation = API.createMenu("Animationen", "Die wichtigsten Animationen", 0, 0, 6);
function FillPlayerMenu() {
    PlayerAnimation.Clear();
    for (var i = 0; i < PlayerAnimationJson.length; i++) {
        var MenuObj = PlayerAnimationJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        PlayerAnimation.AddItem(NewItem);
    }
}

PlayerAnimation.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("PlayerAnimation_ItemSelected", PlayerAnimationJson[index]["Value1"]);
});


