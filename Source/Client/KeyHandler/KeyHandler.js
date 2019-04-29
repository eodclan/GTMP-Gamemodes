"use strict";
API.onKeyDown.connect(function (sender, e) {
    switch (e.KeyCode) {
        case Keys.E:
            API.triggerServerEvent("KeyboardKey_E_Pressed");
            break;
        case Keys.K:
            API.triggerServerEvent("KeyboardKey_K_Pressed");
            break;
        case Keys.M:
            API.triggerServerEvent("KeyboardKey_M_Pressed");
            break;
        case Keys.I:
            API.triggerServerEvent("KeyboardKey_I_Pressed");
            break;
        case Keys.U:
            API.triggerServerEvent("KeyboardKey_U_Pressed");
            break;
        case Keys.Z:
            API.triggerServerEvent("KeyboardKey_Z_Pressed");
            break;
        case Keys.Y:
            API.triggerServerEvent("KeyboardKey_Y_Pressed");
            break;
    }
});


API.onKeyUp.connect(function (sender, e) {
    switch (e.KeyCode) {
        case Keys.E:
            API.triggerServerEvent("KeyboardKey_E_Released");
            break;
    }
});
