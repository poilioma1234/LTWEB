namespace WebApplication1.Models
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var life = new Category { CategoryName = "Cuộc sống" };
                var programming = new Category { CategoryName = "Lập trình" };
                var health = new Category { CategoryName = "Sức Khỏe" };

                context.Categories.AddRange(
                    life,
                    programming,
                    health);

                context.SaveChanges();
            }

            if (context.Books.Any())
            {
                return;
            }

            context.Books.AddRange(
                new Book
                {
                    Title = "Cho tôi xin một vé đi tuổi thơ",
                    Author = "Nguyễn Nhật Ánh",
                    Price = 61600,
                    Description = "Tác phẩm kể lại những kỷ niệm tuổi thơ trong trẻo, nhiều cảm xúc và gần gũi với độc giả Việt Nam.",
                    Image = "cho-toi-xin-mot-ve-di-tuoi-tho.png",
                    CategoryId = 1
                },
                new Book
                {
                    Title = "Lập trình C cơ bản",
                    Author = "TS. Le Xuan Viet",
                    Price = 78000,
                    Description = "Sách nhập môn lập trình C, trình bày các khái niệm nền tảng, cấu trúc điều khiển, hàm và mảng.",
                    Image = "lap-trinh-c-co-ban.png",
                    CategoryId = 2
                },
                new Book
                {
                    Title = "Core Java: Fundamentals, Volume 1",
                    Author = "Cay Horstmann",
                    Price = 320000,
                    Description = "Tài liệu nền tảng về Java, phù hợp cho người học lập trình hướng đối tượng và xây dựng ứng dụng.",
                    Image = "core-java-fundamentals.png",
                    CategoryId = 2
                },
                new Book
                {
                    Title = "Cuộc Sống Rất Giống Cuộc Đời",
                    Author = "Hải Đỏ",
                    Price = 61000,
                    Description = "Câu chuyện về những ước mơ, suy nghĩ và trải nghiệm trưởng thành trong đời sống hằng ngày.",
                    Image = "cuoc-song-rat-giong-cuoc-doi.png",
                    CategoryId = 1
                });

            context.SaveChanges();
        }
    }
}
