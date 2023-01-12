using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Domain.Models
{
    public class CustomUsers : IdentityUser
    {
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ContactNumber { get; set; }

    }
}
