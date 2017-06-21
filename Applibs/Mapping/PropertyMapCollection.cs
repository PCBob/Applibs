
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Applibs.Mapping
{
    public interface IPropertyMapCollection : IEnumerable<KeyValuePair<string, IPropertyMap>>
    {
        IEnumerable<string> Names { get; }

        IEnumerable<IPropertyMap> PropertyMaps { get; }

        IPropertyMap Get(string name);
    }

    internal sealed class PropertyMapCollection : IPropertyMapCollection
    {
        private readonly IDictionary<string, IPropertyMap> _body;

        internal PropertyMapCollection(Type t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            var propertyinfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType.IsSimpleType()).ToList();
            _body = new Dictionary<string, IPropertyMap>(propertyinfos.Count, StringComparer.CurrentCultureIgnoreCase);
            propertyinfos.ForEach(p => _body.Add(p.Name, new PropertyMap(p)));
        }

        public IEnumerable<string> Names => _body.Keys;

        public IEnumerable<IPropertyMap> PropertyMaps => _body.Values;

        public IPropertyMap Get(string name)
        {
            _body.TryGetValue(name, out IPropertyMap value);
            return value;
        }

        public IEnumerator<KeyValuePair<string, IPropertyMap>> GetEnumerator() => _body.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _body.GetEnumerator();
    }
}