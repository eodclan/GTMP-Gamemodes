"use strict";
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "setRegisterUiVisible":
            RegisterUIOpen = args[0];
            if (args[0]) {
                API.setCanOpenChat(false);
                cursorIndex = 0;
                API.disableVehicleEnteringKeys(true);
                API.disableAlternativeMainMenuKey(true);
                API.callNative("0xA0EBB943C300E693", false);
                RegisterCamera = API.createCamera(args[1], new Vector3(0, 0, 0));
                API.pointCameraAtPosition(RegisterCamera, args[2]);
                API.setActiveCamera(RegisterCamera);
                minPasswordLength = args[3];
            }
            else {
                API.after(1000, "CloseRegisterStep1");
                API.after(1200, "CLoseRegisterStep2");
            }
            break;
    }
});
function CloseRegisterStep1() {
    API.setCanOpenChat(true);
    API.disableVehicleEnteringKeys(false);
    API.disableAlternativeMainMenuKey(false);
    API.setActiveCamera(null);
    API.setHudVisible(true);
}
function CLoseRegisterStep2() {
    API.fadeScreenIn(1000);
}
API.onUpdate.connect(function () {
    if (RegisterUIOpen) {
        DrawRegisterScreen();
        API.disableAllControlsThisFrame();
    }
});
var RegisterCamera = null;
var RegisterUIOpen = false;
var firstpassword = "";
var password = "";
var firstPasswordPlaceholder = "Passwort eingeben";
var passwordPlaceholder = "Passwort erneut eingeben";
var res = API.getScreenResolutionMaintainRatio();
var safe = (res.Height / 4);
var posX = safe * 2.5;
var posY = safe;
var Width = res.Width - (5 * safe);
var Height = 2 * safe;
var FirstPasswordY = posY + 170;
var PasswordY = posY + 290;
var defaultTextBoxBg = new Vector3(241, 241, 241);
var highlightTextBoxBg = new Vector3(183, 255, 96);
var defaultButtonBg = new Vector3(55, 55, 55);
var highlightButtonBg = new Vector3(122, 122, 122);
var cursorIndex = 0;
var ErrorMessage = "";
var minPasswordLength = 4;
function DrawRegisterScreen() {
    API.drawRectangle(posX, posY, Width, Height, 30, 30, 30, 240);
    API.drawText("RP-Deluxe", posX + (Width / 2), posY + 20, 2, 255, 255, 255, 255, 1, 1, false, false, 500);
    API.drawText("Passwort eingeben", posX + 50, FirstPasswordY, 0.7, 255, 255, 255, 255, 4, 0, false, false, 500);
    if (cursorIndex == 0) {
        API.drawRectangle(posX + 50, FirstPasswordY + 50, Width - 100, 50, highlightTextBoxBg.X, highlightTextBoxBg.Y, highlightTextBoxBg.Z, 255);
    }
    else {
        API.drawRectangle(posX + 50, FirstPasswordY + 50, Width - 100, 50, defaultTextBoxBg.X, defaultTextBoxBg.Y, defaultTextBoxBg.Z, 255);
    }
    if (firstpassword.length == 0) {
        API.drawText(firstPasswordPlaceholder, posX + 60, FirstPasswordY + 50, 0.7, 50, 50, 50, 255, 4, 0, false, false, Width - 120);
    }
    else {
        API.drawText(firstpassword.replace(/./g, "*"), posX + 60, FirstPasswordY + 60, 0.7, 50, 50, 50, 255, 4, 0, false, false, Width - 120);
    }
    API.drawText("Passwort erneut eingeben", posX + 50, PasswordY, 0.7, 255, 255, 255, 255, 4, 0, false, false, 500);
    if (cursorIndex == 1) {
        API.drawRectangle(posX + 50, PasswordY + 50, Width - 100, 50, highlightTextBoxBg.X, highlightTextBoxBg.Y, highlightTextBoxBg.Z, 255);
    }
    else {
        API.drawRectangle(posX + 50, PasswordY + 50, Width - 100, 50, defaultTextBoxBg.X, defaultTextBoxBg.Y, defaultTextBoxBg.Z, 255);
    }
    if (password.length == 0) {
        API.drawText(passwordPlaceholder, posX + 60, PasswordY + 50, 0.7, 50, 50, 50, 255, 4, 0, false, false, Width - 120);
    }
    else {
        API.drawText(password.replace(/./g, "*"), posX + 60, PasswordY + 60, 0.7, 50, 50, 50, 255, 4, 0, false, false, Width - 120);
    }
    if (cursorIndex == 2) {
        API.drawRectangle(posX + 50, PasswordY + 130, Width - 100, 65, highlightButtonBg.X, highlightButtonBg.Y, highlightButtonBg.Z, 240);
    }
    else {
        API.drawRectangle(posX + 50, PasswordY + 130, Width - 100, 65, defaultButtonBg.X, defaultButtonBg.Y, defaultButtonBg.Z, 240);
    }
    API.drawText("Register", posX + (Width / 2), PasswordY + 135, 0.7, 255, 255, 255, 255, 4, 1, false, false, 500);
    if (ErrorMessage != "") {
        API.drawText("ERROR: " + ErrorMessage, posX + 50, PasswordY + 200, 0.5, 255, 0, 0, 255, 4, 0, false, false, 500);
    }
}
API.onKeyDown.connect(function (sender, e) {
    if (RegisterUIOpen) {
        switch (e.KeyCode) {
            case Keys.Tab:
                switch (cursorIndex) {
                    case 0:
                        cursorIndex = 1;
                        break;
                    case 1:
                        cursorIndex = 2;
                        break;
                    case 2:
                        cursorIndex = 0;
                        break;
                }
                break;
            case Keys.Up:
                if (cursorIndex == 0) {
                    cursorIndex = 2;
                }
                else {
                    cursorIndex--;
                }
                break;
            case Keys.Down:
                if (cursorIndex == 2) {
                    cursorIndex = 0;
                }
                else {
                    cursorIndex++;
                }
                break;
            case Keys.Enter:
                if (cursorIndex == 2) {
                    RegisterButtonTriggered();
                }
                break;
            case Keys.Back:
                switch (cursorIndex) {
                    case 0:
                        if (firstpassword.length != 0) {
                            firstpassword = firstpassword.substring(0, firstpassword.length - 1);
                        }
                        if (firstpassword.length < minPasswordLength) {
                            ErrorMessage = "Das Passwort muss mindestens " + minPasswordLength + " Zeichen lang sein!";
                        }
                        else {
                            ErrorMessage = "";
                        }
                        break;
                    case 1:
                        if (password.length != 0) {
                            password = password.substring(0, password.length - 1);
                        }
                        if (password == firstpassword) {
                            ErrorMessage = "";
                        }
                        else {
                            ErrorMessage = "Passwords doesn't match!";
                        }
                        break;
                }
                break;
            default:
                switch (cursorIndex) {
                    case 0:
                        firstpassword += API.getCharFromKey(e.KeyValue, e.Shift, e.Control, e.Alt);
                        if (firstpassword.length < minPasswordLength) {
                            ErrorMessage = "Das Passwort muss mindestens " + minPasswordLength + " Zeichen lang sein!";
                        }
                        else {
                            ErrorMessage = "";
                        }
                        break;
                    case 1:
                        password += API.getCharFromKey(e.KeyValue, e.Shift, e.Control, e.Alt);
                        if (password == firstpassword) {
                            ErrorMessage = "";
                        }
                        else {
                            ErrorMessage = "Passwörter stimmen nicht überein!";
                        }
                        break;
                }
                break;
        }
    }
});
function RegisterButtonTriggered() {
    if (firstpassword == password) {
        if (firstpassword.length >= minPasswordLength) {
            RegisterUIOpen = false;
            API.setCanOpenChat(true);
            API.triggerServerEvent("account_registerButtonPressed", firstpassword, password);
            ErrorMessage = "";
        }
        else {
            ErrorMessage = "Das Passwort muss mindestens " + minPasswordLength + " Zeichen lang sein!";
        }
    }
    else {
        ErrorMessage = "Passwörter stimmen nicht überein!";
    }
}
