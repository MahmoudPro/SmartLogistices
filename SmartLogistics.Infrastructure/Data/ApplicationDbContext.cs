using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartLogistics.Application.Common.Interfaces;
using SmartLogistics.Domain.Common;
using SmartLogistics.Domain.Entities;

namespace SmartLogistics.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // قراءة وتطبيق كل الـ Configurations تلقائياً
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // التعامل مع الكيانات التي تدعم الـ Auditing والـ Soft Delete
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    // تم إعداد قيم CreatedAt تلقائياً في الـ Entity، ولكن هنا نضمن تعيينها بـ UTC في الـ DB
                    // entry.Entity.CreatedBy = _currentUserService.UserId; // ستضاف لاحقاً مع الـ Identity
                    break;

                case EntityState.Modified:
                    // استخدام الـ Reflection أو الوصول المباشر لتحديث الحقول المحمية (Private setters)
                    entry.Property(x => x.LastModifiedAt).CurrentValue = DateTime.UtcNow;
                    // entry.Property(x => x.LastModifiedBy).CurrentValue = _currentUserService.UserId;
                    break;

                case EntityState.Deleted:
                    // تحويل الحذف النهائي إلى Soft Delete تلقائياً
                    entry.State = EntityState.Modified;

                    entry.Property(x => x.IsDeleted).CurrentValue = true;
                    entry.Property(x => x.DeletedAt).CurrentValue = DateTime.UtcNow;
                    // entry.Property(x => x.DeletedBy).CurrentValue = _currentUserService.UserId;
                    break;
            }
        }

        // --- [ملاحظة هندسية متقدمة] ---
        // هنا في الخطوات القادمة سنقوم بكتابة كود الـ Outbox Pattern 
        // بحيث نقوم بتحويل الـ DomainEvents الموجودة في الـ BaseEntity إلى OutboxMessages
        // وحفظها في قاعدة البيانات داخل نفس الـ Transaction تلقائياً قبل الـ SaveChanges!

        return base.SaveChangesAsync(cancellationToken);
    }
}