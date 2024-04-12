namespace MovieAppWebApi.Models
{
    public class Film
    {
        public Film()
        {
            FilmCategories = new List<FilmCategories>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public DateTime Release {  get; set; }


        public ICollection<FilmCategories> FilmCategories { get; set; }

    }
}
