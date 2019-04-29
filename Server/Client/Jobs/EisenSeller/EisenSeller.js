"use strict";
var EisenSellerBuyMenu = null;
var EisenSellerSellMenu = null;
var EisenSellerMenu = null;
var EisenSellerJson;
var EisenSellerSellJson;
var EisenSellerBuyItem;
var EisenSellerSellItem;
var EisenSellerMenuImage = "";
API.onResourceStart.connect(function () {
    EisenSellerBuyMenu = API.createMenu(" ", "Gedulde dich!", 0, 0, 6);
    EisenSellerSellMenu = API.createMenu(" ", "Verkaufen", 0, 0, 6);
    EisenSellerMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(EisenSellerBuyMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
    API.setMenuBannerTexture(EisenSellerSellMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
    API.setMenuBannerTexture(EisenSellerMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
    EisenSellerBuyItem = API.createMenuItem("Gedulde dich!", "");
    EisenSellerMenu.AddItem(EisenSellerBuyItem);
    EisenSellerMenu.BindMenuToItem(EisenSellerBuyMenu, EisenSellerBuyItem);
    EisenSellerSellItem = API.createMenuItem("Verkaufen", "");
    EisenSellerMenu.AddItem(EisenSellerSellItem);
    EisenSellerMenu.BindMenuToItem(EisenSellerSellMenu, EisenSellerSellItem);
    EisenSellerBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("EisenSellerBuyItem", EisenSellerJson[index]["Id"]);
    });
    EisenSellerSellMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("EisenSellerSellItem", EisenSellerSellJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "EisenSeller_OpenMenu":
            API.closeAllMenus();
            EisenSellerJson = JSON.parse(args[0]);
            EisenSellerSellJson = JSON.parse(args[1]);
            SetMenuImage(args[2]);
            FillEisenSellerMenu();
            FillEisenSellerSellMenu();
            EisenSellerMenu.Visible = true;
            break;
        case "EisenSeller_CloseMenu":
            EisenSellerBuyMenu.Visible = false;
            EisenSellerMenu.Visible = false;
            break;
        case "EisenSeller_RefreshEisenSellerMenu":
            EisenSellerJson = JSON.parse(args[0]);
            EisenSellerSellJson = JSON.parse(args[1]);
            FillEisenSellerMenu();
            FillEisenSellerSellMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(EisenSellerBuyMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
        API.setMenuBannerTexture(EisenSellerSellMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
        API.setMenuBannerTexture(EisenSellerMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(EisenSellerBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(EisenSellerSellMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(EisenSellerMenu, "Client/MenuImages/" + image);
    }
}
function FillEisenSellerMenu() {
    EisenSellerBuyMenu.Clear();
    for (var i = 0; i < EisenSellerJson.length; i++) {
        var EisenSellerObj = EisenSellerJson[i];
        if (EisenSellerObj["Count"] == 0) {
            var RightLabel = "~r~Habe geduld bei den Abbau von Eisen!";
        }
    }
}
function FillEisenSellerSellMenu() {
    EisenSellerSellMenu.Clear();
    for (var i = 0; i < EisenSellerSellJson.length; i++) {
        var EisenSellerInvObj = EisenSellerSellJson[i];
        if (EisenSellerInvObj["Count"] != 0) {
            var NewItem = API.createMenuItem(EisenSellerInvObj["Name"], "~b~jeder ~w~" + EisenSellerInvObj["Description"] + " ~g~$");
            NewItem.SetRightLabel(EisenSellerInvObj["Count"] + "x");
            EisenSellerSellMenu.AddItem(NewItem);
        }
    }
}
