using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using RealEstate_Server.Models;

namespace RealEstate_Server.Controllers
{
    public class AdminController : BaseController
    {
        MyContext context = new MyContext();
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("interface", "Backend");
            return RedirectToAction("Insertions", false);
        }

        public IActionResult Exit()
        {
            this.HttpContext.Session.SetString("interface", "Frontend");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Insertions(bool all, int id = 1)
        {
            if(this.HttpContext.Session.GetString("role") == "Customer")
                return RedirectToAction("Index", "Home");

            List<Insertion> insertions = new List<Insertion>();
            if (all)
            {
                if (this.HttpContext.Session.GetString("role") != "Administrator")
                    return RedirectToAction("Insertions", false);

                insertions = this.context.Insertions.ToList();
            }
            else
            {
                int userId = ViewBag.UserId;
                insertions = this.context.Insertions.Where(x => x.UserId == userId).ToList();
            }

            insertions.Reverse();
            this.ViewBag.Insertions = insertions;
            this.ViewBag.Page = id;
            this.ViewBag.Count = insertions.Count;
            this.ViewBag.All = all;
            this.ViewBag.User = ViewBag.UserId;
            return View();
        }

        public IActionResult Requests(bool all)
        {
            if (this.HttpContext.Session.GetString("role") == "Customer")
                return RedirectToAction("Index", "Home");

            List<Models.Request> requests = new List<Models.Request>();
            if (all)
            {
                if (this.HttpContext.Session.GetString("role") != "Administrator")
                    return RedirectToAction("Requests", false);

                requests = this.context.Requests.ToList();
            }
            else
            {
                string userId = this.HttpContext.Session.GetString("userId");
                List<Insertion> insertions = this.context.Insertions.Where(x => x.UserId == Convert.ToInt32(userId)).ToList();
                foreach (Insertion i in insertions)
                {
                    foreach (Models.Request r in this.context.Requests)
                    {
                        if (r.InsertionId == i.Id)
                            requests.Add(r);
                    }
                }
            }
            requests.Reverse();
            this.ViewBag.Requests = requests;
            this.ViewBag.Count = requests.Count;
            this.ViewBag.All = all;
            return View();
        }

        public IActionResult Chats(bool all)
        {
            if (this.HttpContext.Session.GetString("role") == "Customer")
                return RedirectToAction("Index", "Home");

            List<Models.Chat> chats = new List<Chat>();
            if (all)
            {
                if (this.HttpContext.Session.GetString("role") != "Administrator")
                    return RedirectToAction("Chats", false);

                chats = this.context.Chats.ToList();
            }
            else
            {
                int userId = ViewBag.UserId;
                chats = this.context.Chats.Where(x => x.UserId == userId).ToList();
            }

            this.ViewBag.Count = chats.Count;
            this.ViewBag.Chats = chats;
            this.ViewBag.All = all;
            return View();
        }

        [HttpGet]
        public IActionResult Reply(int id)
        {
            if (this.HttpContext.Session.GetString("role") == "Customer")
                return RedirectToAction("Index", "Home");

            Models.Request r = this.context.Requests.Find(id);
            this.ViewBag.Request = r;
            this.ViewBag.CustomerId = r.CustomerId;
            return View(new Message() {ToBroker = false });
        }

        [HttpGet]
        public IActionResult Chat(int chatId)
        {
            if (this.HttpContext.Session.GetString("role") == "Customer")
                return RedirectToAction("Index", "Home");

            List<Message> messages = this.context.Messages.Where(x => x.ChatId == chatId).ToList();
            this.ViewBag.Messages = messages;
            int customerId = this.context.Chats.Find(chatId).CustomerId;
            Customer c = this.context.Customers.Find(customerId);
            this.ViewBag.Customer = $"{c.Name} {c.Surname}";
            this.ViewBag.ChatTitle = this.context.Chats.Find(chatId).Title;
            return View(new Message() {ToBroker = false});
        }

        [HttpGet]
        public IActionResult Users()
        {
            if (this.HttpContext.Session.GetString("role") != "Administrator")
                return RedirectToAction("Index", "Home");

            ViewBag.Users = this.context.Users.ToList().OrderBy(x => x.Username);
            return View();
        }

        [HttpGet]
        public IActionResult Properties() 
        {
            if (this.HttpContext.Session.GetString("role") != "Administrator")
                return RedirectToAction("Index", "Home");

            List<Property> properties = this.context.Properties.ToList();
            return View(properties);
        }

        public IActionResult Delete(string act, string name, int id, string old, bool all) 
        {
            this.ViewBag.A = act;
            this.ViewBag.N = name;
            this.ViewBag.I = id;
            this.ViewBag.O = old;
            this.ViewBag.All = all;
            return View();
        }
    }
}
