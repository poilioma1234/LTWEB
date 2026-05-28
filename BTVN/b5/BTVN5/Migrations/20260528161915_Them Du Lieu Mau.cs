using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTVN5.Migrations
{
    /// <inheritdoc />
    public partial class ThemDuLieuMau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm dữ liệu vào bảng Grades
            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "GradeId", "GradeName" },
                values: new object[] { 1, "21DTHA1" }
            );

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "GradeId", "GradeName" },
                values: new object[] { 2, "21DTHA2" }
            );

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "GradeId", "GradeName" },
                values: new object[] { 3, "21DTHA3" }
            );

            // Thêm dữ liệu vào bảng Students
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "FirstName", "LastName", "GradeId" },
                values: new object[] { 1, "Khuyến", "Bùi", 1 }
            );

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "FirstName", "LastName", "GradeId" },
                values: new object[] { 2, "Toàn", "Nguyễn", 1 }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa dữ liệu bảng Students trước vì Students có khóa ngoại GradeId
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1
            );

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2
            );

            // Sau đó mới xóa dữ liệu bảng Grades
            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "GradeId",
                keyValue: 1
            );

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "GradeId",
                keyValue: 2
            );

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "GradeId",
                keyValue: 3
            );
        }
    }
}