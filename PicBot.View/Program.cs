using PicBot.Bootstrap;
using PicBot.Domain.Enums;
using PicBot.View;

await HostExtensions
     .CreateHost(EBotType.PicBot)
     .ConfigureWebHostDefaults(
          webBuilder =>
          {
              webBuilder
                 .UseStartup<Startup>()
                 .UseShutdownTimeout(TimeSpan.FromMilliseconds(10000));
          })
     .Build()
     .StartBotAsync();