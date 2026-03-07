using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Lioncore.WebApi.Services;

public class ViteHostedService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string path = Path.Combine(Environment.CurrentDirectory, "ViteHostedService.sh");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            path = Path.Combine(Environment.CurrentDirectory, "ViteHostedService.bat");
        }

        Process.Start(path);
    }
}
