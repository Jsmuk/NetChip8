using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace NetChip8.DesktopGL;
internal class Worker : IHostedService
{
    private readonly IGame _game;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public Worker(IGame game, IHostApplicationLifetime applicationLifetime)
    {
        _game = game;
        _applicationLifetime = applicationLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationLifetime.ApplicationStarted.Register(OnStarted);

        return Task.CompletedTask;
    }

    private void OnStarted()
    {
        _game.Run();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _game.Game.Exit();
        _game.Dispose();
        
        return Task.CompletedTask;
    }
}
