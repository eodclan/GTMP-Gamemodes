var desdiamantblip;
var diamantmarker;

API.onKeyDown.connect(function (sender, key) {
    if (key.KeyCode == Keys.Y) {
        API.triggerServerEvent("StartDiamantMine");
    }

    if (key.KeyCode == Keys.Y) {
        API.triggerServerEvent("DiamantMineComplete");
    }
});

API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "waypointDiamantMine") {
        API.setWaypoint(args[0], args[1]);
    }

    if (eventName == "nextDiamantMine") {
        var textLabel = API.createTextLabel("Drücken Sie Y, um die Aufgabe abzuschließen", args[0], 1, 1);

        if (desdiamantblip != null)
        {
            API.setBlipPosition(desdiamantblip, args[0]);
            API.deleteEntity(diamantmarker);
        }
        else
        {
            desdiamantblip = API.createBlip(args[0]); 
        }
       
        diamantmarker = API.createMarker(27, args[0], new Vector3(), new Vector3(), new Vector3(2, 2, 2), 255, 0, 0, 255);
        API.attachEntity(textLabel, diamantmarker, "0", new Vector3(), new Vector3());
      
	API.setWaypoint(args[1], args[2]);
    }
});