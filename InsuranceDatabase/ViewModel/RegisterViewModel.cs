using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryMVC.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Рік народження")]
        [Remote(action: "YearValid", controller: "Account")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} повинно мати мінімум {2} та максимум {1} символів.", MinimumLength = 6)]

        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage ="Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

    }
}