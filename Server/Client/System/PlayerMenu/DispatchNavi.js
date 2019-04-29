API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "DispatchNavi":
            if ((args[0]) == "EMS") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "ACLS") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "Police") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "Fahrschule") { API.setWaypoint((args[1]), (args[2])); }
            break;
    }
});