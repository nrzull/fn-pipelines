using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TgMate.API.Extensions;

public static class BuilderLoggingExtensions
{
  public static void UseLogging(this WebApplicationBuilder builder)
  {
    builder.Logging.AddOpenTelemetry(opts =>
    {
      opts.SetResourceBuilder(
          ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName)
        )
        .AddConsoleExporter();
    });

    builder
      .Services.AddOpenTelemetry()
      .ConfigureResource(opts => opts.AddService(builder.Environment.ApplicationName))
      .WithTracing(opts =>
      {
        opts.AddSource(Tracing.Source.Name);
        opts.AddAspNetCoreInstrumentation();
        opts.AddConsoleExporter();
      })
      .WithMetrics(opts =>
      {
        opts.AddAspNetCoreInstrumentation();
        opts.AddConsoleExporter();
      });
  }
}
