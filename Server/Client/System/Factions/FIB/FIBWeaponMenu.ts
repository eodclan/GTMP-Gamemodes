﻿/// <reference path="../../../types-gt-mp/Definitions/index.d.ts" /> 
 
var FIBWeaponMenu = API.createMenu("FIB Weapons", "", 0, 0, 6); 
var FIBWeaponJson; 
 
API.onServerEventTrigger.connect(function (eventName, args) { 
  switch (eventName) { 
	case "FIB_OpenWeaponMenu": 
	  API.closeAllMenus(); 
	  FIBWeaponJson = JSON.parse(args[0]); 
	  FillFIBWeaponMenu(); 
	  FIBWeaponMenu.Visible = true; 
	  break; 
	case "FIB_CloseWeaponMenu": 
	  API.closeAllMenus(); 
	  break; 
  } 
}); 
 
 
function FillFIBWeaponMenu() { 
	FIBWeaponMenu.Clear(); 
	for (var i = 0; i < FIBWeaponJson.length; i++) { 
	  var MenuObj = FIBWeaponJson[i]; 
	var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]); 
	if (MenuObj["RightLabel"] != "") { 
	  NewItem.SetRightLabel(MenuObj["RightLabel"]); 
	} 
	FIBWeaponMenu.AddItem(NewItem); 
  } 
} 
 
FIBWeaponMenu.OnItemSelect.connect(function (menu, item, index) { 
	API.triggerServerEvent("FIB_WeaponMenuItemSelected", FIBWeaponJson[index]["Value1"]) 
});