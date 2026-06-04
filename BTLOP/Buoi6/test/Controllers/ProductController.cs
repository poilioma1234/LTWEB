using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using test.Models;
using test.Repository;

namespace test.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int? categoryId, string? q)
        {
            await LoadCategoriesAsync(categoryId);

            var products = categoryId.HasValue
                ? await _productRepository.GetByCategoryAsync(categoryId.Value)
                : await _productRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var keyword = q.Trim();
                products = products.Where(product =>
                    product.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    (product.Description?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            ViewBag.SearchTerm = q;
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyProducts()
        {
            var products = await _productRepository.GetByOwnerAsync(GetCurrentUserId());
            return View(products);
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create(Product product, IFormFile? imageUrl)
        {
            product.OwnerId = GetCurrentUserId();

            if (imageUrl != null && imageUrl.Length > 0)
            {
                product.ImageUrl = await SaveImageAsync(imageUrl);
            }

            if (ModelState.IsValid)
            {
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(MyProducts));
            }

            await LoadCategoriesAsync(product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdForOwnerAsync(id, GetCurrentUserId());

            if (product == null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageUrl)
        {
            var existingProduct = await _productRepository.GetByIdForOwnerAsync(product.Id, GetCurrentUserId());

            if (existingProduct == null)
            {
                return NotFound();
            }

            product.OwnerId = existingProduct.OwnerId;
            product.ImageUrl = existingProduct.ImageUrl;

            if (imageUrl != null && imageUrl.Length > 0)
            {
                product.ImageUrl = await SaveImageAsync(imageUrl);
            }

            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(MyProducts));
            }

            await LoadCategoriesAsync(product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdForOwnerAsync(id, GetCurrentUserId());

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdForOwnerAsync(id, GetCurrentUserId());

            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(MyProducts));
        }

        private async Task LoadCategoriesAsync(int? selectedCategoryId = null)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new InvalidOperationException("Không xác định được người dùng hiện tại.");
        }

        private static async Task<string> SaveImageAsync(IFormFile imageUrl)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(imageUrl.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageUrl.CopyToAsync(fileStream);
            }

            return "/images/" + fileName;
        }
    }
}
