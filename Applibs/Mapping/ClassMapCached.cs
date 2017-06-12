
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
                var classMapType = t.GetTypeInfo().Assembly.DefinedTypes
                    .SingleOrDefault(_ => string.Equals(en, _.Name, StringComparison.CurrentCultureIgnoreCase)
                        &&
                        (
                            _.BaseType != null
                            && _.BaseType.GetTypeInfo().IsGenericType
                            &&
                            (
                                _.BaseType.GetGenericTypeDefinition() == typeof(AutoClassMap<,>)
                            || _.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<,>)
                            )
                        )
                        );
#else
                var classMapType =
                    t.Assembly.DefinedTypes.SingleOrDefault(
                        _ => string.Equals(en, _.Name, StringComparison.CurrentCultureIgnoreCase)
                        &&
                        (
                            _.BaseType != null
                            && _.BaseType.IsGenericType
                            &&
                            (
                                _.BaseType.GetGenericTypeDefinition() == typeof(AutoClassMap<,>)
                            || _.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<,>)
                            )
                        )
                        );
#endif
                if (classMapType == null)
                {
                    value = new AutoClassMap<TKey, TEntity>();
                }
                else
                {
                    value = (IClassMap)Activator.CreateInstance(
#if NetCore
                        classMapType.AsType()
#else
                        classMapType
#endif
                        );
                }
                Cached.TryAdd(t, value);
            }
            return value;
        }
    }
}