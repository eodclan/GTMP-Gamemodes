/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />

var VehicleMainMenuJson;
var VehicleInventoryJson;
var VehiclePlayerInventoryJson;
var VehicleMainMenu = API.createMenu("Fahrzeugmenü", "", 0, 0, 6);
var VehicleInventoryMenu = API.createMenu("Fahrzeug Inventar", "Verfügbare Artikel:", 0, 0, 6);
var VehiclePlayerInventoryMenu = API.createMenu("Dein Inventar", "Verfügbare Artikel:", 0, 0, 6);

API.onServerEventTrigger.connect(function (eventName, args) {
	switch (eventName) {
		case "Vehicle_CloseAllMenus":
			API.closeAllMenus();
			break;
		case "Vehicle_UpdateInventory":
			VehicleInventoryJson = JSON.parse(args[0]);
			FillVehicleInventory();
			break;
		case "Vehicle_UpdatePlayerInventory":
			VehiclePlayerInventoryJson = JSON.parse(args[0]);
			FillVehiclePlayerInventory();
			break;
		case "Vehicle_OpenMenu":
			API.closeAllMenus();
			VehicleMainMenuJson = JSON.parse(args[0]);
			FillVehicleMainMenu();
			VehicleMainMenu.Visible = true;
			break;
		case "Vehicle_OpenInventory":
			API.closeAllMenus();
			FillVehicleInventory();
			VehicleInventoryMenu.Visible = true;
			break;
		case "Vehicle_OpenPlayerInventory":
			API.closeAllMenus();
			VehiclePlayerInventoryMenu.Visible = true;
			break;
	}
});

function FillVehicleMainMenu() {
	VehicleMainMenu.Clear();
	for (var i = 0; i < VehicleMainMenuJson.length; i++) {
		var vehicleMenuObj = VehicleMainMenuJson[i];
		var NewItem = API.createMenuItem(vehicleMenuObj["Title"], vehicleMenuObj["Description"]);
		if (vehicleMenuObj["RightLabel"] != "") {
			NewItem.SetRightLabel(vehicleMenuObj["RightLabel"]);
		}
		VehicleMainMenu.AddItem(NewItem);
	}
}

function FillVehicleInventory() {
	VehicleInventoryMenu.Clear();
	for (var i = 0; i < VehicleInventoryJson.length; i++) {
		var vehicleMenuObj = VehicleInventoryJson[i];
		var NewItem = API.createMenuItem(vehicleMenuObj["Name"], vehicleMenuObj["Description"]);
		NewItem.SetRightLabel(vehicleMenuObj["Count"] + "x");
		VehicleInventoryMenu.AddItem(NewItem);
	}
}

function FillVehiclePlayerInventory() {
	VehiclePlayerInventoryMenu.Clear();
	for (var i = 0; i < VehiclePlayerInventoryJson.length; i++) {
		var vehicleMenuObj = VehiclePlayerInventoryJson[i];
		var NewItem = API.createMenuItem(vehicleMenuObj["Name"], vehicleMenuObj["Description"]);
		NewItem.SetRightLabel(vehicleMenuObj["Count"] + "x");
		VehiclePlayerInventoryMenu.AddItem(NewItem);
	}
}

VehicleMainMenu.OnItemSelect.connect(function (menu, item, index) {
	API.triggerServerEvent("Vehicle_MainMenuItemSelected", VehicleMainMenuJson[index]["Value1"])
});

VehicleInventoryMenu.OnItemSelect.connect(function (menu, item, index) {
	var usercountinput = API.getUserInput("0", 4);
	var usercount = parseInt(usercountinput);
	if (!isNaN(usercount)) {
		API.triggerServerEvent("Vehicle_InventoryMenuItemSelected", VehicleInventoryJson[index]["Id"], usercount);
	} else {
        API.sendNotification("~r~Bitte geben Sie eine gültige Nummer ein!");
	}
});

VehiclePlayerInventoryMenu.OnItemSelect.connect(function (menu, item, index) {
	var usercountinput = API.getUserInput("0", 4);
	var usercount = parseInt(usercountinput);
	if (!isNaN(usercount)) {
		API.triggerServerEvent("Vehicle_PlayerInventoryMenuItemSelected", VehiclePlayerInventoryJson[index]["Id"], usercount);
	} else {
        API.sendNotification("~r~Bitte geben Sie eine gültige Nummer ein!");
	}
});

VehicleInventoryMenu.OnMenuClose.connect(function (menu) {
	API.triggerServerEvent("Vehicle_CloseTrunk");
});

VehiclePlayerInventoryMenu.OnMenuClose.connect(function (menu) {
	API.triggerServerEvent("Vehicle_CloseTrunk");
});