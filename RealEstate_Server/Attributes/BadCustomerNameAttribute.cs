using Microsoft.AspNetCore.Mvc;
using RealEstate_Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RealEstate_Server.Attributes
{
    public class BadCustomerNameAttribute : ValidationAttribute
    {
        MyContext context = new MyContext();

        public override bool IsValid(object? value)
        {
            this.ErrorMessage = "Username is already used";
            foreach (Customer cstmr in context.Customers)
            {
                if (value.ToString() == cstmr.Username)
                {
                    return false;
                }
            }
            return true;
        }
    }
}