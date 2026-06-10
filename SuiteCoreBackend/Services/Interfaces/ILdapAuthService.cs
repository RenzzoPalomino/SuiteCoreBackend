using SuiteCoreBackend.Models.Entities;
using System.DirectoryServices.Protocols;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface ILdapAuthService
    {
        public LdapUser? Authenticate(string email, string password);
 
    }
}
