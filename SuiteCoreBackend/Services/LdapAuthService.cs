using Microsoft.Extensions.Options;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;
using System.Net;
using System.DirectoryServices.Protocols;

namespace SuiteCoreBackend.Services
{
    public class LdapAuthService : ILdapAuthService
    {
        private readonly LdapSettings _settings;

        public LdapAuthService(IOptions<LdapSettings> settings)
        {
            _settings = settings.Value;
        }

        public LdapUser? Authenticate(string email, string password)
        {
            var identifier = new LdapDirectoryIdentifier(
                _settings.Server, _settings.Port, true, false);

            using var connection = new LdapConnection(identifier);
            connection.SessionOptions.SecureSocketLayer = _settings.UseSSL;
            connection.SessionOptions.ProtocolVersion = 3;
            connection.AuthType = AuthType.Basic;
            connection.Timeout = TimeSpan.FromSeconds(10);

            try
            {
                connection.Bind(new NetworkCredential(
                    _settings.ServiceUser, _settings.ServicePassword));

                // Pedir todos los atributos necesarios de una sola vez
                var search = new SearchRequest(
                    _settings.BaseDn,
                    $"(mail={email})",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    "distinguishedName", "displayName", "givenName", "sn",
                    "mail", "sAMAccountName", "memberOf", "department", "title"
                );

                var response = (SearchResponse)connection.SendRequest(search);
                if (response.Entries.Count == 0) return null;

                var entry = response.Entries[0];

                // Validar contraseña
                connection.Bind(new NetworkCredential(entry.DistinguishedName, password));

                // Mapear atributos
                return MapUser(entry);
            }
            catch (LdapException) { return null; }
            catch (DirectoryOperationException) { return null; }
        }

        private LdapUser MapUser(SearchResultEntry entry)
        {
            var user = new LdapUser
            {
                DisplayName = GetAttr(entry, "displayName"),
                FirstName = GetAttr(entry, "givenName"),
                LastName = GetAttr(entry, "sn"),
                Email = GetAttr(entry, "mail"),
                Username = GetAttr(entry, "sAMAccountName"),
                Department = GetAttr(entry, "department"),
                Title = GetAttr(entry, "title"),
            };

            // memberOf devuelve los DNs completos de cada grupo
            // ej: "CN=Administradores,OU=Grupos,DC=empresa,DC=com"
            if (entry.Attributes["memberOf"] != null)
            {
                foreach (var groupDn in entry.Attributes["memberOf"].GetValues(typeof(string)))
                {
                    var dn = groupDn.ToString()!;
                    user.Groups.Add(dn);

                    // Extraer solo el CN como nombre de rol
                    var cn = dn.Split(',')[0].Replace("CN=", "").Trim();
                    user.Roles.Add(cn);
                }
            }

            return user;
        }

        // Helper para evitar null checks repetidos
        private string GetAttr(SearchResultEntry entry, string attr) =>
            entry.Attributes[attr]?[0]?.ToString() ?? "";
    }
}
