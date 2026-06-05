using System.Security.Claims;
using GiuaKy.Data;
using GiuaKy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiuaKy.Controllers
{
    [AllowAnonymous]
    [Route("courses")]
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
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
                Title = "Học phần"
            };

            return View("~/Views/Home/Index.cshtml", viewModel);
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
