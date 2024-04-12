using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppWebApi.Models;

namespace MovieAppWebApi.Migrations
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MovieAppDbContext _context;
        public CategoriesController(MovieAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var categories = await _context.Categories
        .Select(c => new
        {
            c.Id,
            c.Name,
            c.ParentCategoryId,
            ChildCategories = c.ChildCategories.Select(child => new { child.Id, child.Name }) 
        })
        .ToListAsync();

            return Ok(categories);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategories(int id, Category category)
        {
            if (id !=category.Id)
            {
                return BadRequest();
            }
            _context.Entry(category).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id))
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


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.ParentCategoryId,
                    ParentCategory = c.ParentCategory != null ? new { c.ParentCategory.Id, c.ParentCategory.Name } : null, 
                    ChildCategories = c.ChildCategories.Select(child => new { child.Id, child.Name }), 
                    FilmCategories = c.FilmCategories.Select(fc => new { fc.FilmId, Film = new { fc.Film.Id, fc.Film.Name } })
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("categoriesDetails")]
        public async Task<IActionResult> GetCategoriesWithDetails()
        {
            var categories = await _context.Categories
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.ParentCategoryId,
                    FilmsCount = c.FilmCategories.Count,
                    NestingLevel = c.ParentCategoryId == null ? 0 : 1 + _context.Categories.Count(p => p.Id == c.ParentCategoryId)  
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<object>>PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var response = new
            {
                category.Id,
                category.Name,
                category.ParentCategoryId
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
    }


}

