/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />

var ClothingShopMainMenu = API.createMenu("Klamottenladen", "Verfügbare Kategorien:", 0, 0, 6);
var ClothingShopAccessories = API.createMenuItem("Accessories", "");
var ClothingShopTops = API.createMenuItem("Oberteile", "");
var ClothingShopLegs = API.createMenuItem("Hosen", "");
var ClothingShopFeets = API.createMenuItem("Schuhe", "");
var ClothingShopMasks = API.createMenuItem("Masken", "");
var ClothingShopHair = API.createMenuItem("Harre", "");
var ClothingShopBackpacks = API.createMenuItem("Taschen", "");
var ClothingShopTorso = API.createMenuItem("Torsos", "");
var ClothingShopJson;
var ClothingShopAccessoriesMenu = API.createMenu("Accessories ", "Verfügbare Accessories:", 0, 0, 6);
var ClothingShopTopMenu = API.createMenu("Oberteile", "Verfügbare Oberteile:", 0, 0, 6);
var ClothingShopLegMenu = API.createMenu("Hosen", "Verfügbare Hosen:", 0, 0, 6);
var ClothingShopFeetMenu = API.createMenu("Schuhe", "Verfügbare Schuhe:", 0, 0, 6);
var ClothingShopMaskMenu = API.createMenu("Masken", "Verfügbare Masken:", 0, 0, 6);
var ClothingShopHairMenu = API.createMenu("Harre", "Verfügbare Harre:", 0, 0, 6);
var ClothingShopBackpacksMenu = API.createMenu("Taschen ", "Verfügbare Taschen:", 0, 0, 6);
var ClothingShopTorsoMenu = API.createMenu("Torsos", "Verfügbare Torsos:", 0, 0, 6);


ClothingShopMainMenu.BindMenuToItem(ClothingShopAccessoriesMenu, ClothingShopAccessories);
ClothingShopMainMenu.BindMenuToItem(ClothingShopTopMenu, ClothingShopTops);
ClothingShopMainMenu.BindMenuToItem(ClothingShopLegMenu, ClothingShopLegs);
ClothingShopMainMenu.BindMenuToItem(ClothingShopFeetMenu, ClothingShopFeets);
ClothingShopMainMenu.BindMenuToItem(ClothingShopMaskMenu, ClothingShopMasks);
ClothingShopMainMenu.BindMenuToItem(ClothingShopHairMenu, ClothingShopHair);
ClothingShopMainMenu.BindMenuToItem(ClothingShopBackpacksMenu, ClothingShopBackpacks);
ClothingShopMainMenu.BindMenuToItem(ClothingShopTorsoMenu, ClothingShopTorso);

API.onServerEventTrigger.connect(function (eventName, args) {
	switch (eventName) {
		case "ClothingShop_OpenMenu":
			API.closeAllMenus();
			ClothingShopJson = JSON.parse(args[0]);
			FillClothingShopMenu();
			ClothingShopMainMenu.Visible = true;
			break;
		case "ClothingShop_CloseMenu":
            ClothingShopMainMenu.Visible = false;
            ClothingShopAccessoriesMenu.Visible = false;
			ClothingShopTopMenu.Visible = false;
			ClothingShopLegMenu.Visible = false;
            ClothingShopFeetMenu.Visible = false;
            ClothingShopMaskMenu.Visible = false;
            ClothingShopHairMenu.Visible = false;
            ClothingShopBackpacksMenu.Visible = false;
            ClothingShopTorsoMenu.Visible = false;
			break;
	}
});

function FillClothingShopMenu() {
    ClothingShopMainMenu.Clear();
    ClothingShopAccessoriesMenu.Clear();
	ClothingShopTopMenu.Clear();
	ClothingShopLegMenu.Clear();
    ClothingShopFeetMenu.Clear();
    ClothingShopMaskMenu.Clear();
    ClothingShopHairMenu.Clear();
    ClothingShopBackpacksMenu.Clear();
    ClothingShopTorsoMenu.Clear();

    if (ClothingShopJson["AvailableTorso"].length != 0) {
        ClothingShopMainMenu.AddItem(ClothingShopTorso);
    }

    for (var i = 0; i < ClothingShopJson["AvailableTorso"].length; i++) {
        var menuObj = ClothingShopJson["AvailableTorso"][i];
        var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
        if (menuObj["AlreadyBought"]) {
            NewItem.SetRightLabel("Bereits gekauft");
        } else {
            NewItem.SetRightLabel(menuObj["Price"] + "$");
        }
        ClothingShopTorsoMenu.AddItem(NewItem);
    }

    if (ClothingShopJson["AvailableAccessories"].length != 0) {
        ClothingShopMainMenu.AddItem(ClothingShopAccessories);
    }

    for (var i = 0; i < ClothingShopJson["AvailableAccessories"].length; i++) {
        var menuObj = ClothingShopJson["AvailableAccessories"][i];
        var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
        if (menuObj["AlreadyBought"]) {
            NewItem.SetRightLabel("Bereits gekauft");
        } else {
            NewItem.SetRightLabel(menuObj["Price"] + "$");
        }
        ClothingShopAccessoriesMenu.AddItem(NewItem);
    } 

	if (ClothingShopJson["AvailableTops"].length != 0) {
		ClothingShopMainMenu.AddItem(ClothingShopTops);
	}

	for (var i = 0; i < ClothingShopJson["AvailableTops"].length; i++) {
		var menuObj = ClothingShopJson["AvailableTops"][i];
		var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
		if (menuObj["AlreadyBought"]) {
            NewItem.SetRightLabel("Bereits gekauft");
		} else {
			NewItem.SetRightLabel(menuObj["Price"] + "$");
		}
		ClothingShopTopMenu.AddItem(NewItem);
	}

	if (ClothingShopJson["AvailableLegs"].length != 0) {
		ClothingShopMainMenu.AddItem(ClothingShopLegs);
	}

	for (var i = 0; i < ClothingShopJson["AvailableLegs"].length; i++) {
		var menuObj = ClothingShopJson["AvailableLegs"][i];
		var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
		if (menuObj["AlreadyBought"]) {
            NewItem.SetRightLabel("Bereits gekauft");
		} else {
			NewItem.SetRightLabel(menuObj["Price"] + "$");
		}
		ClothingShopLegMenu.AddItem(NewItem);
    }

    if (ClothingShopJson["AvailableMasks"].length != 0) {
        ClothingShopMainMenu.AddItem(ClothingShopMasks);
    }

    for (var i = 0; i < ClothingShopJson["AvailableMasks"].length; i++) {
        var menuObj = ClothingShopJson["AvailableMasks"][i];
        var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
        if (menuObj["AlreadyBought"]) {
            NewItem.SetRightLabel("Bereits gekauft");
        } else {
            NewItem.SetRightLabel(menuObj["Price"] + "$");
        }
        ClothingShopMaskMenu.AddItem(NewItem);
    }

    if (ClothingShopJson["AvailableBackpacks "].length != 0) {
        ClothingShopMainMenu.AddItem(ClothingShopBackpacks);
    }

    for (var i = 0; i < ClothingShopJson["AvailableBackpacks"].length; i++) {
        var menuObj = ClothingShopJson["AvailableBackpacks"][i];
        var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
        if (menuObj["AlreadyBought"]) {
            NewItem.SetRightLabel("Bereits gekauft");
        } else {
            NewItem.SetRightLabel(menuObj["Price"] + "$");
        }
        ClothingShopBackpacksMenu.AddItem(NewItem);
    }

	if (ClothingShopJson["AvailableFeets"].length != 0) {
		ClothingShopMainMenu.AddItem(ClothingShopFeets);
	} 

	for (var i = 0; i < ClothingShopJson["AvailableFeets"].length; i++) {
		var menuObj = ClothingShopJson["AvailableFeets"][i];
		var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
		if (menuObj["AlreadyBought"]) {
            NewItem.SetRightLabel("Bereits gekauft");
		} else {
			NewItem.SetRightLabel(menuObj["Price"] + "$");
		}
		ClothingShopFeetMenu.AddItem(NewItem);
	}
}
if (ClothingShopJson["AvailableHair"].length != 0) {
    ClothingShopMainMenu.AddItem(ClothingShopHair);
}

for (var i = 0; i < ClothingShopJson["AvailableHair"].length; i++) {
    var menuObj = ClothingShopJson["AvailableHair"][i];
    var NewItem = API.createMenuItem(menuObj["Id"] + "", "");
    if (menuObj["AlreadyBought"]) {
        NewItem.SetRightLabel("Bereits gekauft");
    } else {
        NewItem.SetRightLabel(menuObj["Price"] + "$");
    }
    ClothingShopHairMenu.AddItem(NewItem);
}

ClothingShopAccessoriesMenu.OnMenuClose.connect(function (menu) {
    API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopMainMenu.OnMenuClose.connect(function (menu) {
	API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopTopMenu.OnMenuClose.connect(function (menu) {
	API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopLegMenu.OnMenuClose.connect(function (menu) {
	API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopFeetMenu.OnMenuClose.connect(function (menu) {
	API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopMaskMenu.OnMenuClose.connect(function (menu) {
    API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopHairMenu.OnMenuClose.connect(function (menu) {
    API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopBackpacksMenu.OnMenuClose.connect(function (menu) {
    API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopTorsoMenu.OnMenuClose.connect(function (menu) {
    API.triggerServerEvent("ClothingShop_Close");
});

ClothingShopAccessoriesMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("ClothingShop_Buy", "accessories", ClothingShopJson["AvailableAccessories"][index]["Id"]);
});

ClothingShopAccessoriesMenu.OnIndexChange.connect(function (menu, index) {
    API.triggerServerEvent("ClothingShop_Preview", "accessories", ClothingShopJson["AvailableAccessories"][index]["Id"]);
}); 

ClothingShopTopMenu.OnItemSelect.connect(function (menu, item, index) {
	API.triggerServerEvent("ClothingShop_Buy", "top", ClothingShopJson["AvailableTops"][index]["Id"]);
});

ClothingShopTopMenu.OnIndexChange.connect(function (menu, index) {
	API.triggerServerEvent("ClothingShop_Preview", "top", ClothingShopJson["AvailableTops"][index]["Id"]);
});


ClothingShopLegMenu.OnItemSelect.connect(function (menu, item, index) {
	API.triggerServerEvent("ClothingShop_Buy", "leg", ClothingShopJson["AvailableLegs"][index]["Id"]);
});

ClothingShopLegMenu.OnIndexChange.connect(function (menu, index) {
	API.triggerServerEvent("ClothingShop_Preview", "leg", ClothingShopJson["AvailableLegs"][index]["Id"]);
});

ClothingShopFeetMenu.OnItemSelect.connect(function (menu, item, index) {
	API.triggerServerEvent("ClothingShop_Buy", "feet", ClothingShopJson["AvailableFeets"][index]["Id"]);
});

ClothingShopFeetMenu.OnIndexChange.connect(function (menu, index) {
	API.triggerServerEvent("ClothingShop_Preview", "feet", ClothingShopJson["AvailableFeets"][index]["Id"]);
});

ClothingShopMaskMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("ClothingShop_Buy", "masks", ClothingShopJson["AvailableMasks"][index]["Id"]);
});

ClothingShopMaskMenu.OnIndexChange.connect(function (menu, index) {
    API.triggerServerEvent("ClothingShop_Preview", "masks", ClothingShopJson["AvailableMasks"][index]["Id"]);
});

ClothingShopHairMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("ClothingShop_Buy", "hair", ClothingShopJson["AvailableHair"][index]["Id"]);
});

ClothingShopHairMenu.OnIndexChange.connect(function (menu, index) {
    API.triggerServerEvent("ClothingShop_Preview", "hair", ClothingShopJson["AvailableHair"][index]["Id"]);
});

ClothingShopBackpacksMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("ClothingShop_Buy", "backpacks", ClothingShopJson["AvailableBackpacks"][index]["Id"]);
});

ClothingShopBackpacksMenu.OnIndexChange.connect(function (menu, index) {
    API.triggerServerEvent("ClothingShop_Preview", "backpacks", ClothingShopJson["AvailableBackpacks"][index]["Id"]);
}); 

ClothingShopTorsoMenu.OnItemSelect.connect(function (menu, item, index) {
    API.triggerServerEvent("ClothingShop_Buy", "torso", ClothingShopJson["AvailableTorso"][index]["Id"]);
});

ClothingShopTorsoMenu.OnIndexChange.connect(function (menu, index) {
    API.triggerServerEvent("ClothingShop_Preview", "torso", ClothingShopJson["AvailableTorso"][index]["Id"]);
});

