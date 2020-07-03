using FWTL.Core.Commands;
using System;

namespace FWTL.Core.Services
{
    public interface IRequestToCommandMapper
    {
        TCommand Map<TRequest, TCommand>(TRequest request)
            where TCommand : class, ICommand
            where TRequest : class, IRequest;

        TCommand Map<TRequest, TCommand>(TRequest request, Action<TCommand> afterMap)
            where TCommand : class, ICommand
            where TRequest : class, IRequest;
    }
}