using AspNetMvcSample.Models;
using AspNetMvcSample.Services;
using Microsoft.AspNetCore.Mvc;
using Segment.Analytics;
using Segment.Serialization;

namespace AspNetMvcSample.Controllers
{
    public class PizzaController : AnalyticsController
    {
        public PizzaController(Analytics analytics) : base(analytics)
        {
        }

        public IActionResult Index()
        {
            ViewModel.pizzas = PizzaService.GetAll();
            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult OnPost(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            PizzaService.Add(pizza);
            Analytics.Track("New Pizza Added", new JsonObject
            {
                ["id"] = pizza.Id,
                ["isGlutenFree"] = pizza.IsGlutenFree,
                ["name"] = pizza.Name,
                ["price"] = (double)pizza.Price,
                ["size"] = (int)pizza.Size
            });

            return RedirectToAction("Index");
        }

        public IActionResult OnPostDelete(int id)
        {
            PizzaService.Delete(id);
            Analytics.Track("Pizza Deleted", new JsonObject
            {
                ["id"] = id
            });
            return RedirectToAction("Index");
        }
    }
}
