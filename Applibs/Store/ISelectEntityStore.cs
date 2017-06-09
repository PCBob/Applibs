
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Applibs.Where;

namespace Applibs.Store
{
    public interface ISelectEntityStorein<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        int Count(IDbConnection conn, Expression<Func<TEntity, object>> member = null, WhereClauseResult whereClause = null, IDbTransaction tr = null);

        bool Exists(IDbConnection conn, WhereClauseResult whereClause = null, IDbTransaction tr = null);

        TEntity Single(IDbConnection conn, TKey id, IDbTransaction tr = null);

        IEnumerable<TEntity> Select(IDbConnection conn, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null);

        IEnumerable<TEntity> Select(IDbConnection conn, IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null);

        IPageResult<TKey, TEntity> SelectPage(IDbConnection conn, int pageNumber, int pageSize, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null);

        IPageResult<TKey, TEntity> SelectPage(IDbConnection conn, int pageNumber, int pageSize, IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null);

        Task<int> CountAsync(IDbConnection conn, Expression<Func<TEntity, object>> member = null, WhereClauseResult whereClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        Task<bool> ExistsAsync(IDbConnection conn, WhereClauseResult whereClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        Task<TEntity> SingleAsync(IDbConnection conn, TKey id, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        Task<IEnumerable<TEntity>> SelectAsync(IDbConnection conn, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        Task<IEnumerable<TEntity>> SelectAsync(IDbConnection conn, IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        Task<IPageResult<TKey, TEntity>> SelectPageAsync(IDbConnection conn, int pageNumber, int pageSize, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        Task<IPageResult<TKey, TEntity>> SelectPageAsync(IDbConnection conn, int pageNumber, int pageSize, IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));
    }
}