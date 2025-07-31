using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RealEstate_Server.Attributes
{
    public class BadUserNameAttribute : ValidationAttribute
    {
        MyContext context = new MyContext();

        public override bool IsValid(object? value)
        {
            this.ErrorMessage = "Username is already used";
            foreach (User usr in context.Users)
            {
                if (value.ToString() == usr.Username)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
