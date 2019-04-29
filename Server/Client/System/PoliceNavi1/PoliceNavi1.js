
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PoliceNavi1":

            if ((args[0]) == "S1") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S2") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S3") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S4") { API.setWaypoint((args[1]), (args[2])); }


            break;
    }

});