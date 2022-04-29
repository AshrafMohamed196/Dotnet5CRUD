using Dotnet5CRUD.Models;
using Dotnet5CRUD.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet5CRUD.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private List<string> _allowedExtension = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;


        public MoviesController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this._toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var Movies = await _context.movies.OrderByDescending(r => r.Rate).ToListAsync();
            return View(Movies);
        }
        // Create Movie
        public async Task<IActionResult> Create()
        {
            var ViewModel = new MovieFormViewModel()
            {
                genres = await _context.genres.OrderBy(g => g.Name).ToListAsync()
            };
            return View("MovieForm", ViewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.genres = await _context.genres.OrderBy(g => g.Name).ToListAsync();
                return View("MovieForm", model);
            }

            // check only for Existing poster(file) or not
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.genres = await _context.genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("Poster", "please Select Movie Poster");
                return View("MovieForm", model);
            }
            // here we should always check file size and extension
            var poster = files.FirstOrDefault();
            if (!_allowedExtension.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.genres = await _context.genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Only PNG, JPG images are allowed !");
                return View("MovieForm", model);
            }
            // Notes => Size is calculated in bytes
            if (poster.Length > _maxAllowedPosterSize)
            {
                model.genres = await _context.genres.OrderBy(g => g.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB !");
                return View("MovieForm", model);
            }

            using var datastream = new MemoryStream();
            await poster.CopyToAsync(datastream);

            var Movies = new Movie
            {
                Title = model.Title,
                GenreId = model.GenreId,
                Year = model.Year,
                Rate = model.Rate,
                StoryLine = model.StoryLine,
                Poster = datastream.ToArray()
            };
            _context.movies.Add(Movies);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Movie Created Successfully");
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
                return BadRequest();

            var movie = await _context.movies.FindAsync(Id);
            if (movie == null)
                return NotFound(); // Error404

            var viewmodel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                Year = movie.Year,
                Rate = movie.Rate,
                StoryLine = movie.StoryLine,
                Poster = movie.Poster,
                genres = await _context.genres.OrderBy(g => g.Name).ToListAsync()
            };
            return View("MovieForm", viewmodel);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            // check for all model
            if (!ModelState.IsValid)
            {
                model.genres = await _context.genres.OrderBy(g => g.Name).ToListAsync();
                return View("MovieForm", model);
            }


            var movie = await _context.movies.FindAsync(model.Id);
            if (movie == null)
                return NotFound();



            // check only for Existing poster(file) or not
            var files = Request.Form.Files;
            if (files.Any())
            {
                var Poster = files.FirstOrDefault();

                using var datastream = new MemoryStream();
                await Poster.CopyToAsync(datastream);

                model.Poster = datastream.ToArray();
                if (!_allowedExtension.Contains(Path.GetExtension(Poster.FileName).ToLower()))
                {
                    model.genres = await _context.genres.OrderBy(g => g.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Only PNG, JPG images are allowed !");
                    return View("MovieForm", model);
                }
                if (Poster.Length > _maxAllowedPosterSize)
                {
                    model.genres = await _context.genres.OrderBy(g => g.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB !");
                    return View("MovieForm", model);
                }
                movie.Poster = model.Poster;
            }

            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.StoryLine = model.StoryLine;

            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Movie Updated Succssfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
                return BadRequest();

            var movie = await _context.movies.Include(g => g.Genre).SingleOrDefaultAsync(m => m.Id == Id);
            if (movie == null)
                return NotFound();

            return View(movie);

        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
                return BadRequest();

            // Error => خد بالك لازم تبعت نوع ال genre والا هيضرب ايرور
            var movie = await _context.movies.Include(g => g.Genre).SingleOrDefaultAsync(m => m.Id == Id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }
        [HttpPost,ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int? Id, bool notUsed)
        {
            if (Id == null)
                return BadRequest();

            var movie = await _context.movies.FindAsync(Id);

            if (movie == null)
                return NotFound();

            _context.movies.Remove(movie);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Movie Deleted Successfully");
            return RedirectToAction(nameof(Index));
        }
    }
}
