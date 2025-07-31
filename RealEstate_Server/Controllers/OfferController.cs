using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms;
using RealEstate_Server.Models;
using System.Text.RegularExpressions;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace RealEstate_Server.Controllers
{
    public class OfferController : BaseController
    {
        MyContext context = new MyContext();
        public IActionResult Index(int id = 1)
        {
            Filter filter = new Filter()
            {
                StartPrice = Convert.ToInt32(this.HttpContext.Session.GetString("startPrice")),
                EndPrice = Convert.ToInt32(this.HttpContext.Session.GetString("endPrice")),
                Location = this.HttpContext.Session.GetString("location"),
                Type = this.HttpContext.Session.GetString("type"),
                StartArea = float.Parse(this.HttpContext.Session.GetString("startArea")),
                EndArea = float.Parse(this.HttpContext.Session.GetString("endArea"))
            };

            this.ViewBag.MaxPrice = this.context.Insertions.OrderBy(x => x.Price).Last().Price;
            this.ViewBag.MaxArea = this.context.Insertions.OrderBy(x => x.Area).Last().Area;

            this.ViewBag.Insertions = FilterInsertions(filter);

            this.ViewBag.Category = this.HttpContext.Session.GetString("category");
            this.ViewBag.Start = id;
            return View(filter);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {

            Insertion insertion = context.Insertions.Find(id);

            //vlastnosti inzeratu
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (PropertyInsertion item in context.PropertiesInsertions.Where(x => x.InsertionId == id).ToList())
            {
                properties[this.context.Properties.Find(item.PropertyId).PropertyName] = item.Value;
            }

            //Podrobnosti inzeratu
            this.ViewBag.Insertion = insertion;
            this.ViewBag.Photos = context.Photos.Where(x => x.InsertionId == id).ToList();
            this.ViewBag.Properties = properties;
            this.ViewBag.User = this.context.Users.Find(insertion.UserId);
            Models.Request r = new Models.Request() { InsertionId = id };

            //Oblibene
            this.ViewBag.Favourite = false;
            foreach (CustomerInsertion item in this.context.CustomersInsertions.Where(x => x.CustomerId == Convert.ToInt32(this.HttpContext.Session.GetString("userId"))))
            {
                if (item.InsertionId == id)
                    this.ViewBag.Favourite = true;
            }

            //Predvyplneni formu
            if (this.HttpContext.Session.GetString("role") == "Customer")
            {
                r.Name = this.context.Customers.Find(ViewBag.UserId).Name;
                r.Surname = this.context.Customers.Find(ViewBag.UserId).Surname;
                r.Email = this.context.Customers.Find(ViewBag.UserId).Email;
            }

            return View(r);
        }

        [HttpPost]
        public IActionResult Detail(Models.Request request)
        {
            if (this.HttpContext.Session.GetString("login") != null && this.HttpContext.Session.GetString("role") == "Customer")
                request.CustomerId = ViewBag.UserId;
            else
                request.CustomerId = 1;

            //if (this.context.Requests.Any())
            //    request.Id = context.Requests.OrderBy(x => x.Id).Last().Id + 1;
            //else
            //    request.Id = 1;

            request.Date = DateTime.Now;
            this.context.Requests.Add(request);
            this.context.SaveChanges();
            return RedirectToAction("Detail", new { request.InsertionId });
        }

        private List<Insertion> FilterInsertions(Filter filter)
        {
            List<Insertion> insertions = this.context.Insertions.Where(x => x.Disabled == false).ToList();
            insertions.Reverse();

            if (this.HttpContext.Session.GetString("category") != "All")
                insertions = insertions.Where(x => x.Category == this.HttpContext.Session.GetString("category")).ToList();
            if (filter.EndPrice != 0)
                insertions = insertions.Where(x => x.Price >= filter.StartPrice && x.Price <= filter.EndPrice).ToList();
            if (filter.Location != "")
                insertions = insertions.Where(x => x.Location.Contains(filter.Location)).ToList();

            if (filter.Type != "")
            {
                if (Regex.IsMatch(filter.Type, @"^.+\+$"))
                {
                    if (Regex.IsMatch(filter.Type, @"^.+kk\+$"))
                    {
                        insertions = insertions.Where(x => Regex.IsMatch(x.Type, @"^([5-9]|[1-9][0-9]+)\+kk$")).ToList();
                    }
                    else
                    {
                        insertions = insertions.Where(x => Regex.IsMatch(x.Type, @"^([5-9]|[1-9][0-9]+)\+1$")).ToList();
                    }
                }
                else
                {
                    insertions = insertions.Where(x => x.Type == filter.Type).ToList();
                }
            }

            if (filter.EndArea != 0)
                insertions = insertions.Where(x => x.Area >= filter.StartArea && x.Area <= filter.EndArea).ToList();

            return insertions;
        }
    }
}
