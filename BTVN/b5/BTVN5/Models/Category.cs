using System.ComponentModel.DataAnnotations;

namespace BTVN5.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên chủ đề không được để trống")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}