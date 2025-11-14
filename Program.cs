using ??_Án_L?p_Trình_Web_Bán_Th?c_?n_Nhanh.Models;
using DoAnLapTrinhWebBanThucAnNhanh.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Thêm MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// K?t n?i Database SQL Server
builder.Services.AddDbContext<FastFoodContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FastFoodContext")));

var app = builder.Build();

// C?u hình pipeline x? lý request
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ??nh ngh?a route m?c ??nh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
