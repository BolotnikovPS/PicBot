namespace PicBot.Domain.Contexts.BotPlatform;

public class FileBox
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FileId { get; set; }

    public DateTime Create { get; set; }

    public virtual User User { get; set; }
}