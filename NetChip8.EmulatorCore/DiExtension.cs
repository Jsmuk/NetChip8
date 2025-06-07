using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetChip8.Emulator.Shared.Interfaces;
using NetChip8.EmulatorCore.Services;

namespace NetChip8.EmulatorCore;
public static class DiExtension
{
    public static IHostBuilder AddChip8Emulator(this IHostBuilder applicationBuilder)
    {
        applicationBuilder.ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<IMemoryService, MemoryService>();
            services.AddSingleton<IProcessorService, ProcessorService>();
            services.AddSingleton<IRegisterService, RegisterService>();
            services.AddSingleton<IFrameBufferService, FrameBufferService>();

            services.AddHostedService<EmulatorWorker>();
        });
        
        return applicationBuilder;
    }
}
