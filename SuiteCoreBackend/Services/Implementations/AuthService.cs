using AutoMapper;
using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Helpers;
using SuiteCoreBackend.Infrastructure.Implementations;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;

namespace SuiteCoreBackend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ILdapAuthService _ldapAuthService;
    private readonly IJwtService _jwtService;
    private readonly IRadiusSessionService _radiusSessionService;
    private readonly JwtSettings _jwtSettings; 
    private readonly IMapper _mapper;
    private readonly IUserActivityRepository _userActivityRepository;

    private readonly DateTimeHelper _dateHelper = new DateTimeHelper();

    public AuthService(
        ILdapAuthService ldapAuthService,
        IJwtService jwtService,
        IRadiusSessionService radiusSessionService,
        IOptions<JwtSettings> jwtOptions,
        IUserActivityRepository userActivityRepository,
        IMapper mapper)
    {
        _ldapAuthService = ldapAuthService;
        _jwtService = jwtService;
        _radiusSessionService = radiusSessionService;
        _jwtSettings = jwtOptions.Value;
        _userActivityRepository = userActivityRepository;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, string clientIp)
    {
        LdapUser user = _ldapAuthService.Authenticate(request.Username, request.Password);

        if (user is null)
            return null;

        string sessionId = generateIdSession();
        /*Suspendido temporalmente*/
        //await _radiusSessionService.StartSessionAsync(user.Username, clientIp, sessionId);

        var currentDatetime = _dateHelper.GetPeruDateTime();

        _userActivityRepository.RegisterLogin(new UserActivity
        {
            SessionId = sessionId,
            Username = user.Username,
            IpAddress = clientIp,
            StartedAt = currentDatetime,
            EndedAt = currentDatetime.AddMinutes(_jwtSettings.ExpiresInMinutes),
            LastActivityAt = currentDatetime
        });


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

    #region utilidades de sesion
    public string generateIdSession()
    {
        var sessionId = Guid.NewGuid().ToString("N")[..16].ToUpper();
        return sessionId;
    }
    #endregion
}
