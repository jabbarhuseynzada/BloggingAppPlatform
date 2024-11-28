using Core.Entities.Concrete;
using Core.Extensions;
using Core.Helpers.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Helpers.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }

        private DateTime _expirationDate;
        private TokenOptions _tokenOptions;


        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateAccessToken(User user, List<OperationClaim> opeartionClaims)
        {
            _expirationDate = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredential(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, opeartionClaims);
            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityHandler.WriteToken(jwt);

            return new AccessToken
            {
                Expiration = _expirationDate,
                Token = token,
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, List<OperationClaim> opeartionClaims)
        {
            var jwtSecurityToken = new JwtSecurityToken(
                 issuer: tokenOptions.Issuer,
                 audience: tokenOptions.Audience,
                 expires: _expirationDate,
                 notBefore: DateTime.Now,
                 claims: SetClaims(user, opeartionClaims),

                 signingCredentials: signingCredentials
                );
            return jwtSecurityToken;
        }

        /*private static IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {

            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.Username}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            //var userIdentity = new ClaimsIdentity(claims);
            return claims;

        }*/
        private static IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddUserId(user.Id.ToString()); // Add userId claim here
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName(user.Username);
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            return claims;
        }


        public static int? GetUserIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token is null or empty", nameof(token));
            }

            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

               /* // Log all claims for inspection
                foreach (var claim in jwtToken.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
*/
                // Make sure to search for the correct claim type
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (int.TryParse(userIdClaim, out int userId))
                {
                    return userId; // Return the parsed userId
                }
            }

            return null; // Return null if userId claim is not found or invalid
        }

        public static List<string> JwtGetUserRoles(string token)
        {
            var roles = new List<string>();

            if (string.IsNullOrWhiteSpace(token))
            {
                return roles;
            }

            var handler = new JwtSecurityTokenHandler();

            // Validate the token and read claims (this example doesn't include validation parameters, you should set them)
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                // Specify the signing key here
                // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey")),
                ValidateIssuer = false,
                ValidateAudience = false,
                // Add your valid issuer and audience if needed
            };

            try
            {
                var jwtToken = handler.ReadJwtToken(token);
                roles = jwtToken.Claims
                    .Where(c => c.Type == "role") // Assuming roles are stored with the "role" claim type
                    .Select(c => c.Value)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Token validation failed.", ex);
            }

            return roles;
        }
    }
}
