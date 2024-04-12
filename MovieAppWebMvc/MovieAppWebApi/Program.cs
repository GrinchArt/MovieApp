using Microsoft.EntityFrameworkCore;

namespace MovieAppWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
               
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<MovieAppDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("MovieAppConnectionString") ?? throw new InvalidOperationException("Connection string 'MovieAppConnectionString' not found.")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowConnection", builder =>
                {
                    builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseRouting();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            app.UseCors("AllowConnection");
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            });
            app.MapControllers();
            app.Run();
        }
    }
}
