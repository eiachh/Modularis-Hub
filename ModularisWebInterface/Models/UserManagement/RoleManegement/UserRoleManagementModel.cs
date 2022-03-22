using ModularisWebInterface.Models.Database;
using System;
using System.Collections.Generic;
using ModularisWebInterface.Models.UserManagement;
using ModularisWebInterface.Models.UserManagement.Confirmation;

namespace ModularisWebInterface.Models.UserManagement.RoleManagement
{
    public class UserRoleManagementModel
    {
        public List<UserRoleManagementDisplayItem> UserConfirmations { get; set; }
        public UserRoleManagementModel() { }
        public UserRoleManagementModel(List<UserRoleManagementDisplayItem> userConfirmations)
        {
            UserConfirmations = userConfirmations;
        }
    }
}
