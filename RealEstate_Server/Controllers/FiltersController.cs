using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Controllers
{
    public class FiltersController : BaseController
    {
        MyContext context = new MyContext();

        [HttpPost]
        public IActionResult Filter(Filter filter)
        {
            this.SetDefaultFilters();

            if (filter.StartPrice != null)
                this.HttpContext.Session.SetString("startPrice", filter.StartPrice.ToString());
            if (filter.EndPrice != null)
                this.HttpContext.Session.SetString("endPrice", filter.EndPrice.ToString());

            if (filter.Location != null)
                this.HttpContext.Session.SetString("location", filter.Location);
            else
                this.HttpContext.Session.SetString("location", "");

            if (filter.Type != null)
                this.HttpContext.Session.SetString("type", filter.Type);
            else
                this.HttpContext.Session.SetString("type", "");

            if (filter.StartArea != null)
                this.HttpContext.Session.SetString("startArea", filter.StartArea.ToString());
            if (filter.EndArea != null)
                this.HttpContext.Session.SetString("endArea", filter.EndArea.ToString());

            return RedirectToAction("Index", "Offer");
        }

        [HttpGet]
        public IActionResult Category(string category)
        {
            SetDefaultFilters();

            this.HttpContext.Session.SetString("category", category);
            return RedirectToAction("Index", "Offer");
        }

        private void SetDefaultFilters()
        {
            if (this.HttpContext.Session.GetString("startPrice") == null)
                this.HttpContext.Session.SetString("startPrice", "0");

            int maxprice = this.context.Insertions.OrderBy(x => x.Price).Last().Price;
            if (this.HttpContext.Session.GetString("endPrice") == null || this.HttpContext.Session.GetString("endPrice") == "0")
                this.HttpContext.Session.SetString("endPrice", maxprice.ToString());

            if (this.HttpContext.Session.GetString("location") == null)
                this.HttpContext.Session.SetString("location", "");

            if (this.HttpContext.Session.GetString("type") == null)
                this.HttpContext.Session.SetString("type", "");

            if (this.HttpContext.Session.GetString("startArea") == null)
                this.HttpContext.Session.SetString("startArea", "0");

            double maxArea = context.Insertions.OrderBy(x => x.Area).Last().Area;
            if (this.HttpContext.Session.GetString("endArea") == null || this.HttpContext.Session.GetString("endArea") == "0")
                this.HttpContext.Session.SetString("endArea", maxArea.ToString());
        }
    }
}