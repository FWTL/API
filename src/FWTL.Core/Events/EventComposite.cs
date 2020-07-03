namespace FWTL.Core.Events
{
    public class EventComposite
    {
        public EventComposite(IEvent @event)
        {
            Event = @event;
            Metadata = new EventMetadata();
        }

        public EventComposite(IEvent @event, EventMetadata metadata)
        {
            Event = @event;
            Metadata = metadata;
        }

        public IEvent Event { get; set; }

        public EventMetadata Metadata { get; set; }
    }
}