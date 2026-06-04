using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace test.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Range(1_000, 100_000_000, ErrorMessage = "Giá sản phẩm phải nằm trong khoảng từ 1.000 đến 100.000.000")]
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? ImageUrl { get; set; }

        public List<ProductImage>? Images { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Mã danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Danh mục")]
        public Category? Category { get; set; }

        [Display(Name = "Người đăng")]
        public string? OwnerId { get; set; }

        [Display(Name = "Người đăng")]
        public ApplicationUser? Owner { get; set; }
    }
}
