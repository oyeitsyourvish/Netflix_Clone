using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Data;
using NetflixClone.Models;
using NetflixClone.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace NetflixClone.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Added Constructor dependency injection for ApplicationDbContext
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get the list of movies from the database and pass it to the view
        public async Task<IActionResult> Index()
        {
            // Include the related MovieCategories and Categories when fetching movies aslo called eager loading.
            // This will allow us to access the categories of each movie in the view without additional database queries.
            var movies = await _context.Movies.Include(m => m.MovieCategories).ThenInclude(m => m.Category).ToListAsync();

            return View(movies);
        }
        //--------------------------------------------------------------------------------------------------------------------

        // CREATE a new movie and save it to the database
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new MovieCreateViewModel
            {
                Categories = await GetCategorySelectList()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Create(MovieCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Title = model.Title,
                    Description = model.Description,
                    ThumbnailUrl = model.ThumbnailUrl,
                    BannerUrl = model.BannerUrl,
                    VideoUrl = model.VideoUrl,
                    ReleaseYear = model.ReleaseYear,
                    Duration = model.Duration,
                    Rating = model.Rating,
                    IsFeatured = model.IsFeatured,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                foreach (var categoryId in model.SelectedCategoryIds)
                {
                    var movieCategory = new MovieCategory
                    {
                        MovieId = movie.Id,
                        CategoryId = categoryId
                    };

                    _context.MovieCategories.Add(movieCategory);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Categories = await GetCategorySelectList();
            return View(model);
        }
        //--------------------------------------------------------------------------------------------------------------------

        //DETAILS of a movie

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieCategories)
                .ThenInclude(mc => mc.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        //--------------------------------------------------------------------------------------------------------------------

        // EDIT a movie and save changes to the database
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(m => m.MovieCategories).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieEditViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ThumbnailUrl = movie.ThumbnailUrl,
                BannerUrl = movie.BannerUrl,
                VideoUrl = movie.VideoUrl,
                ReleaseYear = movie.ReleaseYear,
                Duration = movie.Duration,
                Rating = movie.Rating,
                IsFeatured = movie.IsFeatured,

                SelectedCategoryIds = movie.MovieCategories
            .Select(mc => mc.CategoryId)
            .ToList(),

                Categories = await GetCategorySelectList()

            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var movie = await _context.Movies.Include(m => m.MovieCategories).FirstOrDefaultAsync(m => m.Id == id);

                if (movie == null)
                {
                    return NotFound();
                }

                movie.Title = model.Title;
                movie.Description = model.Description;
                movie.ThumbnailUrl = model.ThumbnailUrl;
                movie.BannerUrl = model.BannerUrl;
                movie.VideoUrl = model.VideoUrl;
                movie.ReleaseYear = model.ReleaseYear;
                movie.Duration = model.Duration;
                movie.Rating = model.Rating;
                movie.IsFeatured = model.IsFeatured;

                movie.MovieCategories.Clear();

                foreach (var categoryId in model.SelectedCategoryIds)
                {
                    movie.MovieCategories.Add(new MovieCategory
                    {
                        MovieId = movie.Id,
                        CategoryId = categoryId
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            model.Categories = await GetCategorySelectList();

            return View(model);
        }
        //--------------------------------------------------------------------------------------------------------------------

        // DELETE a movie from the database
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie != null)
            {
                _context.Movies.Remove(movie);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }
        //--------------------------------------------------------------------------------------------------------------------

       






        //---------------------------------------------------------------------------------------------------------------------
        //This gets categories from SQL Server and converts them into items that our Razor form can display
        private async Task<List<SelectListItem>> GetCategorySelectList()
        {
            return await _context.Categories.OrderBy(c => c.Name).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
        }

    }
}
