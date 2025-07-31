using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using RealEstate_Server.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RealEstate_Server.Controllers
{
    public class LoginController : BaseController
    {
        MyContext context = new MyContext();
        [HttpGet]
        public IActionResult LoginUser()
        {
            return View(new LoginUser());
        }

        [HttpGet]
        public IActionResult LoginCustomer() 
        {
            return View(new LoginUser());
        }

        [HttpPost]
        public IActionResult LoginUser(LoginUser user)
        {
            if (CheckUser(user))
            {
                this.HttpContext.Session.SetString("userId", user.Id.ToString());
                this.ViewBag.UserId = user.Id;
                this.HttpContext.Session.SetString("login", user.Username);                
                this.HttpContext.Session.SetString("role", context.Users.Where(x=>x.Username == user.Username).FirstOrDefault().Role);
                this.HttpContext.Session.SetString("username", user.Username);
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult LoginCustomer(LoginUser user) 
        {
            if (CheckCustomer(user))
            {
                this.HttpContext.Session.SetString("userId", user.Id.ToString());
                this.ViewBag.UserId = user.Id;
                this.HttpContext.Session.SetString("login", user.Username);
                this.HttpContext.Session.SetString("role", "Customer");
                this.HttpContext.Session.SetString("username", user.Username);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            this.HttpContext.Session.Remove("userId");
            this.HttpContext.Session.Remove("login");
            this.HttpContext.Session.Remove("role");
            this.HttpContext.Session.Remove("username");
            this.ViewBag.UserId = 0;
            return RedirectToAction("LoginCustomer");
        }

        private bool CheckUser(LoginUser user)
        {
            foreach (User u in context.Users)
            {
                if (user.Username == u.Username && user.Password != null)
                    if (BCrypt.Net.BCrypt.Verify(user.Password, u.Password))
                    {
                        user.Id = u.Id;
                        return true;
                    }
            }

            return false;
        }

        private bool CheckCustomer(LoginUser user)
        {
            List<Customer> customers = this.context.Customers.ToList();
            customers.RemoveAt(0);
            foreach (Customer c in customers)
            {
                if (user.Username == c.Username && user.Password != null)
                    if (BCrypt.Net.BCrypt.Verify(user.Password, c.Password))
                    {
                        user.Id = c.Id;
                        return true;
                    }
            }
            return false;
        }
    }
}
