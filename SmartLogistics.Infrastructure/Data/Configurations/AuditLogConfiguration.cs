using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLogistics.Domain.Entities;

namespace SmartLogistics.Infrastructure.Data.Configurations
{
    public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable(nameof(AuditLog));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.UserId)
                .IsRequired()
                .HasMaxLength(128);
            builder.Property(a => a.Action).IsRequired().HasMaxLength(50);
            builder.Property(a => a.TableName).IsRequired().HasMaxLength(100);

            // استخدام nvarchar(max) للـ JSON لأن مساحته غير محددة
            builder.Property(a => a.KeyValues).IsRequired();
            builder.Property(a => a.OldValues);
            builder.Property(a => a.NewValues);

            // وضع Index على الـ TableName والـ DateTime لأننا سنبحث بهما دائماً في التقارير
            builder.HasIndex(a => a.TableName);
            builder.HasIndex(a => a.DateTime);

        }
    }
}
