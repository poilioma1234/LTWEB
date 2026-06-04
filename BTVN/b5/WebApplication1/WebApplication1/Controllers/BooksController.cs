using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BooksController : Controller
    {
        private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var books = _context.Books
                .Include(book => book.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                books = books.Where(book => book.CategoryId == categoryId.Value);
            }

            ViewBag.SelectedCategoryId = categoryId;

            return View(await books.OrderBy(book => book.Title).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(item => item.Category)
                .FirstOrDefaultAsync(item => item.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.RequireImage = true;
            await LoadCategoriesAsync();
            return View(new Book());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            ValidateImage(book, imageRequired: true);

            if (!ModelState.IsValid)
            {
                ViewBag.RequireImage = true;
                await LoadCategoriesAsync(book.CategoryId);
                return View(book);
            }

            book.Image = await SaveImageAsync(book.ImageFile);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync(book.CategoryId);
            ViewBag.RequireImage = false;
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            var existingBook = await _context.Books.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            if (existingBook == null)
            {
                return NotFound();
            }

            ValidateImage(book, imageRequired: false);

            if (!ModelState.IsValid)
            {
                book.Image = existingBook.Image;
                ViewBag.RequireImage = false;
                await LoadCategoriesAsync(book.CategoryId);
                return View(book);
            }

            if (book.ImageFile != null && book.ImageFile.Length > 0)
            {
                DeleteImage(existingBook.Image);
                book.Image = await SaveImageAsync(book.ImageFile);
            }
            else
            {
                book.Image = existingBook.Image;
            }

            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(item => item.Category)
                .FirstOrDefaultAsync(item => item.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                DeleteImage(book.Image);
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategoriesAsync(int? selectedCategoryId = null)
        {
            var categories = await _context.Categories
                .OrderBy(category => category.CategoryId)
                .ToListAsync();

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", selectedCategoryId);
        }

        private void ValidateImage(Book book, bool imageRequired)
        {
            if (book.ImageFile == null || book.ImageFile.Length == 0)
            {
                if (imageRequired)
                {
                    ModelState.AddModelError(nameof(Book.ImageFile), "Vui lòng chọn hình ảnh sách.");
                }

                return;
            }

            var extension = Path.GetExtension(book.ImageFile.FileName).ToLowerInvariant();

            if (!AllowedImageExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(Book.ImageFile), "Chỉ chấp nhận file ảnh jpg, jpeg, png, gif hoặc webp.");
            }
        }

        private async Task<string> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return "default-book.png";
            }

            var imageFolder = GetImageFolder();
            Directory.CreateDirectory(imageFolder);

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(imageFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }

        private void DeleteImage(string? imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName) || imageName == "default-book.png")
            {
                return;
            }

            var filePath = Path.Combine(GetImageFolder(), imageName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private string GetImageFolder()
        {
            return Path.Combine(_environment.WebRootPath, "Content", "ImageBooks");
        }
    }
}
