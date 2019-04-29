/// <reference path="../types-gt-mp/index.d.ts" />
/*
			@SkyLaGer 
			Site: http://gta-dev.ru/ 
*/
/*==========================================*/
/* Основной класс для работы браузера       */
/*==========================================*/
class WebBrowser {
    browser: GrandTheftMultiplayer.Client.GUI.Browser;
    private readonly path: string;
    private open: boolean;
    
    constructor(resourcePath) {
        this.path = resourcePath;
        this.open = false;
    }

    show(local:boolean, optPath:string, chatstate:boolean = false, cursorstate:boolean = true) {
        if (this.open === false) {
            
            this.open = true;
            const resolution = API.getScreenResolution();

            this.browser = API.createCefBrowser(resolution.Width, resolution.Height, local);
            API.waitUntilCefBrowserInit(this.browser);
            API.setCefBrowserPosition(this.browser, 0, 0);
            API.loadPageCefBrowser(this.browser, optPath ? optPath : this.path);
            
            API.setCanOpenChat(chatstate);
            API.showCursor(cursorstate);
        }
    }

    destroy() {
        this.open = false;
        API.destroyCefBrowser(this.browser);
        API.setCanOpenChat(true);
        API.setHudVisible(true);
        API.showCursor(false);
    }

    eval(string) {
        this.browser.eval(string);
    }
}


/*==========================================*/
/*  Переменные                              */
/*==========================================*/
const cef = new WebBrowser("");

/*==========================================*/
/*  Работа с браузером                      */
/*==========================================*/
API.onServerEventTrigger.connect((eventName, args) => {
    
    if (eventName === "CEFDestroy") {
        cef.destroy();
    }
    else if (eventName === "CEFCreate") {
        if (args[1] === void 0) { args[1] = false; }
        cef.show(args[1], args[0]); 
    }
});


API.onResourceStop.connect(() => {
    if (cef != null) {
        cef.destroy();
    }
});


