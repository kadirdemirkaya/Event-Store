namespace EventSourching.Domain.Common
{
    public class Events
    {
        public long EventNumber { get; set; }
        public string EventType { get; set; }
        public DateTime Created { get; set; }
        public Guid EventId { get; set; }
        public string EventStreamId { get; set; }
        public object Data { get; set; }
        public string Metadata { get; set; }
    }
}
