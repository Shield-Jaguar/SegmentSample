using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetMvcSample.Models;
using Segment.Analytics;

namespace AspNetMvcSample.Controllers
{
    public class HomeController : AnalyticsController
    {
        public HomeController(Analytics analytics) : base(analytics)
        {
        }

        public IActionResult Index()
        {
            return View(ViewModel);
        }

        public IActionResult Pizza()
        {
            return RedirectToAction("Index", "Pizza");
        }

        public IActionResult Privacy()
        {
            return View(ViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewModel.ErrorViewModel =
                new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier};
            return View(ViewModel);
        }
    }
}
