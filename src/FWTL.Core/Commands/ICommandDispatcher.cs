using System;
using System.Threading.Tasks;

namespace FWTL.Core.Commands
{
    public interface ICommandDispatcher
    {
        Task<Guid> DispatchAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand;

        Task<Guid> DispatchAsync<TRequest, TCommand>(TRequest request)
            where TCommand : class, ICommand
            where TRequest : class, IRequest;

        Task<Guid> DispatchAsync<TRequest, TCommand>(TRequest request, Action<TCommand> afterMap)
            where TCommand : class, ICommand
            where TRequest : class, IRequest;
    }
}