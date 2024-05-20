using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using UserService.Helpers;
using UserService.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace UserService.Authorization
{
    public interface IJwtUtils
    {
        public List<Claim> GenerateClaims(User user);
        public string GenerateJwtToken(User user);
        public string ValidateJwtToken(string token);
     }

    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings; 
        
        public JwtUtils()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("./appsettings.json",
                    optional: false, reloadOnChange: true);
            var configuration = configurationBuilder.Build();

            // Ensure _context is initialized properly
            _appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();//GetValue<AppSettings>("AppSettings");
        }

        public List<Claim> GenerateClaims(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("avatarId", user.AvatarId.ToString()),
            };
            return claims.ToList();
        }

        public string GenerateJwtToken(User user)
        {
            List<Claim> claims = GenerateClaims(user);
    
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
    
            JwtHeader header = new JwtHeader(signIn);
    
            JwtPayload payload = new JwtPayload(
null,
null,
claims, 
                null,
                DateTime.UtcNow.AddDays(7));
    
            JwtSecurityToken token = new JwtSecurityToken(header, payload);
    
            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return serializedToken;
        }

        public string ValidateJwtToken(string token)
        {
            /*if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }*/
            return "";
        }
    }
}