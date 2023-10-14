var username = document.querySelector('#username')
var email = document.querySelector('#email')
var password = document.querySelector('#password')
var cfnPassword = document.querySelector('#cfPassword')
var form = document.querySelector('form')

function showError(input, message) {
    let parent = input.parentElement;
    let small = parent.querySelector('small')
    parent.classList.add('Error')
    small.innerText = message
}

function showSuccess(input) {
    let parent = input.parentElement;
    let small = parent.querySelector('small')
    parent.classList.remove('Error')
    small.innerText = ''
}

function checkEmptyError(listInput) {
    let isEmptyError = false;
    listInput.forEach(input => {
        input.value = input.value.trim()

        if (!input.value) {
            isEmptyError = true;
            showError(input, 'Khong duoc de trong')
        } else {
            showSuccess(input)
        }
    });
    return isEmptyError
}



function checkEmailError(input) {
    const regexEmail =
        /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;

    input.value = input.value.trim();

    let isEmailError = !regexEmail.test(input.value);

    if (regexEmail.test(input.value)) {
        showSuccess(input);
    } else {
        showError(input, 'Email validate');
    }

    return isEmailError;
}

function checklenghError(input, min, max) {
    input.value = input.value.trim();
    if (input.value.length < min) {
        showError(input, `Phai có it nhat ${min} ky tu`)
        return true;
    }

    if (input.value.length > max) {
        showError(input, `Khong duoc qua ${max} ky tu`)
        return true;
    }

    return false;
}

function checkMatchPasswordError(passwordInput, cfnPasswordInput) {
    if (passwordInput.value !== cfnPasswordInput.value) {
        showError(cfnPasswordInput, 'Mat khau khong trung khop')
        return true
    }
    return false
}

form.addEventListener('submit', function (e) {
    e.preventDefault();



    let isEmptyError = checkEmptyError([username, email, password, cfnPassword])
    let isEmailError = checkEmailError(email);
    let isUsernameLengthError = checklenghError(username, 3, 10);
    let isPasswordLengthError = checklenghError(password, 3, 10);
    let isMatchError = checkMatchPasswordError(password, cfnPassword)

    if (isEmptyError || isEmailError || isUsernameLengthError || isPasswordLengthError || isMatchError) {
        //không làm gì hết 
    } else {
        //mã khác
    }
});

