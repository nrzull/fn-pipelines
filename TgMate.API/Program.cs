using Microsoft.EntityFrameworkCore;

namespace TgMate.API;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

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
    app.MapControllers();

    app.Run();
  }
}
