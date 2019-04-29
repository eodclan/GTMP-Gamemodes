"use strict";
var screenFX = "DeathFailMPDark";
var wastedTextDelay = 750;
var camEffect = 2;
var textTimerHandle = null;
var ver = "";

API.onUpdate.connect(function () {
    var res = API.getScreenResolutionMaintainRatio();
    API.drawText("RP-Deluxe " + ver, (res.Width / 2) + 668.4375, (res.Height / 2) - 540, 0.5, 255, 255, 255, 255, 4, 1, true, false, 0); 
});

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "setServerVersion":
		ver = args[0];
		break
	}
});

API.onKeyDown.connect(function (sender, key) {
    if (key.KeyCode == Keys.G) {
        API.triggerServerEvent("sirenToggle");
    }
    if (key.KeyCode == Keys.F2) {
        API.triggerServerEvent("ChangeVoiceRange");
    }
    if (key.KeyCode === Keys.ControlKey) {
        ctrlPressed = true;
    }	
});

function showWastedText() {
    API.playSoundFrontEnd("TextHit", "WastedSounds");
    API.showMissionPassedMessage("~r~Du brauchst ein Medic...");
}

function resetDeathEffect() {
    if (textTimerHandle != null) {
        API.stop(textTimerHandle);
        textTimerHandle = null;
    }

    API.stopScreenEffect(screenFX);
    API.callNative("_SET_CAM_EFFECT", 0);
}

API.onResourceStart.connect(function () {
    API.setShowWastedScreenOnDeath(false);
});

API.onPlayerRespawn.connect(function () {
    resetDeathEffect();
});

API.onPlayerDeath.connect(function (killer, weapon) {
    API.playScreenEffect(screenFX, 0, false);
    API.playSoundFrontEnd("MP_Flash", "WastedSounds");
    API.callNative("_SET_CAM_EFFECT", camEffect);

    if (textTimerHandle != null) API.stop(textTimerHandle);
    textTimerHandle = API.after(wastedTextDelay, "showWastedText");
});

API.onResourceStop.connect(function () {
    resetDeathEffect();
    API.setShowWastedScreenOnDeath(true);
});