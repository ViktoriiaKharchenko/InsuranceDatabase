using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceDatabase
{
    public partial class Brokers
    {
        public Brokers()
        {
            BrokersCategories = new HashSet<BrokersCategories>();
            Documents = new HashSet<Documents>();
        }
        
        public string  FullName  { get { return this.Name+" "+this.Surname; } set { } }
        public int Id { get; set; }
        [Remote(action: "NameValid", controller: "Brokers")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Remote(action: "SurnameValid", controller: "Brokers")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Дата народження")]
        
        [Remote(action: "DateValid", controller: "Brokers")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Remote(action: "PhoneValid", controller: "Brokers")]
        [Display(Name = "Телефон")]
        [StringLength (10, ErrorMessage = "Телефон занадто довгий")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Remote(action: "PassportValid", controller: "Brokers", AdditionalFields = nameof(Id))]
        [Display(Name = "Номер паспорта")]
        [StringLength(9, ErrorMessage = "Телефон занадто довгий")]
        public string Passport { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [EmailAddress (ErrorMessage ="Невіриний формат данних")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Категорії")]
        public virtual ICollection<BrokersCategories> BrokersCategories { get; set; }
        public virtual ICollection<Documents> Documents { get; set; }
    }
}
