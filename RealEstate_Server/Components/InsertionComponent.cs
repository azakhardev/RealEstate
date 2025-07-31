using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Components
{
    public class InsertionComponent : ViewComponent
    {
        MyContext context = new MyContext();
        public IViewComponentResult Invoke(InsertionModel insertion, string action, bool all)
        {
            ViewBag.All = all;
            ViewBag.AllProperties = this.context.Properties.ToList();
            ViewBag.Action = action;
            return View(insertion);
        }
    }
}
