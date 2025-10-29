using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_backend.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(string username, string role)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username), "Username cannot be null or empty");
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role), "Role cannot be null or empty");
            if (role != "User" && role != "Admin")
                throw new ArgumentException("Invalid role. Role must be 'User' or 'Admin'.", nameof(role));

            // Validate configuration
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryMinutes = _configuration["JwtSettings:ExpiryMinutes"];

            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("JwtSettings:SecretKey", "SecretKey is null or missing from configuration");
            if (secretKey.Length < 32)
                throw new ArgumentException("JwtSettings:SecretKey", "SecretKey must be at least 32 characters long for security");
            if (string.IsNullOrEmpty(issuer))
                throw new ArgumentNullException("JwtSettings:Issuer", "Issuer is null or missing from configuration");
            if (string.IsNullOrEmpty(audience))
                throw new ArgumentNullException("JwtSettings:Audience", "Audience is null or missing from configuration");
            if (!double.TryParse(expiryMinutes, out double expiry) || expiry <= 0)
                throw new ArgumentException("JwtSettings:ExpiryMinutes", "ExpiryMinutes must be a valid positive number");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(expiry),

                claims: authClaims,
                signingCredentials: creds
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));

        }
    }
}
