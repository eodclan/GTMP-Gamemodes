"use strict";
var EisenBuyMenu = null;
var EisenMenu = null;
var EisenJson;
var EisenBuyItem;
var EisenMenuImage = "";
API.onResourceStart.connect(function () {
    EisenBuyMenu = API.createMenu(" ", "Abkaufen", 0, 0, 6);
    EisenMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(EisenBuyMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
    API.setMenuBannerTexture(EisenMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
    EisenBuyItem = API.createMenuItem("Abkaufen", "");
    EisenMenu.AddItem(EisenBuyItem);
    EisenMenu.BindMenuToItem(EisenBuyMenu, EisenBuyItem);
    EisenBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("EisenBuyItem", EisenJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Eisen_OpenMenu":
            API.closeAllMenus();
            EisenJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillEisenMenu();
            EisenMenu.Visible = true;
            break;
        case "Eisen_CloseMenu":
            EisenBuyMenu.Visible = false;
            EisenMenu.Visible = false;
            break;
        case "Eisen_RefreshEisenMenu":
            EisenJson = JSON.parse(args[0]);
            FillEisenMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(EisenBuyMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
        API.setMenuBannerTexture(EisenMenu, "Client/MenuImages/eisenui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(EisenBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(EisenMenu, "Client/MenuImages/" + image);
    }
}
function FillEisenMenu() {
    EisenBuyMenu.Clear();
    for (var i = 0; i < EisenJson.length; i++) {
        var EisenObj = EisenJson[i];
        var NewItem = API.createMenuItem(EisenObj["Name"], "");
        if (EisenObj["Count"] == 0) {
            var RightLabel = "~r~Kein Eisen da!";
        }
        else {
            var RightLabel = EisenObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        EisenBuyMenu.AddItem(NewItem);
    }
}
