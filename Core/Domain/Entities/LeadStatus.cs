using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class LeadStatus
    {
        public LeadStatuses Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public virtual ICollection<Lead> Leads { get; set; }
    }
}
