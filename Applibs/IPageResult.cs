
using System;
using System.Collections.Generic;

namespace Applibs
{
    public interface IPageResult<TKey, TEntity> : ICollection<TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        int PageNumber { get; }

        int PageSize { get; }

        int TotalNumberOfRecords { get; }

        int TotalPage { get; }

        IEnumerable<TEntity> Items { get; }

        IPageResult<TKey, TEntity> AddItems(IEnumerable<TEntity> items);
    }
}