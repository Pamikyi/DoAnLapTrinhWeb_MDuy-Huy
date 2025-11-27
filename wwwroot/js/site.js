// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const sidebar = document.getElementById('sidebar');
const mainContent = document.getElementById('mainContent');
const sidebarToggle = document.getElementById('sidebarToggle');
const logoutBtn = document.getElementById('logoutBtn');

// Toggle sidebar
sidebarToggle.addEventListener('click', () => {
    sidebar.classList.toggle('hidden');
    mainContent.classList.toggle('full-width');
});

// Logout
logoutBtn.addEventListener('click', () => alert('Đăng xuất (demo)'));

// SPA navigation
document.querySelectorAll('.sidebar .nav-link').forEach(link => {
    link.addEventListener('click', e => {
        e.preventDefault();
        const pageId = link.dataset.page;
        document.querySelectorAll('.page').forEach(p => p.style.display = 'none');
        document.getElementById(pageId).style.display = 'block';
        document.querySelectorAll('.sidebar .nav-link').forEach(l => l.classList.remove('active'));
        link.classList.add('active');
    });
});

// Chart Dashboard
const ctx = document.getElementById('chartRevenue');
new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6'],
        datasets: [{ label: 'Doanh thu (triệu VNĐ)', data: [10, 14, 9, 16, 20, 25], backgroundColor: 'rgba(54,162,235,0.7)' }]
    }
});

// Login
document.getElementById("loginForm").addEventListener("submit", function (event) {
    event.preventDefault(); // Ngăn tải lại trang

    let user = document.getElementById("username").value;
    let pass = document.getElementById("password").value;

    // Kiểm tra tài khoản admin
    if (user === "admin" && pass === "admin123") {
        window.location.href = "admin.html"; // chuyển sang trang admin
    }
    // Kiểm tra tài khoản người dùng
    else if (user === "user" && pass === "user123") {
        window.location.href = "index.html"; // chuyển sang trang chủ
    }
    // Sai thông tin
    else {
        document.getElementById("error").classList.remove("d-none");
    }
});

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

 updateCart();
