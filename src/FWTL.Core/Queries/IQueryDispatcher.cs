using FWTL.Core.Commands;
using System;
using System.Threading.Tasks;

namespace FWTL.Core.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery command)
            where TQuery : class, IQuery;

        Task<TResult> DispatchAsync<TRequest, TQuery, TResult>(TRequest request)
            where TQuery : class, IQuery
            where TRequest : class, IRequest;

        Task<TResult> DispatchAsync<TRequest, TQuery, TResult>(TRequest request, Action<TQuery> afterMap)
            where TQuery : class, IQuery
            where TRequest : class, IRequest;
    }
}