/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />

var DealerBuyMenu = null;
var DealerSellMenu = null;
var DealerMenu = null;
var DealerJson;
var DealerSellJson;
var DealerBuyItem;
var DealerSellItem;
var DealerMenuImage = "";
API.onResourceStart.connect(function () {
    DealerBuyMenu = API.createMenu(" ", "Kaufen", 0, 0, 6);
    DealerSellMenu = API.createMenu(" ", "Verkaufen", 0, 0, 6);
    DealerMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(DealerBuyMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    API.setMenuBannerTexture(DealerSellMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    API.setMenuBannerTexture(DealerMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    DealerBuyItem = API.createMenuItem("Kaufen", "");
    DealerMenu.AddItem(DealerBuyItem);
    DealerMenu.BindMenuToItem(DealerBuyMenu, DealerBuyItem);
    DealerSellItem = API.createMenuItem("Verkaufen", "");
    DealerMenu.AddItem(DealerSellItem);
    DealerMenu.BindMenuToItem(DealerSellMenu, DealerSellItem);
    DealerBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("DealerBuyItem", DealerJson[index]["Id"]);
    });
    DealerSellMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("DealerSellItem", DealerSellJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Dealer_OpenMenu":
            API.closeAllMenus();
            DealerJson = JSON.parse(args[0]);
            DealerSellJson = JSON.parse(args[1]);
            SetMenuImage(args[2]);
            FillDealerMenu();
            FillDealerSellMenu();
            DealerMenu.Visible = true;
            break;
        case "Dealer_CloseMenu":
            DealerBuyMenu.Visible = false;
            DealerMenu.Visible = false;
            break;
        case "Dealer_RefreshDealerMenu":
            DealerJson = JSON.parse(args[0]);
            DealerSellJson = JSON.parse(args[1]);
            FillDealerMenu();
            FillDealerSellMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(DealerBuyMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
        API.setMenuBannerTexture(DealerSellMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
        API.setMenuBannerTexture(DealerMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(DealerBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(DealerSellMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(DealerMenu, "Client/MenuImages/" + image);
    }
}
function FillDealerMenu() {
    DealerBuyMenu.Clear();
    for (var i = 0; i < DealerJson.length; i++) {
        var DealerObj = DealerJson[i];
        var NewItem = API.createMenuItem(DealerObj["Name"], "");
        if (DealerObj["Count"] == 0) {
            var RightLabel = "~r~Kein Dealer da!";
        }
        else {
            var RightLabel = DealerObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        DealerBuyMenu.AddItem(NewItem);
    }
}
function FillDealerSellMenu() {
    DealerSellMenu.Clear();
    for (var i = 0; i < DealerSellJson.length; i++) {
        var DealerInvObj = DealerSellJson[i];
        if (DealerInvObj["Count"] != 0) {
            var NewItem = API.createMenuItem(DealerInvObj["Name"], "~b~jeder ~w~" + DealerInvObj["Description"] + " ~g~$");
            NewItem.SetRightLabel(DealerInvObj["Count"] + "x");
            DealerSellMenu.AddItem(NewItem);
        }
    }
}
