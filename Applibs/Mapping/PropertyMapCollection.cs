
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Applibs.Mapping
{
    public interface IPropertyMapCollection : IEnumerable<KeyValuePair<string, PropertyMap>>
    {
        IEnumerable<string> Names { get; }

        IEnumerable<PropertyMap> PropertyMaps { get; }

        PropertyMap Get(string name);
    }

    internal sealed class PropertyMapCollection : IPropertyMapCollection
    {
        private readonly IDictionary<string, PropertyMap> _body;

        internal PropertyMapCollection(Type t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            var propertyinfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType.IsSimpleType()).ToList();
            _body = new Dictionary<string, PropertyMap>(propertyinfos.Count, StringComparer.CurrentCultureIgnoreCase);
            propertyinfos.ForEach(p => _body.Add(p.Name, new PropertyMap(p)));
        }

        public IEnumerable<string> Names => _body.Keys;

        public IEnumerable<PropertyMap> PropertyMaps => _body.Values;

        public PropertyMap Get(string name)
        {
            _body.TryGetValue(name, out PropertyMap value);
            return value;
        }

        public IEnumerator<KeyValuePair<string, PropertyMap>> GetEnumerator() => _body.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _body.GetEnumerator();
    }
}