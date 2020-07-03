using FWTL.Core.Commands;
using FWTL.Core.Queries;
using System;

namespace FWTL.Core.Services
{
    public interface IRequestToQueryMapper
    {
        TQuery Map<TRequest, TQuery>(TRequest request)
            where TQuery : class, IQuery
            where TRequest : class, IRequest;

        TQuery Map<TRequest, TQuery>(TRequest request, Action<TQuery> afterMap)
            where TQuery : class, IQuery
            where TRequest : class, IRequest;
    }
}