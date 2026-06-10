using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuiteCoreBackend.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        public JwtService(IOptions<JwtSettings> jwtOptions)
        {
            // Obtenemos las configuraciones JWT registradas en Program.cs
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(LdapUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.GivenName, user.FirstName),
                new("department", user.Department),
                new("username", user.Username),
            };
            // Un claim por rol — [Authorize(Roles = "Administradores")] funciona directo
            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role));


            //Convertimos la Key del appsettings en una clave segura, esta clave se usará para firmar el token
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key)
            );


            //Definimos el algoritmo de firma
            //HmacSha256 es uno de los más usados para JWT simétrico
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
