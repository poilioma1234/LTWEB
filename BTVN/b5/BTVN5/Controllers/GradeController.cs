using BTVN5.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTVN5.Controllers
{
    public class GradeController : Controller
    {
        // Thuộc tính có thể truy cập và thực thi truy vấn đến CSDL
        private readonly ApplicationDbContext _context;

        public GradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action xem danh sách các lớp học
        // GET: /Grade/Index
        public IActionResult Index()
        {
            // Lấy danh sách tất cả các lớp học từ cơ sở dữ liệu
            List<Grade> listGrade = _context.Grades.ToList();

            // Trả về view "Index" và truyền danh sách lớp học vào view để hiển thị
            return View(listGrade);
        }
    }
}