"use strict";
var SellerBuyMenu = null;
var SellerSellMenu = null;
var SellerMenu = null;
var SellerJson;
var SellerSellJson;
var SellerBuyItem;
var SellerSellItem;
var SellerMenuImage = "";
API.onResourceStart.connect(function () {
    SellerBuyMenu = API.createMenu(" ", "Du kannst hier nur Verkaufen!", 0, 0, 6);
    SellerSellMenu = API.createMenu(" ", "Verkaufen", 0, 0, 6);
    SellerMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(SellerBuyMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    API.setMenuBannerTexture(SellerSellMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    API.setMenuBannerTexture(SellerMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    SellerBuyItem = API.createMenuItem("Information", "");
    SellerMenu.AddItem(SellerBuyItem);
    SellerMenu.BindMenuToItem(SellerBuyMenu, SellerBuyItem);
    SellerSellItem = API.createMenuItem("Verkaufen", "");
    SellerMenu.AddItem(SellerSellItem);
    SellerMenu.BindMenuToItem(SellerSellMenu, SellerSellItem);
    SellerBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("SellerBuyItem", SellerJson[index]["Id"]);
    });
    SellerSellMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("SellerSellItem", SellerSellJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Seller_OpenMenu":
            API.closeAllMenus();
            SellerJson = JSON.parse(args[0]);
            SellerSellJson = JSON.parse(args[1]);
            SetMenuImage(args[2]);
            FillSellerMenu();
            FillSellerSellMenu();
            SellerMenu.Visible = true;
            break;
        case "Seller_CloseMenu":
            SellerBuyMenu.Visible = false;
            SellerMenu.Visible = false;
            break;
        case "Seller_RefreshSellerMenu":
            SellerJson = JSON.parse(args[0]);
            SellerSellJson = JSON.parse(args[1]);
            FillSellerMenu();
            FillSellerSellMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(SellerBuyMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
        API.setMenuBannerTexture(SellerSellMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
        API.setMenuBannerTexture(SellerMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(SellerBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(SellerSellMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(SellerMenu, "Client/MenuImages/" + image);
    }
}
function FillSellerMenu() {
    SellerBuyMenu.Clear();
    for (var i = 0; i < SellerJson.length; i++) {
        var SellerObj = SellerJson[i];
        if (SellerObj["Count"] == 0) {
            var RightLabel = "~r~Hier kannst du nichts Kaufen!";
        }
    }
}
function FillSellerSellMenu() {
    SellerSellMenu.Clear();
    for (var i = 0; i < SellerSellJson.length; i++) {
        var SellerInvObj = SellerSellJson[i];
        if (SellerInvObj["Count"] != 0) {
            var NewItem = API.createMenuItem(SellerInvObj["Name"], "~b~jeder ~w~" + SellerInvObj["Description"] + " ~g~$");
            NewItem.SetRightLabel(SellerInvObj["Count"] + "x");
            SellerSellMenu.AddItem(NewItem);
        }
    }
}
