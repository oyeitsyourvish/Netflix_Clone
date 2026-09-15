using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Data;
using NetflixClone.ViewModels;

namespace NetflixClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieCategories)
                .ThenInclude(mc => mc.Category)
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                FeaturedMovie = movies
                    .FirstOrDefault(m => m.IsFeatured),

                TrendingMovies = movies
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(10)
                    .ToList()
            };

            foreach (var category in movies
                .SelectMany(m => m.MovieCategories)
                .Select(mc => mc.Category)
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.Name))
            {
                var categoryMovies = movies
                    .Where(m => m.MovieCategories
                        .Any(mc => mc.CategoryId == category.Id))
                    .ToList();

                viewModel.Categories.Add(
                    new CategoryMovieViewModel
                    {
                        CategoryName = category.Name,
                        Movies = categoryMovies
                    });
            }

            return View(viewModel);
        }
    }
}