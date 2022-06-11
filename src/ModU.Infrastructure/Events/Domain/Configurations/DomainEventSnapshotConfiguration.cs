using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Configurations;

internal sealed class DomainEventSnapshotConfiguration : IEntityTypeConfiguration<DomainEventSnapshot>
{
    public void Configure(EntityTypeBuilder<DomainEventSnapshot> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.HasIndex(b => b.Queue);

        builder.Property(b => b.Name)
            .HasMaxLength(100);

        builder.Property(b => b.Queue)
            .HasMaxLength(64);

        builder.Property(b => b.Type)
            .HasMaxLength(200);

        builder.Property(b => b.AggregateType)
            .HasMaxLength(200);

        builder.Property(b => b.SpanId)
            .HasMaxLength(100);
        
        builder.Property(b => b.TraceId)
            .HasMaxLength(100);
    }
}