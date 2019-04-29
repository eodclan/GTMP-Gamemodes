"use strict";
var genderItem = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "open_CharCreator":
            CharCreatorMenu.Visible = true;
            API.setCanOpenChat(false);
            break;
    }
});
API.onUpdate.connect(function () {
});
var CharCreatorMenu = API.createMenu("Charakter Erstellung", "", 0, 0, 6);
CharCreatorMenu.ResetKey(menuControl.Back);
var gender_list = new List(String);
gender_list.Add("Männlich");
gender_list.Add("Weiblich");

var first_name_item = API.createMenuItem("Vorname", "Dein Charakter Vorname");
var last_name_item = API.createMenuItem("Nachname", "Dein Charakter Nachname");
var gender_selector = API.createListItem("Geschlecht", "~r~Wenn Sie das Geschlecht �ndern, wird Ihre Anpassung zur�ckgesetzt.", gender_list, 0);
var char_save_button = API.createMenuItem("Charakter Speichern", "");
var char_first_name = "";
var char_last_name = "";
CharCreatorMenu.AddItem(first_name_item);
CharCreatorMenu.AddItem(last_name_item);
CharCreatorMenu.AddItem(gender_selector);
CharCreatorMenu.AddItem(char_save_button);
CharCreatorMenu.OnItemSelect.connect(function (menu, item, index) {
    switch (item) {
        case first_name_item:
            char_first_name = API.getUserInput(char_first_name, 100);
            first_name_item.SetRightLabel(char_first_name);
            break;
        case last_name_item:
            char_last_name = API.getUserInput(char_last_name, 100);
            last_name_item.SetRightLabel(char_last_name);
            break;
        case char_save_button:
            API.triggerServerEvent("CharacterCreatorConfirm", gender_selector.Index, char_first_name, char_last_name);
            CharCreatorMenu.Visible = false;
            API.setCanOpenChat(true);
            break;
    }
});
gender_selector.OnListChanged.connect(function (item, newIndex) {
});
