using System.Diagnostics;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NetChip8.Emulator.Shared.Interfaces;
using NetChip8.EmulatorCore.Services;

namespace NetChip8.EmulatorCore;
public class EmulatorWorker : IHostedService 
{
    private readonly IProcessorService _processorService;
    private readonly IMemoryService _memoryService;
    private readonly ILogger<EmulatorWorker> _logger;
    
    private readonly int _targetHz;
    private double _targetFrameTime;
    private int _cycleCount = 0;
    private double _lastTimeCheck = 0;
    private int _statsCount = 0;

    private CancellationTokenSource? _cts;
    private Task? _task;
    
    public EmulatorWorker(IProcessorService processorService, IMemoryService memoryService, ILogger<EmulatorWorker> logger)
    {
        _processorService = processorService;
        _memoryService = memoryService;
        _logger = logger;

        _targetHz = 700;
        _targetFrameTime = 1000.0 / _targetHz;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        _memoryService.LoadProgram("ibm-logo");

        _task = Task.Run(() => CpuLoop(_cts.Token), cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();
        return _task ?? Task.CompletedTask;
    }

    private async Task CpuLoop(CancellationToken token)
    {
        var stopwatch = Stopwatch.StartNew();
        while (!token.IsCancellationRequested)
        {
            var frameStartTime = stopwatch.Elapsed.TotalMilliseconds;
            
            _processorService.Cycle();
            _cycleCount++;

            if (frameStartTime - _lastTimeCheck >= 1000)
            {
                if (_statsCount++ == 5)
                {
                    _logger.LogInformation(
                        "Average Frequency: {CycleCount}Hz (Target: {TargetFrequency} - Delta {Delta})", _cycleCount,
                        _targetHz, _cycleCount - _targetHz);
                    _statsCount = 0;
                }

                _cycleCount = 0;
                _lastTimeCheck = frameStartTime;
            }
            
            var frameEndTime = stopwatch.Elapsed.TotalMilliseconds;
            
            var elapsed = frameEndTime - frameStartTime;
            var delay = _targetFrameTime - elapsed;

            if (delay > 0)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(delay), token);
            }
        }
    }
    
}
