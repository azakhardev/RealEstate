using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Components
{
    public class OfferComponent : ViewComponent
    {
        MyContext context = new MyContext();
        public IViewComponentResult Invoke(Insertion insertion)
        {
            this.ViewBag.Insertion = insertion;

            return View();
        }
    }
}
