using System.ComponentModel.DataAnnotations;

namespace ModularisWebInterface.Models.Database
{
    public class ModularisUserClaims
    {
        public ModularisUserClaims() { }

        [Key]
        public string Id { get; set; }
        public string ClaimType { get; set; }
    }
}
