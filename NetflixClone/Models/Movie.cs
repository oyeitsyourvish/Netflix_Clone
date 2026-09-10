namespace NetflixClone.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string BannerUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public int Duration { get; set; }
        public decimal Rating { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();

    }
}
