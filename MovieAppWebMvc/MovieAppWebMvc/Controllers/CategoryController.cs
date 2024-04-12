using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieAppWebMvc.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MovieAppWebMvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // GET: CategoryController
        public  async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            var response = await client.GetAsync("categories/categoriesDetails");

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var categories = await JsonSerializer.DeserializeAsync<List<CategoryDetail>>(responseStream, options);
                return View(categories);
            }
            else
            {
                return NotFound();
            }

        }


        // GET: CategoryController/Create
        public async Task<IActionResult> Create()
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            var response = await client.GetAsync("categories");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var categories = await JsonSerializer.DeserializeAsync<IEnumerable<MovieAppWebMvc.Models.Category>>(responseContent, options);

                ViewBag.ParentCategoryId = new SelectList(categories, "Id", "Name");
                return View();
            }
            else
            {
                return NotFound();
            }
        }
        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ParentCategoryId")] Category category)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            var categoryToCreate = new { category.Name, category.ParentCategoryId };

            var categoryJson = new StringContent(JsonSerializer.Serialize(categoryToCreate),Encoding.UTF8, "application/json");
            var checkResponse = await client.PostAsync("categories", categoryJson);
            if (checkResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }

        // GET: CategoryController/Edit/5
        public async  Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            var response = await client.GetAsync($"categories/{id}"); 

            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadFromJsonAsync<MovieAppWebMvc.Models.Category>();


                var categoriesResponse = await client.GetAsync("categories");
                if (!categoriesResponse.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                var categories = await categoriesResponse.Content.ReadFromJsonAsync<IEnumerable<MovieAppWebMvc.Models.Category>>();
                ViewBag.ParentCategoryId = new SelectList(categories.Where(c => c.Id != id), "Id", "Name", category.ParentCategoryId);

                return View(category);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");
            var categoryToSend = new
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };
            var categoryJson = new StringContent(JsonSerializer.Serialize(categoryToSend),Encoding.UTF8, "application/json");

            var checkResponse = await client.PutAsync($"categories/{category.Id}",categoryJson);  

                if (checkResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                { 
                    return View(category);
                } 
        }


        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            var response = await client.DeleteAsync($"categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
