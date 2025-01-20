using Microsoft.EntityFrameworkCore;
using PicBot.Domain.Contexts.BotPlatform;

namespace PicBot.Application.Abstractions.DBContext;

public interface IBotPlatformDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Verification> Verifications { get; set; }
    DbSet<FileBox> FilesBox { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}