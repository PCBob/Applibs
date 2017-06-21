
using System;
using System.Linq;
#if NetCore
using System.Reflection;
#endif

namespace Applibs.Mapping
{
    internal static class ClassMapCached<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        private static readonly Lazy<IClassMap> ClassMapInternal = new Lazy<IClassMap>(() =>
        {
            // 获取实体类型
            Type t = typeof(TEntity);
            string name = $"{t.Name}ClassMap";
#if NetCore
            var classMapType = t.GetTypeInfo().Assembly.DefinedTypes
                .SingleOrDefault(_ => string.Equals(name, _.Name, StringComparison.CurrentCultureIgnoreCase) && (_.BaseType != null && _.BaseType.GetTypeInfo().IsGenericType && (_.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<,>) || _.BaseType.GetGenericTypeDefinition() == typeof(AutoClassMap<,>))));
#else
            var classMapType = t.Assembly.DefinedTypes.SingleOrDefault(_ => string.Equals(name, _.Name, StringComparison.CurrentCultureIgnoreCase) && (_.BaseType != null && _.BaseType.IsGenericType && (_.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<,>) || _.BaseType.GetGenericTypeDefinition() == typeof(AutoClassMap<,>))));
#endif
            if (classMapType == null)
            {
                return new AutoClassMap<TKey, TEntity>();
            }

            return (IClassMap)Activator.CreateInstance(
#if NetCore
                classMapType.AsType()
#else
                classMapType
#endif
                );
        });

        static ClassMapCached()
        {
        }

        internal static IClassMap ClassMap => ClassMapInternal.Value;
    }
}