using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace RM.AuthServer
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      Console.Title = "IdentityServer";

      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
  }
}
