using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task.NET.Domain.Entities;
using Task.NET.Shared.EntityConfigurations;

namespace Task.NET.Infrastructure.Configuration;

public class ToDoConfiguration : AuditableEntityConfiguration<ToDo, Guid>
{
    public override void ConfigureEntities(EntityTypeBuilder<ToDo> builder)
    {
        builder.Property(x => x.DateTimeOfExpiry)
            .HasColumnName("dtDateTimeOfExpiry")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("sTitle")
            .HasConversion(x => x.Value, value => new Domain.ValueObjects.ToDo.Title(value));

        builder.Property(x => x.Description)
            .HasColumnName("sDescription")
            .HasConversion(x => x.Value, value => new Domain.ValueObjects.ToDo.Description(value));

        builder.Property(x => x.Complete)
            .HasColumnName("iComplete");
    }
}
