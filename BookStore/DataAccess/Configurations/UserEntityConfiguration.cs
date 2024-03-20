using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.DataAccess.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(e => e.Id).HasName("User_pkey");

            builder.ToTable("Users");

            builder.Property(e => e.Login).HasMaxLength(100);
            builder.Property(e => e.Password).HasMaxLength(100);
            builder.HasIndex(e=>e.Login).IsUnique();
        }
    }
}
