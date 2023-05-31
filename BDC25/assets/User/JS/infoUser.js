function validatePassword() {
    var newPassword = document.getElementById("new-password");
    var errPassword = document.getElementById("new-password-error");
    const password = newPassword.value.trim();
    if (!password) {
        errPassword.textContent = 'Password is required';
    } else if (password.length < 8) {
        errPassword.textContent = 'Password must be at least 8 characters long';
    } else {
        errPassword.textContent = '';
    }
}

function validateConfirmPassword() {
    var newPassword = document.getElementById("new-password");
    var confirm = document.getElementById("confirm-password");
    var confirmPass = document.getElementById("conf-password-error");

    if (newPassword.value != confirm.value) {
        confirmPass.textContent = 'Passwords do not match';
    } else {
        confirmPass.textContent = '';
    }
}