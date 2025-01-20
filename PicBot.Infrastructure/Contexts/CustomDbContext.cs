using Microsoft.EntityFrameworkCore;
using PicBot.Domain.Contexts.BotPlatform;
using PicBot.Domain.Contexts.BotPlatform.Enums;

namespace PicBot.Infrastructure.Contexts;

internal abstract class CustomDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
           .UseCollation("utf8mb4_general_ci")
           .HasCharSet("utf8mb4");

        modelBuilder
           .Entity<User>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.TgUserId);

                    entity
                       .Property(e => e.Id)
                       .HasMaxLength(11);
                    entity
                       .Property(e => e.TgUserId)
                       .IsRequired();
                    entity
                       .Property(e => e.ChatId)
                       .IsRequired();
                    entity.Property(e => e.UserName);
                    entity.Property(e => e.FirstName);
                    entity.Property(e => e.LastName);
                    entity
                       .Property(e => e.Role)
                       .IsRequired()
                       .HasMaxLength(50)
                       .HasConversion(
                            v => v.ToString(),
                            v => (EUserRoles)Enum.Parse(typeof(EUserRoles), v)
                            )
                       .IsUnicode(false)
                       .HasDefaultValue(EUserRoles.User);
                    entity
                       .Property(e => e.BlockType)
                       .HasMaxLength(50)
                       .HasConversion(
                            v => v.ToString(),
                            v => (EUserBlockType)Enum.Parse(typeof(EUserBlockType), v)
                            )
                       .IsUnicode(false)
                       .HasDefaultValue(EUserBlockType.None);
                    entity.Property(e => e.RegisterDate);
                });

        CustomOnModelCreating(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    public abstract void CustomOnModelCreating(ModelBuilder modelBuilder);
}