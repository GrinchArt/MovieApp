using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAppWebApi.Models;

namespace MovieAppWebApi.Configuration
{
    public class FilmCategoriesEntityConfiguration : IEntityTypeConfiguration<FilmCategories>
    {
        public void Configure(EntityTypeBuilder<FilmCategories> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Film)
                .WithMany(x => x.FilmCategories)
                .HasForeignKey(x=>x.FilmId);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.FilmCategories)
                .HasForeignKey(x=>x.CategoryId);
        }
    }
}
