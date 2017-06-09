﻿
using System;
using System.Reflection;

namespace Applibs.Mapping
{
    public sealed class PropertyMap : IPropertyMap
    {
        private readonly PropertyInfo _property = null;

        internal PropertyMap(PropertyInfo property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
            Name = _property.Name;
            ColumnName = _property.Name;
            Ignored = false;
            IsReadOnly = false;
            CanModified = true;
        }

        public string Name { get; }

        public string ColumnName { get; private set; }

        public bool Ignored { get; private set; }

        public bool IsReadOnly { get; private set; }

        public bool CanModified { get; private set; }

        public PropertyInfo Property => _property;

        public PropertyMap Column(string columnName)
        {
            ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
            return this;
        }

        public PropertyMap Ignord()
        {
            Ignored = true;
            return this;
        }

        public PropertyMap ReadOnly()
        {
            IsReadOnly = true;
            return this;
        }

        public PropertyMap UnModified()
        {
            CanModified = false;
            return this;
        }
    }
}