using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceDatabase
{
    public partial class Types
    {
        public Types()
        {
            Documents = new HashSet<Documents>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Тип")]
        [Remote(action: "TypeValid", controller: "Types", AdditionalFields = nameof(Id))]
        public string Type { get; set; }
        
        public int CategoryId { get; set; }
        [Display(Name = "Інформація")]
        public string Info { get; set; }

        [Display(Name = "Категорія")]
        public virtual Categories Category { get; set; }
        public virtual ICollection<Documents> Documents { get; set; }
    }
}
