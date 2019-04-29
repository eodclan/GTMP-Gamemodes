"use strict";
var SheriffWeaponMenu = API.createMenu("Waffenkammer", "", 0, 0, 6);
var SheriffWeaponJson;
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Sheriff_OpenWeaponMenu":
            API.closeAllMenus();
            SheriffWeaponJson = JSON.parse(args[0]);
            FillSheriffWeaponMenu();
            SheriffWeaponMenu.Visible = true;
            break;
        case "Sheriff_CloseWeaponMenu":
            API.closeAllMenus();
            break;
    }
});
function FillSheriffWeaponMenu() {
    SheriffWeaponMenu.Clear();
    for (var i = 0; i < SheriffWeaponJson.length; i++) {
        var MenuObj = SheriffWeaponJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        SheriffWeaponMenu.AddItem(NewItem);
    }
}
SheriffWeaponMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("Sheriff_WeaponMenuItemSelected", SheriffWeaponJson[index]["Value1"]);
});
