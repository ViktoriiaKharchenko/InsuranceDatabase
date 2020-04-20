using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Models
{
    public class User : IdentityUser
    {

        
        public int Year { get; set; }
    }
}