"use strict";
/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />
var resolution = API.getScreenResolutionMaintainRatio();
var resX = resolution.Width;
var resY = resolution.Height;
var isTaxiFare = false;
var isCustomer = false;
var currentToPay = 0;
var currentFare = 1;
var notToDisplay = 0;
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "update_taxi_fare") {
        currentFare = args[0];
        notToDisplay = args[1];
    }
});
API.onUpdate.connect(() => {
    const localPlayer = API.getLocalPlayer();
    if (!API.isPlayerInAnyVehicle(localPlayer)) {
        return;
    }
    var veh = API.getPlayerVehicle(localPlayer);
    if (API.getVehicleClass(API.getEntityModel(veh)) === 13) {
        return;
    }
    var rpm = API.getVehicleRPM(veh);
    var isDriver = API.getPlayerVehicleSeat(localPlayer);
    if (isDriver === -1) {
        if (API.isPlayerInAnyVehicle(localPlayer)) {
            // Geschwindigkeit
            var pointX = new Point(parseInt(resX - 300 + ""), parseInt(resY - 350 + ""));
            var pointSize = new Size(250, 250);
            var velocity = API.getEntityVelocity(veh);
            var speed = Math.sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y + velocity.Z * velocity.Z) * 4.45;
            if (speed > 250) {
                speed = 255;
                var speed2 = Math.sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y + velocity.Z * velocity.Z) * 3.6;
                API.drawText(`~r~${Math.round(speed2)}MPH`, resX - 5, resY - 300, 0.4, 255, 255, 255, 255, 4, 2, false, true, 0);
            }
            API.dxDrawTexture("Client/HUD/speedo.png", pointX, pointSize);
            API.dxDrawTexture("Client/HUD/line.png", pointX, pointSize, speed - 5);
        }
    }
    if (isDriver === 0 || isDriver === 1 || isDriver === 2 || isDriver === 4 || isDriver === 5 || isDriver === 6 || isDriver === 7
        || isDriver === 8 || isDriver === 9 || isDriver === 10 || isDriver === 11 || isDriver === 12 || isDriver === 13 || isDriver === 14) {
        if (API.isPlayerInAnyVehicle(localPlayer)) {
            // Geschwindigkeit
            var pointX = new Point(parseInt(resX - 300 + ""), parseInt(resY - 350 + ""));
            var pointSize = new Size(250, 250);
            var velocity = API.getEntityVelocity(veh);
            var speed = Math.sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y + velocity.Z * velocity.Z) * 4.45;
            if (speed > 250) {
                speed = 255;
                var speed2 = Math.sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y + velocity.Z * velocity.Z) * 3.6;
                API.drawText(`~r~${Math.round(speed2)}MPH`, resX - 5, resY - 300, 0.4, 255, 255, 255, 255, 4, 2, false, true, 0);
            }
            API.dxDrawTexture("Client/HUD/speedo.png", pointX, pointSize);
            API.dxDrawTexture("Client/HUD/line.png", pointX, pointSize, speed - 5);
        }
    }
});
