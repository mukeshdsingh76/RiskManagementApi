using IdentityServer4.Models;
using System.Collections.Generic;

namespace RM.AuthServer
{
  public static class Config
  {
    public static IEnumerable<ApiResource> GetApiResources()
    {
      return new List<ApiResource>
      {
          new ApiResource("RiskManagement", "RiskManagement API")
      };
    }

    public static IEnumerable<Client> GetClients()
    {
      return new List<Client>
      {
        new Client
        {
            ClientId = "RiskManagementClientId",
            ClientClaimsPrefix = "client_",
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            ClientSecrets = { new Secret("RiskManagementSecretKey".Sha256()) },
            AllowedScopes = { "RiskManagement" },
            Claims = {
                new System.Security.Claims.Claim("role", "Manager"),
                new System.Security.Claims.Claim("permission", "view_claims")
            }

        }
      };
    }
  }
}
