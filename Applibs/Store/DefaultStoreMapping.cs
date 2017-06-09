
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Applibs.Store
{
    public class DefaultStoreMapping : IStoreMapping
    {
        public DefaultStoreMapping()
        {
        }

        public virtual string GetTableName<TEntity>() where TEntity : class => this.GetTableName(typeof(TEntity));

        public virtual string GetTableName(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.Name;
        }

        public virtual string GetColumnName<TEntity>(Expression<Func<TEntity, object>> member) where TEntity : class
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            return member.GetMemberName();
        }

        public virtual string GetColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            return propertyInfo.Name;
        }

        public virtual string GetEscapeTableName<TEntity>(StorageDialectSettings dialectSettings) where TEntity : class => this.GetEscapeTableName(this.GetTableName<TEntity>(), dialectSettings);

        public virtual string GetEscapeTableName(Type type, StorageDialectSettings dialectSettings) => this.GetEscapeTableName(this.GetTableName(type), dialectSettings);

        public virtual string GetEscapeTableName(string name, StorageDialectSettings dialectSettings)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (dialectSettings == null)
            {
                throw new ArgumentNullException(nameof(dialectSettings));
            }

            return $"{dialectSettings.LeadingEscape}{name}{dialectSettings.TailingEscape}";
        }

        public virtual string GetEscapeColumnName<TEntity>(Expression<Func<TEntity, object>> member, StorageDialectSettings dialectSettings) where TEntity : class => this.GetEscapeColumnName(this.GetColumnName<TEntity>(member), dialectSettings);

        public virtual string GetEscapeColumnName(PropertyInfo propertyInfo, StorageDialectSettings dialectSettings) => this.GetEscapeColumnName(this.GetColumnName(propertyInfo), dialectSettings);

        public virtual string GetEscapeColumnName(string name, StorageDialectSettings dialectSettings)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (dialectSettings == null)
            {
                throw new ArgumentNullException(nameof(dialectSettings));
            }

            return $"{dialectSettings.LeadingEscape}{name}{dialectSettings.TailingEscape}";
        }
    }
}