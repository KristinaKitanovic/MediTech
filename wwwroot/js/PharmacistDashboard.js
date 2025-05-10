function showPopup(button) {
  const popup = document.getElementById("table-popup");
  popup.style.display = "block";
  const IdInput = document.querySelector('.popup input[name="IdInput"]');

  const productName = document.getElementById("productName");
  const row = button.closest("tr");

  IdInput.value = row.querySelector("td:first-child").textContent;
  productName.textContent = row.querySelector("td:nth-child(2)").textContent;
}

function closePopup() {
  const popup = document.getElementById("table-popup");
  popup.style.display = "none";
}

function editProduct(
  id,
  label,
  price,
  description,
  type,
  expirationDate,
  quantity,
) {
  // Show the edit window and hide the add window
  document.querySelector(".edit-window").style.display = "block";
  document.querySelector(".add-window").style.display = "none";

  // Populate the form fields
  document.querySelector('.edit-window input[name="IdInput"]').value = id;
  document.querySelector('.edit-window input[name="Label"]').value = label;
  document.querySelector('.edit-window input[name="Price"]').value = price;
  document.querySelector('.edit-window input[name="Description"]').value =
    description;
  document.querySelector('.edit-window select[name="Type"]').value = type;

  // Set values for quantity and expiration date
  document.querySelector('.edit-window input[name="ExpirationDate"]').value =
    expirationDate;
  document.querySelector('.edit-window input[name="Quantity"]').value =
    quantity;
}

function saveChanges() {
  document.querySelector(".edit-window").style.display = "none";
  document.querySelector(".add-window").style.display = "block";
}

document.addEventListener("DOMContentLoaded", function () {
  const filterSelect = document.querySelector("select[name='Filter']");
  const rows = document.querySelectorAll(".table-item");
  filterSelect.addEventListener("change", function () {
    const selectedType = filterSelect.value.toLowerCase();

    for (const row of rows) {
      const productType = row
        .querySelector("td:nth-child(5)")
        .textContent.toLowerCase();
      if (
        productType === selectedType ||
        selectedType === "all" ||
        selectedType === ""
      ) {
        row.style.display = "";
      } else {
        row.style.display = "none";
      }
    }
  });
});
document.addEventListener("DOMContentLoaded", function () {
  const sortSelect = document.querySelector("select[name='Sort']");
  const orderSelect = document.querySelector("select[name='Order']");
  const tableBody = document.querySelector("tbody");
  const rows = Array.from(document.querySelectorAll(".table-item"));

  function sortTable() {
    const orderDirection = orderSelect.value;
    if (orderDirection === "" || orderDirection === "ascending") {
      sortTableAsc();
    } else {
      sortTableDesc();
    }
  }
  function sortTableAsc() {
    const sortBy = sortSelect.value;
    if (!sortBy) return;

    rows.sort((rowA, rowB) => {
      let valA, valB;

      switch (sortBy) {
        case "ExpirationDate":
          valA = new Date(rowA.querySelector("td:nth-child(7)").textContent);
          valB = new Date(rowB.querySelector("td:nth-child(7)").textContent);
          return valA - valB;
        case "Quantity":
          valA = parseInt(rowA.querySelector("td:nth-child(8)").textContent);
          valB = parseInt(rowB.querySelector("td:nth-child(8)").textContent);
          return valA - valB;
        case "Price":
          valA = parseFloat(rowA.querySelector("td:nth-child(3)").textContent);
          valB = parseFloat(rowB.querySelector("td:nth-child(3)").textContent);
          return valA - valB;
        case "Name":
          valA = rowA
            .querySelector("td:nth-child(2)")
            .textContent.toLowerCase();
          valB = rowB
            .querySelector("td:nth-child(2)")
            .textContent.toLowerCase();
          return valA.localeCompare(valB);
        case "Type":
          valA = rowA
            .querySelector("td:nth-child(5)")
            .textContent.toLowerCase();
          valB = rowB
            .querySelector("td:nth-child(5)")
            .textContent.toLowerCase();
          return valA.localeCompare(valB);
      }
    });

    rows.forEach((row) => tableBody.appendChild(row));
  }
  function sortTableDesc() {
    const sortBy = sortSelect.value;
    if (!sortBy) return;

    rows.sort((rowA, rowB) => {
      let valA, valB;

      switch (sortBy) {
        case "ExpirationDate":
          valA = new Date(rowA.querySelector("td:nth-child(7)").textContent);
          valB = new Date(rowB.querySelector("td:nth-child(7)").textContent);
          return valB - valA;
        case "Quantity":
          valA = parseInt(rowA.querySelector("td:nth-child(8)").textContent);
          valB = parseInt(rowB.querySelector("td:nth-child(8)").textContent);
          return valB - valA;
        case "Price":
          valA = parseFloat(rowA.querySelector("td:nth-child(3)").textContent);
          valB = parseFloat(rowB.querySelector("td:nth-child(3)").textContent);
          return valB - valA;
        case "Name":
          valA = rowA
            .querySelector("td:nth-child(2)")
            .textContent.toLowerCase();
          valB = rowB
            .querySelector("td:nth-child(2)")
            .textContent.toLowerCase();
          return valB.localeCompare(valA);
        case "Type":
          valA = rowA
            .querySelector("td:nth-child(5)")
            .textContent.toLowerCase();
          valB = rowB
            .querySelector("td:nth-child(5)")
            .textContent.toLowerCase();
          return valB.localeCompare(valA);
      }
    });

    rows.forEach((row) => tableBody.appendChild(row));
  }

  sortSelect.addEventListener("change", sortTable);
  orderSelect.addEventListener("change", sortTable);
});

document.addEventListener("DOMContentLoaded", function () {
  const rows = document.querySelectorAll(".table-item");
  const today = new Date();
  today.setHours(0, 0, 0, 0); // Resetujemo vreme na 00:00:00

  for (const row of rows) {
    const expirationDate = new Date(
      row.querySelector("td:nth-child(7)").textContent,
    );
    expirationDate.setHours(0, 0, 0, 0); // Resetujemo vreme i ovde

    const timeDiff = (expirationDate - today) / (24 * 60 * 60 * 1000); // Prava razlika u danima

    if (timeDiff < 0) {
      // Ako je rok prošao
      row.style.backgroundColor = "#FF6F61";
    } else if (timeDiff < 10) {
      // Ako rok ističe za manje od 10 dana
      row.style.backgroundColor = "#F4A7B9";
    }
  }
});

function Scroll() {
  const chatmessages = document.getElementById("chat-messages-ph");
  chatmessages.scrollTop = chatmessages.scrollHeight;
}
window.onload = function () {
  const chatmessages = document.getElementById("chat-messages-ph");
  chatmessages.scrollTop = chatmessages.scrollHeight;
};

function generateCode() {
  let allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
  let code = "";
  for (let i = 0; i < 6; i++) {
    let randomIndex = Math.floor(Math.random() * allowedChars.length);
    code += allowedChars[randomIndex];
  }
  const generatedCode = document.getElementById("generatedCode");
  generatedCode.value = code;
}
