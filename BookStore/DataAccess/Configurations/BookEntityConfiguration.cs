using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Configurations
{
    public class BookEntityConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasKey(e => e.Id).HasName("Book_pkey");

            builder.ToTable("Books");

            builder.Property(e => e.AuthorId).ValueGeneratedOnAdd();
            builder.Property(e => e.PublisherId).ValueGeneratedOnAdd();
            builder.Property(e => e.Title).HasMaxLength(100);
            builder.HasIndex(e => e.Title).IsUnique();

            builder.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Book_AuthorId_fkey");

            builder.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Book_PublisherId_fkey");

            builder.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Book_GenreId_fkey");
        }
    }
}
