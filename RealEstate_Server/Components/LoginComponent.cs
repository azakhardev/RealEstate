using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Components
{
    public class LoginComponent : ViewComponent
    {
        MyContext context = new MyContext();
        public IViewComponentResult Invoke(LoginUser login, string action)
        {
            ViewBag.Login = login;
            ViewBag.Action = action;
            return View(login);
        }
    }
}
