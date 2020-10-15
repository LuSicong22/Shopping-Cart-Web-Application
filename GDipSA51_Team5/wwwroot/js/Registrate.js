window.onload = function () {
    let errDiv = document.getElementById("err_msg");

    let form = document.getElementById("form");
    form.onsubmit = function () {
        let elemUname = document.getElementById("username");
        let elemPWD1 = document.getElementById("NewPWD");
        let elemPWD2 = document.getElementById("ConfirmedPWD");

        let username = elemUname.value.trim();
        let NewPWD = elemPWD1.value.trim();
        let ConfirmedPWD = elemPWD2.value.trim();

        if (username.length === 0 || ConfirmedPWD.length === 0 || NewPWD === 0) {
            errDiv.innerHTML = "Please fill up all fields.";
            return false; 
        }
        return true;
    }

    let elem = document.getElementsByClassName("form-control");
    for (let i = 0; i < elem.length; i++) {

        elem[i].onfocus = function () {
            errDiv.innerHTML = "";//delete the error message.
        }
    }
}