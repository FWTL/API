using FluentValidation;
using FWTL.Common.Commands;
using FWTL.Common.Extensions;
using FWTL.Core.Commands;
using FWTL.Core.Services;
using FWTL.Core.Validation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FWTL.RabbitMq
{
    public class RequestDispatcher : ICommandDispatcher
    {
        private readonly IClientFactory _clientFactory;
        private readonly IServiceProvider _context;
        private readonly IRequestToCommandMapper _requestToCommandMapper;

        public RequestDispatcher(
            IServiceProvider context,
            IClientFactory clientFactory,
            IRequestToCommandMapper requestToCommandMapper)
        {
            _context = context;
            _clientFactory = clientFactory;
            _requestToCommandMapper = requestToCommandMapper;
        }

        public async Task<Guid> DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var validator = _context.GetService<IValidator<TCommand>>();
            if (validator.IsNotNull())
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var client =
                _clientFactory.CreateRequestClient<TCommand>(new Uri("queue:commands"), TimeSpan.FromMinutes(10));
            var response = await client.GetResponse<Response>(command);
            if (response.Message.Errors.Any())
            {
                throw new AppValidationException(response.Message.Errors);
            }

            return response.Message.Id;
        }

        public async Task<Guid> DispatchAsync<TRequest, TCommand>(TRequest request)
            where TCommand : class, ICommand
            where TRequest : class, IRequest
        {
            var command = _requestToCommandMapper.Map<TRequest, TCommand>(request);
            return await DispatchAsync(command);
        }

        public async Task<Guid> DispatchAsync<TRequest, TCommand>(TRequest request, Action<TCommand> afterMap)
            where TCommand : class, ICommand
            where TRequest : class, IRequest
        {
            var command = _requestToCommandMapper.Map(request, afterMap);
            return await DispatchAsync(command);
        }
    }
}