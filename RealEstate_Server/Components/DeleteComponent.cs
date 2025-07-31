using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Components
{
    public class DeleteComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string action, string name, int id, string old, bool all = false)
        {
            this.ViewBag.Action = action;
            this.ViewBag.Name = name;
            this.ViewBag.Id = id;
            this.ViewBag.Old = old;
            this.ViewBag.All = all;
            return View();
        }
    }
}