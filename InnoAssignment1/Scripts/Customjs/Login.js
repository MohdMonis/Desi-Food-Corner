var logout = document.getElementById("logoutbtn");

logout.addEventListener("click", (dets) => {
    var result = confirm("Do you really want to logout?");
    if (result) {
        if (window.location.href.indexOf("/Order/ViewOrdersForStoreKeeper")>=0) {
            window.location.href = window.location.href.replace("/Order/ViewOrdersForStoreKeeper", "/Login/Logout");
        } else if (window.location.href.indexOf("/Order/ViewOrdersForDeliveryBoy")>=0) {
            window.location.href = window.location.href.replace("/Order/ViewOrdersForDeliveryBoy", "/Login/Logout");
        } else if (window.location.href.indexOf("/Order/ViewOrdersForCustomer")>=0) {
            window.location.href = window.location.href.replace("/Order/ViewOrdersForCustomer", "/Login/Logout");
        }
    }
})