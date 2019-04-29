"use strict";
var GoldSellerBuyMenu = null;
var GoldSellerSellMenu = null;
var GoldSellerMenu = null;
var GoldSellerJson;
var GoldSellerSellJson;
var GoldSellerBuyItem;
var GoldSellerSellItem;
var GoldSellerMenuImage = "";
API.onResourceStart.connect(function () {
    GoldSellerBuyMenu = API.createMenu(" ", "Gedulde dich!", 0, 0, 6);
    GoldSellerSellMenu = API.createMenu(" ", "Verkaufen", 0, 0, 6);
    GoldSellerMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(GoldSellerBuyMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
    API.setMenuBannerTexture(GoldSellerSellMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
    API.setMenuBannerTexture(GoldSellerMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
    GoldSellerBuyItem = API.createMenuItem("Gedulde dich!", "");
    GoldSellerMenu.AddItem(GoldSellerBuyItem);
    GoldSellerMenu.BindMenuToItem(GoldSellerBuyMenu, GoldSellerBuyItem);
    GoldSellerSellItem = API.createMenuItem("Verkaufen", "");
    GoldSellerMenu.AddItem(GoldSellerSellItem);
    GoldSellerMenu.BindMenuToItem(GoldSellerSellMenu, GoldSellerSellItem);
    GoldSellerBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("GoldSellerBuyItem", GoldSellerJson[index]["Id"]);
    });
    GoldSellerSellMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("GoldSellerSellItem", GoldSellerSellJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "GoldSeller_OpenMenu":
            API.closeAllMenus();
            GoldSellerJson = JSON.parse(args[0]);
            GoldSellerSellJson = JSON.parse(args[1]);
            SetMenuImage(args[2]);
            FillGoldSellerMenu();
            FillGoldSellerSellMenu();
            GoldSellerMenu.Visible = true;
            break;
        case "GoldSeller_CloseMenu":
            GoldSellerBuyMenu.Visible = false;
            GoldSellerMenu.Visible = false;
            break;
        case "GoldSeller_RefreshGoldSellerMenu":
            GoldSellerJson = JSON.parse(args[0]);
            GoldSellerSellJson = JSON.parse(args[1]);
            FillGoldSellerMenu();
            FillGoldSellerSellMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(GoldSellerBuyMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
        API.setMenuBannerTexture(GoldSellerSellMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
        API.setMenuBannerTexture(GoldSellerMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(GoldSellerBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(GoldSellerSellMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(GoldSellerMenu, "Client/MenuImages/" + image);
    }
}
function FillGoldSellerMenu() {
    GoldSellerBuyMenu.Clear();
    for (var i = 0; i < GoldSellerJson.length; i++) {
        var GoldSellerObj = GoldSellerJson[i];
        if (GoldSellerObj["Count"] == 0) {
            var RightLabel = "~r~Habe geduld bei den Abbau von Eisen!";
        }
    }
}
function FillGoldSellerSellMenu() {
    GoldSellerSellMenu.Clear();
    for (var i = 0; i < GoldSellerSellJson.length; i++) {
        var GoldSellerInvObj = GoldSellerSellJson[i];
        if (GoldSellerInvObj["Count"] != 0) {
            var NewItem = API.createMenuItem(GoldSellerInvObj["Name"], "~b~jeder ~w~" + GoldSellerInvObj["Description"] + " ~g~$");
            NewItem.SetRightLabel(GoldSellerInvObj["Count"] + "x");
            GoldSellerSellMenu.AddItem(NewItem);
        }
    }
}
