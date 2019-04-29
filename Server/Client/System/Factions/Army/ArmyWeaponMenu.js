"use strict";
var ArmyWeaponMenu = API.createMenu("Army Weapons", "", 0, 0, 6);
var ArmyWeaponJson;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Army_OpenWeaponMenu":
            API.closeAllMenus();
            ArmyWeaponJson = JSON.parse(args[0]);
            FillArmyWeaponMenu();
            ArmyWeaponMenu.Visible = true;
            break;
        case "Army_CloseWeaponMenu":
            API.closeAllMenus();
            break;
    }
});

function FillArmyWeaponMenu() {
    ArmyWeaponMenu.Clear();
    for (var i = 0; i < ArmyWeaponJson.length; i++) {
        var MenuObj = ArmyWeaponJson[i];
        var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]);
        if (MenuObj["RightLabel"] != "") {
            NewItem.SetRightLabel(MenuObj["RightLabel"]);
        }
        ArmyWeaponMenu.AddItem(NewItem);
    }
}

ArmyWeaponMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("Army_WeaponMenuItemSelected", ArmyWeaponJson[index]["Value1"]);
});
