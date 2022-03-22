using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using ModularisWebInterface.Models.Database;

namespace ModularisWebInterface.Models.UserManagement.Confirmation
{
    public class ManualConfirmation : IUserConfirmation<IdentityUser>
    {
        private AppDbContext _db;
        public ManualConfirmation(AppDbContext db)
        {
            _db = db;
        }
        public Task<bool> IsConfirmedAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var asdasd  = _db.UserConfirmation.FirstOrDefault(elem => elem.Id == user.Id);
            if(asdasd is null)
            {
                CreateFalseCConfirnment(user);
                return Task.FromResult(false);
            }


            return Task.FromResult(asdasd.IsConfirmedManually);
        }

        private void CreateFalseCConfirnment(IdentityUser user)
        {
            _db.UserConfirmation.Add(new UserConfirmation { Id = user.Id, IsConfirmedManually = false });
            _db.SaveChanges();
        }
    }
}
