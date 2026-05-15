using Microsoft.AspNetCore.Mvc;

namespace s6_b4.Controllers
{
    public class SanPhamController1 : Controller
    {
        public IActionResult Index()
        {
            //ViewBag.TenSP = "Iphone 14 Pro Max";
            //ViewBag.GiaSP = 30000000;
            //ViewBag.HinhAnh = "https://cdn.tgdd.vn/Products/Images/42/247882/iphone-14-pro-max-xam-1.jpg";
            //ViewBag.MoTa = "iPhone 14 Pro Max là chiếc điện thoại cao cấp nhất của Apple, được trang bị nhiều tính năng và công nghệ tiên tiến. Với thiết kế sang trọng";
            var sp = new Models.Product
            {
                Id = 1,
                Name = "Iphone 14 Pro Max"
            };


            return View();
        }

        public IActionResult hello()
        {
            return View();
        }


        public IActionResult BaiTap2() {
            var sanpham = new Models.SanPhamViewModule
            {
                TenSanPham = "Iphone 14 Pro Max",
                GiaBan = 30000000,
                AnhMoTa = "meo.jpg"
            };
            return View(sanpham);
        }
    }
}
