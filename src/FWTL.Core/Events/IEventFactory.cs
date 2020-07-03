namespace FWTL.Core.Events
{
    public interface IEventFactory
    {
        EventComposite Make(IEvent @event);
    }
}