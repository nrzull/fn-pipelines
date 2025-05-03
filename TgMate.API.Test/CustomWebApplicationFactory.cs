using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TgMate.API.Test;

public class CustomWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    base.ConfigureWebHost(builder);

    builder.ConfigureAppConfiguration(
      (_ctx, conf) =>
      {
        var appSettings = new Dictionary<string, string?>
        {
          { "Logging:LogLevel:Default", LogLevel.Information.ToString() },
          { "Logging:LogLevel:Microsoft.EntityFrameworkCore", LogLevel.Warning.ToString() },
        };

        conf.AddInMemoryCollection(appSettings);
      }
    );

    builder.ConfigureServices(services =>
    {
      var db = services.SingleOrDefault(d =>
        d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
      );

      if (db is { })
      {
        services.Remove(db);
        services.AddDbContext<ApplicationDbContext>(opts => opts.UseNpgsql(connectionString));
      }
    });
  }
}
