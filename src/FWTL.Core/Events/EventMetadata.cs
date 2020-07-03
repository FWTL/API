using System;

namespace FWTL.Core.Events
{
    public class EventMetadata
    {
        public Guid CommandId { get; set; }

        public Guid EventId { get; set; }

        public string EventType { get; set; }
    }
}