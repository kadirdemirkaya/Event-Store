namespace EventSourching.Domain.Common.Abstractions
{
    public interface IDomainEvent
    {
        public string Type { get; set; }

        public string Payload { get; set; }
    }
}
