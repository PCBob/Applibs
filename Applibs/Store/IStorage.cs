
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Applibs.Store
{
    public interface IStorage
    {
        void Execute(Action<IDbConnection> callback);

        void Execute(Action<IDbConnection, IDbTransaction> callback, IsolationLevel iso = IsolationLevel.ReadCommitted);

        TResult Execute<TResult>(Func<IDbConnection, TResult> callback);

        TResult Execute<TResult>(Func<IDbConnection, IDbTransaction, TResult> callback, IsolationLevel iso = IsolationLevel.ReadCommitted);

        Task ExecuteAsync(Func<IDbConnection, CancellationToken, Task> callback, CancellationToken cctoken = default(CancellationToken));

        Task ExecuteAsync(Func<IDbConnection, IDbTransaction, CancellationToken, Task> callback, IsolationLevel iso = IsolationLevel.ReadCommitted, CancellationToken cctoken = default(CancellationToken));

        Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, CancellationToken, Task<TResult>> callback, CancellationToken cctoken = default(CancellationToken));

        Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, IDbTransaction, CancellationToken, Task<TResult>> callback, IsolationLevel iso = IsolationLevel.ReadCommitted, CancellationToken cctoken = default(CancellationToken));
    }
}