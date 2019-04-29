"use strict";
var DealerBuyMenu = null;
var DealerMenu = null;
var DealerJson;
var DealerBuyItem;
var DealerMenuImage = "";
API.onResourceStart.connect(function () {
    DealerBuyMenu = API.createMenu(" ", "Kaufen", 0, 0, 6);
    DealerMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(DealerBuyMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    API.setMenuBannerTexture(DealerMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    DealerBuyItem = API.createMenuItem("Kaufen", "");
    DealerMenu.AddItem(DealerBuyItem);
    DealerMenu.BindMenuToItem(DealerBuyMenu, DealerBuyItem);
    DealerBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("DealerBuyItem", DealerJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Dealer_OpenMenu":
            API.closeAllMenus();
            DealerJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillDealerMenu();
            DealerMenu.Visible = true;
            break;
        case "Dealer_CloseMenu":
            DealerBuyMenu.Visible = false;
            DealerMenu.Visible = false;
            break;
        case "Dealer_RefreshDealerMenu":
            DealerJson = JSON.parse(args[0]);
            FillDealerMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(DealerBuyMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
        API.setMenuBannerTexture(DealerMenu, "Client/MenuImages/Dealerui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(DealerBuyMenu, "Client/MenuImages/" + image);
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
