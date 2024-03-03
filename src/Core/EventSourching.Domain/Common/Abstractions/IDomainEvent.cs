namespace EventSourching.Domain.Common.Abstractions
{
    public interface IDomainEvent
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public bool IsActive { get; set; }
    }
}
