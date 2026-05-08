using Microsoft.AspNetCore.Mvc;
using s6_b4.Models;


namespace s6_b4.Controllers
{
    public class StudentController : Controller
    {
        // Biến static để lưu danh sách sinh viên đăng ký
        private static List<StudentRegistration> RegisteredStudents = new List<StudentRegistration>();

        // Action GET: /Student/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Action POST: /Student/ShowKQ
        [HttpPost]
        public IActionResult ShowKQ(StudentRegistration student)
        {
            // Thêm sinh viên vào danh sách
            RegisteredStudents.Add(student);

            // Đếm số lượng sinh viên cùng chuyên ngành
            int sameMajorCount = RegisteredStudents.FindAll(s => s.ChuyenNganh == student.ChuyenNganh).Count;

            // Truyền dữ liệu sang View ShowKQ
            ViewBag.MSSV = student.MSSV;
            ViewBag.HoTen = student.HoTen;
            ViewBag.ChuyenNganh = student.ChuyenNganh;
            ViewBag.SameMajorCount = sameMajorCount;

            return View();
        }
    }
}