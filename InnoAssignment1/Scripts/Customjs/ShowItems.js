var addItem = document.getElementById("ad-cards");
var orderItem = document.getElementById("rm-cards");
//var boxes = Array.from(document.getElementsByTagName('input'));
// ['f71fafbc-0ddd-ed11-8847-002248d5daa1', 'Rohu Fish Curry', 'Spicy Deep fried in Mustard oil', 'Non-Veg', '420.0000000000', 'Sea Food']

// ${JSON.stringify(dets.target.parentElement)}

/*<div id="f71fafbc-0ddd-ed11-8847-002248d5daa1">Rohu Fish Curry<button data-itemdets="f71fafbc-0ddd-ed11-8847-002248d5daa1,Rohu Fish Curry,Spicy Deep fried in Mustard oil,Non-Veg,420.0000000000,Sea Food" type="button" class="item">add Item</button></div>*/
addItem.addEventListener("click", (dets) => {
    if (dets.target.type == "button") {
        var itemdetails = (dets.target.dataset.itemdets).split(',');
        var html = `<div>
                        <div>
                            <h3>${itemdetails[1]}</h3>
                            <span>Discription: ${itemdetails[2]}</span> | 
                            <span>Type: ${itemdetails[3]}</span> | 
                            <span>Category: ${itemdetails[5]}</span> | 
                            <span>Price: ${itemdetails[4]}</span>
                        </div>
                        <span>
                            <label for="${itemdetails[0]}">Quantity</label>
                            <input required id="${itemdetails[0]}" name="quantity" placeholder="Enter Quantity" />
                        </span>
                        <button data-itemdets="${dets.target.dataset.itemdets}" type="button" class="rmvitem">remove Item</button>
                    </div>`;
        orderItem.insertAdjacentHTML("afterbegin", html);
        dets.target.parentElement.remove();
        boxes = Array.from(document.getElementsByTagName('input'));
        console.log(boxes);
    }
})

orderItem.addEventListener("click", (dets) => {
    if (dets.target.type == "button") {
        var item = dets.target.dataset.itemdets.split(',');
        var itemtag = `<div id="${item[0]}">${item[1]}<button data-itemdets="${dets.target.dataset.itemdets}" type="button" class="item">add Item</button></div>`;
        addItem.insertAdjacentHTML("beforeend", itemtag);
        dets.target.parentElement.remove();
        boxes = Array.from(document.getElementsByTagName('input'));
        console.log(boxes);
    }
})