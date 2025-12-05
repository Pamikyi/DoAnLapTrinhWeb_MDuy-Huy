// Elements
const sidebarToggle = document.getElementById("sidebarToggle");
const sidebar = document.getElementById("sidebar");
const logoutBtn = document.getElementById("logout");
const mainContent = document.getElementById("mainContent");

// Toggle sidebar
sidebarToggle.addEventListener("click", () => {
    sidebar.classList.toggle("d-none");
    mainContent.classList.toggle("full-width");
});

// Logout
logoutBtn?.addEventListener("click", () => {
    alert("Đăng xuất thành công !");
});

// Chart Dashboard
const ctx = document.getElementById("chartRevenue");
if (ctx) {
    new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["T1", "T2", "T3", "T4", "T5", "T6"],
            datasets: [
                {
                    label: "Doanh thu (triệu VNĐ)",
                    data: [10, 14, 9, 16, 20, 25],
                    backgroundColor: "rgba(54,162,235,0.7)",
                },
            ],
        },
    });
}

function loadProducts() {
    fetch("/Admin/Products")
        .then(res => res.json())
        .then(data => {

            let html = "";
            let i = 1;

            data.forEach(p => {
                html += `
                <tr>
                    <td>${i++}</td>
                    <td>${p.productName}</td>
                    <td>${p.categoryName}</td>
                    <td>${p.price.toLocaleString()} ₫</td>
                    <td>
                        ${p.isAvailable 
                            ? '<span class="badge bg-success">Còn</span>'
                            : '<span class="badge bg-secondary">Hết</span>'}
                    </td>
                    <td>
                        <a href="/Products/Edit/${p.productID}" class="btn btn-warning btn-sm">
                            <i class="bi bi-pencil"></i>
                        </a>
                        <a href="/Products/Delete/${p.productID}" class="btn btn-danger btn-sm">
                            <i class="bi bi-trash"></i>
                        </a>
                    </td>
                </tr>`;
            });

            document.getElementById("productTable").innerHTML = html;
        });
}