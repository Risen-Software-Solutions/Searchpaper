using CliWrap;

namespace Lioncore.WebApi.Services;

public class ViteHostedService : BackgroundService
{
    private readonly ILogger<ViteHostedService> _logger;

    public ViteHostedService(ILogger<ViteHostedService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Cli.Wrap("npm")
            .WithArguments(["run", "dev"])
            .WithWorkingDirectory(Path.Combine(Environment.CurrentDirectory, "ClientApp"))
            .WithStandardOutputPipe(PipeTarget.ToDelegate(info => _logger.LogInformation(info)))
            .WithStandardErrorPipe(PipeTarget.ToDelegate(info => _logger.LogError(info)))
            .ExecuteAsync(stoppingToken);
    }
}
