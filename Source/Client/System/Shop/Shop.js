"use strict";
var ShopBuyMenu = null;
var ShopMenu = null;
var ShopJson;
var ShopBuyItem;
var ShopMenuImage = "";
API.onResourceStart.connect(function () {
    ShopBuyMenu = API.createMenu(" ", "Kaufen", 0, 0, 6);
    ShopMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(ShopBuyMenu, "Client/MenuImages/shopui_title_24-7.jpg");
    API.setMenuBannerTexture(ShopMenu, "Client/MenuImages/shopui_title_24-7.jpg");
    ShopBuyItem = API.createMenuItem("Kaufen", "");
    ShopMenu.AddItem(ShopBuyItem);
    ShopMenu.BindMenuToItem(ShopBuyMenu, ShopBuyItem);
    ShopBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("ShopBuyItem", ShopJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Shop_OpenMenu":
            API.closeAllMenus();
            ShopJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillShopMenu();
            ShopMenu.Visible = true;
            break;
        case "Shop_CloseMenu":
            ShopBuyMenu.Visible = false;
            ShopMenu.Visible = false;
            break;
        case "Shop_RefreshShopMenu":
            ShopJson = JSON.parse(args[0]);
            FillShopMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(ShopBuyMenu, "Client/MenuImages/shopui_title_24-7.jpg");
        API.setMenuBannerTexture(ShopMenu, "Client/MenuImages/shopui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(ShopBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(ShopMenu, "Client/MenuImages/" + image);
    }
}
function FillShopMenu() {
    ShopBuyMenu.Clear();
    for (var i = 0; i < ShopJson.length; i++) {
        var ShopObj = ShopJson[i];
        var NewItem = API.createMenuItem(ShopObj["Name"], "");
        if (ShopObj["Count"] == 0) {
            var RightLabel = "~r~Ausverkauft!";
        }
        else {
            var RightLabel = ShopObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        ShopBuyMenu.AddItem(NewItem);
    }
}
