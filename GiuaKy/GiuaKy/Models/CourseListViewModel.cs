namespace GiuaKy.Models
{
    public class CourseListViewModel
    {
        public List<Course> Courses { get; set; } = new();
        public HashSet<int> EnrolledCourseIds { get; set; } = new();
        public string? Search { get; set; }
        public string Title { get; set; } = "Danh sách học phần";
    }
}
