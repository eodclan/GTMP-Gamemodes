﻿var desfishblip;
var marker;

API.onKeyDown.connect(function (sender, e) {
	if (e.KeyCode === Keys.Y) {

		API.triggerServerEvent("StartFisherMission");

	}	
		
	if (e.KeyCode === Keys.Q) {

		API.triggerServerEvent("FisherComplete");

	}
});

API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "") {

        API.setWaypoint(args[0], args[1]);
      
    }


    if (eventName == "nextfisherJob") {

       

        if (desfishblip != null)
        {
          
            API.setBlipPosition(desfishblip, args[0]);
            API.deleteEntity(marker);
        }
        else
        {
            desfishblip = API.createBlip(args[0]);
            
        }
      
       marker = API.createMarker(1, args[0], new Vector3(), new Vector3(), new Vector3(1, 1, 1), 255, 0, 0, 255);
       API.setWaypoint(args[1], args[2]);

        
    }

});