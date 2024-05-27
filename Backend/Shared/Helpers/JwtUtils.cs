using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Models;
using Shared.Dtos;


namespace Shared.Helpers
{
    public interface IJwtUtils
    {
        public List<Claim> GenerateClaims(User user);
        public string GenerateJwtToken(User user);
        public UserDto ValidateJwtToken(string token);
     }

    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings; 
        
        public JwtUtils()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("./host.json",
                    optional: false, reloadOnChange: true);
            var configuration = configurationBuilder.Build();

            _appSettings = new AppSettings();
            _appSettings.Secret = "";
            // Ensure _context is initialized properly
            _appSettings.Secret = configuration.GetSection("AppSettings:Secret").Value.ToString();//<AppSettings>();//GetValue<AppSettings>("AppSettings");
        }

        public List<Claim> GenerateClaims(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("avatarId", user.AvatarId.ToString()),
            };
            return claims.ToList();
        }

        public string GenerateJwtToken(User user)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[] 
                    { 
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("id", user.Id.ToString()),
                        new Claim("Name", user.Name),
                        new Claim("Email", user.Email),
                        new Claim("avatarId", user.AvatarId.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddDays(7),  
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha512)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        public UserDto ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
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
                UserDto user = new UserDto();
                user.UserId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                user.Email = jwtToken.Claims.First(x => x.Type.Equals("Email")).Value;
                user.Name = jwtToken.Claims.First(x => x.Type.Equals("Name")).Value;
                user.AvatarId = int.Parse(jwtToken.Claims.First(x => x.Type == "avatarId").Value);
                return user;

                // return user id from JWT token if validation successful
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}