
using System;
using System.Collections;
using System.Collections.Generic;

namespace Applibs
{
    public class PageResult<TKey, TEntity> : IPageResult<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly List<TEntity> _body = null;

        public PageResult(int pageNumber, int pageSize, int totalNumberOfRecords)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalNumberOfRecords = totalNumberOfRecords;

            this.TotalPage = Convert.ToInt32(Math.Ceiling((double)TotalNumberOfRecords / PageSize));

            this._body = new List<TEntity>();
        }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalNumberOfRecords { get; }

        public int TotalPage { get; }

        public IEnumerable<TEntity> Items => this._body;

        public int Count => this._body.Count;

        public bool IsReadOnly => false;

        public void Add(TEntity item) => this._body.Add(item);

        public IPageResult<TKey, TEntity> AddItems(IEnumerable<TEntity> items)
        {
            this._body.AddRange(items);
            return this;
        }

        public void Clear() => this._body.Clear();

        public bool Contains(TEntity item) => this._body.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex) => this._body.CopyTo(array, arrayIndex);

        public IEnumerator<TEntity> GetEnumerator() => this._body.GetEnumerator();

        public bool Remove(TEntity item) => this._body.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => this._body.GetEnumerator();
    }
}