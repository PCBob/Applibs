
using System;
using System.Collections.Concurrent;
using System.Linq;

#if NetCore
using System.Reflection;
#endif

namespace Applibs.Mapping
{
    internal static class ClassMapCached
    {
        private static readonly ConcurrentDictionary<Type, IClassMap> Cached = null;

        static ClassMapCached()
        {
            Cached = new ConcurrentDictionary<Type, IClassMap>();
        }

        internal static IClassMap Fetch<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            Type t = typeof(TEntity);
            Cached.TryGetValue(t, out IClassMap value);
            if (value == null)
            {
                var en = $"{t.Name}ClassMap";
#if NetCore
                var src = t.GetTypeInfo();
                var classMapType = t.GetTypeInfo().Assembly.DefinedTypes
                    .Single(_ => string.Equals(en, _.Name, StringComparison.CurrentCultureIgnoreCase)
                        && 
                        (
                            src.BaseType != null 
                            && src.BaseType.GetTypeInfo().IsGenericType
                            && 
                            (
                            src.BaseType.GetGenericTypeDefinition() == typeof  (AutoClassMap<,>) 
                            || src.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<,>)
                            )
                        )
                        ).AsType();
#else
                var classMapType =
                    t.Assembly.DefinedTypes.Single(
                        _ => string.Equals(en, _.Name, StringComparison.CurrentCultureIgnoreCase)
                        &&
                        (
                            t.BaseType != null
                            && t.BaseType.IsGenericType
                            &&
                            (
                            t.BaseType.GetGenericTypeDefinition() == typeof(AutoClassMap<,>)
                            || t.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<,>)
                            )
                        )
                        ).AsType();
#endif
                if (classMapType == null)
                {
                    value = new AutoClassMap<TKey, TEntity>();
                }
                else
                {
                    value = (IClassMap)Activator.CreateInstance(classMapType);
                }
                Cached.TryAdd(t, value);
            }
            return value;
        }
    }
}