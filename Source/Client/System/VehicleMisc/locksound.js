"use strict";

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PlayUnlockVehicleSound":
		// API.sendNotification('Event fired');
            var posX = args[0].x;
			var posY = args[0].y;
			var posZ = args[0].z;
			API.startMusic("Client/System/VehicleMisc/unlocksound.mp3", false);
           
			
			break;
		case "PlayLockVehicleSound":
		 //API.sendNotification('Event fired');
            var posX = args[0].x;
			var posY = args[0].y;
			var posZ = args[0].z;
			API.startMusic("Client/System/VehicleMisc/locksound.mp3", false);
         
			break;
    }
});

