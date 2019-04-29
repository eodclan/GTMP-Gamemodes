//scooby doo snacks

API.onKeyDown.connect(function(sender, key) {
    if (!API.isChatOpen()) {
        if (key.KeyCode === Keys.L) {
            if (API.isPlayerInAnyVehicle(API.getLocalPlayer())) {
                API.triggerServerEvent("vehicle_special_action");
            }
        }
        else if (key.KeyCode === Keys.Left) {
            if (API.isPlayerInAnyVehicle(API.getLocalPlayer())) {
                if (API.getPlayerVehicleSeat(API.getLocalPlayer()) === -1) {
                    API.triggerServerEvent("vehicle_indicator_left");
                }
            }
        }
        else if (key.KeyCode === Keys.Right) {
            if (API.isPlayerInAnyVehicle(API.getLocalPlayer())) {
                if (API.getPlayerVehicleSeat(API.getLocalPlayer()) === -1) {
                    API.triggerServerEvent("vehicle_indicator_right");
                }
            }
        }
        else if(key.KeyCode === Keys.G)
        {
            trigger_check_locked = true;
            //trigger_door_client_fix = true;
        }
        /*else if(key.KeyCode === Keys.F)
        {
            trigger_door_client_fix = true;
        }*/
    }
});

API.onKeyUp.connect(function(sender, key) {
    if (!API.isChatOpen()) {
        if (key.KeyCode === Keys.I) {
            var local_plr = API.getLocalPlayer()
            if (API.isPlayerInAnyVehicle(local_plr) === true && API.getPlayerVehicleSeat(local_plr) === -1) {
                var veh = API.getPlayerVehicle(local_plr);
                if (API.returnNative("GET_ENTITY_SPEED", 7, veh) * 2.236936 < 25) {
                    API.triggerServerEvent("trigger_convertible_roof", veh);
                    storage_top_car = veh;
                    trigger_top_check = true;
                }
            }
        }
    }
});

var trigger_check_locked = false;
var trigger_door_client_fix = false;

API.onEntityStreamIn.connect(function(ent, entType) {
    if (entType === 1) {
        API.triggerServerEvent("sync_vehicle_data", ent);
    }
});

function checkForRoofTimeout() {
    if (trigger_top_check && storage_top_car != null) {
        if (API.returnNative("IS_VEHICLE_A_CONVERTIBLE", 8, storage_top_car, false)) {
            var roof_status = API.returnNative("GET_CONVERTIBLE_ROOF_STATE", 0, storage_top_car);
            if (roof_status === 0 || roof_status === 2) {
                checkForTimeout = false;
                trigger_top_check = false;
                storage_top_car = null;
            }
        }

    }
}

var trigger_top_check = false;
var storage_top_car = null;
var trigger_top_debouncer_up = true;
var trigger_top_debouncer_down = true;
var oldTime = 0;
var oldTime2 = 0;
var checkForKey = false;
var checkForTimeout = false;

//Set vehicle out of control
API.onUpdate.connect(function(){
    var local_plr = API.getLocalPlayer();
    if (trigger_top_check && storage_top_car != null) {
        if (API.doesEntityExist(storage_top_car)) {
            var roof_status = API.returnNative("GET_CONVERTIBLE_ROOF_STATE", 0, storage_top_car);
            if (roof_status === 1 && trigger_top_debouncer_up) {
                trigger_top_debouncer_up = false;
                trigger_top_debouncer_down = true;
                API.triggerServerEvent("lower_convertible_roof", storage_top_car);
                checkForTimeout = true;
                oldTime2 = API.getGameTime();
            } else if (roof_status === 3 && trigger_top_debouncer_down) {
                trigger_top_debouncer_down = false;
                trigger_top_debouncer_up = true;
                API.triggerServerEvent("raise_convertible_roof", storage_top_car);
                checkForTimeout = true;
                oldTime2 = API.getGameTime();
            }
        }
        else {
            trigger_top_check = false;
            storage_top_car = null;
            trigger_top_debouncer_up = true;
            trigger_top_debouncer_down = true;
        }
    }

    if (API.isPlayerInAnyVehicle(local_plr) === true && API.getPlayerVehicleSeat(local_plr) === -1) {
        var veh = API.getPlayerVehicle(local_plr);
        var vehicleClass = API.getVehicleClass(API.getEntityModel(veh));
        if (vehicleClass === 0 ||
            vehicleClass === 1 ||
            vehicleClass === 2 ||
            vehicleClass === 3 ||
            vehicleClass === 4 ||
            vehicleClass === 5 ||
            vehicleClass === 6 ||
            vehicleClass === 7 ||
            vehicleClass === 9 ||
            vehicleClass === 10 ||
            vehicleClass === 11 ||
            vehicleClass === 12 ||
            vehicleClass === 17 ||
            vehicleClass === 18 ||
            vehicleClass === 19 ||
            vehicleClass === 20) {
            //var onroof_stuck = API.returnNative("IS_VEHICLE_STUCK_ON_ROOF", 8, API.getPlayerVehicle(local_plr));
            var veh_allwheels = API.returnNative("IS_VEHICLE_ON_ALL_WHEELS", 8, API.getPlayerVehicle(local_plr));
            if (veh_allwheels === false) {
                var height = API.returnNative("GET_ENTITY_HEIGHT_ABOVE_GROUND", 7, API.getPlayerVehicle(local_plr));
                if (height > 1.0) {
                    API.callNative("DISABLE_CONTROL_ACTION", 0, 59, true);
                    API.callNative("DISABLE_CONTROL_ACTION", 0, 60, true);
                } else {
                    var rot = API.getEntityRotation(veh);
                    if (rot.X > 90.0 || rot.X < -90.0) {
                        API.callNative("DISABLE_CONTROL_ACTION", 0, 59, true);
                        API.callNative("DISABLE_CONTROL_ACTION", 0, 60, true);
                    }
                }
            }
            if (API.returnNative("IS_VEHICLE_A_CONVERTIBLE", 8, API.getPlayerVehicle(local_plr), false)) {
                API.callNative("DISABLE_CONTROL_ACTION", 0, 101, true);
            }
        }
    }

    if (checkForTimeout) {
        if (API.getGameTime() - oldTime2 >= 500) {
            checkForRoofTimeout();
        }
    }

    if(trigger_check_locked)
    {
        if(!API.isPlayerInAnyVehicle(local_plr))
        {
            var vehicle = API.returnNative("GET_VEHICLE_PED_IS_TRYING_TO_ENTER", 0, local_plr);
            if (vehicle !== null)
            {
                //API.sendChatMessage("The veh is: " + veh);
                var DAT_VEHICLE_THO = null;
                var vehicles = API.getAllVehicles();
                for (var i = 0; i < vehicles.Length; i++) {
                    if (vehicles[i].Value === vehicle) {
                        DAT_VEHICLE_THO = vehicles[i];
                        //API.sendChatMessage("FOUND");
                        break;
                    }
                }
                trigger_check_locked = false;
                if (DAT_VEHICLE_THO !== null) {
                    if (API.getVehicleLocked(DAT_VEHICLE_THO)) {
                        trigger_door_client_fix = false;
                        //API.sendChatMessage("Vehicle is locked");
                        API.triggerServerEvent("player_ragdolled");
                    }
                    else {
                        //API.sendChatMessage("Vehicle is unlocked");
                    }
                }
            }
        }
        else
            trigger_check_locked = false;
    }
    if(trigger_door_client_fix)
    {
        if(!API.isPlayerInAnyVehicle(API.getLocalPlayer()))
        {
            var vehicle = API.returnNative("GET_VEHICLE_PED_IS_TRYING_TO_ENTER", 0, API.getLocalPlayer());
            if (vehicle !== null)
            {
                //API.sendChatMessage("The veh is: " + veh);
                var DAT_VEHICLE_THO = null;
                var vehs = API.getAllVehicles();
                for (var i = 0; i < vehs.Length; i++) {
                    if (vehs[i].Value === vehicle) {
                        DAT_VEHICLE_THO = vehs[i];
                        //API.sendChatMessage("FOUND");
                        break;
                    }
                }
                if (DAT_VEHICLE_THO !== null) {
                    /*if(API.returnNative("0x218297BF0CFD853B", 0, DAT_VEHICLE_THO, 0) === API.getLocalPlayer().Value)
                    {
                        API.sendChatMessage("PED1 IS PLAYER");
                        API.callNative("0x7C65DAC73C35C862 ", DAT_VEHICLE_THO, 0, true, false);
                        trigger_door_client_fix = false;
                    }*/
                    if (API.returnNative("0x218297BF0CFD853B", 0, DAT_VEHICLE_THO, 1) ===
                        API.getLocalPlayer().Value) {
                        //API.sendChatMessage("PED2 IS PLAYER");
                        API.callNative("0x7C65DAC73C35C862", DAT_VEHICLE_THO, 1, true, false);
                        API.callNative("0xF2BFA0430F0A0FCB", DAT_VEHICLE_THO, 1, 1, API.f(0.01));
                        trigger_door_client_fix = false;
                    } else if (API.returnNative("0x218297BF0CFD853B", 0, DAT_VEHICLE_THO, 2) ===
                        API.getLocalPlayer().Value) {
                        //API.sendChatMessage("PED3 IS PLAYER");
                        API.callNative("0x7C65DAC73C35C862", DAT_VEHICLE_THO, 2, true, false);
                        API.callNative("0xF2BFA0430F0A0FCB", DAT_VEHICLE_THO, 2, 1, API.f(0.01));
                        trigger_door_client_fix = false;
                    } else if (API.returnNative("0x218297BF0CFD853B", 0, DAT_VEHICLE_THO, 3) ===
                        API.getLocalPlayer().Value) {
                        //API.sendChatMessage("PED4 IS PLAYER");
                        API.callNative("0x7C65DAC73C35C862", DAT_VEHICLE_THO, 3, true, false);
                        API.callNative("0xF2BFA0430F0A0FCB", DAT_VEHICLE_THO, 3, 1, API.f(0.01));
                        trigger_door_client_fix = false;
                    }
                    /*else
                    {
                        API.sendChatMessage("PED IS NOT PLAYER");
                    }*/
                }
            }
        }
        else
            trigger_check_locked = false;
    }
});