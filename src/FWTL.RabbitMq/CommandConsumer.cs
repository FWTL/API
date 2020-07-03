using FluentValidation;
using FWTL.Common.Commands;
using FWTL.Core.Commands;
using FWTL.Core.Events;
using MassTransit;
using System.Threading.Tasks;

namespace FWTL.RabbitMq
{
    public class CommandConsumer<TCommand> : IConsumer<TCommand> where TCommand : class, ICommand
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ICommandHandler<TCommand> _handler;
        private readonly IEventFactory _eventFactory;

        public CommandConsumer(
            ICommandHandler<TCommand> handler,
            IEventFactory eventFactory,
            IEventDispatcher eventDispatcher)
        {
            _handler = handler;
            _eventFactory = eventFactory;
            _eventDispatcher = eventDispatcher;
        }

        public async Task Consume(ConsumeContext<TCommand> context)
        {
            try
            {
                await _handler.ExecuteAsync(context.Message);
                await context.RespondAsync(new Response(context.RequestId.Value));

                foreach (var @event in _handler.Events)
                {
                    await context.Publish(@event);
                }
            }
            catch (ValidationException ex)
            {
                await context.RespondAsync(new Response(ex));
            }
        }
    }
}