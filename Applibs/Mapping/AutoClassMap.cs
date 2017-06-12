
using System;

namespace Applibs.Mapping
{
    public class AutoClassMap<TKey, TEntity> : ClassMap<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        public AutoClassMap()
        {
            Map(_ => _.Id)
                .ReadOnly()
                .UnModified()
                ;
        }
    }
} 