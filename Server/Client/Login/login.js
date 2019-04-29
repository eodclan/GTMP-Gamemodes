wrongLogin(); 

$("#login").on('click', function (e) {
    e.preventDefault()
    const email = $("#l-email").val();
    if (isEmpty(email)) {
        wrongLogin("Benutzername ausfüllen!");
        showError('#l-email');
        return false;
    }
    const password = $("#l-password").val();
    if (isEmpty(password)) {
        wrongLogin("Passwort ausfüllen!");
        showError('#l-password');
        return false;
    }
    
    $('#login').html("<i class='fa fa-spinner fa-spin fa-fw'></i>");
    	resourceCall("login_sendEvent", email, password);
});

$("#l-email, #l-password").on('focus', function () {
    clearErrors()
});

function badLogin() {
    $('#login').html('Fehlgeschlagen');
    wrongLogin('Ihre Zugangsdaten sind Falsch!')
}

function wrongLogin(message) {
    $(".error-message > p").empty();
    $(".error-message > p").append(message);
    $(".error-message").css('display', 'block');
}

