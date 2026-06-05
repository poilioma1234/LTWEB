using System.Security.Claims;
using GiuaKy.Data;
using GiuaKy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiuaKy.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("enroll")]
    public class EnrollController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{courseId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var courseExists = await _context.Courses.AnyAsync(course => course.Id == courseId);

            if (!courseExists)
            {
                return NotFound();
            }

            var alreadyEnrolled = await _context.Enrollments
                .AnyAsync(enrollment => enrollment.UserId == userId && enrollment.CourseId == courseId);

            if (!alreadyEnrolled)
            {
                _context.Enrollments.Add(new Enrollment
                {
                    UserId = userId,
                    CourseId = courseId,
                    EnrollDate = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("cancel/{courseId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(item => item.UserId == userId && item.CourseId == courseId);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyCourses));
        }

        [HttpGet("my-courses")]
        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var enrollments = await _context.Enrollments
                .Include(enrollment => enrollment.Course)
                .AsNoTracking()
                .Where(enrollment => enrollment.UserId == userId)
                .OrderByDescending(enrollment => enrollment.EnrollDate)
                .ToListAsync();

            return View(enrollments);
        }
    }
}
