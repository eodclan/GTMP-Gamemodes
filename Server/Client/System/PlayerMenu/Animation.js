"use strict";
/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />
var startAnimation = false;
API.onKeyDown.connect(function (player, args) {
    if (API.isChatOpen())
        return;
    if (!API.isCursorShown() && !API.isPlayerInAnyVehicle(API.getLocalPlayer())) {
        if (API.hasEntitySyncedData(API.getLocalPlayer(), "CUFFED") && API.getEntitySyncedData(API.getLocalPlayer(), "CUFFED") === true && startAnimation === false || API.hasEntitySyncedData(API.getLocalPlayer(), "CALL_IS_STARTED") && API.getEntitySyncedData(API.getLocalPlayer(), "CALL_IS_STARTED").toString() === "1" || API.hasEntitySyncedData(API.getLocalPlayer(), "IS_DEATH") && API.getEntitySyncedData(API.getLocalPlayer(), "IS_DEATH") === true) {
            return;
        }
        if ((args.KeyCode === Keys.NumPad0 || args.KeyCode === Keys.NumPad1 || args.KeyCode === Keys.NumPad2 || args.KeyCode === Keys.NumPad3 || args.KeyCode === Keys.NumPad4 || args.KeyCode === Keys.NumPad5 || args.KeyCode === Keys.NumPad6 || args.KeyCode === Keys.NumPad7 || args.KeyCode === Keys.NumPad8 || args.KeyCode === Keys.NumPad9) && !API.isChatOpen() && startAnimation === true) {
            API.triggerServerEvent("StopAnimation");
            startAnimation = false;
        }
        else if (args.KeyCode === Keys.NumPad0 && startAnimation === false) {
            API.triggerServerEvent("Num0", "Num0");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad1 && startAnimation === false) {
            API.triggerServerEvent("Num1", "Num1");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad2 && startAnimation === false) {
            API.triggerServerEvent("Num2", "Num2");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad3 && startAnimation === false) {
            API.triggerServerEvent("Num3", "Num3");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad4 && startAnimation === false) {
            API.triggerServerEvent("Num4", "Num4");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad5 && startAnimation === false) {
            API.triggerServerEvent("Num5", "Num5");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad6 && startAnimation === false) {
            API.triggerServerEvent("Num6", "Num6");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad7 && startAnimation === false) {
            API.triggerServerEvent("Num7", "Num7");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad8 && startAnimation === false) {
            API.triggerServerEvent("Num8", "Num8");
            startAnimation = true;
        }
        else if (args.KeyCode === Keys.NumPad9 && startAnimation === false) {
            API.triggerServerEvent("Num9", "Num9");
            startAnimation = true;
        }
    }
});
