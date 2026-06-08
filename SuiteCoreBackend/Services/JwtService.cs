using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

        public string GenerateToken(Guid userId, string email, string role)
        {
            var claims = new List<Claim>
            {
                // Identificador único del usuario
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),

                // Correo del usuario
                new Claim(ClaimTypes.Email, email),

                // Rol del usuario
                new Claim(ClaimTypes.Role, role),

                // Identificador único del token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

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
