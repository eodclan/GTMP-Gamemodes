"use strict";
var keyboardInternet = null;
var res = API.getScreenResolution();
var showInternetBrowser = false;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Tastaturbelegung_OpenMenu":
            TastaturbelegungStartMenu();
            break;
        case "closeTastaturbelegung_OpenMenu":
            API.destroyCefBrowser(keyboardInternet);
            API.showCursor(false);
            API.setCanOpenChat(true);
            API.setHudVisible(true);
            showInternetBrowser = false;
            break;

    }
});

function TastaturbelegungStartMenu() {
	if (showInternetBrowser === false) {
            keyboardInternet = API.createCefBrowser(1480, 780, false);
            API.waitUntilCefBrowserInit(keyboardInternet);
            API.setCefBrowserPosition(keyboardInternet, 50, 50);
            API.loadPageCefBrowser(keyboardInternet, "http://rpdeluxe.ml/Tastaturbelegung/tastaturbelegung.html");
            API.showCursor(true);
            API.setCanOpenChat(false);
            API.setHudVisible(true);
            showInternetBrowser = true;
	}
}
