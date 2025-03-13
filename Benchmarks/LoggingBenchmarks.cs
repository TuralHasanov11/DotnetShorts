using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Benchmarks;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class LoggingBenchmarks : IDisposable
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly Action<ILogger, string, Exception?> _highPerformanceLogger;
    private readonly IServiceProvider _serviceProvider;
    private bool _disposed;

    public LoggingBenchmarks()
    {
        // Setup for Logging Factory
        _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        // Setup for Dependency Injection
        _serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

        // Setup for High Performance Logger
        _highPerformanceLogger = LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(HighPerformanceLogging)),
            "High performance log: {Message}");
    }

    [Benchmark]
    public void LoggingFactory()
    {
        var logger = _loggerFactory.CreateLogger<LoggingBenchmarks>();

        logger.LogInformation("Dependency Injection Logging: {Message}", "Message");
    }

    [Benchmark]
    public void LoggingWithDI()
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<LoggingBenchmarks>>();

        logger.LogInformation("Dependency Injection Logging: {Message}", "Message");
    }

    [Benchmark]
    public void HighPerformanceLogging()
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<LoggingBenchmarks>>();

        _highPerformanceLogger(logger, "Message", null);
    }

    [Benchmark]
    public void SourceGeneratedLogging()
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<LoggingBenchmarks>>();

        logger.LogSourceGenerated("Message");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _loggerFactory.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public static partial class LoggingBenchmarksExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Source Generated Logging: {Message}")]
    public static partial void LogSourceGenerated(this ILogger<LoggingBenchmarks> logger, string message);
}
