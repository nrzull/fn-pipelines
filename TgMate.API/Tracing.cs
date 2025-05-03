using System.Diagnostics;

namespace TgMate.API;

public static class Tracing
{
  public static ActivitySource Source = new ActivitySource("TgMate.API");
}
