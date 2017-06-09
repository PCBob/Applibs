﻿
using System;

using Applibs.Store;

namespace Applibs.Sorting
{
    public sealed class SortClause : ISortClause
    {
        private readonly IStoreMapping _storeMapping = null;
        private readonly StorageDialectSettings _dialectSettings = null;

        public SortClause(IStoreMapping storeMapping, StorageDialectSettings dialectSettings)
        {
            _storeMapping = storeMapping ?? throw new ArgumentNullException(nameof(storeMapping));
            _dialectSettings = dialectSettings ?? throw new ArgumentNullException(nameof(dialectSettings));
        }

        public ISortClauseBuilder<TKey, TEntity> Invoke<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey> => new SortClauseBuilder<TKey, TEntity>(this, _storeMapping, _dialectSettings);
    }
}