// Elements
const sidebarToggle = document.getElementById("sidebarToggle");
const sidebar = document.getElementById("sidebar");
const logoutBtn = document.getElementById("logout");
const mainContent = document.getElementById("mainContent");

// Toggle sidebar
sidebarToggle?.addEventListener("click", () => {
    sidebar.classList.toggle("d-none");
});

// Logout
logoutBtn?.addEventListener("click", () => {
    alert("Đăng xuất thành công !");
});

// SPA navigation
document.querySelectorAll(".sidebar .nav-link").forEach((link) => {
    link.addEventListener("click", (e) => {
        e.preventDefault();

        const pageId = link.dataset.page;

        document.querySelectorAll(".page")
            .forEach((p) => (p.style.display = "none"));

        document.getElementById(pageId).style.display = "block";

        document.querySelectorAll(".sidebar .nav-link")
            .forEach((l) => l.classList.remove("active"));

        link.classList.add("active");
    });
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
