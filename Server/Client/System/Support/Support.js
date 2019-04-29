"use strict";
var suInternet = null;
var res = API.getScreenResolution();
var showInternetBrowser = false;
var ctrlPressed = false;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Support_OpenMenu":
            SupportStartMenu();
            break;
        case "closeSupport_OpenMenu":
            API.destroyCefBrowser(suInternet);
            API.showCursor(false);
            API.setCanOpenChat(true);
            API.setHudVisible(true);
            showInternetBrowser = false;
            break;

    }
});

function SupportStartMenu() {
	if (showInternetBrowser === false) {
            suInternet = API.createCefBrowser(1480, 780, false);
            API.waitUntilCefBrowserInit(suInternet);
            API.setCefBrowserPosition(suInternet, 50, 50);
            API.loadPageCefBrowser(suInternet, "http://xlrstats.egc-com.eu/Support/support.html");
            API.showCursor(true);
            API.setCanOpenChat(false);
            API.setHudVisible(true);
            showInternetBrowser = true;
	}
}

API.onKeyUp.connect(function (sender, key) {
    if (key.KeyCode === Keys.ControlKey) {
            API.closeAllMenus();
    }
});