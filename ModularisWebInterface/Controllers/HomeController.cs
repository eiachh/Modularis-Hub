using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModularisWebInterface.Models;
using ModularisWebInterface.Models.Database;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace ModularisWebInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger,
             UserManager<IdentityUser> userManager,
             SignInManager<IdentityUser> signInManager,
             AppDbContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            
             var usr = await _userManager.GetUserAsync(HttpContext.User);
            //var token = await _userManager.GenerateUserTokenAsync(usr, "me", "purpose?");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
               
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                //var claims = HttpContext.User.Claims.Append(new Claim(ClaimTypes.Name, "Bob"));
                if (signInResult.Succeeded)
                {
                    var asdad = HttpContext.User;
                    return RedirectToAction("Privacy");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _db.UserConfirmation.Add(new UserConfirmation(user.Id));
                _db.SaveChanges();

                return RedirectToAction("Login");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
