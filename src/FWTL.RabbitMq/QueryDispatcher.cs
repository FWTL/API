using FluentValidation;
using FWTL.Common.Extensions;
using FWTL.Core.Commands;
using FWTL.Core.Queries;
using FWTL.Core.Services;
using FWTL.Core.Validation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FWTL.RabbitMq
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IClientFactory _clientFactory;
        private readonly IServiceProvider _context;
        private readonly IRequestToQueryMapper _requestToQueryMapper;

        public QueryDispatcher(
            IServiceProvider context,
            IClientFactory clientFactory,
            IRequestToQueryMapper requestToQueryMapper)
        {
            _context = context;
            _clientFactory = clientFactory;
            _requestToQueryMapper = requestToQueryMapper;
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery command)
            where TQuery : class, IQuery
        {
            var validator = _context.GetService<IValidator<TQuery>>();
            if (validator.IsNotNull())
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var client = _clientFactory.CreateRequestClient<TQuery>(new Uri("queue:queries"), TimeSpan.FromMinutes(10));
            var response = await client.GetResponse<Common.Commands.Response<TResult>>(command);
            if (response.Message.Errors.Any())
            {
                throw new AppValidationException(response.Message.Errors);
            }

            return response.Message.Result;
        }

        public async Task<TResult> DispatchAsync<TRequest, TQuery, TResult>(TRequest request)
            where TQuery : class, IQuery
            where TRequest : class, IRequest
        {
            var query = _requestToQueryMapper.Map<TRequest, TQuery>(request);
            return await DispatchAsync<TQuery, TResult>(query);
        }

        public async Task<TResult> DispatchAsync<TRequest, TQuery, TResult>(TRequest request, Action<TQuery> afterMap)
            where TQuery : class, IQuery
            where TRequest : class, IRequest
        {
            var query = _requestToQueryMapper.Map<TRequest, TQuery>(request, afterMap);
            return await DispatchAsync<TQuery, TResult>(query);
        }
    }
}