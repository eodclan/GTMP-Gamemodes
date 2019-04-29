"use strict";
var res = API.getScreenResolutionMaintainRatio();
var cursorIndex = 0;
var LoginCamera = null
var myBrowser = null; //we set it to null because the browser is not yet created, we cannot have a var that has empty value, so we use null
login_sendEvent();

function login_sendEvent(username, password){
	//API.sendChatMessage(username + " - " + password);
	API.triggerServerEvent("account_loginButtonPressed", username, password);
}

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "setLoginUiVisible":
			
            if (args[0]) {
				createBrowser();
                API.setCanOpenChat(false);
                API.disableVehicleEnteringKeys(true);
                API.disableAlternativeMainMenuKey(true);
                cursorIndex = 0;
                API.callNative("0xA0EBB943C300E693", false);
                LoginCamera = API.createCamera(args[1], new Vector3(0, 0, 0));
                API.pointCameraAtPosition(LoginCamera, args[2]);
                API.setActiveCamera(LoginCamera);
            }
            
            break;
		case "Login_UserName_False":
			myBrowser.call("badLogin");
			return false;
		break
		case "Login_Password_False":
			myBrowser.call("badLogin");
			return false;
		break
			
		case "Login_Success":
		//API.sendChatMessage("~g~Login erfolgreich");
		CloseLoginStep();
		break
    }
});
function CloseLoginStep() {
	//myBrowser = API.createCefBrowser(res.Width, res.Height);
	API.destroyCefBrowser(myBrowser); //destroy the CEF browser
    API.setCanOpenChat(true);
    API.disableVehicleEnteringKeys(false);
    API.disableAlternativeMainMenuKey(false);
    API.setActiveCamera(null);
    API.setHudVisible(true);
	API.showCursor(false); //stop showing the cursor

	//API.setSnowEnabled(true, false, false);
}

function createBrowser(){
 //var res = API.getScreenResolution(); //this gets the client's screen resoulution
		myBrowser = API.createCefBrowser(res.Width, res.Height); //we're initializing the browser here. This will be the full size of the user's screen.
		API.waitUntilCefBrowserInit(myBrowser); //this stops the script from getting ahead of itself, it essentially pauses until the browser is initialized
		API.setCefBrowserPosition(myBrowser, 0, 0); //The orientation (top left) corner in relation to the user's screen.  This is useful if you do not want a full page browser.  0,0 is will lock the top left corner of the browser to the top left of the screen.
		API.loadPageCefBrowser(myBrowser, "Client/Login/login.html"); //This loads the HTML file of your choice.      .    API.setCefBrowserHeadless(myBrowser, true); //this will remove the scroll bars from the bottom/right side
		//API.setCefBrowserHeadless(myBrowser, true);
		API.showCursor(true); //This will show the mouse cursor
		//API.setCanOpenChat(false);  //This disables the chat, so the user can type in a form without opening the chat and causing issues.
				
       
}
API.onUpdate.connect(function () {
 
});
