using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GiuaKy.Models
{
    public class CourseFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên học phần.")]
        [Display(Name = "Tên học phần")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số tín chỉ.")]
        [Range(1, 10, ErrorMessage = "Số tín chỉ phải từ 1 đến 10.")]
        [Display(Name = "Số tín chỉ")]
        public int Credits { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giảng viên.")]
        [Display(Name = "Giảng viên")]
        public string Lecturer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Hình ảnh")]
        public IFormFile? ImageFile { get; set; }

        public string? CurrentImage { get; set; }
    }
}
