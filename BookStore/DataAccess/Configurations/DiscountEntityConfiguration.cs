using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.DataAccess.Configurations
{
    public class DiscountEntityConfiguration: IEntityTypeConfiguration<DiscountEntity>
    {
        public void Configure(EntityTypeBuilder<DiscountEntity> builder)
        {
            builder.HasKey(e => e.Id).HasName("Discount_pkey");

            builder.ToTable("Discounts");

            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Title).HasMaxLength(100);
            builder.Property(e => e.AuthorName).HasMaxLength(100);
            builder.Property(e => e.PublisherName).HasMaxLength(100);            
            builder.Property(e => e.Genre).HasMaxLength(100);
            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}
