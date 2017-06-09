
using System;

namespace Applibs.Where
{
    public interface IWhereClause
    {
        IWhereClauseBuilder<TKey, TEntity> Invoke<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>;
    }
}