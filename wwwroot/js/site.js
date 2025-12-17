// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


const sidebar = document.getElementById('sidebar');
const sidebarToggle = document.getElementById('sidebarToggle');
const logoutBtn = document.getElementById('logoutBtn');

// Cart
 function formatCurrency(num) {
   return num.toLocaleString('vi-VN') + ' ₫';
 }

 function updateCart() {
   let rows = document.querySelectorAll("#cartTable tbody tr");
   let grandTotal = 0;

   rows.forEach(row => {
     let price = parseInt(row.querySelector(".price").getAttribute("data-val"));
     let qty = parseInt(row.querySelector(".qty").value);
     let total = price * qty;
     row.querySelector(".total").textContent = formatCurrency(total);
     grandTotal += total;
   });

   document.getElementById("grandTotal").textContent = formatCurrency(grandTotal);
 }

 document.querySelectorAll(".qty").forEach(input => {
   input.addEventListener("change", updateCart);
 });

 document.querySelectorAll(".deleteBtn").forEach(btn => {
   btn.addEventListener("click", function () {
     this.closest("tr").remove();
     updateCart();
   });
 });