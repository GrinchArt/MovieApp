using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieAppWebMvc.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieAppWebMvc.Controllers
{
    public class FilmsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FilmsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        // GET: FilmsController
        public async Task<IActionResult> Index(){
            var client = _httpClientFactory.CreateClient("MovieApi");
            var response = await client.GetAsync("films");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var films = await JsonSerializer.DeserializeAsync<IEnumerable<FilmDetail>>(responseContent, options);
                return View(films);
            }
            else
            {
                return NotFound();
            }
        }

 

        // GET: FilmsController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: FilmsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Director,Release")] Film film)
        {
            //var client = _httpClientFactory.CreateClient("MovieApi");
            //var filmJson = new StringContent(JsonSerializer.Serialize(film), Encoding.UTF8, "application/json");
            //var checkResponse = await client.PostAsync("films",filmJson);

            //if (checkResponse.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    return View(film);
            //}
            var client = _httpClientFactory.CreateClient("MovieApi");

            var filmToCreate = new { film.Name, film.Director, film.Release };

            var filmJson = new StringContent(JsonSerializer.Serialize(filmToCreate), Encoding.UTF8, "application/json");
            var checkResponse = await client.PostAsync("films", filmJson);

            if (checkResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {              
                return View(film);
            }
        }

        // GET: FilmsController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            var checkResponse = await client.GetAsync($"films/{id}");

            if (checkResponse.IsSuccessStatusCode)
            {
                var responseContent = await checkResponse.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                var film = await JsonSerializer.DeserializeAsync<Film>(responseContent, options);
              
                if (film != null)
                {
                    return View(film);
                }
                else
                {
                    return NotFound();
                }

            }
            else
            {
                return NotFound();
            }
            
        }

        // POST: FilmsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Director,Release")] Film film)
        {
            if (id !=film.Id)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            var filmToSend = new
            {
                Id = film.Id,
                Name = film.Name,
                Director = film.Director,
                Release = film.Release
            };
            var filmJson = new StringContent(JsonSerializer.Serialize(filmToSend), Encoding.UTF8, "application/json");

            var checkResponse = await client.PutAsync($"films/{film.Id}",filmJson);

            if (checkResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(film);
            }
        }

      
        // POST: FilmsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            var response = await client.DeleteAsync($"films/{id}");

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
