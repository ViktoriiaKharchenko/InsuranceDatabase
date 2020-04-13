using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceDatabase
{

    public partial class Documents
    {
       
        public int Id { get; set; }
        [Remote(action: "NumValid", controller: "Documents")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(10, ErrorMessage = "Невірний формат данних")]
        [Display(Name = "№ Договору")]
        public string Number { get; set; }
    
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Дата складання")]
        [Remote(action: "DateValid", controller: "Documents")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Сума")]
        public Decimal Sum { get; set; }
       
        
        public int TypeId { get; set; }
        public int ClientId { get; set; }
        public int BrokerId { get; set; }

       
        [Display(Name = "Брокер")]
        public virtual Brokers Broker { get; set; }
        [Display(Name = "Клієнт")]
        public virtual Clients Client { get; set; }
        [Display(Name = "Тип страхування")]
        public virtual Types Type { get; set; }
    }
}
