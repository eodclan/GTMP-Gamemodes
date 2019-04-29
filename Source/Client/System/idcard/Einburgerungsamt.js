"use strict";
let menuPool = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "OpenEinburgerungsMenu":
            API.closeAllMenus();

            let menu = API.createMenu("~w~Einbürgerungsamt", "", 0, 0, 6);
            let item1 = API.createMenuItem("Ausweis beantragen", "~g~500$");
            let item2 = API.createMenuItem("Schließen", "");

            // This is how you handle menu item selection
            item1.Activated.connect(function (menu, item) {
                API.triggerServerEvent("SetIDCardTrueInDatabase");
                API.closeAllMenus();
            });
            item2.Activated.connect(function (menu, item) {
                API.closeAllMenus();
            });


            menu.AddItem(item1);
            menu.AddItem(item2)


            menu.Visible = true;
            break;
    }
});