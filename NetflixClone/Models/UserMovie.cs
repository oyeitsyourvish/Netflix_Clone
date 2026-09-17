using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Models
{
    public class UserMovie
    {
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public int MovieId { get; set; }

        public Movie Movie { get; set; } = null!;

        public DateTime AddedAt { get; set; }
    }
}