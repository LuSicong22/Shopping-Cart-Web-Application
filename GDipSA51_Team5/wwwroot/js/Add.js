window.onload = function () {
    let elemlist = document.getElementsByClassName("add_button");

    for (let i = 0; i < elemlist.length; i++) {
        elemlist[i].addEventListener("click", onAdd)
    }
}

function onAdd(event) {
    let elem = event.currentTarget;
    let productId = elem.getAttribute("photo_id");
    sendAdd(true, productId)
}

function sendAdd(addIt, productId) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Add/Addin");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {

        if (this.readyState === XMLHttpRequest.DONE) {
            // receive response from server
            if (this.status === 200 || this.status === 302) {
                let data = JSON.parse(this.responseText);
                document.getElementById("cartNumber").textContent = data.total; //receive the total from "Add controller, Addin action" and update number to html
            }
        }
    };

    // send add choice to server
    xhr.send(JSON.stringify({
        AddIt: addIt,
        ProductId: productId
    }));
}