using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLogistics.Domain.Entities;

namespace SmartLogistics.Infrastructure.Data.Configurations
{
    public sealed class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength (50);

            builder.Property(d => d.LicenseNumber)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(d => d.LicenseNumber).IsUnique();

            builder.Property(d => d.NationalID).IsRequired().HasMaxLength(50);
            builder.HasIndex(d => d.NationalID).IsUnique();

            builder.Property(d => d.PassportNumber).HasMaxLength(50);
            builder.Property(d => d.Nationality).HasMaxLength(50);
            builder.Property(d => d.SuspensionReason).HasMaxLength(500);

            builder.Property(d => d.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

            // Auditing
            builder.Property(d => d.CreatedBy).HasMaxLength(100);
            builder.Property(d => d.LastModifiedBy).HasMaxLength(100);
            builder.Property(d => d.DeletedBy).HasMaxLength(100);

            builder.HasQueryFilter(d => !d.IsDeleted);

            // إخبار EF Core بكيفية التعامل مع الـ Backing Field الخاص بالقائمة لإبقائها كـ Read-Only
            builder.Metadata.FindNavigation(nameof(Driver.Shipments))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
