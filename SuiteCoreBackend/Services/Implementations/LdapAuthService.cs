using Microsoft.Extensions.Options;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SuiteCoreBackend.Services.Implementations
{
    public class LdapAuthService : ILdapAuthService
    {
        private readonly LdapSettings _settings;

        public LdapAuthService(IOptions<LdapSettings> settings)
        {
            _settings = settings.Value;
        }

        public LdapUser? Authenticate(string username, string password)
        {
            using var connection = CreateServiceConnection();

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.ServiceUser, _settings.ServicePassword));

                var search = new SearchRequest(
                    _settings.BaseDn,
                    $"(uid={username})",
                    SearchScope.Subtree,
                    "distinguishedName", "displayName", "givenName", "sn",
                    "uid", "uidNumber", "gidNumber", "memberOf", "department", "title"
                );

                var response = (SearchResponse)connection.SendRequest(search);
                if (response.Entries.Count == 0) return null;

                var entry = response.Entries[0];

                // Validar contraseña
                connection.Bind(new NetworkCredential(entry.DistinguishedName, password));

                return MapUser(entry);
            }
            catch (LdapException ldapex)
            {
                throw new Exception($"Servicio LDAP no disponible: {ldapex.Message}");
            }
            catch (DirectoryOperationException)
            {
                throw new Exception($"No se pudo completar la operación LDAP para el usuario {username}");
            }
        }

        public List<LdapRole> GetRoles()
        {
            using var connection = CreateServiceConnection();

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.ServiceUser, _settings.ServicePassword));

                var search = new SearchRequest(
                    _settings.BaseDn,
                    "(|(objectClass=group)(objectClass=groupOfNames)(objectClass=posixGroup))",
                    SearchScope.Subtree,
                    "cn", "gidNumber", "description"
                );

                var response = (SearchResponse)connection.SendRequest(search);

                return response.Entries
                    .Cast<SearchResultEntry>()
                    .Select(e => new LdapRole
                    {
                        Id = GetAttr(e, "gidNumber"),
                        Name = GetAttr(e, "cn"),
                        Description = GetAttr(e, "description")
                    })
                    .Where(r => !string.IsNullOrEmpty(r.Name) && !string.IsNullOrEmpty(r.Id))
                    .OrderBy(r => r.Name)
                    .ToList();
            }
            catch (LdapException ldapex)
            {
                throw new Exception($"Servicio LDAP no disponible: {ldapex.Message}");
            }
        }

        public List<LdapUser> GetUsersByGid(string gidNumber)
        {
            using var connection = CreateServiceConnection();

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.ServiceUser, _settings.ServicePassword));

                // Miembros suplementarios: uids listados en memberUid del grupo
                var supplementaryUids = new List<string>();
                var groupSearch = new SearchRequest(
                    _settings.BaseDn,
                    $"(&(gidNumber={gidNumber})(objectClass=posixGroup))",
                    SearchScope.Subtree,
                    "memberUid"
                );
                var groupResponse = (SearchResponse)connection.SendRequest(groupSearch);
                if (groupResponse.Entries.Count > 0)
                {
                    var attr = groupResponse.Entries[0].Attributes["memberUid"];
                    if (attr != null)
                        supplementaryUids = attr.GetValues(typeof(string))
                            .Select(v => v.ToString()!)
                            .ToList();
                }

                // Combinar: miembros primarios (gidNumber en el usuario) + suplementarios (memberUid del grupo)
                var supplementaryFilter = supplementaryUids.Count > 0
                    ? string.Concat(supplementaryUids.Select(uid => $"(uid={uid})"))
                    : "";

                var membersFilter = supplementaryUids.Count > 0
                    ? $"(|(gidNumber={gidNumber}){supplementaryFilter})"
                    : $"(gidNumber={gidNumber})";

                var userSearch = new SearchRequest(
                    _settings.BaseDn,
                    membersFilter,
                    SearchScope.Subtree,
                    "displayName", "givenName", "sn", "uid",
                    "uidNumber", "gidNumber", "memberOf", "department", "title", "description"
                );

                var userResponse = (SearchResponse)connection.SendRequest(userSearch);

                return userResponse.Entries
                    .Cast<SearchResultEntry>()
                    .Select(MapUser)
                    .Where(u => !string.IsNullOrEmpty(u.Username) && !string.IsNullOrEmpty(u.UidNumber))
                    .ToList();
            }
            catch (LdapException ldapex)
            {
                throw new Exception($"Servicio LDAP no disponible: {ldapex.Message}");
            }
        }

        public LdapUser CreateUser(CreateLdapUserDto dto)
        {
            using var connection = CreateServiceConnection();

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.AdminUser, _settings.AdminPassword));

                var nextUidNumber = GetNextUidNumber(connection);
                var dn = $"uid={dto.Username},ou=People,{_settings.BaseDn}";
                var displayName = $"{dto.FirstName} {dto.LastName}".Trim();

                var addRequest = new AddRequest(dn,
                    new DirectoryAttribute("objectClass", "top", "inetOrgPerson", "posixAccount"),
                    new DirectoryAttribute("uid", dto.Username),
                    new DirectoryAttribute("cn", displayName),
                    new DirectoryAttribute("givenName", dto.FirstName),
                    new DirectoryAttribute("sn", dto.LastName),
                    new DirectoryAttribute("displayName", displayName),
                    new DirectoryAttribute("uidNumber", nextUidNumber.ToString()),
                    new DirectoryAttribute("gidNumber", dto.GidNumber),
                    new DirectoryAttribute("homeDirectory", $"/home/{dto.Username}"),
                    new DirectoryAttribute("userPassword", HashPassword(dto.Password))
                );

                if (!string.IsNullOrWhiteSpace(dto.Department))
                    addRequest.Attributes.Add(new DirectoryAttribute("departmentNumber", dto.Department));

                if (!string.IsNullOrWhiteSpace(dto.Title))
                    addRequest.Attributes.Add(new DirectoryAttribute("title", dto.Title));

                connection.SendRequest(addRequest);

                return new LdapUser
                {
                    DisplayName = displayName,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Username = dto.Username,
                    UidNumber = nextUidNumber.ToString(),
                    GidNumber = dto.GidNumber,
                    Department = dto.Department,
                    Title = dto.Title
                };
            }
            catch (DirectoryOperationException ex) when (ex.Response?.ResultCode == ResultCode.InsufficientAccessRights)
            {
                throw new UnauthorizedAccessException(
                    "El usuario administrador no tiene permisos de escritura en LDAP.");
            }
            catch (DirectoryOperationException ex) when (ex.Response?.ResultCode == ResultCode.EntryAlreadyExists)
            {
                throw new InvalidOperationException(
                    $"Ya existe un usuario con el nombre '{dto.Username}' en LDAP.");
            }
            catch (DirectoryOperationException ex)
            {
                throw new Exception($"Error LDAP al crear usuario ({ex.Response?.ResultCode}): {ex.Message}");
            }
            catch (LdapException ex)
            {
                throw new Exception($"Servicio LDAP no disponible: {ex.Message}");
            }

        }

        public LdapUser UpdateUser(string username, UpdateLdapUserDto dto)
        {
            using var connection = CreateServiceConnection();

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.AdminUser, _settings.AdminPassword));

                var dn = ResolveUserDn(connection, username)
                    ?? throw new KeyNotFoundException($"No se encontró el usuario '{username}' en LDAP.");

                var displayName = $"{dto.FirstName} {dto.LastName}".Trim();

                var modifyRequest = new ModifyRequest(dn);
                modifyRequest.Modifications.Add(Mod("givenName", dto.FirstName));
                modifyRequest.Modifications.Add(Mod("sn", dto.LastName));
                modifyRequest.Modifications.Add(Mod("displayName", displayName));
                modifyRequest.Modifications.Add(Mod("cn", displayName));

                if (!string.IsNullOrWhiteSpace(dto.GidNumber))
                    modifyRequest.Modifications.Add(Mod("gidNumber", dto.GidNumber!));

                connection.SendRequest(modifyRequest);

                return new LdapUser
                {
                    DisplayName = displayName,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Username = username,
                    GidNumber = dto.GidNumber ?? ""
                };
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DirectoryOperationException ex) when (ex.Response?.ResultCode == ResultCode.InsufficientAccessRights)
            {
                throw new UnauthorizedAccessException(
                    "El usuario administrador no tiene permisos de escritura en LDAP.");
            }
            catch (DirectoryOperationException ex)
            {
                throw new Exception($"Error LDAP al actualizar usuario ({ex.Response?.ResultCode}): {ex.Message}");
            }
            catch (LdapException ex)
            {
                throw new Exception($"Servicio LDAP no disponible: {ex.Message}");
            }
        }

        public void DisableUser(string username)
        {
            using var connection = CreateServiceConnection();

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.AdminUser, _settings.AdminPassword));

                var dn = ResolveUserDn(connection, username)
                    ?? throw new KeyNotFoundException($"No se encontró el usuario '{username}' en LDAP.");

                // Reemplaza la contraseña con un hash inválido — impide el bind sin eliminar la entrada
                var lockedHash = $"{{!}}{HashPassword(Guid.NewGuid().ToString())}";

                var modifyRequest = new ModifyRequest(dn);
                modifyRequest.Modifications.Add(Mod("userPassword", lockedHash));
                modifyRequest.Modifications.Add(Mod("description", "DISABLED"));

                connection.SendRequest(modifyRequest);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DirectoryOperationException ex) when (ex.Response?.ResultCode == ResultCode.InsufficientAccessRights)
            {
                throw new UnauthorizedAccessException(
                    "El usuario administrador no tiene permisos de escritura en LDAP.");
            }
            catch (DirectoryOperationException ex)
            {
                throw new Exception($"Error LDAP al deshabilitar usuario ({ex.Response?.ResultCode}): {ex.Message}");
            }
            catch (LdapException ex)
            {
                throw new Exception($"Servicio LDAP no disponible: {ex.Message}");
            }
        }

        public void EnableUser(string username)
        {
            using var connection = CreateServiceConnection();

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.AdminUser, _settings.AdminPassword));

                var dn = ResolveUserDn(connection, username)
                    ?? throw new KeyNotFoundException($"No se encontró el usuario '{username}' en LDAP.");

                var modifyRequest = new ModifyRequest(dn);
                modifyRequest.Modifications.Add(Mod("userPassword", HashPassword(_settings.DefaultPassword)));
                var deleteDescription = new DirectoryAttributeModification
                {
                    Name = "description",
                    Operation = DirectoryAttributeOperation.Delete
                };
                modifyRequest.Modifications.Add(deleteDescription);

                connection.SendRequest(modifyRequest);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (DirectoryOperationException ex) when (ex.Response?.ResultCode == ResultCode.InsufficientAccessRights)
            {
                throw new UnauthorizedAccessException(
                    "El usuario administrador no tiene permisos de escritura en LDAP.");
            }
            catch (DirectoryOperationException ex)
            {
                throw new DirectoryOperationException($"Error LDAP al habilitar usuario ({ex.Response?.ResultCode}): {ex.Message}");
            }
            catch (LdapException ex)
            {
                throw new Exception($"Servicio LDAP no disponible: {ex.Message}");
            }
        }

        private string? ResolveUserDn(LdapConnection connection, string username)
        {
            var search = new SearchRequest(
                _settings.BaseDn,
                $"(uid={username})",
                SearchScope.Subtree,
                "distinguishedName"
            );

            var response = (SearchResponse)connection.SendRequest(search);
            return response.Entries.Count > 0
                ? response.Entries[0].DistinguishedName
                : null;
        }

        private int GetNextUidNumber(LdapConnection connection)
        {
            var search = new SearchRequest(
                _settings.BaseDn,
                "(objectClass=posixAccount)",
                SearchScope.Subtree,
                "uidNumber"
            );

            var response = (SearchResponse)connection.SendRequest(search);

            var max = response.Entries
                .Cast<SearchResultEntry>()
                .Select(e => int.TryParse(GetAttr(e, "uidNumber"), out var n) ? n : 0)
                .DefaultIfEmpty(10000)
                .Max();

            return max + 1;
        }

        private static DirectoryAttributeModification Mod(string name, string value)
        {
            var mod = new DirectoryAttributeModification
            {
                Name = name,
                Operation = DirectoryAttributeOperation.Replace
            };
            mod.Add(value);
            return mod;
        }

        // SSHA: SHA1(password + salt), luego Base64({hash}{salt})
        private static string HashPassword(string password)
        {
            var salt = new byte[8];
            RandomNumberGenerator.Fill(salt);

            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combined = passwordBytes.Concat(salt).ToArray();

            var hash = SHA1.HashData(combined);
            var hashWithSalt = hash.Concat(salt).ToArray();

            return $"{{SSHA}}{Convert.ToBase64String(hashWithSalt)}";
        }

        private LdapConnection CreateServiceConnection()
        {
            var identifier = new LdapDirectoryIdentifier(
                _settings.Server, _settings.Port, false, false);

            var connection = new LdapConnection(identifier);
            connection.SessionOptions.SecureSocketLayer = _settings.UseSSL;
            connection.SessionOptions.ProtocolVersion = 3;
            connection.AuthType = AuthType.Basic;
            connection.Timeout = TimeSpan.FromSeconds(10);

            return connection;
        }

        private LdapUser MapUser(SearchResultEntry entry)
        {
            var user = new LdapUser
            {
                DisplayName = GetAttr(entry, "displayName"),
                FirstName = GetAttr(entry, "givenName"),
                LastName = GetAttr(entry, "sn"),
                Username = GetAttr(entry, "uid"),
                UidNumber = GetAttr(entry, "uidNumber"),
                GidNumber = GetAttr(entry, "gidNumber"),
                Department = GetAttr(entry, "department"),
                Title = GetAttr(entry, "title"),
                IsActive = !GetAttr(entry, "description").Trim().Equals("DISABLED", StringComparison.OrdinalIgnoreCase),
            };

            if (entry.Attributes["memberOf"] != null)
            {
                foreach (var groupDn in entry.Attributes["memberOf"].GetValues(typeof(string)))
                {
                    var dn = groupDn.ToString()!;
                    user.Groups.Add(dn);
                    user.Roles.Add(dn.Split(',')[0].Replace("CN=", "").Trim());
                }
            }

            return user;
        }

        private string GetAttr(SearchResultEntry entry, string attr) =>
            entry.Attributes[attr]?[0]?.ToString() ?? "";
    }
}
