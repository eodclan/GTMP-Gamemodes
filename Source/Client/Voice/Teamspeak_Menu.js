"use strict";
var ts3Internet = null;
var res = API.getScreenResolution();
var showInternetBrowser = false;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "Teamspeak_OpenMenu":
            TeamspeakStartMenu();
            break;
        case "closeTeamspeak_OpenMenu":
            API.destroyCefBrowser(ts3Internet);
            API.showCursor(false);
            API.setCanOpenChat(true);
            API.setHudVisible(true);
            showInternetBrowser = false;
            break;

    }
});

function TeamspeakStartMenu() {
	if (showInternetBrowser === false) {
            ts3Internet = API.createCefBrowser(1480, 780, false);
            API.waitUntilCefBrowserInit(ts3Internet);
            API.setCefBrowserPosition(ts3Internet, 50, 50);
            API.loadPageCefBrowser(ts3Internet, "https://rpdeluxe.ml/Teamspeak/teamspeak.html");
            API.showCursor(true);
            API.setCanOpenChat(false);
            API.setHudVisible(true);
            showInternetBrowser = true;
	}
}
