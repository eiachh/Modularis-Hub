using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ModularisWebInterface.Controllers
{
    public class RestModuleCallController : Controller
    {
        private readonly MainHub _mainHub;
        public RestModuleCallController(MainHub hub)
        {
            _mainHub = hub;
        }
        [HttpPost]
        public async Task<IActionResult>TargetedCall()
        {
            Console.WriteLine("TargetedCall received!");
            await _mainHub.TargetedCall("ModularisMainBot", "WriteToBotChannel", Request.HttpContext.Request.Form.FirstOrDefault().Key);
            return Json("OK");
        }
    }
}
