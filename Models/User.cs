using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InoLibrary.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public List<Publication> Publications { get; set; }
        public User()
        {
            Publications = new List<Publication>();
        }
    }
}
