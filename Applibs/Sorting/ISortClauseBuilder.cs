
using System;
using System.Linq.Expressions;

namespace Applibs.Sorting
{
    public interface ISortClauseBuilder<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        ISortClause Object { get; }

        ISortClauseBuilder<TKey, TEntity> Ascending(Expression<Func<TEntity, object>> member);

        ISortClauseBuilder<TKey, TEntity> Descending(Expression<Func<TEntity, object>> member);

        ISortClauseBuilder<TKey, TEntity> Clear();

        string Build();
    }
}