(() => {
  "use strict";

  // =========================================================
  // 0) CẤU HÌNH CHUNG
  // =========================================================

  // ✅ Map "key" (menu) -> "Action" trong AdminController
  const PAGE_MAP = {
    dashboard: "Dashboard",
    products: "ProductList",
    categories: "CategoryList",
    orders: "OrderList",
    users: "UserList",
    contacts: "ContactList"
  };

  // ✅ ID vùng render nội dung (PartialView) trong Admin SPA
  const MAIN_CONTAINER_ID = "mainContent"; // <- nếu bạn dùng id khác, sửa tại đây

  // =========================================================
  // 1) HELPER DOM
  // =========================================================

  // Lấy element vùng render
  const mainEl = () => document.getElementById(MAIN_CONTAINER_ID);

  // Hiển thị loading spinner khi đang fetch
  function showLoading(text) {
    const el = mainEl();
    if (!el) return;

    el.innerHTML = `
      <div class="text-center mt-5">
        <div class="spinner-border text-primary"></div>
        <p class="fw-bold mt-2">${text || "Đang tải..."}</p>
      </div>`;
  }

  // Set active class cho menu sidebar theo key
  function setActiveMenuByKey(key) {
    document.querySelectorAll(".nav-link").forEach(x => x.classList.remove("active"));
    document.querySelector(`.nav-link[data-page="${key}"]`)?.classList.add("active");
  }

  // Bind click cho sidebar: click menu -> load partial tương ứng
  function bindSidebar() {
    document.querySelectorAll(".nav-link[data-page]").forEach(link => {
      link.addEventListener("click", e => {
        e.preventDefault();
        const pageKey = (link.dataset.page || "").toLowerCase();

        loadPage(pageKey);
        setActiveMenuByKey(pageKey);
      });
    });
  }

  // Toggle sidebar (thu gọn / mở rộng)
  function bindToggleSidebar() {
    const btn = document.getElementById("toggleSidebar");
    if (!btn) return;

    btn.addEventListener("click", () => {
      document.getElementById("sidebar")?.classList.toggle("hidden");
      document.querySelector(".main-content")?.classList.toggle("full-width");
      document.querySelector(".navbar")?.classList.toggle("full-width");
    });
  }

  // =========================================================
  // 2) ANTI-FORGERY TOKEN + HEADERS
  // =========================================================

  // Lấy token trực tiếp từ form đang submit (Create/Edit)
  function getTokenFromForm(form) {
    return form?.querySelector('input[name="__RequestVerificationToken"]')?.value || null;
  }

  // Lấy token từ 1 form ẩn chung trong layout (dùng cho Delete)
  function getTokenFromHiddenForm() {
    return document.querySelector('#antiForgeryForm input[name="__RequestVerificationToken"]')?.value || null;
  }

  // Header chung cho fetch:
  // - X-Requested-With giúp server biết đây là request AJAX
  // - RequestVerificationToken dùng cho [ValidateAntiForgeryToken]
  function baseHeaders(token) {
    const h = { "X-Requested-With": "XMLHttpRequest" };
    if (token) h["RequestVerificationToken"] = token;
    return h;
  }

  // =========================================================
  // 3) LOAD PAGE (SPA) - GET PartialView
  // =========================================================

  function loadPage(pageKey) {
    const action = PAGE_MAP[pageKey];
    const el = mainEl();

    if (!el) return;

    if (!action) {
      el.innerHTML = `<div class="alert alert-danger mt-3">Page không tồn tại: ${pageKey}</div>`;
      return;
    }

    showLoading("Đang tải dữ liệu...");

    // ✅ Fetch partial GET
    fetch(`/Admin/${action}`, { cache: "no-store", headers: baseHeaders() })
      .then(r => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`);
        return r.text();
      })
      .then(html => el.innerHTML = html)
      .catch(err => el.innerHTML = `<div class="alert alert-danger mt-3">${String(err.message || err)}</div>`);
  }

  // =========================================================
  // 4) PRODUCTS CRUD - GET/POST cho Create/Edit/Delete
  // =========================================================

  // -------------------------
  // 4.1) OPEN CREATE (GET)
  // -------------------------
  // Mở form tạo sản phẩm: Controller trả về PartialView("_CreateProduct")
  window.openCreateProduct = function () {
    const el = mainEl();
    if (!el) return;

    showLoading("Đang tải form...");

    fetch("/Admin/CreateProduct", { cache: "no-store", headers: baseHeaders() })
      .then(r => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`);
        return r.text();
      })
      .then(html => el.innerHTML = html)
      .catch(err => el.innerHTML = `<div class="alert alert-danger mt-3">Lỗi load Create: ${String(err.message || err)}</div>`);
  };

  // -------------------------
  // 4.2) SUBMIT CREATE (POST)
  // -------------------------
  // Submit form tạo sản phẩm:
  // - dùng FormData để gửi cả file upload
  // - gửi token chống CSRF
  window.submitCreateProduct = function (form) {
    const el = mainEl();
    if (!el) return false;

    showLoading("Đang lưu sản phẩm...");

    const fd = new FormData(form);
    const token = getTokenFromForm(form);

    fetch("/Admin/CreateProduct", {
      method: "POST",
      body: fd,
      headers: baseHeaders(token),
      credentials: "same-origin"
    })
      .then(async r => {
        const text = await r.text();
        if (!r.ok) throw new Error(text || `HTTP ${r.status}`);
        return text;
      })
      .then(html => {
        // ✅ Controller thường return ProductList() => trả list mới
        el.innerHTML = `<div class="alert alert-success mt-3">✅ Thêm sản phẩm thành công</div>` + html;
        setActiveMenuByKey("products");
      })
      .catch(err => {
        el.innerHTML = `<div class="alert alert-danger mt-3">❌ Lưu thất bại:<br/>${String(err.message || err)}</div>`;
      });

    return false; // ✅ chặn reload trang
  };

  // -------------------------
  // 4.3) OPEN EDIT (GET)
  // -------------------------
  // Load form edit theo id: /Admin/EditProduct?id=123
  window.openEditProduct = function (id) {
    const el = mainEl();
    if (!el) return;

    showLoading("Đang tải sản phẩm...");

    fetch(`/Admin/EditProduct?id=${encodeURIComponent(id)}`, { cache: "no-store", headers: baseHeaders() })
      .then(r => {
        if (!r.ok) throw new Error(`HTTP ${r.status}`);
        return r.text();
      })
      .then(html => el.innerHTML = html)
      .catch(err => el.innerHTML = `<div class="alert alert-danger mt-3">Lỗi load Edit: ${String(err.message || err)}</div>`);
  };

  // -------------------------
  // 4.4) SUBMIT EDIT (POST)
  // -------------------------
  window.submitEditProduct = function (form) {
    const el = mainEl();
    if (!el) return false;

    showLoading("Đang lưu thay đổi...");

    const fd = new FormData(form);
    const token = getTokenFromForm(form);

    fetch("/Admin/EditProduct", {
      method: "POST",
      body: fd,
      headers: baseHeaders(token),
      credentials: "same-origin"
    })
      .then(async r => {
        const text = await r.text();
        if (!r.ok) throw new Error(text || `HTTP ${r.status}`);
        return text;
      })
      .then(html => {
        el.innerHTML = `<div class="alert alert-success mt-3">✅ Cập nhật thành công</div>` + html;
        setActiveMenuByKey("products");
      })
      .catch(err => {
        el.innerHTML = `<div class="alert alert-danger mt-3">❌ Lưu thất bại:<br/>${String(err.message || err)}</div>`;
      });

    return false;
  };

  // -------------------------
  // 4.5) DELETE (POST)
  // -------------------------
  // Xóa theo id:
  // - lấy token từ antiForgeryForm ẩn
  // - gửi FormData { id }
  window.deleteProduct = function (id) {
    if (!confirm("Bạn có chắc muốn xoá sản phẩm này?")) return;

    const el = mainEl();
    if (!el) return;

    const token = getTokenFromHiddenForm();
    const fd = new FormData();
    fd.append("id", id);

    showLoading("Đang xoá...");

    fetch("/Admin/DeleteProduct", {
      method: "POST",
      body: fd,
      headers: baseHeaders(token),
      credentials: "same-origin"
    })
      .then(async r => {
        const text = await r.text();
        if (!r.ok) throw new Error(text || `HTTP ${r.status}`);
        return text;
      })
      .then(html => {
        el.innerHTML = `<div class="alert alert-success mt-3">✅ Đã xoá</div>` + html;
        setActiveMenuByKey("products");
      })
      .catch(err => alert("Xoá thất bại: " + String(err.message || err)));
  };

  // =========================================================
  // 5) INIT APP
  // =========================================================
  document.addEventListener("DOMContentLoaded", () => {
    bindSidebar();
    bindToggleSidebar();

    // Load dashboard mặc định
    loadPage("dashboard");
    setActiveMenuByKey("dashboard");
  });

  // =========================================================
  // 6) EXPOSE (nếu cần gọi từ HTML)
  // =========================================================
  window.loadPage = loadPage;

})();