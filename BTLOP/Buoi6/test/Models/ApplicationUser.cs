using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }
    }
}
