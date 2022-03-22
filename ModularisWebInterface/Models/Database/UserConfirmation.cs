using System.ComponentModel.DataAnnotations;

namespace ModularisWebInterface.Models.Database
{
    public class UserConfirmation
    {
        public UserConfirmation() { }
        public UserConfirmation(string Id)
        {
            this.Id = Id;
        }

        [Key]
        public string Id { get; set; }
        public bool IsConfirmedManually { get; set; }
    }
}
