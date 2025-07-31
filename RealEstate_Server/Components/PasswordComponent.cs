using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Components
{
    public class PasswordComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string controller, int id)
        {
            ViewBag.Id = id;
            ViewBag.Controller = controller;
            return View(new Password());
        }
    }
}
