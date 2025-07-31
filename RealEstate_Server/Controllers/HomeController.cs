using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;
using System.Diagnostics;

namespace RealEstate_Server.Controllers
{
    public class HomeController : BaseController
    {
        MyContext context = new MyContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {            
            List<Insertion> insertions = context.Insertions.Where(x => x.Disabled == false).ToList();
            insertions.Reverse();
            
            ViewBag.Offers = insertions;
            ViewBag.LatestOffers = insertions.Take(6);

            ViewBag.LandCount = insertions.Where(x =>x.Category == "Land" && !x.Disabled).Count();
            ViewBag.FlatCount = insertions.Where(x => x.Category == "Flat" && !x.Disabled).Count();
            ViewBag.HouseCount = insertions.Where(x => x.Category == "House" && !x.Disabled).Count();
            ViewBag.CommercialCount = insertions.Where(x => x.Category == "Commercial" && !x.Disabled).Count();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}