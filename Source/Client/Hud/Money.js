"use strict";
var res_X = API.getScreenResolutionMaintainRatio().Width;
API.onUpdate.connect(function () {
    if (API.hasEntitySyncedData(API.getLocalPlayer(), "hud")) {
        if (API.getEntitySyncedData(API.getLocalPlayer(), "hud")) {
            if (API.hasEntitySyncedData(API.getLocalPlayer(), "cash")) {
                API.drawText(API.getEntitySyncedData(API.getLocalPlayer(), "cash") + " $", res_X - 15, 63 + 40, 0.8, 255, 255, 255, 255, 4, 2, false, true, 0);
            }
        }
    }
});