using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiuaKy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVietnameseCourseText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Lập trình");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Cơ sở dữ liệu");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Mạng máy tính");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Nguyễn Văn A", "Lập trình Web" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Trần Thị B", "Lập trình C#" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Lê Văn C", "Cơ sở dữ liệu" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Phạm Thị D", "Hệ quản trị cơ sở dữ liệu" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Hoàng Văn E", "Mạng máy tính" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Đỗ Thị F", "An toàn thông tin" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Bùi Văn G", "Phân tích thiết kế hệ thống" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Đặng Thị H", "Trí tuệ nhân tạo" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Võ Văn I", "Kiểm thử phần mềm" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Ngô Thị K", "Điện toán đám mây" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Lap trinh");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Co so du lieu");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Mang may tinh");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Nguyen Van A", "Lap trinh Web" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Tran Thi B", "Lap trinh C#" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Le Van C", "Co so du lieu" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Pham Thi D", "He quan tri co so du lieu" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Hoang Van E", "Mang may tinh" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Do Thi F", "An toan thong tin" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Bui Van G", "Phan tich thiet ke he thong" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Dang Thi H", "Tri tue nhan tao" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Vo Van I", "Kiem thu phan mem" });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Lecturer", "Name" },
                values: new object[] { "Ngo Thi K", "Dien toan dam may" });
        }
    }
}
