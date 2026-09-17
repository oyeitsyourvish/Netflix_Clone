using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Data;
using NetflixClone.Models;

namespace NetflixClone.Controllers
{
    [Authorize]
    public class MyListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MyListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // My List page
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var movies = await _context.UserMovies
                .Where(um => um.UserId == userId)
                .Include(um => um.Movie)
                .OrderByDescending(um => um.AddedAt)
                .Select(um => um.Movie)
                .ToListAsync();

            return View(movies);
        }

        // Add movie to My List
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int movieId)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Challenge();
            }

            var movieExists = await _context.Movies
                .AnyAsync(m => m.Id == movieId);

            if (!movieExists)
            {
                return NotFound();
            }

            var alreadyExists = await _context.UserMovies
                .AnyAsync(um =>
                    um.UserId == userId &&
                    um.MovieId == movieId);

            if (!alreadyExists)
            {
                var userMovie = new UserMovie
                {
                    UserId = userId,
                    MovieId = movieId,
                    AddedAt = DateTime.UtcNow
                };

                _context.UserMovies.Add(userMovie);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "MyList", new { id = movieId });
        }


        // Remove movie from My List
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int movieId)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Challenge();
            }

            var userMovie = await _context.UserMovies
                .FirstOrDefaultAsync(um =>
                    um.UserId == userId &&
                    um.MovieId == movieId);

            if (userMovie != null)
            {
                _context.UserMovies.Remove(userMovie);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
