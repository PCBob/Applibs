
using System;

namespace Applibs.Mapping
{
    public interface IClassMap
    {
        string SchemaName { get; }

        string TableName { get; }

        IPropertyMapCollection Properties { get; }
    }

    public interface IClassMap<TKey, TEntity> : IClassMap
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
    }
}

