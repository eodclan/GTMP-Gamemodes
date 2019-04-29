"use strict";
var browserInternet = null;
var res = API.getScreenResolution();
var showInternetBrowser = false;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Tablet_OpenMenu":
            TabletStartMenu();
            break;
        case "closeTablet_OpenMenu":
            API.destroyCefBrowser(browserInternet);
            API.showCursor(false);
            API.setCanOpenChat(true);
            API.setHudVisible(true);
            showInternetBrowser = false;
            break;

    }
});

function TabletStartMenu() {
	if (showInternetBrowser === false) {
            browserInternet = API.createCefBrowser(1480, 780, false);
            API.waitUntilCefBrowserInit(browserInternet);
            API.setCefBrowserPosition(browserInternet, 50, 50);
            API.loadPageCefBrowser(browserInternet, "http://rpdeluxe.ml/Tablet/tablet.html");
            API.showCursor(true);
            API.setCanOpenChat(false);
            API.setHudVisible(true);
            showInternetBrowser = true;
	}
}
