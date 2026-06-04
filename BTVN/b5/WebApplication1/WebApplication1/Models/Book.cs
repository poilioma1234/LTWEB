using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sách.")]
        [StringLength(150, ErrorMessage = "Tên sách tối đa 150 ký tự.")]
        [Display(Name = "Ten sach")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tác giả.")]
        [StringLength(150, ErrorMessage = "Tác giả tối đa 150 ký tự.")]
        [Display(Name = "Tac gia")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập giá bán.")]
        [Range(1000, 10000000, ErrorMessage = "Giá bán phải từ 1.000 đến 10.000.000 VND.")]
        [Column(TypeName = "decimal(18,0)")]
        [Display(Name = "Gia ban")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        [StringLength(2000, ErrorMessage = "Mô tả tối đa 2000 ký tự.")]
        [Display(Name = "Mo ta")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Hinh anh")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chủ đề.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn chủ đề.")]
        [Display(Name = "Chu de")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [NotMapped]
        [Display(Name = "Hinh anh")]
        public IFormFile? ImageFile { get; set; }
    }
}
