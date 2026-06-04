using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using test.Repository;

namespace test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

            builder.Services.AddScoped<IProductRepository, EFProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await SeedRolesAndUsersAsync(scope.ServiceProvider);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }

        private static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await dbContext.Database.MigrateAsync();

            foreach (var roleName in new[] { "Admin", "Member" })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = await EnsureUserAsync(
                userManager,
                email: "admin@gmail.com",
                userName: "admin",
                password: "123456",
                roleName: "Admin",
                fullName: "Quan tri vien",
                address: "TP. Ho Chi Minh");

            var memberUser = await EnsureUserAsync(
                userManager,
                email: "member@gmail.com",
                userName: "member",
                password: "123456",
                roleName: "Member",
                fullName: "Thanh vien mau",
                address: "TP. Ho Chi Minh");

            await SeedSampleCatalogAsync(dbContext, memberUser.Id, adminUser.Id);
        }

        private static async Task<ApplicationUser> EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string userName,
            string password,
            string roleName,
            string fullName,
            string address)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    FullName = fullName,
                    Address = address,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(error => error.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }

            return user;
        }

        private static async Task SeedSampleCatalogAsync(ApplicationDbContext dbContext, string memberUserId, string adminUserId)
        {
            var phoneCategoryId = await EnsureCategoryAsync(dbContext, "Điện thoại");
            var laptopCategoryId = await EnsureCategoryAsync(dbContext, "Laptop");
            var accessoryCategoryId = await EnsureCategoryAsync(dbContext, "Phụ kiện");
            var watchCategoryId = await EnsureCategoryAsync(dbContext, "Đồng hồ");

            if (await dbContext.Products.AnyAsync())
            {
                return;
            }

            dbContext.Products.AddRange(
                new Product
                {
                    Name = "iPhone 15 Pro",
                    Price = 28990000,
                    Description = "Điện thoại cao cấp với chip A17 Pro, camera sắc nét và thiết kế titan.",
                    ImageUrl = "https://placehold.co/640x420/e0f2fe/0f172a?text=iPhone+15+Pro",
                    CategoryId = phoneCategoryId,
                    OwnerId = adminUserId
                },
                new Product
                {
                    Name = "Samsung Galaxy S24",
                    Price = 21990000,
                    Description = "Màn hình AMOLED sáng đẹp, hiệu năng mạnh và nhiều tính năng AI.",
                    ImageUrl = "https://placehold.co/640x420/dbeafe/0f172a?text=Galaxy+S24",
                    CategoryId = phoneCategoryId,
                    OwnerId = memberUserId
                },
                new Product
                {
                    Name = "MacBook Air M3",
                    Price = 27990000,
                    Description = "Laptop mỏng nhẹ, pin lâu, phù hợp học tập và làm việc hằng ngày.",
                    ImageUrl = "https://placehold.co/640x420/fef3c7/0f172a?text=MacBook+Air+M3",
                    CategoryId = laptopCategoryId,
                    OwnerId = adminUserId
                },
                new Product
                {
                    Name = "Dell XPS 13",
                    Price = 24990000,
                    Description = "Laptop Windows cao cấp với màn hình đẹp và thân máy gọn gàng.",
                    ImageUrl = "https://placehold.co/640x420/ecfccb/0f172a?text=Dell+XPS+13",
                    CategoryId = laptopCategoryId,
                    OwnerId = memberUserId
                },
                new Product
                {
                    Name = "Tai nghe Sony WH-1000XM5",
                    Price = 7990000,
                    Description = "Tai nghe chống ồn chủ động, âm thanh chi tiết và đeo thoải mái.",
                    ImageUrl = "https://placehold.co/640x420/fae8ff/0f172a?text=Sony+WH-1000XM5",
                    CategoryId = accessoryCategoryId,
                    OwnerId = memberUserId
                },
                new Product
                {
                    Name = "Apple Watch Series 9",
                    Price = 9990000,
                    Description = "Đồng hồ thông minh theo dõi sức khỏe, luyện tập và thông báo nhanh.",
                    ImageUrl = "https://placehold.co/640x420/fce7f3/0f172a?text=Apple+Watch+S9",
                    CategoryId = watchCategoryId,
                    OwnerId = adminUserId
                });

            await dbContext.SaveChangesAsync();
        }

        private static async Task<int> EnsureCategoryAsync(ApplicationDbContext dbContext, string categoryName)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(item => item.Name == categoryName);

            if (category != null)
            {
                return category.Id;
            }

            category = new Category { Name = categoryName };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            return category.Id;
        }
    }
}
