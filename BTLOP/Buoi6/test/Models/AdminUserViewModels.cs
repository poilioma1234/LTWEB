using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class AdminUserListItemViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string Roles { get; set; } = string.Empty;

        public int ProductCount { get; set; }
    }

    public class AdminUserEditViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Họ tên")]
        public string? FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn role.")]
        [Display(Name = "Role")]
        public string RoleName { get; set; } = "Member";

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string? NewPassword { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int UserCount { get; set; }

        public int ProductCount { get; set; }

        public int CategoryCount { get; set; }

        public int MemberProductCount { get; set; }
    }
}
