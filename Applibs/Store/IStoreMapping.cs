
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Applibs.Store
{
    public interface IStoreMapping
    {
        string GetTableName<TEntity>() where TEntity : class;

        string GetTableName(Type type);

        string GetColumnName<TEntity>(Expression<Func<TEntity, object>> member) where TEntity : class;

        string GetColumnName(PropertyInfo propertyInfo);

        string GetEscapeTableName<TEntity>(StorageDialectSettings dialectSettings) where TEntity : class;

        string GetEscapeTableName(Type type, StorageDialectSettings dialectSettings);

        string GetEscapeTableName(string name, StorageDialectSettings dialectSettings);

        string GetEscapeColumnName<TEntity>(Expression<Func<TEntity, object>> member, StorageDialectSettings dialectSettings) where TEntity : class;

        string GetEscapeColumnName(PropertyInfo propertyInfo, StorageDialectSettings dialectSettings);

        string GetEscapeColumnName(string name, StorageDialectSettings dialectSettings);
    }
}
