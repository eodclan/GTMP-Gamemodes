"use strict";
var res = API.getScreenResolutionMaintainRatio();
var mapMarginLeft = API.getScreenResolutionMaintainRatio().Width / 64;
var mapMarginBottom = API.getScreenResolutionMaintainRatio().Height / 60;
var mapWidth = API.getScreenResolutionMaintainRatio().Width / 7.11;
var mapHeight = API.getScreenResolutionMaintainRatio().Height / 5.71;
var resX = mapMarginLeft;
//var resY = API.getScreenResolutionMaintainRatio().Height - mapHeight - mapMarginBottom - mapHeight - mapMarginBottom;
var resY = mapMarginBottom;
var resXCompass = resX;
var BarPosX = Math.round(mapMarginLeft);
var BarPosY =  API.getScreenResolutionMaintainRatio().Height / 2 - 90;
var BarBGHeight = 42;
var BarBGWidth = 42;
var BarHeight = 36;
var BarWidth = 36;
var Hunger = 20;
var Teamspeak = 20;
var Thirst = 20;
var fadedIn = false;
var HUDTicks = 0;
var isVehicleDriver = false;
var isInVehicle = false;
var BlinkTicks = 0;
var BlinkStatus = false;
API.onUpdate.connect(function () {
    DrawPlayerBars();
	
});
API.onPlayerEnterVehicle.connect(function (veh, targetSeat) {
    if (targetSeat == -1) {
        isVehicleDriver = true;
    }
    isInVehicle = true;
});
API.onPlayerExitVehicle.connect(function (veh, fromSeat) {
    isVehicleDriver = false;
    isInVehicle = false;
});
function DrawPlayerBars() {
    if (API.hasEntitySyncedData(API.getLocalPlayer(), "hud")) {
        if (API.getEntitySyncedData(API.getLocalPlayer(), "hud")) {
            if (HUDTicks >= 100) {
                fadedIn = API.isScreenFadedIn();
                HUDTicks = 0;
                if (isInVehicle) {
                    if (API.getPlayerVehicleSeat(API.getLocalPlayer()) == -1) {
                        isVehicleDriver = true;
                    }
                    else {
                        isVehicleDriver = false;
                    }
                }
            }
            else {
                HUDTicks++;
            }
            if (BlinkTicks >= 25) {
                if (BlinkStatus) {
                    BlinkStatus = false;
                }
                else {
                    BlinkStatus = true;
                }
                BlinkTicks = 0;
            }
            else {
                BlinkTicks++;
            }
			
            if (API.hasEntitySyncedData(API.getLocalPlayer(), "hunger")) {
                Hunger = API.getEntitySyncedData(API.getLocalPlayer(), "hunger");
                if (Hunger > 100) {
                    Hunger = 100;
                }
                if (Hunger < 0) {
                    Hunger = 0;
                }
                API.drawRectangle(BarPosX, BarPosY, BarBGWidth, BarBGHeight, 0, 0, 0, 100);
                API.drawRectangle(BarPosX + 3, BarPosY + ((BarHeight / 100) * (100 - Hunger)) + 3, BarWidth, (BarHeight / 100) * Hunger, 255, 202, 96, 150);
                if (fadedIn) {
                   API.dxDrawTexture("Client/Hud/food.png", new Point(BarPosX + 8, BarPosY + 5), new Size(26, 26));
                }
            }
            if (API.hasEntitySyncedData(API.getLocalPlayer(), "thirst")) {
                Thirst = API.getEntitySyncedData(API.getLocalPlayer(), "thirst");
                if (Thirst > 100) {
                    Thirst = 100;
                }
                if (Thirst < 0) {
                    Thirst = 0;
                }
                API.drawRectangle(BarPosX, BarPosY + 46, BarBGWidth, BarBGHeight, 0, 0, 0, 100);
                API.drawRectangle(BarPosX + 3, BarPosY + 46 + ((BarHeight / 100) * (100 - Thirst)) + 3, BarWidth, (BarHeight / 100) * Thirst, 0, 222, 255, 150);
                if (fadedIn) {
                    API.dxDrawTexture("Client/Hud/drink.png", new Point(BarPosX + 8, BarPosY + 51), new Size(26, 26));
                }
            }
            if (API.hasEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE")) {
		Teamspeak = API.getEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE");
                if (Teamspeak > 100) {
                    Teamspeak = 100;
                }
                if (Teamspeak < 0) {
                    Teamspeak = 0;
                }
                API.drawRectangle(BarPosX , BarPosY  + 92, BarBGWidth, BarBGHeight, 0, 0, 0, 100);
                API.drawRectangle(BarPosX + 3, BarPosY + 92 + ((BarHeight / 100) * (100 - Teamspeak)) + 3, BarWidth, (BarHeight / 100) * Teamspeak, 145, 249, 152, 150);
                if (fadedIn) {
                   if (API.getEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE") == "Weit") {
                    Teamspeak = 100;
                    API.dxDrawTexture("Client/Hud/img/voice3.png", new Point(BarPosX + 8, BarPosY + 97), new Size(26, 26));
                }
                if (API.getEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE") == "Normal") {
                    Teamspeak = 50;
                    API.dxDrawTexture("Client/Hud/img/voice2.png", new Point(BarPosX + 8, BarPosY + 97), new Size(26, 26));
                }
                if (API.getEntitySyncedData(API.getLocalPlayer(), "VOICE_RANGE") == "Kurz") {
                    Teamspeak = 0;
                    API.dxDrawTexture("Client/Hud/img/voice1.png", new Point(BarPosX + 8, BarPosY + 97), new Size(26, 26));
                }

                }
            }

            if (isVehicleDriver) {
                var veh = API.getPlayerVehicle(API.getLocalPlayer());
                var vhealth = API.getVehicleHealth(veh);
                if (vhealth > 1000) {
                    vhealth = 1000;
                }
                if (vhealth < 0) {
                    vhealth = 0;
                }
                if (API.hasEntitySyncedData(veh, "fuel") && API.hasEntitySyncedData(veh, "maxfuel")) {
                    var vfuel = API.getEntitySyncedData(veh, "fuel");
                    var vmaxfuel = API.getEntitySyncedData(veh, "maxfuel");
                    if (vfuel > vmaxfuel) {
                        vfuel = vmaxfuel;
                    }
                    if (vfuel < 0) {
                        vfuel = 0;
                    }
                    API.drawRectangle(BarPosX, BarPosY + 138, BarBGWidth, BarBGHeight, 0, 0, 0, 100);
                    if (vfuel <= 5) {
                        if (BlinkStatus) {
                            API.drawRectangle(BarPosX + 3 , BarPosY + 138 + ((BarHeight / vmaxfuel) * (vmaxfuel - vfuel)) + 3, BarWidth, (BarHeight / vmaxfuel) * vfuel, 255, 0, 0, 150);
                        }
                        else {
                            API.drawRectangle(BarPosX + 3, BarPosY + 138 + ((BarHeight / vmaxfuel) * (vmaxfuel - vfuel)) + 3, BarWidth, (BarHeight / vmaxfuel) * vfuel, 255, 243, 110, 150);
                        }
                    }
                    else {
                        API.drawRectangle(BarPosX + 3 , BarPosY + 138 + ((BarHeight / vmaxfuel) * (vmaxfuel - vfuel)) + 3, BarWidth, (BarHeight / vmaxfuel) * vfuel, 255, 243, 110, 150);
                    }
                    if (fadedIn) {
                        API.dxDrawTexture("Client/Hud/fluid.png", new Point(BarPosX + 8, BarPosY + 143), new Size(26, 26));
                    }
                    //resXCompass = resX + 110;
                }
                else {
                   // resXCompass = resX + 88;
                }
            }
            else {
                //resXCompass = resX + 66;
            }
            var currentTimeInMilliseconds = new Date().getTime();
            if (currentTimeInMilliseconds - lastUpdateTickCount > updateTimeoutInMilliseconds) {
                lastUpdateTickCount = currentTimeInMilliseconds;
                updateValues();
            }
            //API.drawText(text, resXCompass, resY + 127, 1.05, 255, 255, 255, 200, 2, 1, false, true, 0);
            //API.drawText("|", resXCompass + 35, resY + 130, 1, 255, 255, 255, 200, 4, 1, false, true, 0);
            API.drawText(zone, resXCompass, resY, 0.5, 255, 255, 255, 200, 4, 0, false, true, 0);
            API.drawText(street, resXCompass, resY + 27, 0.4, 255, 255, 255, 200, 4, 0, false, true, 0);
        }
    }
}
var text;
var zone;
var street;
var updateTimeoutInMilliseconds = 500;
var lastUpdateTickCount = 0;
function updateDirectionText() {
    var cameraDirection = API.getGameplayCamDir();
    if (0.3 < cameraDirection.X && 0.3 < cameraDirection.Y) {
        text = "NE";
    }
    else if (cameraDirection.X < -0.3 && 0.3 < cameraDirection.Y) {
        text = "NW";
    }
    else if (0.3 < cameraDirection.X && cameraDirection.Y < -0.3) {
        text = "SE";
    }
    else if (cameraDirection.X < -0.3 && cameraDirection.Y < -0.3) {
        text = "SW";
    }
    else if (-0.3 < cameraDirection.X && cameraDirection.X < 0.3 && cameraDirection.Y < -0.3) {
        text = "S";
    }
    else if (cameraDirection.X < -0.3 && -0.3 < cameraDirection.Y && cameraDirection.Y < 0.3) {
        text = "W";
    }
    else if (0.3 < cameraDirection.X && -0.3 < cameraDirection.Y && cameraDirection.Y < 0.3) {
        text = "E";
    }
    else if (-0.3 < cameraDirection.X && cameraDirection.X < 0.3 && cameraDirection.Y > 0.3) {
        text = "N";
    }
}
function updateValues() {
    var playerPosition = API.getEntityPosition(API.getLocalPlayer());
    zone = API.getZoneNameLabel(playerPosition);
    street = API.getStreetName(playerPosition);
    updateDirectionText();
}
