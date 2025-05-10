document.addEventListener("DOMContentLoaded", function () {
  const slider = document.querySelector(".slider");
  const slides = document.querySelectorAll(".slider img");
  let index = 0;

  function nextSlide() {
    index = (index + 1) % slides.length;
    slider.style.transform = `translateX(-${index * 100}%)`;
  }

  setInterval(nextSlide, 4000);
  //Logic for searchbox
  const searchbox = document.querySelector(".searchbox");
  const items = document.querySelectorAll(".item");
  searchbox.addEventListener("input", function () {
    const search = searchbox.value.toLowerCase();
    items.forEach((item) => {
      const productName = item
        .querySelector(".label")
        .textContent.toLowerCase();
      if (productName.includes(search)) {
        item.style.display = "";
      } else {
        item.style.display = "none";
      }
    });
  });
});

function filterProducts(type) {
  let items = document.querySelectorAll(".item");
  items.forEach((item) => {
    if (type === "all" || item.getAttribute("data-type") === type) {
      item.style.display = "flex";
    } else {
      item.style.display = "none";
    }
  });
}

function showPopup(button) {
  const popup = document.getElementById("quantityPopup");
  const quantityInput = document.querySelector("input[name='Quantity']");
  const productName = document.getElementById("productName");

  popup.style.display = "flex";
  quantityInput.value = 1;

  const product = button.closest(".item").querySelector(".label").textContent;
  const hiddenProductId = document.querySelector("input[name='ProductId']");
  const productId = parseInt(button.closest(".item").dataset.id);
  hiddenProductId.value = productId;
  productName.textContent = product;
}

function closePopup() {
  document.getElementById("quantityPopup").style.display = "none";
}

function increaseQuantity() {
  const quantityInput = document.querySelector("input[name='Quantity']");
  quantityInput.value = parseInt(quantityInput.value) + 1;
}

function decreaseQuantity() {
  const quantityInput = document.querySelector("input[name='Quantity']");
  quantityInput.value = parseInt(quantityInput.value) - 1;
}

document.getElementById("cancel-button").addEventListener("click", function () {
  closePopup();
});

document
  .getElementById("increase-button")
  .addEventListener("click", function () {
    increaseQuantity();
  });

document
  .getElementById("decrease-button")
  .addEventListener("click", function () {
    decreaseQuantity();
  });

function ShowChat() {
  const chat = document.querySelector(".chat");
  chat.style.display = "flex";
  const chatmessages = document.getElementById("chat-messages");
  chatmessages.scrollTop = chatmessages.scrollHeight;
}
function CloseChat() {
  const chat = document.querySelector(".chat");
  chat.style.display = "none";
}
window.onload = function () {
  const chatmessages = document.getElementById("chat-messages");
  chatmessages.scrollTop = chatmessages.scrollHeight;
};
