"use strict";
var HolzSellerBuyMenu = null;
var HolzSellerSellMenu = null;
var HolzSellerMenu = null;
var HolzSellerJson;
var HolzSellerSellJson;
var HolzSellerBuyItem;
var HolzSellerSellItem;
var HolzSellerMenuImage = "";
API.onResourceStart.connect(function () {
    HolzSellerBuyMenu = API.createMenu(" ", "Gedulde dich!", 0, 0, 6);
    HolzSellerSellMenu = API.createMenu(" ", "Verkaufen", 0, 0, 6);
    HolzSellerMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(HolzSellerBuyMenu, "Client/MenuImages/holzui_title_24-7.jpg");
    API.setMenuBannerTexture(HolzSellerSellMenu, "Client/MenuImages/holzui_title_24-7.jpg");
    API.setMenuBannerTexture(HolzSellerMenu, "Client/MenuImages/holzui_title_24-7.jpg");
    HolzSellerBuyItem = API.createMenuItem("Gedulde dich!", "");
    HolzSellerMenu.AddItem(HolzSellerBuyItem);
    HolzSellerMenu.BindMenuToItem(HolzSellerBuyMenu, HolzSellerBuyItem);
    HolzSellerSellItem = API.createMenuItem("Verkaufen", "");
    HolzSellerMenu.AddItem(HolzSellerSellItem);
    HolzSellerMenu.BindMenuToItem(HolzSellerSellMenu, HolzSellerSellItem);
    HolzSellerBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("HolzSellerBuyItem", HolzSellerJson[index]["Id"]);
    });
    HolzSellerSellMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("HolzSellerSellItem", HolzSellerSellJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "HolzSeller_OpenMenu":
            API.closeAllMenus();
            HolzSellerJson = JSON.parse(args[0]);
            HolzSellerSellJson = JSON.parse(args[1]);
            SetMenuImage(args[2]);
            FillHolzSellerMenu();
            FillHolzSellerSellMenu();
            HolzSellerMenu.Visible = true;
            break;
        case "HolzSeller_CloseMenu":
            HolzSellerBuyMenu.Visible = false;
            HolzSellerMenu.Visible = false;
            break;
        case "HolzSeller_RefreshHolzSellerMenu":
            HolzSellerJson = JSON.parse(args[0]);
            HolzSellerSellJson = JSON.parse(args[1]);
            FillHolzSellerMenu();
            FillHolzSellerSellMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(HolzSellerBuyMenu, "Client/MenuImages/holzui_title_24-7.jpg");
        API.setMenuBannerTexture(HolzSellerSellMenu, "Client/MenuImages/holzui_title_24-7.jpg");
        API.setMenuBannerTexture(HolzSellerMenu, "Client/MenuImages/holzui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(HolzSellerBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(HolzSellerSellMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(HolzSellerMenu, "Client/MenuImages/" + image);
    }
}
function FillHolzSellerMenu() {
    HolzSellerBuyMenu.Clear();
    for (var i = 0; i < HolzSellerJson.length; i++) {
        var HolzSellerObj = HolzSellerJson[i];
        if (HolzSellerObj["Count"] == 0) {
            var RightLabel = "~r~Habe geduld bei den Abbau von Eisen!";
        }
    }
}
function FillHolzSellerSellMenu() {
    HolzSellerSellMenu.Clear();
    for (var i = 0; i < HolzSellerSellJson.length; i++) {
        var HolzSellerInvObj = HolzSellerSellJson[i];
        if (HolzSellerInvObj["Count"] != 0) {
            var NewItem = API.createMenuItem(HolzSellerInvObj["Name"], "~b~jeder ~w~" + HolzSellerInvObj["Description"] + " ~g~$");
            NewItem.SetRightLabel(HolzSellerInvObj["Count"] + "x");
            HolzSellerSellMenu.AddItem(NewItem);
        }
    }
}
