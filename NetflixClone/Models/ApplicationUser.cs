using Microsoft.AspNetCore.Identity;

namespace NetflixClone.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}