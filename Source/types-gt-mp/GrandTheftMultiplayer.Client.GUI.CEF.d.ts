﻿declare namespace GrandTheftMultiplayer.Client.GUI.CEF {

	class Browser {
		readonly BrowserIdentifier: number;
		Headless: boolean;
		Position: System.Drawing.Point;
		Size: System.Drawing.Size;
		eval(code: string): void;
		call(method: string, ..._arguments: any[]): void;
		Dispose(): void;
	}

}
