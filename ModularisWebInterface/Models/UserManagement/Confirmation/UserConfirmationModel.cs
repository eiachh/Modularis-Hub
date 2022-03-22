using System.Collections.Generic;

namespace ModularisWebInterface.Models.UserManagement.Confirmation
{
    public class UserConfirmationModel
    {
        public IEnumerable<UserConfirmationDisplayItem> UserConfirmations { get; set; }
        public UserConfirmationModel(IEnumerable<UserConfirmationDisplayItem> userConfirmations)
        {
            UserConfirmations = userConfirmations;
        }
    }
}
