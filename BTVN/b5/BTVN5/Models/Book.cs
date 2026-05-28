using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BTVN5.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(150)]
        public string? Author { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal? Price { get; set; }

        public string? Description { get; set; }

        [StringLength(100)]
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}