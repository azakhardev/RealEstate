using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RealEstate_Server.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            this.ViewBag.Authenticated = this.HttpContext.Session.GetString("login") != null;            
            this.ViewBag.Interface = this.HttpContext.Session.GetString("interface") ?? "Frontend";
            if (ViewBag.Authenticated)
                this.ViewBag.Role = this.HttpContext.Session.GetString("role");
            if (this.HttpContext.Session.GetString("category") == null)
                this.HttpContext.Session.SetString("category", "All");
            this.ViewBag.UserId = this.HttpContext.Session.GetString("userId") == null ? 0 : Convert.ToInt32(this.HttpContext.Session.GetString("userId"));
            this.ViewBag.Username = this.HttpContext.Session.GetString("username");
        }
    }
}
