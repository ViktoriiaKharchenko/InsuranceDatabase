using System;
using System.Collections.Generic;

namespace InsuranceDatabase
{
    public partial class BrokersCategories
    {
        public int Id { get; set; }
        public int BrokerId { get; set; }
        public int CategoryId { get; set; }

        public virtual Brokers Broker { get; set; }
        public virtual Categories Category { get; set; }
    }
}
