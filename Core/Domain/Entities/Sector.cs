using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Sector
    {
        public Sectors Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public virtual ICollection<Lead> Leads { get; set; }
    }
}
