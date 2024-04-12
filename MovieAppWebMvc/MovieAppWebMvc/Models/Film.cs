using MovieAppWebApi.Models;

namespace MovieAppWebMvc.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public DateTime Release { get; set; }


        public ICollection<FilmCategories> FilmCategories { get; set; }
    }
}
