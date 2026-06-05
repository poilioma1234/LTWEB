using System.Diagnostics;
using System.Security.Claims;
using GiuaKy.Data;
using GiuaKy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiuaKy.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var coursesQuery = _context.Courses.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var keyword = search.Trim();
                coursesQuery = coursesQuery.Where(course => course.Name.Contains(keyword));
            }

            var viewModel = new CourseListViewModel
            {
                Courses = await coursesQuery.OrderBy(course => course.Id).ToListAsync(),
                EnrolledCourseIds = await GetCurrentStudentEnrollmentIdsAsync(),
                Search = search,
                Title = "Danh sách học phần"
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<HashSet<int>> GetCurrentStudentEnrollmentIdsAsync()
        {
            if (!User.IsInRole("Student"))
            {
                return new HashSet<int>();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return new HashSet<int>();
            }

            var courseIds = await _context.Enrollments
                .AsNoTracking()
                .Where(enrollment => enrollment.UserId == userId)
                .Select(enrollment => enrollment.CourseId)
                .ToListAsync();

            return courseIds.ToHashSet();
        }
    }
}
