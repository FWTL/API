using System.Threading.Tasks;

namespace FWTL.Core.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(EventComposite @event);
    }
}