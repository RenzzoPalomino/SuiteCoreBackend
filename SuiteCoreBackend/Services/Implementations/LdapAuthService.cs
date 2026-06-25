using Microsoft.Extensions.Options;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Settings;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Numerics;

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
                    "cn", "gidNumber"
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

                var filter = supplementaryUids.Count > 0
                    ? $"(|(gidNumber={gidNumber}){supplementaryFilter})"
                    : $"(gidNumber={gidNumber})";

                var userSearch = new SearchRequest(
                    _settings.BaseDn,
                    filter,
                    SearchScope.Subtree,
                    "displayName", "givenName", "sn", "uid",
                    "uidNumber", "gidNumber", "memberOf", "department", "title"
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
