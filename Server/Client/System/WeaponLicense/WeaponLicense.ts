/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />

var LicenseBuyMenu = null;
var LicenseSellMenu = null;
var LicenseMenu = null;
var LicenseJson;
var LicenseSellJson;
var LicenseBuyItem;
var LicenseSellItem;
var LicenseMenuImage = "";
API.onResourceStart.connect(function () {
    LicenseBuyMenu = API.createMenu(" ", "Schein Erlernen", 0, 0, 6);
    LicenseSellMenu = API.createMenu(" ", "Schein abgeben", 0, 0, 6);
    LicenseMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(LicenseBuyMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
    API.setMenuBannerTexture(LicenseSellMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
    API.setMenuBannerTexture(LicenseMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
    LicenseBuyItem = API.createMenuItem("Schein Erlernen", "");
    LicenseMenu.AddItem(LicenseBuyItem);
    LicenseMenu.BindMenuToItem(LicenseBuyMenu, LicenseBuyItem);
    LicenseSellItem = API.createMenuItem("Schein abgeben", "");
    LicenseMenu.AddItem(LicenseSellItem);
    LicenseMenu.BindMenuToItem(LicenseSellMenu, LicenseSellItem);
    LicenseBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("LicenseBuyItem", LicenseJson[index]["Id"]);
    });
    LicenseSellMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("LicenseSellItem", LicenseSellJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "License_OpenMenu":
            API.closeAllMenus();
            LicenseJson = JSON.parse(args[0]);
            LicenseSellJson = JSON.parse(args[1]);
            SetMenuImage(args[2]);
            FillLicenseMenu();
            FillLicenseSellMenu();
            LicenseMenu.Visible = true;
            break;
        case "License_CloseMenu":
            LicenseBuyMenu.Visible = false;
            LicenseMenu.Visible = false;
            break;
        case "License_RefreshLicenseMenu":
            LicenseJson = JSON.parse(args[0]);
            LicenseSellJson = JSON.parse(args[1]);
            FillLicenseMenu();
            FillLicenseSellMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(LicenseBuyMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
        API.setMenuBannerTexture(LicenseSellMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
        API.setMenuBannerTexture(LicenseMenu, "Client/MenuImages/licenseui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(LicenseBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(LicenseSellMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(LicenseMenu, "Client/MenuImages/" + image);
    }
}
function FillLicenseMenu() {
    LicenseBuyMenu.Clear();
    for (var i = 0; i < LicenseJson.length; i++) {
        var LicenseObj = LicenseJson[i];
        var NewItem = API.createMenuItem(LicenseObj["Name"], "");
        if (LicenseObj["Count"] == 0) {
            var RightLabel = "~r~Kein Lehrer da!";
        }
        else {
            var RightLabel = LicenseObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        LicenseBuyMenu.AddItem(NewItem);
    }
}
function FillLicenseSellMenu() {
    LicenseSellMenu.Clear();
    for (var i = 0; i < LicenseSellJson.length; i++) {
        var LicenseInvObj = LicenseSellJson[i];
        if (LicenseInvObj["Count"] != 0) {
            var NewItem = API.createMenuItem(LicenseInvObj["Name"], "~b~jeder ~w~" + LicenseInvObj["Description"] + " ~g~$");
            NewItem.SetRightLabel(LicenseInvObj["Count"] + "x");
            LicenseSellMenu.AddItem(NewItem);
        }
    }
}
