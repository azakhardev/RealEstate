using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Components
{
    public class AccountComponent : ViewComponent
    {
        public IViewComponentResult Invoke(User user, string action)
        {
            if (user.Username != null)
                ViewBag.Id = user.Id;
            ViewBag.Action = action;
            return View(user);
        }
    }
}
