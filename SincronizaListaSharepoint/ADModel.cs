namespace SincronizaListaSharepoint
{
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;

    public class ADModel
    {
        /// <summary>
        /// Get User infromation from Active Directory.
        /// </summary>
        /// <param name="domain">Dominio.</param>
        /// <param name="username">Username.</param>
        /// <returns></returns>
        public UserInformation GetUserInformation(string domain, string username)
        {
            UserInformation info = new UserInformation();

            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, username);
                if (user != null)
                {
                    DirectoryEntry directoryEntry = user.GetUnderlyingObject() as DirectoryEntry;

                    info.Name = user.Name;
                    info.Email = user.EmailAddress;
                    info.Username = username;
                    if (directoryEntry.Properties["department"].Value != null)
                    {
                        info.Department = directoryEntry.Properties["department"].Value.ToString();
                    }
                    info.Username = username;
                    info.Phone = user.VoiceTelephoneNumber;
                    info.UUID = user.Guid.Value.ToString();

                    var groups = user.GetGroups();
                    if (groups != null)
                    {
                        foreach (var group in groups)
                        {
                            info.Roles.Add(@group.Name);
                        }
                    }
                    info.Filled = true;
                }
            }

            return info;
        }
    }

    public class UserInformation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string UUID { get; set; }
        public bool Filled { get; set; }

        public List<string> Roles { get; set; }

        public UserInformation()
        {
            Roles = new List<string>();
            UUID = string.Empty;
            Name = string.Empty;
            Username = string.Empty;
            Department = string.Empty;
            Phone = string.Empty;
        }
    }
}
