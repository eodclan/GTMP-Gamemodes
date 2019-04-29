/// <reference path="../types-gt-mp/index.d.ts" />
/*
			@SkyLaGer 
			Site: http://gta-dev.ru/ 
*/
/*==========================================*/
/* Основной класс для работы браузера       */
/*==========================================*/
var WebBrowser = (function () {
    function WebBrowser(resourcePath) {
        this.path = resourcePath;
        this.open = false;
    }
    WebBrowser.prototype.show = function (local, optPath, chatstate, cursorstate) {
        if (chatstate === void 0) { chatstate = false; }
        if (cursorstate === void 0) { cursorstate = true; }
        if (this.open === false) {
            this.open = true;
            var resolution = API.getScreenResolution();
            this.browser = API.createCefBrowser(resolution.Width, resolution.Height, local);
            API.waitUntilCefBrowserInit(this.browser);
            API.setCefBrowserPosition(this.browser, 0, 0);
            API.loadPageCefBrowser(this.browser, optPath ? optPath : this.path);
            API.setCanOpenChat(chatstate);
            API.showCursor(cursorstate);
        }
    };
    WebBrowser.prototype.destroy = function () {
        this.open = false;
        API.destroyCefBrowser(this.browser);
        API.setCanOpenChat(true);
        API.setHudVisible(true);
        API.showCursor(false);
    };
    WebBrowser.prototype.eval = function (string) {
        this.browser.eval(string);
    };
    return WebBrowser;
}());
/*==========================================*/
/*  Переменные                              */
/*==========================================*/
var cef = new WebBrowser("");

/*==========================================*/
/*  Работа с браузером                      */
/*==========================================*/
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName === "CEFDestroy") {
        cef.destroy();
    }
    else if (eventName === "CEFCreate") {
        if (args[1] === void 0) {
            args[1] = false;
        }
        cef.show(args[1], args[0]);
    }
});

API.onResourceStop.connect(function () {
    if (cef != null) {
        cef.destroy();
    }
});
