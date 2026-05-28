using BTVN5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTVN5.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId)
        {
            List<Category> categories = _context.Categories
                .Include(c => c.Books)
                .ToList();

            ViewBag.Categories = categories;

            IQueryable<Book> query = _context.Books
                .Include(b => b.Category);

            if (categoryId != null)
            {
                query = query.Where(b => b.CategoryId == categoryId);
            }

            List<Book> books = query.ToList();

            return View(books);
        }

        public IActionResult Details(int id)
        {
            Book? book = _context.Books
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories
                .Include(c => c.Books)
                .ToList();

            return View(book);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageFile != null && book.ImageFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "Content",
                        "ImageBooks"
                    );

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileExtension = Path.GetExtension(book.ImageFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        book.ImageFile.CopyTo(stream);
                    }

                    book.Image = fileName;
                }

                _context.Books.Add(book);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName",
                book.CategoryId
            );

            return View(book);
        }

        public IActionResult Edit(int id)
        {
            Book? book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName",
                book.CategoryId
            );

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageFile != null && book.ImageFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "Content",
                        "ImageBooks"
                    );

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileExtension = Path.GetExtension(book.ImageFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + fileExtension;
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        book.ImageFile.CopyTo(stream);
                    }

                    book.Image = fileName;
                }

                _context.Books.Update(book);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(
                _context.Categories.ToList(),
                "CategoryId",
                "CategoryName",
                book.CategoryId
            );

            return View(book);
        }

        public IActionResult Delete(int id)
        {
            Book? book = _context.Books
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Book? book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}