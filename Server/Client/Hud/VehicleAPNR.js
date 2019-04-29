// EVENTS
API.onUpdate.connect(Update);
API.onServerEventTrigger.connect(OnServerEvent);

// VARIABLES
var player = API.getLocalPlayer();
var anprPos = new Array; // Stores the front left & right position of the vehicle in relation to its size. (Length/Width)
var resX = API.getScreenResolutionMaintainRatio().Width;
var resY = API.getScreenResolutionMaintainRatio().Height;
var updateFrequency = 1000; // Every second.
var lastUpdateTickCount = 0;
var Pi = 3.14159265359;
var name = null;
var plate = null;
var kmh = null;

function OnServerEvent(eventName, args) {
    switch (eventName) {
        case "SEND_ANPR_POS":
            anprPos = new Array;
            anprPos[0] = args[0];
            anprPos[1] = args[1];
            break;
        case "DELETE_ANPR_POS":
            anprPos = null;
            name = null;
            plate = null;
            kmh = null;
        default:
            break;
    }
}

function Update() {
    if (!API.getHudVisible()) {
        return;
    }

    var inVeh = API.isPlayerInAnyVehicle(player);
    if (inVeh) {

        if (anprPos != null) {
            var veh = API.getPlayerVehicle(player);

            // UI for the ANPR
            API.drawRectangle(resX - 270, 370, 235, 10, 85, 171, 255, 150); // Blue border line
            API.drawRectangle(resX - 270, 240, 235, 130, 0, 0, 0, 125); // Background
            API.drawText("~b~Faction Life", resX - 152.5, 240, 0.5, 255, 255, 255, 255, 4, 1, false, true, 0); // Title
            API.drawText("~w~Fahrzeug: ", resX - 250, 275, 0.35, 255, 255, 255, 255, 4, 0, false, true, 0); // Model text
            API.drawText("~w~Nummernschild: ", resX - 250, 305, 0.35, 255, 255, 255, 255, 4, 0, false, true, 0); // Plate text
            API.drawText("~w~Geschwindigkeit: ", resX - 250, 335, 0.35, 255, 255, 255, 255, 4, 0, false, true, 0); // Speed text

            var currentTimeInMilliseconds = new Date().getTime();
            if (currentTimeInMilliseconds - lastUpdateTickCount > updateFrequency) {
                lastUpdateTickCount = currentTimeInMilliseconds;

                // Retrieve the position and rotation of the vehicle to use when calculating start/end positions.
                var vehRot = API.getEntityRotation(veh);
                var vehiclePos = API.getEntityPosition(veh);

                // Calculate the starting positions relative to the vehicles world position.
                var startPos = new Array;
                startPos[0] = Rotate(vehiclePos, vehRot.Z, (-anprPos[0] / 2), (anprPos[1] / 2));
                startPos[1] = Rotate(vehiclePos, vehRot.Z, (anprPos[0] / 2), (anprPos[1] / 2));

                // Calculate the ending positions relative to the starting positions. The z-value,
                // changes how far out the raycasting will shoot. In this case it will shoot 20 meters,
                // away from the vehicles front position(starting positions)
                var endPos = new Array;
                endPos[0] = Rotate(startPos[0], vehRot.Z, 0, 20);
                endPos[1] = Rotate(startPos[1], vehRot.Z, 0, 20);

                // Uncomment to visually see the rays being shot from the front.
                //API.drawLine(startPos[0], endPos[0], 255, 255, 255, 255);
                //API.drawLine(startPos[1], endPos[1], 255, 255, 255, 255);

                // Fire off the rays using 'API.createRaycast'.
                var hit = false;
                var rays = new Array;
                for (i = 0; i <= 1; i++) {
                    rays[i] = API.createRaycast(startPos[i], endPos[i], 10, veh);

                    if (rays[i].didHitAnything) {
                        var entityHit = rays[i].hitEntity;
                        if (entityHit != null) {
                            // Make sure we hit a vehicle.
                            if (API.getEntityType(entityHit) !== 1) { return; }

                            name = API.getVehicleDisplayName(API.getEntityModel(entityHit));
                            plate = API.getVehicleNumberPlate(entityHit);
                            var velocity = API.getEntityVelocity(entityHit);

                            var speed = Math.sqrt(
                                velocity.X * velocity.X +
                                velocity.Y * velocity.Y +
                                velocity.Z * velocity.Z
                            );
                            kmh = Math.round(speed * 3.6);
                            hit = true;
                        }
                    }
                }
                if (!hit) {
                    name = null;
                    plate = null;
                    kmh = null;
                }
            }
            if (name !== null && plate !== null && kmh !== null) {
                API.drawText("~y~" + name, resX - 50, 275, 0.35, 255, 255, 255, 255, 4, 2, false, true, 0); // Model name value text
                API.drawText("~y~" + plate, resX - 50, 305, 0.35, 255, 255, 255, 255, 4, 2, false, true, 0); // Plate value text
                API.drawText("~y~" + kmh + " km/h", resX - 50, 335, 0.35, 255, 255, 255, 255, 4, 2, false, true, 0); // Speed value text
            }
        }
    }
}

function DegreesToRadians(degree) {
    return degree * Pi / 180.0;
}

function Rotate(point, rotation, xOffset, yOffset) {
    var x = point.X + (xOffset * Math.cos(DegreesToRadians(rotation))) - (yOffset * Math.sin(DegreesToRadians(rotation)));
    var y = point.Y + (xOffset * Math.sin(DegreesToRadians(rotation))) + (yOffset * Math.cos(DegreesToRadians(rotation)));
    return new Vector3(x, y, point.Z);
}