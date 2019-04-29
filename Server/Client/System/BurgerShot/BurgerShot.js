"use strict";
var BurgerShotBuyMenu = null;
var BurgerShotMenu = null;
var BurgerShotJson;
var BurgerShotBuyItem;
var BurgerShotMenuImage = "";
API.onResourceStart.connect(function () {
    BurgerShotBuyMenu = API.createMenu(" ", "Kaufen", 0, 0, 6);
    BurgerShotMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(BurgerShotBuyMenu, "Client/MenuImages/burgershot_title_24-7.jpg");
    API.setMenuBannerTexture(BurgerShotMenu, "Client/MenuImages/burgershot_title_24-7.jpg");
    BurgerShotBuyItem = API.createMenuItem("Kaufen", "");
    BurgerShotMenu.AddItem(BurgerShotBuyItem);
    BurgerShotMenu.BindMenuToItem(BurgerShotBuyMenu, BurgerShotBuyItem);
    BurgerShotBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("BurgerShotBuyItem", BurgerShotJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "BurgerShot_OpenMenu":
            API.closeAllMenus();
            BurgerShotJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillBurgerShotMenu();
            BurgerShotMenu.Visible = true;
            break;
        case "BurgerShot_CloseMenu":
            BurgerShotBuyMenu.Visible = false;
            BurgerShotMenu.Visible = false;
            break;
        case "BurgerShot_RefreshBurgerShotMenu":
            BurgerShotJson = JSON.parse(args[0]);
            FillBurgerShotMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(BurgerShotBuyMenu, "Client/MenuImages/burgershot_title_24-7.jpg");
        API.setMenuBannerTexture(BurgerShotMenu, "Client/MenuImages/burgershot_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(BurgerShotBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(BurgerShotMenu, "Client/MenuImages/" + image);
    }
}
function FillBurgerShotMenu() {
    BurgerShotBuyMenu.Clear();
    for (var i = 0; i < BurgerShotJson.length; i++) {
        var BurgerShotObj = BurgerShotJson[i];
        var NewItem = API.createMenuItem(BurgerShotObj["Name"], "");
        if (BurgerShotObj["Count"] == 0) {
            var RightLabel = "~r~Ausverkauft!";
        }
        else {
            var RightLabel = BurgerShotObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        BurgerShotBuyMenu.AddItem(NewItem);
    }
}
