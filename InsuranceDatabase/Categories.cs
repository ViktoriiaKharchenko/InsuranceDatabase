using System;
using System.Collections.Generic;

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
        public string Category { get; set; }

        public virtual ICollection<BrokersCategories> BrokersCategories { get; set; }
        public virtual ICollection<Types> Types { get; set; }
    }
}
