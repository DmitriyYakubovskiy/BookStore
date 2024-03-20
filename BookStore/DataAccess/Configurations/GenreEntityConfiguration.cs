using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Configurations
{
    public class GenreEntityConfiguration : IEntityTypeConfiguration<GenreEntity>
    {
        public void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            builder.HasKey(e => e.Id).HasName("Genre_pkey");

            builder.ToTable("Genres");

            builder.Property(e => e.Name).HasMaxLength(100);
            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}
