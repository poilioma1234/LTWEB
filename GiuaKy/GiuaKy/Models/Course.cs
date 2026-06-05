namespace GiuaKy.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Lecturer { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
