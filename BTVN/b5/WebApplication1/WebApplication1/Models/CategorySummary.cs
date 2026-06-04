namespace WebApplication1.Models
{
    public class CategorySummary
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int BookCount { get; set; }
    }
}
