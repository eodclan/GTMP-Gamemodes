"use strict";
var WeaponLicenseBuyMenu = null;
var WeaponLicenseMenu = null;
var WeaponLicenseJson;
var WeaponLicenseBuyItem;
var WeaponLicenseMenuImage = "";
API.onResourceStart.connect(function () {
    WeaponLicenseBuyMenu = API.createMenu(" ", "Schein Erstellen", 0, 0, 6);
    WeaponLicenseMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(WeaponLicenseBuyMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
    API.setMenuBannerTexture(WeaponLicenseMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
    WeaponLicenseBuyItem = API.createMenuItem("Schein Akte nehmen", "");
    WeaponLicenseMenu.AddItem(WeaponLicenseBuyItem);
    WeaponLicenseMenu.BindMenuToItem(WeaponLicenseBuyMenu, WeaponLicenseBuyItem);
    WeaponLicenseBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("WeaponLicenseBuyItem", WeaponLicenseJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "WeaponLicense_OpenMenu":
            API.closeAllMenus();
            WeaponLicenseJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillWeaponLicenseMenu();
            WeaponLicenseMenu.Visible = true;
            break;
        case "WeaponLicense_CloseMenu":
            WeaponLicenseBuyMenu.Visible = false;
            WeaponLicenseMenu.Visible = false;
            break;
        case "WeaponLicense_RefreshWeaponLicenseMenu":
            WeaponLicenseJson = JSON.parse(args[0]);
            FillWeaponLicenseMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(WeaponLicenseBuyMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
        API.setMenuBannerTexture(WeaponLicenseMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(WeaponLicenseBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(WeaponLicenseMenu, "Client/MenuImages/" + image);
    }
}
function FillWeaponLicenseMenu() {
    WeaponLicenseBuyMenu.Clear();
    for (var i = 0; i < WeaponLicenseJson.length; i++) {
        var WeaponLicenseObj = WeaponLicenseJson[i];
        var NewItem = API.createMenuItem(WeaponLicenseObj["Name"], "");
        if (WeaponLicenseObj["Count"] == 0) {
            var RightLabel = "~r~Kein Bearbeiter ist gerade da!!";
        }
        else {
            var RightLabel = WeaponLicenseObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        WeaponLicenseBuyMenu.AddItem(NewItem);
    }
}
