
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#if NetCore
using System.Reflection;
#endif

using System.Text;

namespace Applibs
{
    public static class AppUtility
    {
        private static readonly Lazy<Type[]> SimpleTypesInternal = new Lazy<Type[]>(() => new Type[] {
            typeof(byte[]),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(char),
            typeof(string),
            typeof(sbyte),
            typeof(byte),
            typeof(bool),
            typeof(Guid),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(ushort?),
            typeof(uint?),
            typeof(ulong?),
            typeof(short?),
            typeof(int?),
            typeof(long?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?),
            typeof(char?),
            typeof(sbyte?),
            typeof(byte?),
            typeof(bool?),
            typeof(Guid?),
            typeof(DateTime?),
            typeof(DateTimeOffset?),
            typeof(TimeSpan?),
        });

        private static Lazy<IDictionary<Type, object>> _defaultValueMaps = new Lazy<IDictionary<Type, object>>(() => new Dictionary<Type, object>()
        {
            [typeof(byte[])] = new byte[] { },
            [typeof(ushort)] = default(ushort),
        });

        private static readonly Lazy<Type[]> NumericTypesInternal = new Lazy<Type[]>(() => new Type[] {
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(ushort?),
            typeof(uint?),
            typeof(ulong?),
            typeof(short?),
            typeof(int?),
            typeof(long?),
            typeof(float?),
            typeof(double?),
            typeof(decimal?),
        });

        private static readonly Lazy<IDictionary<Type, DbType>> DbTypesInternal = new Lazy<IDictionary<Type, DbType>>(() => new Dictionary<Type, DbType>()
        {
            [typeof(byte[])] = DbType.Binary,
            [typeof(ushort)] = DbType.UInt16,
            [typeof(uint)] = DbType.UInt32,
            [typeof(ulong)] = DbType.UInt64,
            [typeof(short)] = DbType.Int16,
            [typeof(int)] = DbType.Int32,
            [typeof(long)] = DbType.Int64,
            [typeof(float)] = DbType.Single,
            [typeof(double)] = DbType.Double,
            [typeof(decimal)] = DbType.Decimal,
            [typeof(char)] = DbType.AnsiStringFixedLength,
            [typeof(string)] = DbType.String,
            [typeof(sbyte)] = DbType.SByte,
            [typeof(byte)] = DbType.Byte,
            [typeof(bool)] = DbType.Boolean,
            [typeof(Guid)] = DbType.Guid,
            [typeof(DateTime)] = DbType.DateTime,
            [typeof(DateTimeOffset)] = DbType.DateTime,
            [typeof(TimeSpan)] = DbType.Time,
            [typeof(ushort?)] = DbType.UInt16,
            [typeof(uint?)] = DbType.UInt32,
            [typeof(ulong?)] = DbType.UInt64,
            [typeof(short?)] = DbType.Int16,
            [typeof(int?)] = DbType.Int32,
            [typeof(long?)] = DbType.Int64,
            [typeof(float?)] = DbType.Single,
            [typeof(double?)] = DbType.Double,
            [typeof(decimal?)] = DbType.Decimal,
            [typeof(char?)] = DbType.AnsiStringFixedLength,
            [typeof(sbyte?)] = DbType.SByte,
            [typeof(byte?)] = DbType.Byte,
            [typeof(bool?)] = DbType.Boolean,
            [typeof(Guid?)] = DbType.Guid,
            [typeof(DateTime?)] = DbType.DateTime,
            [typeof(DateTimeOffset?)] = DbType.DateTime,
            [typeof(TimeSpan?)] = DbType.Time,
            [typeof(object)] = DbType.Object,
        });

        public static bool IsSimpleType(this Type src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }
#if NetCore
            return src.GetTypeInfo().IsEnum && SimpleTypesInternal.Value.Contains(src) &&
                   (src.GetTypeInfo().IsGenericType &&
                    src.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>) && src.GetTypeInfo()
                        .GenericTypeArguments.Single().GetTypeInfo().IsEnum);
#else
            return src.IsEnum || SimpleTypesInternal.Value.Contains(src) || (src.IsGenericType && src.GetGenericTypeDefinition() == typeof(Nullable<>) && src.GetGenericArguments().Single().IsEnum);
#endif
        }

        public static bool IsNumericType(this Type src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            return NumericTypesInternal.Value.Contains(src);
        }

        public static DbType GetDbType(this Type src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            if (DbTypesInternal.Value.ContainsKey(src))
            {
                return DbTypesInternal.Value[src];
            }

            return DbType.Object;
        }

        public static string GetUniqueStringValue(int length, string prefix = "P_")
        {
            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                resultBuilder.Append(Guid.NewGuid().ToString("N").ToUpper());
            }
            string candidates = resultBuilder.ToString();
            resultBuilder.Clear();
            resultBuilder.Append(prefix);

            var rnd = new Random(DateTime.UtcNow.Millisecond);
            for (int i = 0; i < length; i++)
            {
                var pos = rnd.Next(candidates.Length);
                resultBuilder.Append(candidates[pos]);
            }
            string result = resultBuilder.ToString();
            resultBuilder.Clear();

            return result;
        }

        public static string GetCharString(string @char, int count)
        {
            StringBuilder resultBuilder = new StringBuilder().Insert(0, @char, count);
            string result = resultBuilder.ToString();
            resultBuilder.Clear();

            return result;
        }
    }
}
