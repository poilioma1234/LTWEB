using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                UserCount = await _context.Users.CountAsync(),
                ProductCount = await _context.Products.CountAsync(),
                CategoryCount = await _context.Categories.CountAsync(),
                MemberProductCount = await _context.Products.CountAsync(product => product.OwnerId != null)
            };

            return View(model);
        }
    }
}
