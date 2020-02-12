using System;
using System.Collections.Generic;

namespace InsuranceDatabase
{
    public partial class Clients
    {
        public Clients()
        {
            Documents = new HashSet<Documents>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Passport { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Documents> Documents { get; set; }
    }
}
