using Microsoft.AspNetCore.Identity;

namespace NetflixClone.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public ICollection<UserMovie> UserMovies { get; set; } = new List<UserMovie>();
    }
}