namespace PicBot.Application.Abstractions.DBContext;

public interface IMigratorContext
{
    Task MigrateAsync();
}