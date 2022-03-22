using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ModularisWebInterface.Models.UserManagement
{
    public class UsersModel
    {
        public IEnumerable<IdentityUser> Users { get; set; }
    }
}
