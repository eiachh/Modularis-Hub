using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModularisWebInterface.Models.Database;
using ModularisWebInterface.Models.UserManagement.AuthorizationRequirements;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ModularisWebInterface.Models.UserManagement.Helper
{
    public class UserHelper
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserHelper(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
        }


        public async Task ModifyUserName(string userId, string desiredUserName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.UserName = desiredUserName;

            await _userManager.UpdateAsync(user);
        }

        public void ModifyUserActivation(string userId, bool isActivated)
        {
            var selectedUser = _db.UserConfirmation.FirstOrDefault(user => user.Id == userId);
            selectedUser.IsConfirmedManually = isActivated;
            _db.SaveChanges();
        }

        public async Task ModifyUserClaim(string userId, string claimName, bool shouldHaveClaim)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (shouldHaveClaim)
                await _userManager.AddClaimAsync(user, new Claim(claimName, claimName));
            else
            {
                var claimsOfUser = await _userManager.GetClaimsAsync(user);
                var claimToRemove = claimsOfUser.SingleOrDefault(claim => claim.Type == claimName);
                await _userManager.RemoveClaimAsync(user, claimToRemove);
            }
        }

        public async Task<SingleUserModel> GetSingleUserModelFor(string userId)
        {
            var user = await _db.Users.SingleOrDefaultAsync(user => user.Id == userId);
            if (user is null)
                return null;

            var confirmation = await _db.UserConfirmation.SingleOrDefaultAsync(user => user.Id == userId);

            bool isActivated = false;
            if (confirmation is null)
                await CreateConfirmationData(userId).ConfigureAwait(false);
            else
                isActivated = confirmation.IsConfirmedManually;

            SingleUserModel model = new()
            {
                Id = userId,
                UserName = user.UserName,
                IsActivated = isActivated,
                UserClaims = GetClaimsForUser(userId),
            };

            return model;
        }

        private async Task CreateConfirmationData(string userId)
        {
            UserConfirmation confirmation = new()
            {
                Id = userId,
                IsConfirmedManually = false,
            };

            _db.UserConfirmation.Add(confirmation);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        private IList<UserClaim> GetClaimsForUser(string userId)
        {
            List<UserClaim> claims = new List<UserClaim>();

            var existingClaims = GetExistingClaims();
            var userClaims = _db.UserClaims.Where(x => x.UserId == userId).ToList();

            foreach (var claim in existingClaims)
            {
                claims.Add(new UserClaim { ClaimType = claim, HasClaim = userClaims.Any(userClaim => userClaim.ClaimType == claim) });
            }

            return claims;
        }

        private IEnumerable<string> GetExistingClaims()
        {
            return _db.ModularisUserClaims.AsEnumerable().DistinctBy(x => x.ClaimType).Select(elem => elem.ClaimType);
        }
    }
}
