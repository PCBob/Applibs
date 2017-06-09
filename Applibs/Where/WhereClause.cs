
using System;

using Applibs.Store;

namespace Applibs.Where
{
    public class WhereClause : IWhereClause
    {
        private readonly IStoreMapping _storeMapping = null;
        private readonly StorageDialectSettings _dialectSettings = null;

        public WhereClause(IStoreMapping storeMapping, StorageDialectSettings dialectSettings)
        {
            _storeMapping = storeMapping ?? throw new ArgumentNullException(nameof(storeMapping));
            _dialectSettings = dialectSettings ?? throw new ArgumentNullException(nameof(dialectSettings));
        }

        public IWhereClauseBuilder<TKey, TEntity> Invoke<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey> => new WhereClauseBuilder<TKey, TEntity>(this, _storeMapping, _dialectSettings);
    }
}