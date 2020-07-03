using FluentValidation;
using FWTL.Common.Commands;
using FWTL.Core.Queries;
using MassTransit;
using System.Threading.Tasks;

namespace FWTL.RabbitMq
{
    public class QueryConsumer<TQuery, TResult> : IConsumer<TQuery> where TQuery : class, IQuery
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;

        public QueryConsumer(IQueryHandler<TQuery, TResult> handler)
        {
            _handler = handler;
        }

        public async Task Consume(ConsumeContext<TQuery> context)
        {
            try
            {
                var result = await _handler.HandleAsync(context.Message);
                await context.RespondAsync(new Common.Commands.Response<TResult>(context.RequestId.Value, result));
            }
            catch (ValidationException ex)
            {
                await context.RespondAsync(new Response(ex));
            }
        }
    }
}