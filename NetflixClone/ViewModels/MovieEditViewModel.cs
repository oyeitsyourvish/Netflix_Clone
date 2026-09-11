using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.ViewModels
{
    public class MovieEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [Required]
        public string BannerUrl { get; set; } = string.Empty;

        [Required]
        public string VideoUrl { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int ReleaseYear { get; set; }

        [Range(1, 600)]
        public int Duration { get; set; }

        [Range(0, 10)]
        public decimal Rating { get; set; }

        public bool IsFeatured { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Please select at least one category.")]
        public List<int> SelectedCategoryIds { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
    }
}