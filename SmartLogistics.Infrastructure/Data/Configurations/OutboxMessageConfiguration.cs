using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLogistics.Domain.Entities;

namespace SmartLogistics.Infrastructure.Data.Configurations
{
    public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessage");

            builder.HasKey(o => o.Id);

            builder.Property(o=> o.Type).IsRequired().HasMaxLength(250);
            builder.Property(o=>o.Content).IsRequired();

            builder.HasIndex(o => new { o.ProcessedOnUtc, o.OccurredOnUtc });
        }
    }
}
