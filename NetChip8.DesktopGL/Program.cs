using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetChip8.DesktopGL;
using NetChip8.DesktopGL.GameStates;
using NetChip8.EmulatorCore;

var host = Host.CreateDefaultBuilder(args);
    
host.ConfigureServices((hostContext, services) =>
{
    services.AddSingleton<IGame, NetChip8Renderer>();
    services.AddSingleton<KeyboardMapProvider>();
    services.AddSingleton<IGameStateManager, GameStateManager>();
    services.AddSingleton<EmulatorGameState>();
    services.AddHostedService<Worker>();

});

host.AddChip8Emulator();

host.Build().Run();

