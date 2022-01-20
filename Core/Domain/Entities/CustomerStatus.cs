using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class CustomerStatus
    {
        public CustomerStatuses Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public virtual ICollection<Lead> Leads { get; set; }
    }
}
