using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModularisWebInterface.Models.Database;
using ModularisWebInterface.Models.UserManagement;
using ModularisWebInterface.Models.UserManagement.Helper;
using System.Linq;
using System.Threading.Tasks;

namespace ModularisWebInterface.Controllers
{
    [Authorize(Policy = "UserManagement")]
    public class UserManagementController : Controller
    {
        private readonly AppDbContext _db;
        public UserManagementController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return RedirectToAction("UserListDisplay");
        }
        public async Task<IActionResult> UserListDisplay()
        {
            UsersModel users = new() { Users = await _db.Users.ToListAsync() };
            return View(users);
        }

        public async Task<IActionResult> EditUser(string id, [FromServices] UserHelper userHelper)
        {
            var model = await userHelper.GetSingleUserModelFor(id);

            if(model is null)
                return new JsonResult("User not found");

            return View(model);
        }

        public async Task<IActionResult> DeleteUser(string id, [FromServices] UserHelper userHelper)
        {
            await userHelper.DeleteUser(id);
            return RedirectToAction("UserListDisplay");
        }

        public async Task<IActionResult> ModifyUser(SingleUserModel modifiedUser, [FromServices] UserHelper userHelper)
        {
            var originalUser = await userHelper.GetSingleUserModelFor(modifiedUser.Id);

            if(originalUser.UserName != modifiedUser.UserName)
                await userHelper.ModifyUserName(modifiedUser.Id, modifiedUser.UserName).ConfigureAwait(false);

            if (originalUser.IsActivated != modifiedUser.IsActivated)
                userHelper.ModifyUserActivation(modifiedUser.Id, modifiedUser.IsActivated);

            foreach (var originalCalim in originalUser.UserClaims)
            {
                var modifiedClaim = modifiedUser.UserClaims.FirstOrDefault(claim => claim.ClaimType == originalCalim.ClaimType);
                if (originalCalim.HasClaim != modifiedClaim.HasClaim)
                    await userHelper.ModifyUserClaim(modifiedUser.Id, modifiedClaim.ClaimType, modifiedClaim.HasClaim);
            }

            return RedirectToAction("UserListDisplay");
        }
    }
}
