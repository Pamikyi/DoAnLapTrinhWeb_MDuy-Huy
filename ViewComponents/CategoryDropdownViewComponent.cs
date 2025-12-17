using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnLapTrinhWebBanThucAnNhanh.Models;

namespace DoAnLapTrinhWebBanThucAnNhanh.ViewComponents
{
    public class CategoryDropdownViewComponent : ViewComponent
    {
        private readonly FastFoodDbContext _context;
        public CategoryDropdownViewComponent(FastFoodDbContext context) => _context = context;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return View(categories);
        }
    }
}
