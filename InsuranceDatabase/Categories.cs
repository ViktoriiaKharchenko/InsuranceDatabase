using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceDatabase
{
    public partial class Categories
    {
        public Categories()
        {
            BrokersCategories = new HashSet<BrokersCategories>();
            Types = new HashSet<Types>();
        }

        public int Id { get; set; }
        [Required (ErrorMessage = "Поле не повинно бути порожнім")]
        [Remote(action: "CatValid", controller: "Categories", AdditionalFields = nameof(Id))]
        [Display(Name = "Категорія")]
        public string Category { get; set; }
     
        public virtual ICollection<BrokersCategories> BrokersCategories { get; set; }
        public virtual ICollection<Types> Types { get; set; }
    }
}
