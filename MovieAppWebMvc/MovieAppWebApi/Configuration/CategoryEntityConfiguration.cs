using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAppWebApi.Models;

namespace MovieAppWebApi.Configuration
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasMany(x=>x.ChildCategories)
                .WithOne(x=>x.ParentCategory)
                .HasForeignKey(x=>x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
