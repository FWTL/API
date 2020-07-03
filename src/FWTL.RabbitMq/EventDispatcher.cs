using FWTL.Core.Events;
using FWTL.Core.Services;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace FWTL.RabbitMq
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IGuidService _guidService;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public EventDispatcher(
            IServiceProvider context,
            ISendEndpointProvider sendEndpointProvider,
            IGuidService guidService)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _guidService = guidService;
        }

        public Task DispatchAsync(EventComposite @event)
        {
            return Task.CompletedTask;
        }
    }
}