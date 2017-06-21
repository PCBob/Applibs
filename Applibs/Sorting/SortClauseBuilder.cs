
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Applibs.Mapping;
using Applibs.Store;

namespace Applibs.Sorting
{
    internal sealed class SortClauseBuilder<TKey, TEntity> : ISortClauseBuilder<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IStoreMapping _storeMapping = null;
        private readonly StorageDialectSettings _dialectSettings = null;
        private readonly ISortClause _obj = null;
        private readonly IDictionary<string, Orderby> _body = null;
        private readonly IClassMap _classMap = null;

        internal SortClauseBuilder(ISortClause obj, IStoreMapping storeMapping, StorageDialectSettings dialectSettings)
        {
            _obj = obj ?? throw new ArgumentNullException(nameof(obj));
            _storeMapping = storeMapping ?? throw new ArgumentNullException(nameof(storeMapping));
            _dialectSettings = dialectSettings ?? throw new ArgumentNullException(nameof(dialectSettings));
            _body = new Dictionary<string, Orderby>();
            //_classMap = ClassMapCached.Fetch<TKey, TEntity>();
            this._classMap = ClassMapCached<TKey, TEntity>.ClassMap;
        }

        public ISortClause Object => _obj;

        public ISortClauseBuilder<TKey, TEntity> Ascending(Expression<Func<TEntity, object>> member) => Sort(member,
            Orderby.Ascending);

        public ISortClauseBuilder<TKey, TEntity> Descending(Expression<Func<TEntity, object>> member) => Sort(member,
            Orderby.Descending);

        public ISortClauseBuilder<TKey, TEntity> Clear()
        {
            _body.Clear();
            return this;
        }

        public string Build()
        {
            if (_body == null || !_body.Any())
            {
                return string.Empty;
            }

            StringBuilder orderbyClauseBuilder = new StringBuilder();
            for (int i = 0; i < this._body.Count; i++)
            {
                var kvp = _body.ElementAt(i);
                string pos = kvp.Value == Orderby.Ascending ? _dialectSettings.AscendigStatement : _dialectSettings.DescendingStatement;
                orderbyClauseBuilder.Append($"{kvp.Key} {pos}");

                if (i < _body.Count - 1)
                {
                    orderbyClauseBuilder.Append(", ");
                }
            }
            string orderbyClause = orderbyClauseBuilder.ToString();
            orderbyClauseBuilder.Clear();
            Clear();

            return orderbyClause;
        }

        private ISortClauseBuilder<TKey, TEntity> Sort(Expression<Func<TEntity, object>> member, Orderby orderby)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            var mn = $"{_storeMapping.GetEscapeTableName(_classMap.TableName, _dialectSettings)}{_storeMapping.GetEscapeColumnName(_classMap.Properties.Get(member.GetMemberName()).ColumnName, _dialectSettings)}";
            _body.Insert(mn, orderby);

            return this;
        }
    }
}