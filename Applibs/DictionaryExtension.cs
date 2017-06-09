
using System;
using System.Collections.Generic;

namespace Applibs
{
    public static class DictionaryExtension
    {
        public static IDictionary<TKey, TValue> Insert<TKey, TValue>(this IDictionary<TKey, TValue> src, TKey key, TValue value)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            src[key] = value;
            return src;
        }

        public static IDictionary<TKey, TValue> Copy<TKey, TValue>(this IDictionary<TKey, TValue> src, IDictionary<TKey, TValue> source)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            foreach (var kvp in source)
            {
                src.Insert(kvp.Key, kvp.Value);
            }
            return src;
        }
    }
}