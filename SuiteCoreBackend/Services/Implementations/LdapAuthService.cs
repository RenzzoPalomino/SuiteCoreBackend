using Microsoft.Extensions.Options;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using System.Net;
using System.DirectoryServices.Protocols;

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
            var identifier = new LdapDirectoryIdentifier(
                _settings.Server, _settings.Port, false, false);

            using var connection = new LdapConnection(identifier);
            connection.SessionOptions.SecureSocketLayer = _settings.UseSSL;
            connection.SessionOptions.ProtocolVersion = 3;
            connection.AuthType = AuthType.Basic;
            connection.Timeout = TimeSpan.FromSeconds(10);

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

                // Mapear atributos
                return MapUser(entry);
            }
            catch (LdapException ldapex) 
            {
                throw new Exception($"Servicio LDAP no disponible: {ldapex.Message}");
            }
            catch (DirectoryOperationException){
                throw new Exception($"No se pudo completar la operación LDAP para el usuario {username}");
                
            }
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

        // Helper para evitar null checks repetidos
        private string GetAttr(SearchResultEntry entry, string attr) =>
            entry.Attributes[attr]?[0]?.ToString() ?? "";
    }
}
