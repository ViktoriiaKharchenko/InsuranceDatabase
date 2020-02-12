using System;
using System.Collections.Generic;

namespace InsuranceDatabase
{
    public partial class Documents
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public int TypeId { get; set; }
        public int ClientId { get; set; }
        public int BrokerId { get; set; }

        public virtual Brokers Broker { get; set; }
        public virtual Clients Client { get; set; }
        public virtual Types Type { get; set; }
    }
}
