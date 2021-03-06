﻿
using System.Reflection;

namespace Applibs.Mapping
{
    public interface IPropertyMap
    {
        string Name { get; }

        string ColumnName { get; }

        bool Ignored { get; }

        bool IsReadOnly { get; }

        bool CanModified { get; }

        PropertyInfo Property { get; }

        IPropertyMap Column(string columnName);

        IPropertyMap Ignord();

        IPropertyMap ReadOnly();

        IPropertyMap UnModified();
    }
}