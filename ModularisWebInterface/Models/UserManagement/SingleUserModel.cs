using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModularisWebInterface.Models.Database;
using ModularisWebInterface.Models.UserManagement.AuthorizationRequirements;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModularisWebInterface.Models.UserManagement
{
    public class SingleUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsActivated { get; set; }
        public IList<UserClaim> UserClaims { get; set; }
    }
}
