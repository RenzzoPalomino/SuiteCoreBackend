using AutoMapper;
using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;

namespace SuiteCoreBackend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ITestUserService _testUserService;
    private readonly ILdapAuthService _ldapAuthService;
    private readonly IJwtService _jwtService;
    private readonly IRadiusSessionService _radiusSessionService;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;

    public AuthService(
        ITestUserService testUserService,
        ILdapAuthService ldapAuthService,
        IJwtService jwtService,
        IRadiusSessionService radiusSessionService,
        IOptions<JwtSettings> jwtOptions,
        IMapper mapper)
    {
        _testUserService = testUserService;
        _ldapAuthService = ldapAuthService;
        _jwtService = jwtService;
        _radiusSessionService = radiusSessionService;
        _jwtSettings = jwtOptions.Value;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, string clientIp)
    {
        LdapUser user = _ldapAuthService.Authenticate(request.Username, request.Password);

        if (user is null)
            return null;

        var sessionId = await _radiusSessionService.StartSessionAsync(user.Username, clientIp);
        var token = _jwtService.GenerateToken(user, sessionId);
        var userDto = _mapper.Map<LdapUserDto>(user);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            User = userDto,
            SessionId = sessionId
        };
    }

    public async Task LogoutAsync(string sessionId, string username)
    {
        await _radiusSessionService.StopSessionAsync(sessionId, username);
    }
}
