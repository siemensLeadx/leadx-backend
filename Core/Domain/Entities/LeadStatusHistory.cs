using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class LeadStatusHistory
    {
        public long Id { get; set; }
        public long LeadId { get; set; }
        public virtual Lead Lead { get; set; }
        public LeadStatuses StatusId { get; set; }
        public virtual LeadStatus Status { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedById { get; set; }
        public virtual AppUser CreatedBy { get; set; }
    }
}
