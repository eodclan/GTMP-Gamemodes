"use strict";
var SmartphoneFLNetz = null;
var res = API.getScreenResolution();
var showInternetBrowser = false;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Smartphone_OpenMenu":
            SmartphoneStartMenu();
            break;
        case "closeSmartphone_OpenMenu":
            API.destroyCefBrowser(SmartphoneFLNetz);
            API.showCursor(false);
            API.setCanOpenChat(true);
            API.setHudVisible(true);
            showInternetBrowser = false;
            break;
    }
});

function SmartphoneStartMenu() {
	if (showInternetBrowser === false) {
            SmartphoneFLNetz = API.createCefBrowser(1480, 780, false);
            API.waitUntilCefBrowserInit(SmartphoneFLNetz);
            API.setCefBrowserPosition(SmartphoneFLNetz, 50, 50);
            API.loadPageCefBrowser(SmartphoneFLNetz, "http://rpdeluxe.ml/Smartphone/smartphone.html");
            API.showCursor(true);
            API.setCanOpenChat(false);
            API.setHudVisible(true);
            showInternetBrowser = true;
	}
}
