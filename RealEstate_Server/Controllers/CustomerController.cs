using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;

namespace RealEstate_Server.Controllers
{
    public class CustomerController : BaseController
    {
        MyContext context = new MyContext();
        public IActionResult Index()
        {
            this.HttpContext.Session.SetString("interface", "CustomerAccount");
            return RedirectToAction("Insertions", false);
        }

        public IActionResult Exit()
        {
            this.HttpContext.Session.SetString("interface", "Frontend");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View(new Customer());
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            if (this.ModelState.IsValid)
            {
                customer.Username = customer.Username.Trim();
                customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);
                this.context.Customers.Add(customer);
                this.context.SaveChanges();

                return RedirectToAction("LoginCustomer", "Login");
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult EditCustomer()
        {
            return View(this.context.Customers.Find(ViewBag.UserId));
        }

        [HttpPost]
        public IActionResult EditCustomer(Customer customer)
        {
            Customer editedCustomer = this.context.Customers.Find(ViewBag.UserId);

            editedCustomer.Name = customer.Name;
            editedCustomer.Surname = customer.Surname;
            editedCustomer.Email = customer.Email;

            if (editedCustomer.Username == customer.Username)
            {
                this.context.SaveChanges();
                return RedirectToAction("Insertions");
            }

            if (customer.Username != null)
                editedCustomer.Username = customer.Username.Trim();

            if (this.ModelState.IsValid)
            {
                if (ViewBag.UserId == customer.Id)
                    this.HttpContext.Session.SetString("login", customer.Username);
                this.context.SaveChanges();
                return RedirectToAction("Insertions");
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            this.ViewBag.Id = ViewBag.UserId;
            this.ViewBag.Controller = "Customer";
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(Password password, int id)
        {
            Customer customer = this.context.Customers.Find(id);
            if (BCrypt.Net.BCrypt.Verify(password.OldPassword, customer.Password))
            {
                if (password.NewPassword == password.RepeatedPassword)
                {
                    customer.Password = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
                    this.context.SaveChanges();
                    return RedirectToAction("EditCustomer");
                }
                this.ModelState.AddModelError("NewPassword", "Passwords are not the same");
            }
            else
            {
                this.ModelState.AddModelError("OldPassword", "Wrong password");
            }
            return ChangePassword();
        }

        [HttpGet]
        public IActionResult Insertions(int id = 1)
        {
            int customerId = ViewBag.UserId;
            List<CustomerInsertion> CI = this.context.CustomersInsertions.Where(x => x.CustomerId == customerId).ToList();
            List<Insertion> insertions = new List<Insertion>();
            foreach (CustomerInsertion item in CI)
            {
                insertions.Add(this.context.Insertions.Find(item.InsertionId));
            }
            this.ViewBag.Insertions = insertions;
            this.ViewBag.Page = id;
            return View();
        }

        [HttpGet]
        public IActionResult AddFavourite(int id)
        {
            this.context.CustomersInsertions.Add(new CustomerInsertion { CustomerId = ViewBag.UserId, InsertionId = id });
            this.context.SaveChanges();
            return RedirectToAction("Detail", "Offer", new { id = id });
        }

        [HttpGet]
        public IActionResult DeleteFavourite(int id)
        {
            int customerId = ViewBag.UserId;
            CustomerInsertion CI = this.context.CustomersInsertions.Where(x => x.CustomerId == customerId && x.InsertionId == id).FirstOrDefault();
            this.context.CustomersInsertions.Remove(CI);
            this.context.SaveChanges();
            return RedirectToAction("Detail", "Offer", new { id = id });
        }

        [HttpGet]
        public IActionResult Requests()
        {
            int customerId = ViewBag.UserId;
            List<Request> requests = this.context.Requests.Where(x => x.CustomerId == customerId).ToList();
            this.ViewBag.Requests = requests;
            this.ViewBag.Count = requests.Count;            
            return View();
        }

        [HttpGet]
        public IActionResult DeleteRequest(int id)
        {
            this.context.Requests.Remove(this.context.Requests.Find(id));
            this.context.SaveChanges();
            return RedirectToAction("Requests");
        }

        [HttpGet]
        public IActionResult Chats()
        {
            int customerId = ViewBag.UserId;
            List<Chat> chats = this.context.Chats.Where(x => x.CustomerId == customerId).ToList();
            this.ViewBag.Chats = chats;
            this.ViewBag.Count = chats.Count;
            return View();
        }

        [HttpGet]
        public IActionResult Chat(int chatId)
        {
            Chat chat = this.context.Chats.Find(chatId);
            User user = this.context.Users.Find(chat.UserId);
            this.ViewBag.Messages = this.context.Messages.Where(x => x.ChatId == chat.Id);
            this.ViewBag.Broker = $"{user.Name} {user.Surname}";
            this.ViewBag.ChatTitle = chat.Title;
            return View(new Message() { ToBroker = true }); ;
        }

        [HttpPost]
        public IActionResult Chat(Message message)
        {
            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return RedirectToAction("Chat", new { message.ChatId });
        }
    }
}
