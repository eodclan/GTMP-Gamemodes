"use strict";
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "mapmarker_getposition") {
        var player = API.getLocalPlayer();
        var coords = API.getWaypointPosition();
        coords.Z = 2000;
        API.setEntityPosition(player, coords);
	descent();
    }
});
function descent() {
    var player = API.getLocalPlayer();
    var pos = API.getEntityPosition(player);
    var ground = API.getGroundHeight(pos);
    pos.Z = pos.Z - 100;
    if (ground == 0) {
        API.setEntityPosition(player, pos);
        API.after(1, "descent");
    }
    else {
        pos.Z = ground;
        API.setEntityPosition(player, pos);
    }
}
