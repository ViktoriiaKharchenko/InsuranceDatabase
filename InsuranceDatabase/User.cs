using Microsoft.AspNetCore.Identity;
namespace LibraryMVC.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}