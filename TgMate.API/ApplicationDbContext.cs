using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TgMate.API.Telegram.Pipelines;

namespace TgMate.API;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
  public DbSet<Pipeline> TelegramPipelines { get; set; }
  public DbSet<Unit> TelegramPipelineUnits { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
    base.OnConfiguring(builder);
    builder.UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<Unit>(unit =>
    {
      unit.HasOne(v => v.Pipeline)
        .WithMany(v => v.Units)
        .HasForeignKey(v => v.PipelineId)
        .HasPrincipalKey(v => v.Id);

      unit.HasOne(v => v.Parent)
        .WithMany(v => v.Children)
        .HasForeignKey(v => v.ParentId)
        .HasPrincipalKey(v => v.Id);

      unit.HasOne(v => v.InputHolder)
        .WithMany()
        .HasForeignKey(v => v.InputHolderId)
        .HasPrincipalKey(v => v.Id);

      unit.Property(v => v.Fn)
        .HasConversion(
          v => JsonSerializer.Serialize(v, FnMemberJsonSerializerContext.Default.Fn),
          v => JsonSerializer.Deserialize(v, FnMemberJsonSerializerContext.Default.Fn)!
        );

      unit.Property(v => v.Input)
        .HasConversion(
          v => JsonSerializer.Serialize(v, FnMemberJsonSerializerContext.Default.ListFnMember),
          v => JsonSerializer.Deserialize(v, FnMemberJsonSerializerContext.Default.ListFnMember)!
        );

      unit.Property(v => v.Output)
        .HasConversion(
          v => JsonSerializer.Serialize(v, FnMemberJsonSerializerContext.Default.ListFnMember),
          v => JsonSerializer.Deserialize(v, FnMemberJsonSerializerContext.Default.ListFnMember)!
        );
      ;
    });
  }

  public static string GetConnectionString()
  {
    var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
    var database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
    var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var poolSize = Environment.GetEnvironmentVariable("POSTGRES_POOL_SIZE");

    return $"Host={host}; Port={port}; Database={database}; Username={user}; Password={password}; Maximum Pool Size={poolSize}";
  }
}
