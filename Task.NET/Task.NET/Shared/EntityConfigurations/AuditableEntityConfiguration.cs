using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Task.NET.Shared.Entities;

namespace Task.NET.Shared.EntityConfigurations;

public abstract class AuditableEntityConfiguration<TEntity, TKey>
: IEntityTypeConfiguration<TEntity> where TEntity : Auditable<TKey>
{
    public abstract void ConfigureEntities(EntityTypeBuilder<TEntity> builder);

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasColumnName("iId");

        builder.Property(v => v.InsertedUserId)
            .HasColumnName("iInsertedUserId")
            .IsRequired();

        builder.Property(v => v.InsertedTime)
            .HasColumnName("dtInsertedTime")
            .IsRequired();

        builder.Property(v => v.UpdatedUserId)
            .HasColumnName("iUpdatedUserId");

        builder.Property(v => v.UpdatedTime)
            .HasColumnName("dtUpdatedTime");

        ConfigureEntities(builder);
    }
}