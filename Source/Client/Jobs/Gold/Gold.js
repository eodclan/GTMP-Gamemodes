"use strict";
var GoldBuyMenu = null;
var GoldMenu = null;
var GoldJson;
var GoldBuyItem;
var GoldMenuImage = "";
API.onResourceStart.connect(function () {
    GoldBuyMenu = API.createMenu(" ", "Abkaufen", 0, 0, 6);
    GoldMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(GoldBuyMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
    API.setMenuBannerTexture(GoldMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
    GoldBuyItem = API.createMenuItem("Abkaufen", "");
    GoldMenu.AddItem(GoldBuyItem);
    GoldMenu.BindMenuToItem(GoldBuyMenu, GoldBuyItem);
    GoldBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("GoldBuyItem", GoldJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Gold_OpenMenu":
            API.closeAllMenus();
            GoldJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillGoldMenu();
            GoldMenu.Visible = true;
            break;
        case "Gold_CloseMenu":
            GoldBuyMenu.Visible = false;
            GoldMenu.Visible = false;
            break;
        case "Gold_RefreshGoldMenu":
            GoldJson = JSON.parse(args[0]);
            FillGoldMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(GoldBuyMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
        API.setMenuBannerTexture(GoldMenu, "Client/MenuImages/Goldui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(GoldBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(GoldMenu, "Client/MenuImages/" + image);
    }
}
function FillGoldMenu() {
    GoldBuyMenu.Clear();
    for (var i = 0; i < GoldJson.length; i++) {
        var GoldObj = GoldJson[i];
        var NewItem = API.createMenuItem(GoldObj["Name"], "");
        if (GoldObj["Count"] == 0) {
            var RightLabel = "~r~Kein Gold da!";
        }
        else {
            var RightLabel = GoldObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        GoldBuyMenu.AddItem(NewItem);
    }
}
