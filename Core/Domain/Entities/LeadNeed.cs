namespace Domain.Entities
{
    public class LeadNeed
    {
        public int Id { get; set; }
        public long LeadId { get; set; }
        public virtual Lead Lead { get; set; }
        public int NeededDeviceId { get; set; }
        public virtual NeededDevice NeededDevice { get; set; }
    }
}
