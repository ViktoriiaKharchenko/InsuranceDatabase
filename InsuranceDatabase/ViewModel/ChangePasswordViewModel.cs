using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryMVC.ViewModel
{
    public class ChangePasswordViewModel
    {
       
        public string Id { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        
        //[DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} повинно мати мінімум {2} та максимум {1} символів.", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Required]
        //[DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} повинно мати мінімум {2} та максимум {1} символів.", MinimumLength = 6)]

        public string OldPassword { get; set; }
    }
}
