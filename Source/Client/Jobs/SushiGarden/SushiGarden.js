"use strict";
var FisherSellBuyMenu = null;
var FisherSellSellMenu = null;
var FisherSellMenu = null;
var FisherSellJson;
var FisherSellSellJson;
var FisherSellBuyItem;
var FisherSellSellItem;
var FisherSellMenuImage = "";
API.onResourceStart.connect(function () {
    FisherSellBuyMenu = API.createMenu(" ", "Wir von Sushi Garden sagen danke für ihren Fisch!!", 0, 0, 6);
    FisherSellSellMenu = API.createMenu(" ", "Verkaufen", 0, 0, 6);
    FisherSellMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(FisherSellBuyMenu, "Client/MenuImages/sushigardenui_title_24-7.jpg");
    API.setMenuBannerTexture(FisherSellSellMenu, "Client/MenuImages/sushigardenui_title_24-7.jpg");
    API.setMenuBannerTexture(FisherSellMenu, "Client/MenuImages/sushigardenui_title_24-7.jpg");
    FisherSellBuyItem = API.createMenuItem("Information", "");
    FisherSellMenu.AddItem(FisherSellBuyItem);
    FisherSellMenu.BindMenuToItem(FisherSellBuyMenu, FisherSellBuyItem);
    FisherSellSellItem = API.createMenuItem("Verkaufen", "");
    FisherSellMenu.AddItem(FisherSellSellItem);
    FisherSellMenu.BindMenuToItem(FisherSellSellMenu, FisherSellSellItem);
    FisherSellBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("FisherSellBuyItem", FisherSellJson[index]["Id"]);
    });
    FisherSellSellMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("FisherSellSellItem", FisherSellSellJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "FisherSell_OpenMenu":
            API.closeAllMenus();
            FisherSellJson = JSON.parse(args[0]);
            FisherSellSellJson = JSON.parse(args[1]);
            SetMenuImage(args[2]);
            FillFisherSellMenu();
            FillFisherSellSellMenu();
            FisherSellMenu.Visible = true;
            break;
        case "FisherSell_CloseMenu":
            FisherSellBuyMenu.Visible = false;
            FisherSellMenu.Visible = false;
            break;
        case "FisherSell_RefreshFisherSellMenu":
            FisherSellJson = JSON.parse(args[0]);
            FisherSellSellJson = JSON.parse(args[1]);
            FillFisherSellMenu();
            FillFisherSellSellMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(FisherSellBuyMenu, "Client/MenuImages/sushigardenui_title_24-7.jpg");
        API.setMenuBannerTexture(FisherSellSellMenu, "Client/MenuImages/sushigardenui_title_24-7.jpg");
        API.setMenuBannerTexture(FisherSellMenu, "Client/MenuImages/sushigardenui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(FisherSellBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(FisherSellSellMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(FisherSellMenu, "Client/MenuImages/" + image);
    }
}
function FillFisherSellMenu() {
    FisherSellBuyMenu.Clear();
    for (var i = 0; i < FisherSellJson.length; i++) {
        var FisherSellObj = FisherSellJson[i];
        if (FisherSellObj["Count"] == 0) {
            var RightLabel = "~r~Hier kannst du nichts Kaufen!";
        }
    }
}
function FillFisherSellSellMenu() {
    FisherSellSellMenu.Clear();
    for (var i = 0; i < FisherSellSellJson.length; i++) {
        var FisherSellInvObj = FisherSellSellJson[i];
        if (FisherSellInvObj["Count"] != 0) {
            var NewItem = API.createMenuItem(FisherSellInvObj["Name"], "~b~jeder ~w~" + FisherSellInvObj["Description"] + " ~g~$");
            NewItem.SetRightLabel(FisherSellInvObj["Count"] + "x");
            FisherSellSellMenu.AddItem(NewItem);
        }
    }
}
