using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using RealEstate_Server.Attributes;
using RealEstate_Server.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RealEstate_Server.Controllers
{
    public class AdministratorToolsController : BaseController
    {
        MyContext context = new MyContext();

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View(new User() { });
        }

        [HttpPost]
        public IActionResult CreateAccount(User user)
        {
            if (user.Photo == null)
                user.Photo = "Default_PfP.jpg";

            if (this.ModelState.IsValid)
            {
                user.Username = user.Username.Trim();
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                this.context.Users.Add(user);
                this.context.SaveChanges();
                
                return RedirectToAction("Users", "Admin");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult EditAccount(int id = 0)
        {
            User user = new User();
            if (id == 0)
                user = this.context.Users.Find(ViewBag.UserId);
            else
                user = this.context.Users.Find(id);

            this.ViewBag.Photo = user.Photo;
            return View(user);
        }

        [HttpPost]
        public IActionResult EditAccount(User user, int id)
        {
            User editedUser = this.context.Users.Find(id);

            editedUser.Name = user.Name;
            editedUser.Surname = user.Surname;
            editedUser.Email = user.Email;
            editedUser.Phone = user.Phone;
            if (user.Role != null)
                editedUser.Role = user.Role;
            if (user.Photo != null)
                editedUser.Photo = user.Photo;

            if (editedUser.Username == user.Username)
            {
                this.context.SaveChanges();
                return RedirectToAction("Users", "Admin");
            }

            if (user.Username != null)
                editedUser.Username = user.Username.Trim();

            if (this.ModelState.IsValid)
            {
                if (ViewBag.UserId == user.Id)
                    this.HttpContext.Session.SetString("login", user.Username);
                this.context.SaveChanges();
                return RedirectToAction("Users", "Admin");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult ChangePassword(int id)
        {
            this.ViewBag.Id = id;
            this.ViewBag.Controller = "AdministratorTools";
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(Password password, int id)
        {
            User user = this.context.Users.Find(id);
            if (BCrypt.Net.BCrypt.Verify(password.OldPassword, user.Password))
            {
                if (password.NewPassword == password.RepeatedPassword)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
                    this.context.SaveChanges();
                    return RedirectToAction("EditAccount");
                }

                this.ModelState.AddModelError("NewPassword", "Passwords are not the same");
            }
            else 
            {
                this.ModelState.AddModelError("OldPassword", "Wrong password");
            }

            return ChangePassword(id);
        }

        [HttpGet]
        public IActionResult DeleteAccount(int id)
        {
            this.context.Remove(this.context.Users.Find(id));
            this.context.SaveChanges();
            return RedirectToAction("Users", "Admin");
        }

        [HttpGet]
        public IActionResult CreateInsertion(int userId)
        {
            this.ViewBag.Admin = this.HttpContext.Session.GetString("role") == "Administrator";
            Insertion insertion = new Insertion() { UserId = userId };
            InsertionModel insertionModel = new InsertionModel() { Insertion = insertion };
            insertionModel.Username = this.context.Users.Find(userId).Username;

            return View(insertionModel);
        }

        [HttpPost]
        public IActionResult CreateInsertion(InsertionModel insertionModel)
        {
            insertionModel.Insertion.Id = this.context.Insertions.OrderBy(x => x.Id).Last().Id + 1;
            if (this.context.Users.Where(x => x.Username == insertionModel.Username).Any())
                insertionModel.Insertion.UserId = this.context.Users.Where(x => x.Username == insertionModel.Username).FirstOrDefault().Id;

            this.context.Insertions.Add(insertionModel.Insertion);

            foreach (Photo photo in insertionModel.Photos)
            {
                Photo p = new Photo() { Id = 0, Main = false, Path = photo.Path, InsertionId = insertionModel.Insertion.Id };
                this.context.Photos.Add(p);
            }

            foreach (PropertyModel propModel in insertionModel.Properties)
            {
                int propId = this.context.Properties.Where(x => x.PropertyName == propModel.PropertyName).FirstOrDefault().Id;
                PropertyInsertion PI = new PropertyInsertion() { Id = 0, InsertionId = insertionModel.Insertion.Id, PropertyId = propId, Value = propModel.Value };
                if (!this.context.PropertiesInsertions.Where(x => x.InsertionId == insertionModel.Insertion.Id && x.PropertyId == propId).Any())
                    this.context.PropertiesInsertions.Add(PI);
            }

            if (!Regex.IsMatch(insertionModel.Insertion.Type, @"^[1-9][0-9]*\+(kk|1)$")) 
            {
                return View(insertionModel);
            }
            this.context.SaveChanges();
            return RedirectToAction("Insertions", "Admin", false);            

        }

        [HttpGet]
        public IActionResult EditInsertion(int id, bool all)
        {
            InsertionModel insertionModel = new InsertionModel();
            insertionModel.Insertion = this.context.Insertions.Find(id);
            insertionModel.Username = this.context.Users.Find(insertionModel.Insertion.UserId).Username;

            List<PropertyInsertion> PI = this.context.PropertiesInsertions.Where(x => x.InsertionId == insertionModel.Insertion.Id).ToList();
            foreach (PropertyInsertion pi in PI)
            {
                PropertyModel p = new PropertyModel() { Id = pi.PropertyId, PropertyName = this.context.Properties.Find(pi.PropertyId).PropertyName, Value = pi.Value };
                insertionModel.Properties.Add(p);
            }

            foreach (Photo photo in this.context.Photos)
                if (photo.InsertionId == insertionModel.Insertion.Id)
                    insertionModel.Photos.Add(photo);
            this.ViewBag.All = all;
            return View(insertionModel);
        }

        [HttpPost]
        public IActionResult EditInsertion(InsertionModel insertionModel, bool all)
        {
            Insertion editedInsertion = this.context.Insertions.Find(insertionModel.Insertion.Id);
            editedInsertion.Title = insertionModel.Insertion.Title;
            editedInsertion.Price = insertionModel.Insertion.Price;
            editedInsertion.Disabled = insertionModel.Insertion.Disabled;
            editedInsertion.Location = insertionModel.Insertion.Location;
            editedInsertion.Category = insertionModel.Insertion.Category;
            editedInsertion.Type = insertionModel.Insertion.Type;
            editedInsertion.Area = insertionModel.Insertion.Area;
            editedInsertion.ShortDescription = insertionModel.Insertion.ShortDescription;
            editedInsertion.Description = insertionModel.Insertion.Description;
            if (insertionModel.Insertion.Photo != null)
                editedInsertion.Photo = insertionModel.Insertion.Photo;

            foreach (Photo photo in insertionModel.Photos)
            {
                Photo p = this.context.Photos.Find(photo.Id);
                if (p != null)
                {
                    if (photo.Path != null)
                        p.Path = photo.Path;
                }
                else
                {
                    if (photo.Path != null)
                    {
                        p = new Photo() { Id = 0, Main = false, Path = photo.Path, InsertionId = insertionModel.Insertion.Id };
                        this.context.Photos.Add(p);
                    }
                }
            }

            foreach (PropertyModel propModel in insertionModel.Properties)
            {
                int propId = this.context.Properties.Where(x => x.PropertyName == propModel.PropertyName).FirstOrDefault().Id;
                PropertyInsertion PI = this.context.PropertiesInsertions.Where(x => x.PropertyId == propId && x.InsertionId == insertionModel.Insertion.Id).FirstOrDefault();

                if (PI != null)
                {
                    if (propModel.Value != null)
                        PI.Value = propModel.Value;
                }
                else
                {
                    if (propModel.Value != null)
                    {
                        PI = new PropertyInsertion() { Id = 0, InsertionId = insertionModel.Insertion.Id, PropertyId = propId, Value = propModel.Value };
                        if (!this.context.PropertiesInsertions.Where(x => x.InsertionId == insertionModel.Insertion.Id && x.PropertyId == propId).Any())
                            this.context.PropertiesInsertions.Add(PI);
                    }
                }
            }

            if (!Regex.IsMatch(insertionModel.Insertion.Type, @"^[1-9][0-9]*\+(kk|1)$"))
            {
                return View(insertionModel);
            }
            this.context.SaveChanges();
            return RedirectToAction("Insertions", "Admin", new {all = all });
        }

        [HttpGet]
        public IActionResult DeleteInsertion(int id, bool all)
        {
            Insertion deleteInsertion = this.context.Insertions.Find(id);
            this.context.Insertions.Remove(deleteInsertion);
            this.context.SaveChanges();
            return RedirectToAction("Insertions", "Admin", new {all = all });
        }

        [HttpGet]
        public IActionResult DeletePhoto(int photoId, int insertionId) 
        {
            this.context.Photos.Remove(this.context.Photos.Find(photoId));
            this.context.SaveChanges();
            return RedirectToAction("EditInsertion", new {id = insertionId} );
        }

        [HttpGet]
        public IActionResult DeletePropertyInsertion(int propertyId, int insertionId)
        {
            this.context.PropertiesInsertions.Remove(this.context.PropertiesInsertions.Where(x => x.PropertyId == propertyId && x.InsertionId == insertionId).FirstOrDefault());
            this.context.SaveChanges();
            return RedirectToAction("EditInsertion", new { id = insertionId });
        }

        [HttpPost]
        public IActionResult StartChat(Message message, int requestId)
        {
            Models.Request request = this.context.Requests.Find(requestId);
            Insertion i = this.context.Insertions.Find(request.InsertionId);

            Chat chat = new Chat() { CustomerId = request.CustomerId, UserId = i.UserId, Title = i.Title };
            if (this.context.Chats.Any())
                chat.Id = this.context.Chats.OrderBy(x => x.Id).Last().Id + 1;
            else
                chat.Id = 1;

            message.ChatId = chat.Id;
            Message firstMessage = new Message() { ChatId = message.ChatId, Text = request.Message, ToBroker = true };

            this.context.Chats.Add(chat);
            this.context.Messages.Add(firstMessage);
            this.context.Messages.Add(message);
            this.context.Requests.Remove(request);
            this.context.SaveChanges();
            return RedirectToAction("Chat", "Admin", new { chatId = chat.Id });
        }

        [HttpPost]
        public IActionResult SendMessage(Message message)
        {
            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return RedirectToAction("Chat", "Admin", new { chatId = message.ChatId });
        }

        [HttpGet]
        public IActionResult DeleteChat(int id, bool all)
        {
            this.context.Chats.Remove(this.context.Chats.Find(id));
            this.context.SaveChanges();
            return RedirectToAction("Chats", "Admin", new { all = all });
        }

        public IActionResult DeleteRequest(int id, bool all)
        {
            this.context.Requests.Remove(this.context.Requests.Find(id));
            this.context.SaveChanges();

            return RedirectToAction("Requests", "Admin", new { all = all });
        }

        [HttpPost]
        public IActionResult SaveProperties(List<Property> properties)
        {
            foreach (Property property in properties)
            {
                Property editedProperty = this.context.Properties.Find(property.Id);
                if (editedProperty != null)
                    editedProperty.PropertyName = property.PropertyName;
                else
                    this.context.Properties.Add(property);
            }
            this.context.SaveChanges();
            return RedirectToAction("Properties", "Admin");
        }

        [HttpGet]
        public IActionResult AddProperty()
        {
            if (this.context.Properties.ToList().Last().PropertyName != "")
            {
                string propertyName = "";
                Property property = new Property() { PropertyName = propertyName };
                this.context.Properties.Add(property);
                this.context.SaveChanges();
                return RedirectToAction("Properties", "Admin");
            }
            return RedirectToAction("Properties", "Admin");
        }

        public IActionResult DeleteProperty(int id)
        {
            this.context.Properties.Remove(this.context.Properties.Find(id));
            this.context.SaveChanges();
            return RedirectToAction("Properties", "Admin");
        }
    }
}
