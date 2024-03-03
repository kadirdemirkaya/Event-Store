using System.ComponentModel.DataAnnotations;

namespace EventSourching.Domain.Aggregates.TriggeredEventAggretages
{
    public class TriggeredEvent
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TriggerId { get; set; }
    }
}
