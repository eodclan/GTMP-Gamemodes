
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "PoliceNavi":

            if ((args[0]) == "S1") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S2") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S3") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S4") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S5") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S6") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S7") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S8") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S9") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S10") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S11") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S12") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S13") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S14") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S15") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S16") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S17") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S18") { API.setWaypoint((args[1]), (args[2])); }
            if ((args[0]) == "S19") { API.setWaypoint((args[1]), (args[2])); }

            break;
    }

});