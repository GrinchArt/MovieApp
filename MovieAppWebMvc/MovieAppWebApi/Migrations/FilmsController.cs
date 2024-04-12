using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppWebApi.Models;

namespace MovieAppWebApi.Migrations
{
    [Route("api/films")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly MovieAppDbContext _context;

        public FilmsController(MovieAppDbContext context) 
        { 
        _context = context;
        }

        // GET: api/Films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDetail>>> GetFilmsWithCategories()
        {
            var filmsWithCategories = await _context.Films
                .Select(f => new FilmDetail
                {
                    Id = f.Id,
                    Name = f.Name,
                    Director = f.Director,
                    Release = f.Release,
                    Categories = f.FilmCategories.Select(fc => fc.Category.Name).ToList()
                })
                .ToListAsync();

            return Ok(filmsWithCategories);
        }


        // GET: api/Films/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return film;
        }


        // PUT: api/Films/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id,Film film )
        {
            if (id != film.Id)
            {
                return BadRequest();
            }

            _context.Entry(film).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Films.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            if (_context.Films == null)
            {
                return NotFound();
            }
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(Film film)
        {
            if (_context.Films == null)
            {
                return Problem("Entity set 'MovieAppDbContext.Films'  is null.");
            }
            _context.Films.Add(film);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
        }
    }
}
