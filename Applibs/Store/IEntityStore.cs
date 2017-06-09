
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Applibs.Where;

namespace Applibs.Store
{
    public interface IEntityStore<TKey, TEntity> : ISelectEntityStorein<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        int Insert(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> incrementFields = null, IDbTransaction tr = null);

        Task<int> InsertAsync(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> incrementFields = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        int Delete(IDbConnection conn, TKey id, IDbTransaction tr = null);

        int Delete(IDbConnection conn, WhereClauseResult whereClause, IDbTransaction tr = null);

        Task<int> DeleteAsync(IDbConnection conn, TKey id, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        Task<int> DeleteAsync(IDbConnection conn, WhereClauseResult whereClause, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));

        int Update(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> updateFields = null, WhereClauseResult whereClause = null, IDbTransaction tr = null);

        Task<int> UpdateAsync(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> updateFields = null, WhereClauseResult whereClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken));
    }
}