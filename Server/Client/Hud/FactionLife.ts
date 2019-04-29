/// <reference path="../../types-gt-mp/Definitions/index.d.ts" />
API.onUpdate.connect(function() {
    var res = API.getScreenResolutionMaintainRatio();
	API.drawText("Faction Life", (res.Width / 2) + 818.4375, (res.Height / 2) - 540, 0.69, 255, 255, 255, 255, 1, 1, true, false, 0); 
});