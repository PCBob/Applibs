
using System;

namespace Applibs.Sorting
{
    public interface ISortClause
    {
        ISortClauseBuilder<TKey, TEntity> Invoke<TKey, TEntity>()
            where TKey : IEquatable<TKey> 
            where TEntity : class, IEntity<TKey>;
    }
}