using Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BusinessOpportunityType
    {
        public BusinessOpportunityTypes Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public virtual ICollection<Lead> Leads { get; set; }
    }
}
