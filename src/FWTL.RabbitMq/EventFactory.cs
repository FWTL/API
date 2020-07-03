using FWTL.Core.Events;
using FWTL.Core.Services;
using MassTransit;
using System;

namespace FWTL.RabbitMq
{
    public class EventFactory : IEventFactory
    {
        private readonly ConsumeContext _context;
        private readonly IGuidService _guid;

        public EventFactory(IGuidService guid, ConsumeContext context)
        {
            _guid = guid;
            _context = context;
        }

        public EventComposite Make(IEvent @event)
        {
            var metadata = new EventMetadata
            {
                EventId = _guid.New,
                CommandId = _context.CorrelationId ?? _context.RequestId ?? Guid.Empty,
                EventType = @event.GetType().AssemblyQualifiedName
            };
            return new EventComposite(@event, metadata);
        }
    }
}