using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên chủ đề.")]
        [StringLength(100, ErrorMessage = "Tên chủ đề tối đa 100 ký tự.")]
        [Display(Name = "Chu de")]
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
