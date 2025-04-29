using System.Diagnostics;
using ClarityWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClarityWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            EmailViewModel m = new ();
            return View(m);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult Index(EmailViewModel m)
        {
            if (ModelState.IsValid)
            {

                //its valid, update your database or do soemthing useful here
                return RedirectToAction("Success");
            }
            //its not valid reload the page and let data annotations show the error
            return View(m);
        }
    }
}
