namespace Domain.Entities
{
    public class RewardPrize
    {
        public int RewardClassId { get; set; }
        public virtual RewardClass RewardClass { get; set; }
        public int RewardCriteriaId { get; set; }
        public virtual RewardCriteria RewardCriteria { get; set; }
        public decimal LeadOnlyPrize { get; set; }
        public decimal LeadWithPOPrize { get; set; }
    }
}
