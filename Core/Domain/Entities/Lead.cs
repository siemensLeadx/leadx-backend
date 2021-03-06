using Domain.Common;
using Domain.Enums;
using Helpers.Exceptions;
using Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Lead : Entity<long>
    {
        public Lead()
        {
            LeadNeeds = new HashSet<LeadNeed>();
            StatusHistory = new HashSet<LeadStatusHistory>();
        }
        public string LeadName { get; set; }
        public string HospitalName { get; set; }
        public string City { get; set; }
        public BusinessOpportunityTypes? BusinessOpportunityTypeId { get; set; }
        public virtual BusinessOpportunityType BusinessOpportunityType { get; set; }
        public CustomerStatuses CustomerStatusId { get; set; }
        public virtual CustomerStatus CustomerStatus { get; set; }
        public Regions? RegionId { get; set; }
        public virtual RegionArea RegionArea { get; set; }
        public Sectors? SectorId { get; set; }
        public virtual Sector Sector { get; set; }
        public DateTime CustomerDueDate { get; set; }
        public string Comment { get; set; }
        public string ContactPerson { get; set; }
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
        public LeadStatuses CurrentLeadStatusId { get; private set; }
        public virtual LeadStatus CurrentLeadStatus { get; }
        public int? RewardClassId { get; set; }
        public virtual RewardClass RewardClass { get; set; }
        public int? RewardCriteriaId { get; set; }
        public virtual RewardCriteria RewardCriteria { get; set; }
        public decimal? PromotedPrize { get; private set; }
        public decimal? OrderedPrize { get; private set; }
        public virtual ICollection<LeadNeed> LeadNeeds { get; set; }
        public virtual ICollection<LeadStatusHistory> StatusHistory { get; private set; }
        public bool NotifyUser { get; private set; }
        public string AdminNotes { get; set; }

        public void ChangeStatus(LeadStatuses newStatus, Guid userId, string notes = null)
        {
            Precondition.Requires(IsLeadRewardSet(newStatus), "Must set the reward class and criteria");
            Precondition.Requires(CanPromoteLead(newStatus), "The last status must be approved to promote lead");

            if (CurrentLeadStatusId == newStatus)
            {
                var lastStatusHistory = StatusHistory.Where(x => x.StatusId == newStatus)
                    .OrderByDescending(s => s.CreatedAt)
                    .FirstOrDefault();

                if (lastStatusHistory != null)
                    lastStatusHistory.Notes = notes;
            }
            else
            {
                CurrentLeadStatusId = newStatus;

                StatusHistory.Add(new LeadStatusHistory
                {
                    StatusId = newStatus,
                    CreatedAt = new DateTime().SystemDateTimeNow(),
                    CreatedById = userId,
                    Notes = notes
                });
            }

            NotifyUser = true;
        }

        public void SetRewardPrize(LeadStatuses status, RewardPrize prize)
        {
            if (status == LeadStatuses.Approved)
                PromotedPrize = prize.LeadOnlyPrize;
            else if (status == LeadStatuses.Ordered)
                OrderedPrize = prize.LeadWithPOPrize;
        }

        public bool IsLeadRewardSet(LeadStatuses status)
        {
            if (status == LeadStatuses.Approved || status == LeadStatuses.Ordered)
                return RewardClassId.HasValue && RewardCriteriaId.HasValue;
            else
                return true;
        }

        public bool CanPromoteLead(LeadStatuses status)
        {
            if (status == LeadStatuses.Promoted)
                return CurrentLeadStatusId == LeadStatuses.Approved ||
                       CurrentLeadStatusId == LeadStatuses.Promoted;
            else
                return true;
        }
    }
}
