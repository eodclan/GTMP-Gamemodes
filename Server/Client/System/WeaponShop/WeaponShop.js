"use strict";
var WeaponShopBuyMenu = null;
var WeaponShopMenu = null;
var WeaponShopJson;
var WeaponShopBuyItem;
var WeaponShopMenuImage = "";
API.onResourceStart.connect(function () {
    WeaponShopBuyMenu = API.createMenu(" ", "Kaufen", 0, 0, 6);
    WeaponShopMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(WeaponShopBuyMenu, "Client/MenuImages/ammunationui_title_24-7.jpg");
    API.setMenuBannerTexture(WeaponShopMenu, "Client/MenuImages/ammunationui_title_24-7.jpg");
    WeaponShopBuyItem = API.createMenuItem("Kaufen", "");
    WeaponShopMenu.AddItem(WeaponShopBuyItem);
    WeaponShopMenu.BindMenuToItem(WeaponShopBuyMenu, WeaponShopBuyItem);
    WeaponShopBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("WeaponShopBuyItem", WeaponShopJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "WeaponShop_OpenMenu":
            API.closeAllMenus();
            WeaponShopJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillWeaponShopMenu();
            WeaponShopMenu.Visible = true;
            break;
        case "WeaponShop_CloseMenu":
            WeaponShopBuyMenu.Visible = false;
            WeaponShopMenu.Visible = false;
            break;
        case "WeaponShop_RefreshWeaponShopMenu":
            WeaponShopJson = JSON.parse(args[0]);
            FillWeaponShopMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(WeaponShopBuyMenu, "Client/MenuImages/ammunationui_title_24-7.jpg");
        API.setMenuBannerTexture(WeaponShopMenu, "Client/MenuImages/ammunationui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(WeaponShopBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(WeaponShopMenu, "Client/MenuImages/" + image);
    }
}
function FillWeaponShopMenu() {
    WeaponShopBuyMenu.Clear();
    for (var i = 0; i < WeaponShopJson.length; i++) {
        var WeaponShopObj = WeaponShopJson[i];
        var NewItem = API.createMenuItem(WeaponShopObj["Name"], "");
        if (WeaponShopObj["Count"] == 0) {
            var RightLabel = "~r~Ausverkauft!";
        }
        else {
            var RightLabel = WeaponShopObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        WeaponShopBuyMenu.AddItem(NewItem);
    }
}
