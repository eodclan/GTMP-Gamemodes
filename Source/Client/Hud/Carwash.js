"use strict";

var safety = true;

API.onKeyDown.connect(function (sender, key) {
    if (key.KeyCode === Keys.J) {
        if (safety === true) {
            if (API.hasEntitySyncedData(API.getLocalPlayer(), "IsInCarwash") && API.getEntitySyncedData(API.getLocalPlayer(), "IsInCarwash") === true) {
		API.triggerServerEvent("waschen");
                safety = false;
            }
        }
    }
});

API.onKeyUp.connect(function (player, key) {
    if (key.KeyCode === Keys.J && !API.isChatOpen()) {
        safety = true;
    }
});