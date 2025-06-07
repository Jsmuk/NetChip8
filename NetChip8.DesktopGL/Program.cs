using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetChip8.DesktopGL;

var host = Host.CreateDefaultBuilder(args);
    
host.ConfigureServices((hostContext, services) =>
{
    services.AddSingleton<IGame, NetChip8Renderer>();
    services.AddHostedService<Worker>();
});

host.Build().Run();

