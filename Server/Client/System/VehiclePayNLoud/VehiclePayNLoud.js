"use strict";
var PayNLoudBuyMenu = null;
var PayNLoudMenu = null;
var PayNLoudJson;
var PayNLoudBuyItem;
var PayNLoudMenuImage = "";
API.onResourceStart.connect(function () {
    PayNLoudBuyMenu = API.createMenu(" ", "Kaufen", 0, 0, 6);
    PayNLoudMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(PayNLoudBuyMenu, "Client/MenuImages/vehiclepaynloudui_title_24-7.jpg");
    API.setMenuBannerTexture(PayNLoudMenu, "Client/MenuImages/vehiclepaynloudui_title_24-7.jpg");
    PayNLoudBuyItem = API.createMenuItem("Kaufen", "");
    PayNLoudMenu.AddItem(PayNLoudBuyItem);
    PayNLoudMenu.BindMenuToItem(PayNLoudBuyMenu, PayNLoudBuyItem);
    PayNLoudBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("PayNLoudBuyItem", PayNLoudJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PayNLoud_OpenMenu":
            API.closeAllMenus();
            PayNLoudJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillPayNLoudMenu();
            PayNLoudMenu.Visible = true;
            break;
        case "PayNLoud_CloseMenu":
            PayNLoudBuyMenu.Visible = false;
            PayNLoudMenu.Visible = false;
            break;
        case "PayNLoud_RefreshPayNLoudMenu":
            PayNLoudJson = JSON.parse(args[0]);
            FillPayNLoudMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(PayNLoudBuyMenu, "Client/MenuImages/vehiclepaynloudui_title_24-7.jpg");
        API.setMenuBannerTexture(PayNLoudMenu, "Client/MenuImages/vehiclepaynloudui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(PayNLoudBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(PayNLoudMenu, "Client/MenuImages/" + image);
    }
}
function FillPayNLoudMenu() {
    PayNLoudBuyMenu.Clear();
    for (var i = 0; i < PayNLoudJson.length; i++) {
        var PayNLoudObj = PayNLoudJson[i];
        var NewItem = API.createMenuItem(PayNLoudObj["Name"], "");
        if (PayNLoudObj["Count"] == 0) {
            var RightLabel = "~r~Kein PayNLoud da!";
        }
        else {
            var RightLabel = PayNLoudObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        PayNLoudBuyMenu.AddItem(NewItem);
    }
}
