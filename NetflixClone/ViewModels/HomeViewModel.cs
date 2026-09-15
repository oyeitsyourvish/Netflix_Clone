using NetflixClone.Models;

namespace NetflixClone.ViewModels
{
    public class HomeViewModel
    {
        public Movie? FeaturedMovie { get; set; }

        public List<Movie> TrendingMovies { get; set; }
            = new();

        public List<CategoryMovieViewModel> Categories { get; set; }
            = new();
    }

    public class CategoryMovieViewModel
    {
        public string CategoryName { get; set; }
            = string.Empty;

        public List<Movie> Movies { get; set; }
            = new();
    }
}