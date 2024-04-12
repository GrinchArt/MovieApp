namespace MovieAppWebMvc.Models
{
    public class CategoryDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public int FilmsCount { get; set; }
        public int NestingLevel { get; set; }
    }
}
