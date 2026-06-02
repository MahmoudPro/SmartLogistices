using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLogistics.Domain.Entities;


namespace SmartLogistics.Infrastructure.Data.Configurations
{
    public sealed class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipment");
            
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TrackingNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.TrackingNumber)
                .IsUnique();

            builder.Property(s => s.Origin)
            .IsRequired()
            .HasMaxLength(250);


            builder.Property(s => s.Destination)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Weight)
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

            // الـ Auditing والـ Soft Delete الأساسية من الـ AuditableEntity
            builder.Property(s => s.CreatedBy).HasMaxLength(100);
            builder.Property(s => s.LastModifiedBy).HasMaxLength(100);
            builder.Property(s => s.DeletedBy).HasMaxLength(100);

            // تفعيل الـ Global Query Filter لمنع جلب البيانات المحذوفة منطقياً
            builder.HasQueryFilter(s => !s.IsDeleted);

            builder.HasOne(s => s.Driver)
                .WithMany(d => d.Shipments)
                .HasForeignKey(s => s.DriverId)
                .OnDelete(DeleteBehavior.Restrict);// حماية البيانات من الـ Cascade Delete

            builder.HasIndex(s => s.DriverId);
            builder.HasIndex(s => s.CustomerId);

        }
    }
}
