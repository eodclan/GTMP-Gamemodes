var menuPool = null;

var mainMenu = null;
var mainMenuItems = ["Fahrzeugtuning"];

var modMenu = null;
var vehicleMods = ["Spoilers","Fordere Stoßstange","Hintere Stoßstange","Seitenschweller","Auspuff","Rahmen","Gitter","Motorhaube","Fender","Rechter Fender","Dach","Motor","Bremsen","Getriebe","Hupe","Aufhängung","Rüstung","Turbo","Xenon","Vordere Reifen","Hintere Reifen","Plate holders","Trim Design","Verzierung","Zifferblatt Design","Lenkrad","Schalthebel","Plaketten","Hydraulik","Livery","Plate","Fenstertönung"];
var vehicleModIndexes = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,18,22,23,24,25,27,28,30,33,34,35,38,48,62,69];

var modOptionsMenu = null;
var modOption = null;

//Global variables to keep track of vehicle mods
var isChangingMod = false;
var selectedModType = 0;
var selectedModIndex = -1;

API.onUpdate.connect(function(s, e) 
{
	if (menuPool != null) 
	{
        menuPool.ProcessMenus();
    }
});

//When ModMenu is started, create all the required menus and hide them
API.onResourceStart.connect(function(s, e) 
{
	menuPool = API.getMenuPool();
	createMainMenu();
	createModMenu();
	createModOptionsMenu();
});

API.onServerEventTrigger.connect(function (eventName, args) 
{
  switch (eventName) 
  {
    case 'OpenModMenu':
	openMainMenu();
    break;
  }
});

API.onKeyDown.connect(function (sender, e)
 {
	//M key opens mod menu
    if (e.KeyCode === Keys.F3 && !API.isChatOpen())
    {
	  	openMainMenu();
	  	isChangingMod = false;
    }
  
  //Left arrow for switching mod type
  if (e.KeyCode === Keys.Left && isChangingMod) 
  {
	  if(selectedModIndex >= 0)
	  {
		 selectedModIndex--;	
		 modOption.Text = " " + selectedModIndex;		  
		 API.triggerServerEvent("ChangeVehicleMod", selectedModType, selectedModIndex);
	  }
  }
  
  //Right arrow for switching mod type
  if (e.KeyCode === Keys.Right && isChangingMod) 
  {
	selectedModIndex++;
	modOption.Text = " " + selectedModIndex;
	API.triggerServerEvent("ChangeVehicleMod", selectedModType, selectedModIndex);
  }
})

function createMainMenu() 
{
	//Create the main menu
	mainMenu = API.createMenu("Fahrzeugtuning", "", 0, 0, 6);
	
	for (var i = 0; i < mainMenuItems.length; i++) 
	{
		mainMenu.AddItem(API.createMenuItem(mainMenuItems[i], ""));
	}

	mainMenu.CurrentSelection = 0;
	menuPool.Add(mainMenu);
	mainMenu.Visible = false; 
	
	//Gets called when we select a category
	mainMenu.OnItemSelect.connect(function(sender, item, index)
	{
		switch (index) 
		{
			case 0:
			openModMenu();
			break;
		}
	});
}

function createModMenu() 
{
	//Create the main mod selection menu
	modMenu = API.createMenu("Vehicle Mods", 0, 0, 6);
	
	for (var i = 0; i < vehicleMods.length; i++) 
	{
		modMenu.AddItem(API.createMenuItem(vehicleMods[i], ""));
	}

	modMenu.CurrentSelection = 0;
	menuPool.Add(modMenu);
	modMenu.Visible = false; 
	
	//Gets called when we select a mod category
	modMenu.OnItemSelect.connect(function(sender, item, index)
	{
		selectedModType = vehicleModIndexes[index];
		modMenu.Visible = false; 
		isChangingMod = true;
		API.triggerServerEvent("ChangeVehicleMod", selectedModType, selectedModIndex);
		openModOptions();
	});
}

function createModOptionsMenu() 
{
	modOptionsMenu = API.createMenu("Select mod", 0, 0, 6);
	modOption = API.createMenuItem(" -1", "")
	modOptionsMenu.AddItem(modOption);
	modOptionsMenu.CurrentSelection = 0;
	modOptionsMenu.Visible = false; 
	menuPool.Add(modOptionsMenu);
	 
	//Gets called when we apply a mod
	modOptionsMenu.OnItemSelect.connect(function(sender, item, index)
	{
		API.sendNotification('Fahrzeug Tuning Teil ~g~' + selectedModIndex + "~w~ angewendet");
		modOptionsMenu.Visible = false; 
		modOption.Text = " -1";	
		openModMenu();
		selectedModIndex = -1;
		isChangingMod = false;
	});
}

function openMainMenu() 
{
    mainMenu.CurrentSelection = 0;
	mainMenu.Visible = true; 
}

function openModMenu() 
{
	mainMenu.Visible = false;
	modMenu.Visible = true; 
}

function openModOptions() 
{
	modMenu.Visible = false;
    modOptionsMenu.CurrentSelection = 0;
	modOptionsMenu.Visible = true; 
}
