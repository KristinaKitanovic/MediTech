function showPopup() {
  const popup = document.querySelector("div[name='CheckoutPopup']");
  popup.style.display = "flex";
  document.querySelector("input[name='TotalPrice']").value =
    document.getElementById("totalPrice").innerText;
}

function closePopup() {
  const popup = document.querySelector("div[name='CheckoutPopup']");
  popup.style.display = "none";
}
