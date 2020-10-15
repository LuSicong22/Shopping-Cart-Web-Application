window.onload = function ()
{
    let elemlist = document.getElementsByClassName("quantity");

    let itemsTotal = document.getElementsByClassName("product_total");
    let cartTotal = 0;
    for (let i = 0; i < itemsTotal.length; i++) {
        cartTotal = cartTotal + parseInt(itemsTotal[i].textContent);
    }
    document.querySelector(".grand_total").textContent = cartTotal;

    //in the event of changes is made on the quantity in the cart
    for (let i = 0; i < elemlist.length; i++) {
        elemlist[i].addEventListener('keyup', onChange)
        elemlist[i].addEventListener('mouseup', onChange)
    }

    //remove button on cart page
    let removeList = document.getElementsByClassName("remove_icon");
    for (let i = 0; i < removeList.length; i++) {
        removeList[i].addEventListener("click", confirmRemove);
    }
}

function onChange(event) {
    let elem = event.currentTarget;
    let quantity = elem.value;

    UpdateTotal(elem);

    //pass to AJAX to update the server
    let productId = elem.getAttribute("productid");
    sendChange(productId, quantity)
}

function UpdateTotal(elem) {
    //calculating the cart total and total for each items
    let quantity = elem.value;
    let unitPrice = elem.parentElement.parentElement.parentElement.querySelector(".price").textContent;
    let totalprice = parseInt(unitPrice) * parseInt(quantity);
    elem.parentElement.parentElement.parentElement.querySelector(".product_total").textContent = totalprice;
    let cartTotal = 0;
    var itemslist = document.getElementsByClassName("quantity");
    for (let i = 0; i < itemslist.length; i++) {
        cartTotal = cartTotal + parseInt(itemslist[i].value) * parseInt(itemslist[i].parentElement.parentElement.parentElement.querySelector(".price").textContent);
    }
    document.querySelector(".grand_total").textContent = cartTotal;
    return;
}

function sendChange(productId, value) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/Cart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            // receive response from server
            if (this.status === 200 || this.status === 302) {
                let data = JSON.parse(this.responseText);

                if (this.status === 200) {
                    console.log("Successful operation: " + data.success);
                }
                else if (this.status === 302) {
                    window.location = data.redirect_url;
                }
            }
        }
    };

    xhr.send(JSON.stringify({
        ProductId: productId,
        Value: value
    }));
}

function confirmRemove(event) {
    if (confirm("Confirm remove item from the cart?")) {
        //get product id for removed item
        let elem = event.currentTarget;
        let productId = elem.getAttribute("product_id");

        SendProductId(productId, elem)
    }
}

function SendProductId(productId, elem) {
    //send AJAX request to server to remove record from database
    let xhr = new XMLHttpRequest();

    //send to action method to receive AJAX call
    xhr.open("POST", "/Cart/RemoveItem");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Successful operation: " + data.success);
                elem.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.remove();
                UpdateTotal(elem);
            }
        }
    };
    //send product id to controller as identifier
    xhr.send(JSON.stringify({
        ProductId: productId,
    }));
}