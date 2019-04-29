"use strict";
var medicsearchFLNetz = null;
var res = API.getScreenResolution();
var showInternetBrowser = false;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "medicsearch":
            medicsearchStartMenu();
            break;
        case "closemedicsearch_OpenMenu":
            API.destroyCefBrowser(medicsearchFLNetz);
            API.showCursor(false);
            API.setCanOpenChat(true);
            API.setHudVisible(true);
            showInternetBrowser = false;
            break;
    }
});

function medicsearchStartMenu() {
	if (showInternetBrowser === false) {
            medicsearchFLNetz = API.createCefBrowser(1480, 780, false);
            API.waitUntilCefBrowserInit(medicsearchFLNetz);
            API.setCefBrowserPosition(medicsearchFLNetz, 50, 50);
            API.loadPageCefBrowser(medicsearchFLNetz, "http://rpdeluxe.ml/medicsearch/medicsearch.html");
            API.showCursor(false);
            API.setCanOpenChat(true);
            API.setHudVisible(true);
            showInternetBrowser = true;
	}
}