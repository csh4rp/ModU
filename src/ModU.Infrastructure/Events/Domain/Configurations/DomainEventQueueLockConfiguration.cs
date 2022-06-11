using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Configurations;

internal sealed class DomainEventQueueLockConfiguration : IEntityTypeConfiguration<DomainEventQueueLock>
{
    public void Configure(EntityTypeBuilder<DomainEventQueueLock> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasMaxLength(64);

        builder.Property(b => b.Version)
            .IsConcurrencyToken(true);
    }
}