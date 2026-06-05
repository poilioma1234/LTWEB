using GiuaKy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiuaKy.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>()
                .HasOne(course => course.Category)
                .WithMany(category => category.Courses)
                .HasForeignKey(course => course.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Enrollment>()
                .HasOne(enrollment => enrollment.Course)
                .WithMany(course => course.Enrollments)
                .HasForeignKey(enrollment => enrollment.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Enrollment>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(enrollment => enrollment.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Enrollment>()
                .HasIndex(enrollment => new { enrollment.UserId, enrollment.CourseId })
                .IsUnique();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "admin-role-id",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "student-role-id",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                }
            );

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Lập trình" },
                new Category { Id = 2, Name = "Cơ sở dữ liệu" },
                new Category { Id = 3, Name = "Mạng máy tính" }
            );

            builder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Lập trình Web", Image = "/images/web.jpg", Credits = 3, Lecturer = "Nguyễn Văn A", CategoryId = 1 },
                new Course { Id = 2, Name = "Lập trình C#", Image = "/images/csharp.jpg", Credits = 3, Lecturer = "Trần Thị B", CategoryId = 1 },
                new Course { Id = 3, Name = "Cơ sở dữ liệu", Image = "/images/database.jpg", Credits = 3, Lecturer = "Lê Văn C", CategoryId = 2 },
                new Course { Id = 4, Name = "Hệ quản trị cơ sở dữ liệu", Image = "/images/sql.jpg", Credits = 3, Lecturer = "Phạm Thị D", CategoryId = 2 },
                new Course { Id = 5, Name = "Mạng máy tính", Image = "/images/network.jpg", Credits = 3, Lecturer = "Hoàng Văn E", CategoryId = 3 },
                new Course { Id = 6, Name = "An toàn thông tin", Image = "/images/security.jpg", Credits = 2, Lecturer = "Đỗ Thị F", CategoryId = 3 },
                new Course { Id = 7, Name = "Phân tích thiết kế hệ thống", Image = "/images/analysis.jpg", Credits = 3, Lecturer = "Bùi Văn G", CategoryId = 1 },
                new Course { Id = 8, Name = "Trí tuệ nhân tạo", Image = "/images/ai.jpg", Credits = 3, Lecturer = "Đặng Thị H", CategoryId = 1 },
                new Course { Id = 9, Name = "Kiểm thử phần mềm", Image = "/images/testing.jpg", Credits = 2, Lecturer = "Võ Văn I", CategoryId = 1 },
                new Course { Id = 10, Name = "Điện toán đám mây", Image = "/images/cloud.jpg", Credits = 3, Lecturer = "Ngô Thị K", CategoryId = 3 }
            );
        }
    }
}
