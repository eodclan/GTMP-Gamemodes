"use strict";
var TequiLaLaBuyMenu = null;
var TequiLaLaMenu = null;
var TequiLaLaJson;
var TequiLaLaBuyItem;
var TequiLaLaMenuImage = "";
API.onResourceStart.connect(function () {
    TequiLaLaBuyMenu = API.createMenu(" ", "Kaufen", 0, 0, 6);
    TequiLaLaMenu = API.createMenu(" ", "", 0, 0, 6);
    API.setMenuBannerTexture(TequiLaLaBuyMenu, "Client/MenuImages/tequilalaui_title_24-7.jpg");
    API.setMenuBannerTexture(TequiLaLaMenu, "Client/MenuImages/tequilalaui_title_24-7.jpg");
    TequiLaLaBuyItem = API.createMenuItem("Kaufen", "");
    TequiLaLaMenu.AddItem(TequiLaLaBuyItem);
    TequiLaLaMenu.BindMenuToItem(TequiLaLaBuyMenu, TequiLaLaBuyItem);
    TequiLaLaBuyMenu.OnItemSelect.connect(function (menu, item, index) {
        API.triggerServerEvent("TequiLaLaBuyItem", TequiLaLaJson[index]["Id"]);
    });
});
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "TequiLaLa_OpenMenu":
            API.closeAllMenus();
            TequiLaLaJson = JSON.parse(args[0]);
            SetMenuImage(args[2]);
            FillTequiLaLaMenu();
            TequiLaLaMenu.Visible = true;
            break;
        case "TequiLaLa_CloseMenu":
            TequiLaLaBuyMenu.Visible = false;
            TequiLaLaMenu.Visible = false;
            break;
        case "TequiLaLa_RefreshTequiLaLaMenu":
            TequiLaLaJson = JSON.parse(args[0]);
            FillTequiLaLaMenu();
            break;
    }
});
function SetMenuImage(image) {
    if (image == "") {
        API.setMenuBannerTexture(TequiLaLaBuyMenu, "Client/MenuImages/tequilalaui_title_24-7.jpg");
        API.setMenuBannerTexture(TequiLaLaMenu, "Client/MenuImages/tequilalaui_title_24-7.jpg");
    }
    else {
        API.setMenuBannerTexture(TequiLaLaBuyMenu, "Client/MenuImages/" + image);
        API.setMenuBannerTexture(TequiLaLaMenu, "Client/MenuImages/" + image);
    }
}
function FillTequiLaLaMenu() {
    TequiLaLaBuyMenu.Clear();
    for (var i = 0; i < TequiLaLaJson.length; i++) {
        var TequiLaLaObj = TequiLaLaJson[i];
        var NewItem = API.createMenuItem(TequiLaLaObj["Name"], "");
        if (TequiLaLaObj["Count"] == 0) {
            var RightLabel = "~r~Ausverkauft!";
        }
        else {
            var RightLabel = TequiLaLaObj["BuyPrice"] + " $";
        }
        NewItem.SetRightLabel(RightLabel);
        TequiLaLaBuyMenu.AddItem(NewItem);
    }
}
