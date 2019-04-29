"use strict";
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "open_CharCreator":
            open_CharCreator = true;
            LeftButtonText = args[0];
            RightButtonText = args[1];
			TitleText = args[2];
			MessageText = args[3];
            BlockControls = args[4];
            if (BlockControls) {
                API.disableVehicleEnteringKeys(true);
                API.disableAlternativeMainMenuKey(true);
            }
            break;
        case "closeDialog":
            open_CharCreator = false;
            LeftButtonText = "";
            RightButtonText = "";
            TitleText = "";
            MessageText = "";
            if (BlockControls) {
                API.disableVehicleEnteringKeys(false);
                API.disableAlternativeMainMenuKey(false);
            }
            break;
    }
});
API.onUpdate.connect(function () {
    if (open_CharCreator) {
        DrawDialog();
        if (BlockControls) {
            API.disableAllControlsThisFrame();
        }
    }
});
var open_CharCreator = false;
var BlockControls = false;
var LeftButtonText = "OK";
var RightButtonText = "";
var TitleText = "Character Information";
var MessageText = "Die Charakter Erstellung findest du in unseren RP-Deluxe Network!";
var res = API.getScreenResolutionMaintainRatio();
var posX = (res.Width / 2) - 250;
var posY = (res.Height / 2) - 100;
var buttonIndex = 0;
function DrawDialog() {
    API.drawRectangle(posX, posY, 500, 200, 10, 10, 10, 220);
    API.drawRectangle(posX, posY, 500, 40, 0, 0, 0, 200);
    API.drawText(TitleText, posX + 5, posY, 0.5, 255, 255, 255, 255, 6, 0, false, false, 490);
    API.drawText(MessageText, posX + 250, posY + 40, 0.4, 245, 245, 245, 255, 6, 1, false, false, 490);
    if (RightButtonText != "") {
        if (buttonIndex == 0) {
            API.drawRectangle(posX + 10, posY + 150, 130, 40, 255, 255, 255, 200);
            API.drawRectangle(posX + 360, posY + 150, 130, 40, 0, 0, 0, 200);
            API.drawText(LeftButtonText, posX + 75, posY + 150, 0.5, 0, 0, 0, 255, 6, 1, false, false, 120);
            API.drawText(RightButtonText, posX + 425, posY + 150, 0.5, 255, 255, 255, 255, 6, 1, false, false, 120);
        }
        else {
            API.drawRectangle(posX + 10, posY + 150, 130, 40, 0, 0, 0, 200);
            API.drawRectangle(posX + 360, posY + 150, 130, 40, 255, 255, 255, 200);
            API.drawText(LeftButtonText, posX + 75, posY + 150, 0.5, 255, 255, 255, 255, 6, 1, false, false, 120);
            API.drawText(RightButtonText, posX + 425, posY + 150, 0.5, 0, 0, 0, 255, 6, 1, false, false, 120);
        }
    }
    else {
        API.drawRectangle(posX + 185, posY + 150, 130, 40, 255, 255, 255, 200);
        API.drawText(LeftButtonText, posX + 250, posY + 150, 0.5, 0, 0, 0, 255, 6, 1, false, false, 120);
    }
}
var keyBlock = false;
API.onKeyDown.connect(function (sender, e) {
    if (open_CharCreator) {
        if (!keyBlock) {
            switch (e.KeyCode) {
                case Keys.Left:
                    if (RightButtonText != "") {
                        if (buttonIndex == 0) {
                            buttonIndex = 1;
                        }
                        else {
                            buttonIndex = 0;
                        }
                        keyBlock = true;
                    }
                    break;
                case Keys.Right:
                    if (RightButtonText != "") {
                        if (buttonIndex == 1) {
                            buttonIndex = 0;
                        }
                        else {
                            buttonIndex = 1;
                        }
                        keyBlock = true;
                    }
                    break;
                case Keys.Enter:
                    SendSelection();
                    break;
            }
        }
    }
});
API.onKeyUp.connect(function (sender, e) {
    keyBlock = false;
});
function SendSelection() {
    open_CharCreator = false;
    var LeftButtonText = "";
    var RightButtonText = "";
    var TitleText = "";
    var MessageText = "";
    API.triggerServerEvent("dialogSelected", buttonIndex);
    buttonIndex = 0;
}
