using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTVN5.Migrations
{
    /// <inheritdoc />
    public partial class Them_Du_Lieu_Mau_BookDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
            { 1, "Cuộc sống" },
            { 2, "Lập trình" },
            { 3, "Sức Khỏe" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Title", "Author", "Price", "Description", "Image", "CategoryId" },
                values: new object[,]
                {
            {
                1,
                "Cho tôi xin một vé đi tuổi thơ",
                "Nguyễn Nhật Ánh",
                61600m,
                "Một cuốn sách viết về tuổi thơ, ký ức và những điều giản dị trong cuộc sống.",
                "cho-toi-xin-mot-ve-di-tuoi-tho.jpg",
                1
            },
            {
                2,
                "Lập trình C",
                "TS. Lê Xuân Việt",
                50000m,
                "Sách giới thiệu kiến thức cơ bản về lập trình C.",
                "lap-trinh-c.jpg",
                2
            },
            {
                3,
                "Core Java: Fundamentals, Volume 1",
                "Cay Horstmann",
                120000m,
                "Sách học lập trình Java căn bản.",
                "core-java.jpg",
                2
            },
            {
                4,
                "Cuộc Sống Rất Giống Cuộc Đời",
                "Hải Dớ",
                61000m,
                "Đàn ông tuổi 15 mơ ước thành đàn ông tuổi 20, đàn ông tuổi 20 mơ ước thành đàn ông tuổi 30.",
                "cuoc-song-rat-giong-cuoc-doi.jpg",
                1
            }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);
        }
    }
}
