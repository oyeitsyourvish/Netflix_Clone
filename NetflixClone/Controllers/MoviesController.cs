using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetflixClone.Data;
using NetflixClone.Models;
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
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }
        //--------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.CreatedAt = DateTime.UtcNow;
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        //--------------------------------------------------------------------------------------------------------------------

        public async Task<IActionResult> Details(int? id)
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
        //--------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        //--------------------------------------------------------------------------------------------------------------------

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
    }
}
