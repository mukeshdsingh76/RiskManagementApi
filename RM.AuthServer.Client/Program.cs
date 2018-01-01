using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RM.AuthServer.Client
{
  public static class Program
  {
    public static void Main() => MainAsync().GetAwaiter().GetResult();

    private static async Task MainAsync()
    {
      var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

      IConfigurationRoot configuration = builder.Build();

      var identityServerUrl = configuration.GetValue<string>("IdentityServerUrl");
      var apiServiceUrl = configuration.GetValue<string>("ApiServiceUrl");

      var disco = await DiscoveryClient.GetAsync(identityServerUrl).ConfigureAwait(false);

      var tokenClient = new TokenClient(disco.TokenEndpoint, "RiskManagementClientId", "RiskManagementSecretKey");
      var tokenResponse = await tokenClient.RequestClientCredentialsAsync("RiskManagement").ConfigureAwait(false);

      if (tokenResponse.IsError)
      {
        Console.WriteLine(tokenResponse.Error);
        return;
      }

      Console.WriteLine(tokenResponse.Json);
      Console.WriteLine("\n\n");

      var client = new HttpClient();
      client.SetBearerToken(tokenResponse.AccessToken);

      var response = await client.GetAsync($"{apiServiceUrl}/identity").ConfigureAwait(false);
      if (!response.IsSuccessStatusCode)
      {
        Console.WriteLine(response.StatusCode);
      }
      else
      {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Console.WriteLine(JArray.Parse(content));
      }

      Console.ReadLine();
    }
  }
}
