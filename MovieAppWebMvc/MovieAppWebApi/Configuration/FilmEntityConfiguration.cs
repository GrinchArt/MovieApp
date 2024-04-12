using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAppWebApi.Models;

namespace MovieAppWebApi.Configuration
{
    public class FilmEntityConfiguration : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x=>x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x=>x.Director)
                .IsRequired()
                .HasMaxLength (200);

            builder.Property(x => x.Release)
                .IsRequired();

        }
    }
}
