using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RM.Api
{
  public static class Authorization
  {
    private const string ClaimsPrefix = "client_";
    public static class Type
    {
      public const string Role = "role";
      public const string Permission = "permission";
    }

    public static class Role
    {
      public const string Manager = "Manager";
    }

    public static class Permission
    {
      public const string ViewClaims = "view_claims";
    }

    public static bool IsRoleSatisfied(ClaimsPrincipal User, string roleValue)
    {
      return IsPolicySatisfied(User, Type.Role, roleValue);
    }

    public static bool IsPermissionSatisfied(ClaimsPrincipal User, string permissionValue)
    {
      return IsPolicySatisfied(User, Type.Permission, permissionValue);
    }

    private static bool IsPolicySatisfied(ClaimsPrincipal User, string claimType, string claimValue)
    {
      //System.Console.Out.WriteLine("claimType={0}; claimValue={1}", claimType, claimValue);
      IEnumerable<Claim> claims = User.Claims;
      //System.Console.Out.WriteLine("claims are:");
      //foreach (Claim c in claims)
      //{
      //  System.Console.Out.WriteLine(c.Type + "; " + c.Value);
      //}

      return User.Claims.Any(
          claim =>
              claim.Type.StartsWith(ClaimsPrefix + claimType, System.StringComparison.InvariantCultureIgnoreCase)
              && string.Equals(claim.Value, claimValue, System.StringComparison.InvariantCultureIgnoreCase));
    }
  }
}
