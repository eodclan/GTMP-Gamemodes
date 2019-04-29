var desbus;
var marker;

API.onKeyDown.connect(function (sender, e) {
	if (e.KeyCode === Keys.Y) {

		API.triggerServerEvent("StartEisenMine");

	}	
		
	if (e.KeyCode === Keys.Q) {

		API.triggerServerEvent("EisenMineComplete");

	}
});

API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "") {

        API.setWaypoint(args[0], args[1]);
      
    }

    if (eventName == "nextEisenMine") {
       API.removeWaypoint();
       marker = API.createMarker(1, args[0], new Vector3(), new Vector3(), new Vector3(1, 1, 1), 255, 0, 0, 255);
       API.setWaypoint(args[1], args[2]);

        
    }

});