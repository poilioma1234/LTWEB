using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderBy(user => user.UserName).ToListAsync();
            var model = new List<AdminUserListItemViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new AdminUserListItemViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    Address = user.Address,
                    Roles = string.Join(", ", roles),
                    ProductCount = await _context.Products.CountAsync(product => product.OwnerId == user.Id)
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new AdminUserListItemViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Address = user.Address,
                Roles = string.Join(", ", roles),
                ProductCount = await _context.Products.CountAsync(product => product.OwnerId == user.Id)
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            await LoadRolesAsync(roles.FirstOrDefault() ?? "Member");

            return View(new AdminUserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Address = user.Address,
                RoleName = roles.FirstOrDefault() ?? "Member"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminUserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadRolesAsync(model.RoleName);
                return View(model);
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.Address = model.Address;
            user.EmailConfirmed = true;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                AddErrors(updateResult);
                await LoadRolesAsync(model.RoleName);
                return View(model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, model.RoleName);

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    AddErrors(passwordResult);
                    await LoadRolesAsync(model.RoleName);
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new AdminUserListItemViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Address = user.Address,
                Roles = string.Join(", ", roles),
                ProductCount = await _context.Products.CountAsync(product => product.OwnerId == user.Id)
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (User.Identity?.Name == user.UserName)
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa chính tài khoản đang đăng nhập.");
                var roles = await _userManager.GetRolesAsync(user);
                return View(new AdminUserListItemViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    Address = user.Address,
                    Roles = string.Join(", ", roles),
                    ProductCount = await _context.Products.CountAsync(product => product.OwnerId == user.Id)
                });
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadRolesAsync(string selectedRole)
        {
            var roles = await _roleManager.Roles.Select(role => role.Name!).ToListAsync();
            ViewBag.Roles = new SelectList(roles, selectedRole);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
