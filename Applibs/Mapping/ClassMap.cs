
using System;
using System.Linq.Expressions;

namespace Applibs.Mapping
{
    public abstract class ClassMap<TKey, TEntity> : IClassMap<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IPropertyMapCollection _properties;

        protected ClassMap()
        {
            SchemaName = string.Empty;
            TableName = typeof(TEntity).Name;
            _properties = new PropertyMapCollection(typeof(TEntity));
        }

        public string SchemaName { get; private set; }

        public string TableName { get; private set; }

        public IPropertyMapCollection Properties => _properties;

        protected ClassMap<TKey, TEntity> Schema(string schemaName)
        {
            SchemaName = schemaName ?? throw new ArgumentNullException(nameof(schemaName));
            return this;
        }

        protected ClassMap<TKey, TEntity> Table(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            return this;
        }

        protected PropertyMap Map(Expression<Func<TEntity, object>> column) => _properties.Get(column.GetMemberName());
    }
}