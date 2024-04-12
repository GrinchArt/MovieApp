namespace MovieAppWebMvc.Models
{
    public class FilmDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }
        public DateTime Release { get; set; }
        public List<string> Categories { get; set; }
    }
}
