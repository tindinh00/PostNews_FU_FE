using System.IdentityModel.Tokens.Jwt;

namespace Client.Helpers
{
    public class TokenHelper
    {
        public IDictionary<string, string> GetAllClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                Console.WriteLine("Invalid token.");
            }

            var claimsDictionary = new Dictionary<string, string>();

            foreach (var claim in jwtToken.Claims)
            {
                //Console.WriteLine(claim);
                claimsDictionary[claim.Type] = claim.Value;
            }

            return claimsDictionary;
        }

        public string FindClaimValueByKey(IDictionary<string, string> claims, string claimKey)
        {
            foreach (var claim in claims)
            {
                // Check if the claim key contains the word claimKey
                if (claim.Key.Contains(claimKey, StringComparison.OrdinalIgnoreCase))
                {
                    //Console.WriteLine("finding: " + claim);
                    return claim.Value;
                }
            }

            return null; // Return null if the claimKey claim is not found
        }
    }
}
