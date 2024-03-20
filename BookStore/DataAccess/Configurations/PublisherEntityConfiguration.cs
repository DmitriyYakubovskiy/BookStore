using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Configurations
{
    public class PublisherEntityConfiguration : IEntityTypeConfiguration<PublisherEntity>
    {
        public void Configure(EntityTypeBuilder<PublisherEntity> builder)
        {
            builder.HasKey(e => e.Id).HasName("Publisher_pkey");

            builder.ToTable("Publishers");

            builder.Property(e => e.Name).HasMaxLength(100);
            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}
