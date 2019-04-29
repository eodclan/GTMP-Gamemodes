var desgoldblip;
var goldmarker;

API.onKeyDown.connect(function (sender, key) {
    if (key.KeyCode == Keys.Y) {
        API.triggerServerEvent("StartGoldMine");
    }

    if (key.KeyCode == Keys.Y) {
        API.triggerServerEvent("GoldMineComplete");
    }
});

API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "waypointGoldMine") {
        API.setWaypoint(args[0], args[1]);
    }

    if (eventName == "nextGoldMine") {
        var textLabel = API.createTextLabel("Drücken Sie Y, um die Aufgabe abzuschließen", args[0], 1, 1);

        if (desgoldblip != null)
        {
            API.setBlipPosition(desgoldblip, args[0]);
            API.deleteEntity(goldmarker);
        }
        else
        {
            desgoldblip = API.createBlip(args[0]); 
        }
       
        goldmarker = API.createMarker(27, args[0], new Vector3(), new Vector3(), new Vector3(2, 2, 2), 255, 0, 0, 255);
        API.attachEntity(textLabel, goldmarker, "0", new Vector3(), new Vector3());
      
	API.setWaypoint(args[1], args[2]);
    }
});