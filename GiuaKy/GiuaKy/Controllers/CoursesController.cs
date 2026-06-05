using GiuaKy.Data;
using GiuaKy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiuaKy.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/courses")]
    public class CoursesController : Controller
    {
        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CoursesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(course => course.Category)
                .AsNoTracking()
                .OrderBy(course => course.Id)
                .ToListAsync();

            return View(courses);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View(new CourseFormViewModel());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseFormViewModel model)
        {
            if (model.ImageFile == null)
            {
                ModelState.AddModelError(nameof(model.ImageFile), "Vui lòng chọn hình ảnh minh họa.");
            }
            else if (!IsValidImage(model.ImageFile))
            {
                ModelState.AddModelError(nameof(model.ImageFile), "Chỉ chấp nhận ảnh .jpg, .jpeg, .png, .gif hoặc .webp.");
            }

            if (!await CategoryExistsAsync(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Danh mục không hợp lệ.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model.CategoryId);
                return View(model);
            }

            var course = new Course
            {
                Name = model.Name.Trim(),
                Credits = model.Credits,
                Lecturer = model.Lecturer.Trim(),
                CategoryId = model.CategoryId,
                Image = await SaveImageAsync(model.ImageFile!)
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync(course.CategoryId);

            return View(new CourseFormViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Credits = course.Credits,
                Lecturer = course.Lecturer,
                CategoryId = course.CategoryId,
                CurrentImage = course.Image
            });
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!await CategoryExistsAsync(model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Danh mục không hợp lệ.");
            }

            if (model.ImageFile != null && !IsValidImage(model.ImageFile))
            {
                ModelState.AddModelError(nameof(model.ImageFile), "Chỉ chấp nhận ảnh .jpg, .jpeg, .png, .gif hoặc .webp.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(model.CategoryId);
                return View(model);
            }

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            course.Name = model.Name.Trim();
            course.Credits = model.Credits;
            course.Lecturer = model.Lecturer.Trim();
            course.CategoryId = model.CategoryId;

            if (model.ImageFile != null)
            {
                var oldImage = course.Image;
                course.Image = await SaveImageAsync(model.ImageFile);
                DeleteUploadedImage(oldImage);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses
                .Include(item => item.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            DeleteUploadedImage(course.Image);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategoriesAsync(int? selectedCategoryId = null)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(category => category.Name)
                .Select(category => new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name,
                    Selected = selectedCategoryId == category.Id
                })
                .ToListAsync();

            ViewBag.Categories = categories;
        }

        private async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(category => category.Id == categoryId);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/images/uploads/{fileName}";
        }

        private static bool IsValidImage(IFormFile imageFile)
        {
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            return AllowedImageExtensions.Contains(extension);
        }

        private void DeleteUploadedImage(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || !imagePath.StartsWith("/images/uploads/", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fileName = Path.GetFileName(imagePath);
            var filePath = Path.Combine(_environment.WebRootPath, "images", "uploads", fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
