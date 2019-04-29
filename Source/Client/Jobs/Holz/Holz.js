"use strict";
var HolzBuyMenu = null;
var HolzMenu = null;
var HolzJson;
var HolzBuyItem;
var HolzMenuImage = "";
API.onResourceStart.connect(function () {
    HolzBuyMenu = API.createMenu(" ", "Abkaufen", 0, 0, 6);
    HolzMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(HolzBuyMenu, "Client/MenuImages/holzui_title_24-7.jpg");
    API.setMenuBannerTexture(HolzMenu, "Client/MenuImages/holzui_title_24-7.jpg");
    HolzBuyItem = API.createMenuItem("Abkaufen", "");
    HolzMenu.AddItem(HolzBuyItem);
    HolzMenu.BindMenuToItem(HolzBuyMenu, HolzBuyItem);
    HolzBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("HolzBuyItem", HolzJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Holz_OpenMenu":
            API.closeAllMenus();
            HolzJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillHolzMenu();
            HolzMenu.Visible = true;
            break;
        case "Holz_CloseMenu":
            HolzBuyMenu.Visible = false;
            HolzMenu.Visible = false;
            break;
        case "Holz_RefreshHolzMenu":
            HolzJson = JSON.parse(args[0]);
            FillHolzMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(HolzBuyMenu, "Client/MenuImages/holzui_title_24-7.jpg");
        API.setMenuBannerTexture(HolzMenu, "Client/MenuImages/holzui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(HolzBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(HolzMenu, "Client/MenuImages/" + image);
    }
}
function FillHolzMenu() {
    HolzBuyMenu.Clear();
    for (var i = 0; i < HolzJson.length; i++) {
        var HolzObj = HolzJson[i];
        var NewItem = API.createMenuItem(HolzObj["Name"], "");
        if (HolzObj["Count"] == 0) {
            var RightLabel = "~r~Kein Holz da!";
        }
        else {
            var RightLabel = HolzObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        HolzBuyMenu.AddItem(NewItem);
    }
}
