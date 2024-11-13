function validateEmail(inputId, errorMessageId, errorMessage) {
    var email = $('#' + inputId).val().trim();
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a - zA - Z0 - 9 -]) +\.)+([a-zA-Z0-9]{2,4})+$/;
    let isEmail = regex.test(email)
    if (!isEmail) {
        $('#' + errorMessageId).text(errorMessage);
    } else if (email.length > 150) {
        $('#' + errorMessageId).text('Max 150 character')
    } else {
        $('#' + errorMessageId).text('');
    }
    return isEmail;
}


