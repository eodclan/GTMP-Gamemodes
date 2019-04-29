API.onResourceStart.connect(function () {
    var id1 = API.every(60000, "callthisfunctionwhatever");
});

function callthisfunctionwhatever() {
    API.triggerServerEvent("AddPayDayMinute");
    API.triggerServerEvent("LevelUp");
}