using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModularisWebInterface.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ModularisWebInterface.Controllers
{
    public class BotDisplayController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        [BindProperty]
        public TestModel TestModel { get; set; }

        public BotDisplayController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? id)
        {
            TestModel = new TestModel();
            return View(TestModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    return Json(new { data = 2 });
        //}
    }
}
