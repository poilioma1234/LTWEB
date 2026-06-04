using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? selectedCategoryId)
        {
            ViewBag.SelectedCategoryId = selectedCategoryId;

            var categories = await _context.Categories
                .Select(category => new CategorySummary
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    BookCount = category.Books.Count
                })
                .OrderBy(category => category.CategoryId)
                .ToListAsync();

            return View(categories);
        }
    }
}
