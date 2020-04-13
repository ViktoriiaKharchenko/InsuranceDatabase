using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceDatabase
{
    public partial class Clients
    {
        public Clients()
        {
            Documents = new HashSet<Documents>();
        }
        public string FullName { get { return this.Name + " " + this.Surname; } set { } }
        public int Id { get; set; }
        [Remote(action: "NameValid", controller: "Brokers")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Remote(action: "SurnameValid", controller: "Brokers")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Прізвіще")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Remote(action: "DateValid", controller: "Clients")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата народження")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Remote(action: "PassportValid", controller: "Clients", AdditionalFields = nameof(Id))]
        [StringLength(9, ErrorMessage = "Невірний формат данних")]
        [Display(Name = "Паспорт")]
        public string Passport { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Remote(action: "PhoneValid", controller: "Clients")]
        [StringLength(10, ErrorMessage = "Невірний формат данних")]
        [Display(Name = "Телефон")]
        public string PhoneNum { get; set; }
        [EmailAddress(ErrorMessage ="Невірний формат данних")]
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        public virtual ICollection<Documents> Documents { get; set; }
    }
}
