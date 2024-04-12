using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppWebApi.Models;
using System.Text.Json;

namespace MovieAppWebApi.Migrations
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmCategoriesController : ControllerBase
    {
        private readonly MovieAppDbContext _context;

        public FilmCategoriesController(MovieAppDbContext context)
        {
            _context = context;
        }


        [HttpGet("{filmId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesByFilm(int filmId)
        {
            var categories = await _context.FilmCategories
                .Where(x => x.FilmId == filmId)
                .Select(x => x.Category)
                .ToListAsync();

            if (!categories.Any())
            {
                return NotFound();
            }
            return categories;
        }

        [HttpPost]
        public async Task<ActionResult> AddCategoryToFilm([FromBody] JsonElement requestBody)
        {
            if (!requestBody.TryGetProperty("FilmId", out JsonElement filmIdElement) ||
        !requestBody.TryGetProperty("CategoryId", out JsonElement categoryIdsElement))
            {
                return BadRequest("The request body does not contain a FilmId and CategoryIds.");
            }

            if (!int.TryParse(filmIdElement.ToString(), out int filmId))
            {
                return BadRequest("FilmId is not a valid integer.");
            }

            if (!requestBody.GetProperty("CategoryId").TryGetInt32Array(out int[] categoryIds))
            {
                return BadRequest("CategoryIds is not a valid array of integers.");
            }

            if (!await _context.Films.AnyAsync(f => f.Id == filmId))
            {
                return NotFound("The film does not exist.");
            }

            var invalidCategoryIds = categoryIds.Where(cid => !_context.Categories.Any(c => c.Id == cid)).ToList();
            if (invalidCategoryIds.Any())
            {
                return NotFound($"The following categories do not exist: {string.Join(", ", invalidCategoryIds)}");
            }

            foreach (var categoryId in categoryIds)
            {
                var filmCategory = new FilmCategories
                {
                    FilmId = filmId,
                    CategoryId = categoryId
                };
                _context.FilmCategories.Add(filmCategory);
            }

            await _context.SaveChangesAsync();
            return Ok();

        }
       
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCategoryFromFilm(int id)
        {
            var filmCategory = await _context.FilmCategories.FindAsync(id);
            if (filmCategory == null)
            {
                return NotFound();
            }

            _context.FilmCategories.Remove(filmCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}


public static class JsonElementExtensions
{
    public static bool TryGetInt32Array(this JsonElement element, out int[] array)
    {
        if (element.ValueKind != JsonValueKind.Array)
        {
            array = null;
            return false;
        }

        var buffer = new List<int>();
        foreach (var item in element.EnumerateArray())
        {
            if (int.TryParse(item.ToString(), out var number))
            {
                buffer.Add(number);
            }
            else
            {
                array = null;
                return false;
            }
        }

        array = buffer.ToArray();
        return true;
    }
}