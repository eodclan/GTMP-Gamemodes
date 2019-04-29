﻿/// <reference path="../../../types-gt-mp/Definitions/index.d.ts" /> 
 
var JVAWeaponMenu = API.createMenu("JVA Weapons", "", 0, 0, 6); 
var JVAWeaponJson; 
 
API.onServerEventTrigger.connect(function (eventName, args) { 
  switch (eventName) { 
	case "JVA_OpenWeaponMenu": 
	  API.closeAllMenus(); 
	  JVAWeaponJson = JSON.parse(args[0]); 
	  FillJVAWeaponMenu(); 
	  JVAWeaponMenu.Visible = true; 
	  break; 
	case "JVA_CloseWeaponMenu": 
	  API.closeAllMenus(); 
	  break; 
  } 
}); 
 
 
function FillJVAWeaponMenu() { 
	JVAWeaponMenu.Clear(); 
	for (var i = 0; i < JVAWeaponJson.length; i++) { 
	  var MenuObj = JVAWeaponJson[i]; 
	var NewItem = API.createMenuItem(MenuObj["Title"], MenuObj["Description"]); 
	if (MenuObj["RightLabel"] != "") { 
	  NewItem.SetRightLabel(MenuObj["RightLabel"]); 
	} 
	JVAWeaponMenu.AddItem(NewItem); 
  } 
} 
 
JVAWeaponMenu.OnItemSelect.connect(function (menu, item, index) { 
	API.triggerServerEvent("JVA_WeaponMenuItemSelected", JVAWeaponJson[index]["Value1"]) 
});