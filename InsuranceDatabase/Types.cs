using System;
using System.Collections.Generic;

namespace InsuranceDatabase
{
    public partial class Types
    {
        public Types()
        {
            Documents = new HashSet<Documents>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public int CategoryId { get; set; }
        public string Info { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<Documents> Documents { get; set; }
    }
}
