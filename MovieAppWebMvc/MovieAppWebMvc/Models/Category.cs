using MovieAppWebApi.Models;

namespace MovieAppWebMvc.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public Category? ParentCategory { get; set; }
        public virtual ICollection<FilmCategories> FilmCategories { get; set; } = new List<FilmCategories>();
        public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();

    }
}
