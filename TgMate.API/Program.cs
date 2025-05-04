using Microsoft.EntityFrameworkCore;
using TgMate.API.Auth;
using TgMate.API.Extensions;
using TgMate.API.Telegram;
using TgMate.API.Telegram.Pipelines;
using TgMate.API.Web.Auth;

namespace TgMate.API;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.UseLogging();
    builder.Services.AddControllers();
    builder.Services.AddSignalR();

    builder.Services.AddSingleton<TgWorkerClientPool>();
    builder.Services.AddScoped<PipelinesContext>();
    builder.Services.AddScoped<TgContext>();
    builder.Services.AddScoped<AuthContext>();

    builder.Services.AddCors(options =>
    {
      options.AddPolicy(
        "generic",
        policy =>
        {
          policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Access-Control-Allow-Origin", "Content-Type");
        }
      );
    });

    builder.UseAuthEndpoints();

    builder.WebHost.ConfigureKestrel(opts =>
    {
      opts.ListenLocalhost(int.Parse(Environment.GetEnvironmentVariable("API_WEB_PORT")!));
    });

    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
      opts.UseNpgsql(ApplicationDbContext.GetConnectionString())
    );

    var app = builder.Build();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseCors("generic");

    app.MapControllers();
    app.MapHub<AuthHub>("/authHub");

    app.Run();
  }
}
