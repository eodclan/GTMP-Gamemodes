/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "mapmarker_getposition") {
        var coords = API.getWaypointPosition();
        API.triggerServerEvent("mapmarker_teleport", coords);
    }
});