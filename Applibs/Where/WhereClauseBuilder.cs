
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

#if NetCore
using System.Reflection;
#endif

using Applibs.Mapping;
using Applibs.Store;

namespace Applibs.Where
{
    internal class WhereClauseBuilder<TKey, TEntity> : IWhereClauseBuilder<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IStoreMapping _storeMapping = null;
        private readonly StorageDialectSettings _dialectSettings = null;
        private readonly IWhereClause _obj = null;
        private readonly StringBuilder _builder = null;
        private readonly IDictionary<string, object> _parameters = null;
        private readonly Action _writeAnd = null;
        private readonly Action _writeOr = null;
        private readonly IClassMap _classMap = null;

        internal WhereClauseBuilder(IWhereClause obj, IStoreMapping storeMapping, StorageDialectSettings dialectSettings)
        {
            _obj = obj ?? throw new ArgumentNullException(nameof(obj));
            _storeMapping = storeMapping ?? throw new ArgumentNullException(nameof(storeMapping));
            _dialectSettings = dialectSettings ?? throw new ArgumentNullException(nameof(dialectSettings));
            _builder = new StringBuilder();
            _parameters = new Dictionary<string, object>();
            _writeAnd = () => this.And();
            _writeOr = () => this.Or();

            //  this._classMap = ClassMapCached.Fetch<TKey, TEntity>();
            this._classMap = ClassMapCached<TKey, TEntity>.ClassMap;
        }

        public IWhereClause Object => this._obj;

        protected internal string AndStatement => this._dialectSettings.AndStatement;

        protected internal string OrStatement => this._dialectSettings.OrStatement;

        protected internal string NotStatement => this._dialectSettings.NotStatement;

        protected internal string EqualOperator => this._dialectSettings.EqualOperator;

        protected internal string NotEqualOperator => this._dialectSettings.NotEqualOperator;

        protected internal string IsOperator => this._dialectSettings.IsOperator;

        protected internal string IsNotOperator => this._dialectSettings.IsNotOperator;

        protected internal string GreaterThanOperator => this._dialectSettings.GreaterThanOperator;

        protected internal string GreaterThanOrEqualOperator => this._dialectSettings.GreaterThanOrEqualOperator;

        protected internal string LessThanOperator => this._dialectSettings.LessThanOperator;

        protected internal string LessThanOrEqualOperator => this._dialectSettings.LessThanOrEqualOperator;

        public IWhereClauseBuilder<TKey, TEntity> And()
        {
            this._builder.Append(" ");
            this._builder.Append($"{this.AndStatement}");
            this._builder.Append(" ");

            return this;
        }

        public IWhereClauseBuilder<TKey, TEntity> Or()
        {
            this._builder.Append(" ");
            this._builder.Append($"{this.OrStatement}");
            this._builder.Append(" ");

            return this;
        }

        public IWhereClauseBuilder<TKey, TEntity> StartEscape()
        {
            this._builder.Append("(");

            return this;
        }

        public IWhereClauseBuilder<TKey, TEntity> AndStartEscape()
        {
            this.And();
            this.StartEscape();

            return this;
        }

        public IWhereClauseBuilder<TKey, TEntity> OrStartEscape()
        {
            this.Or();
            this.StartEscape();

            return this;
        }
        public IWhereClauseBuilder<TKey, TEntity> EndEscape()
        {
            this._builder.Append(")");

            return this;
        }

        public IWhereClauseBuilder<TKey, TEntity> Like(Expression<Func<TEntity, object>> member, string value) => this.Render(SqlOperator.Like, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndLike(Expression<Func<TEntity, object>> member, string value) => this.Render(SqlOperator.Like, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrLike(Expression<Func<TEntity, object>> member, string value) => this.Render(SqlOperator.Like, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> NotLike(Expression<Func<TEntity, object>> member, string value) => this.Render(SqlOperator.NotLike, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndNotLike(Expression<Func<TEntity, object>> member, string value) => this.Render(SqlOperator.NotLike, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrNotLike(Expression<Func<TEntity, object>> member, string value) => this.Render(SqlOperator.NotLike, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> Between(Expression<Func<TEntity, object>> member, object value, object other) => this.Render(SqlOperator.Between, null, member, value, other);

        public IWhereClauseBuilder<TKey, TEntity> AndBetween(Expression<Func<TEntity, object>> member, object value, object other) => this.Render(SqlOperator.Between, this._writeAnd, member, value, other);

        public IWhereClauseBuilder<TKey, TEntity> OrBetween(Expression<Func<TEntity, object>> member, object value, object other) => this.Render(SqlOperator.Between, this._writeOr, member, value, other);

        public IWhereClauseBuilder<TKey, TEntity> NotBetween(Expression<Func<TEntity, object>> member, object value, object other) => this.Render(SqlOperator.NotBetween, null, member, value, other);

        public IWhereClauseBuilder<TKey, TEntity> AndNotBetween(Expression<Func<TEntity, object>> member, object value, object other) => this.Render(SqlOperator.NotBetween, this._writeAnd, member, value, other);

        public IWhereClauseBuilder<TKey, TEntity> OrNotBetween(Expression<Func<TEntity, object>> member, object value, object other) => this.Render(SqlOperator.NotBetween, this._writeOr, member, value, other);

        public IWhereClauseBuilder<TKey, TEntity> In<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values) => this.Render(SqlOperator.In, null, member, values, null);

        public IWhereClauseBuilder<TKey, TEntity> AndIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values) => this.Render(SqlOperator.In, this._writeAnd, member, values, null);

        public IWhereClauseBuilder<TKey, TEntity> OrIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values) => this.Render(SqlOperator.In, this._writeOr, member, values, null);

        public IWhereClauseBuilder<TKey, TEntity> NotIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values) => this.Render(SqlOperator.NotIn, null, member, values, null);

        public IWhereClauseBuilder<TKey, TEntity> AndNotIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values) => this.Render(SqlOperator.NotIn, this._writeAnd, member, values, null);

        public IWhereClauseBuilder<TKey, TEntity> OrNotIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values) => this.Render(SqlOperator.NotIn, this._writeOr, member, values, null);

        public IWhereClauseBuilder<TKey, TEntity> IsNull(Expression<Func<TEntity, object>> member) => this.Render(SqlOperator.IsNull, null, member, null, null);

        public IWhereClauseBuilder<TKey, TEntity> AndIsNull(Expression<Func<TEntity, object>> member) => this.Render(SqlOperator.IsNull, this._writeAnd, member, null, null);

        public IWhereClauseBuilder<TKey, TEntity> OrIsNull(Expression<Func<TEntity, object>> member) => this.Render(SqlOperator.IsNull, this._writeOr, member, null, null);

        public IWhereClauseBuilder<TKey, TEntity> IsNotNull(Expression<Func<TEntity, object>> member) => this.Render(SqlOperator.IsNotNull, null, member, null, null);

        public IWhereClauseBuilder<TKey, TEntity> AndIsNotNull(Expression<Func<TEntity, object>> member) => this.Render(SqlOperator.IsNotNull, this._writeAnd, member, null, null);

        public IWhereClauseBuilder<TKey, TEntity> OrIsNotNull(Expression<Func<TEntity, object>> member) => this.Render(SqlOperator.IsNotNull, this._writeOr, member, null, null);

        public IWhereClauseBuilder<TKey, TEntity> Equal(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.Equal, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> Equal<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.Equal, null, member, other);

        public IWhereClauseBuilder<TKey, TEntity> AndEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.Equal, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.Equal, this._writeAnd, member, other);

        public IWhereClauseBuilder<TKey, TEntity> OrEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.Equal, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.Equal, this._writeOr, member, other);

        public IWhereClauseBuilder<TKey, TEntity> NotEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.NotEqual, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> NotEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.NotEqual, null, member, other);

        public IWhereClauseBuilder<TKey, TEntity> AndNotEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.NotEqual, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndNotEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.NotEqual, this._writeAnd, member, other);

        public IWhereClauseBuilder<TKey, TEntity> OrNotEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.NotEqual, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrNotEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.NotEqual, this._writeOr, member, other);

        public IWhereClauseBuilder<TKey, TEntity> GreaterThan(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.GreaterThan, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> GreaterThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.GreaterThan, null, member, other);

        public IWhereClauseBuilder<TKey, TEntity> AndGreaterThan(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.GreaterThan, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndGreaterThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.GreaterThan, this._writeAnd, member, other);

        public IWhereClauseBuilder<TKey, TEntity> OrGreaterThan(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.GreaterThan, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrGreaterThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.GreaterThan, this._writeOr, member, other);

        public IWhereClauseBuilder<TKey, TEntity> GreaterThanOrEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.GreaterThanOrEqual, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> GreaterThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.GreaterThanOrEqual, null, member, other);

        public IWhereClauseBuilder<TKey, TEntity> AndGreaterThanOrEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.GreaterThanOrEqual, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndGreaterThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.GreaterThanOrEqual, this._writeAnd, member, other);

        public IWhereClauseBuilder<TKey, TEntity> OrGreaterThanOrEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.GreaterThanOrEqual, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrGreaterThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.GreaterThanOrEqual, this._writeOr, member, other);

        public IWhereClauseBuilder<TKey, TEntity> LessThan(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.LessThan, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> LessThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.LessThan, null, member, other);

        public IWhereClauseBuilder<TKey, TEntity> AndLessThan(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.LessThan, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndLessThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.LessThan, this._writeAnd, member, other);

        public IWhereClauseBuilder<TKey, TEntity> OrLessThan(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.LessThan, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrLessThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.LessThan, this._writeOr, member, other);

        public IWhereClauseBuilder<TKey, TEntity> LessThanOrEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.LessThanOrEqual, null, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> LessThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.LessThanOrEqual, null, member, other);

        public IWhereClauseBuilder<TKey, TEntity> AndLessThanOrEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.LessThanOrEqual, this._writeAnd, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> AndLessThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>(SqlOperator.LessThanOrEqual, this._writeAnd, member, other);

        public IWhereClauseBuilder<TKey, TEntity> OrLessThanOrEqual(Expression<Func<TEntity, object>> member, object value) => this.Render(SqlOperator.LessThanOrEqual, this._writeOr, member, value, null);

        public IWhereClauseBuilder<TKey, TEntity> OrLessThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
            => this.Render<TFKey, TOther>
            (SqlOperator.LessThanOrEqual, this._writeOr, member, other);

        public IWhereClauseBuilder<TKey, TEntity> Clear()
        {
            this._builder.Clear();
            this._parameters.Clear();

            return this;
        }

        public WhereClauseResult Build()
        {
            try
            {
                string whereClause = this._builder.ToString();

                return new WhereClauseResult(whereClause, this._parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Clear();
            }
        }

        private IWhereClauseBuilder<TKey, TEntity> Render(SqlOperator @operator, Action write, Expression<Func<TEntity, object>> member, object value, object other)
        {
            string GetParameterName() => $"{this._dialectSettings.ParameterPrefix}{AppUtility.GetUniqueStringValue(10)}";
            void InsertParameter(string key, object obj) => this._parameters.Insert(key, obj);

            write?.Invoke(); // ???
            //Func<string> getParameterName = () => $"{this._dialectSettings.ParameterPrefix}{AppUtility.GetUniqueStringValue(10)}";
            //Action<string, object> insertParameter = (arg1, arg2) => this._parameters.Insert(arg1, arg2);

            string pn = null;
            object pv = null;
            string spn = null;
            object spv = null;

            string mn = $"{this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings)}.{this._storeMapping.GetEscapeColumnName(_classMap.Properties.Get(member.GetMemberName()).ColumnName, this._dialectSettings)}";
            switch (@operator)
            {
                case SqlOperator.Like:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (value.GetType() != typeof(string))
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} LIKE {pn}");

                    break;
                case SqlOperator.NotLike:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (value.GetType() != typeof(string))
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.NotStatement} LIKE {pn}");

                    break;
                case SqlOperator.Between:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (other == null)
                    {
                        throw new ArgumentNullException(nameof(other));
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    if (!other.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    if (value.GetType() != other.GetType())
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    spn = GetParameterName();
                    spv = other;
                    InsertParameter(pn, pv);
                    InsertParameter(spn, spv);
                    this._builder.Append($"{mn} BETWEEN {pn} {this.AndStatement} {spn}");

                    break;
                case SqlOperator.NotBetween:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (other == null)
                    {
                        throw new ArgumentNullException(nameof(other));
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    if (!other.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    if (value.GetType() != other.GetType())
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    spn = GetParameterName();
                    spv = other;
                    InsertParameter(pn, pv);
                    InsertParameter(spn, spv);
                    this._builder.Append($"{mn} {this.NotStatement} BETWEEN {pn} {this.AndStatement} {spn}");

                    break;
                case SqlOperator.In:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (
#if NetCore
                        value.GetType().GetTypeInfo().IsGenericType
#else
                        value.GetType().IsGenericType
#endif
                        )
                    {
                        if (!typeof(IEnumerable).IsAssignableFrom(value.GetType().GetGenericTypeDefinition()))
                        {
                            throw new InvalidOperationException();
                        }
                        if (!value.GetType().GetGenericArguments().Single().IsSimpleType())
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    else if (value.GetType().IsArray)
                    {
                        if (!value.GetType().GetElementType().IsSimpleType())
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} IN {pn}");

                    break;
                case SqlOperator.NotIn:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (
#if NetCore
                        value.GetType().GetTypeInfo().IsGenericType
#else
                        value.GetType().IsGenericType
#endif
                        )
                    {
                        if (!typeof(IEnumerable).IsAssignableFrom(value.GetType().GetGenericTypeDefinition()))
                        {
                            throw new InvalidOperationException();
                        }
                        if (!value.GetType().GetGenericArguments().Single().IsSimpleType())
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    else if (value.GetType().IsArray)
                    {
                        if (!value.GetType().GetElementType().IsSimpleType())
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.NotStatement} IN {pn}");

                    break;
                case SqlOperator.IsNull:
                    this._builder.Append($"{mn} {this.IsOperator} NULL");

                    break;
                case SqlOperator.IsNotNull:
                    this._builder.Append($"{mn} {this.IsNotOperator} NULL");

                    break;
                case SqlOperator.Equal:
                    if (value == null)
                    {
                        this._builder.Append($"{mn} {this.IsOperator} NULL");

                        break;
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.EqualOperator} {pn}");

                    break;
                case SqlOperator.NotEqual:
                    if (value == null)
                    {
                        this._builder.Append($"{mn} {this.IsNotOperator} NULL");

                        break;
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }

                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.NotEqualOperator} {pn}");

                    break;
                case SqlOperator.GreaterThan:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.GreaterThanOperator} {pn}");

                    break;
                case SqlOperator.GreaterThanOrEqual:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.GreaterThanOrEqualOperator} {pn}");

                    break;
                case SqlOperator.LessThan:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.LessThanOperator} {pn}");

                    break;
                case SqlOperator.LessThanOrEqual:
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }
                    if (!value.GetType().IsSimpleType())
                    {
                        throw new InvalidOperationException();
                    }
                    pn = GetParameterName();
                    pv = value;
                    InsertParameter(pn, pv);
                    this._builder.Append($"{mn} {this.LessThanOrEqualOperator} {pn}");

                    break;
                default:
                    throw new NotSupportedException();
            }

            return this;
        }

        private IWhereClauseBuilder<TKey, TEntity> Render<TFKey, TOther>(SqlOperator @operator, Action write, Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>
        {
            write?.Invoke(); // ???

            string mn = $"{this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings)}.{this._storeMapping.GetEscapeColumnName(_classMap.Properties.Get(member.GetMemberName()).ColumnName, this._dialectSettings)}";
            var otherClassMap = ClassMapCached.Fetch<TFKey, TOther>();
            string othmn = $"{this._storeMapping.GetEscapeTableName(otherClassMap.TableName, this._dialectSettings)}.{this._storeMapping.GetEscapeColumnName(otherClassMap.Properties.Get(other.GetMemberName()).ColumnName, this._dialectSettings)}";

            switch (@operator)
            {
                case SqlOperator.Equal:
                    this._builder.Append($"{mn} {this.EqualOperator} {othmn}");
                    break;
                case SqlOperator.NotEqual:
                    this._builder.Append($"{mn} {this.NotEqualOperator} {othmn}");
                    break;
                case SqlOperator.GreaterThan:
                    this._builder.Append($"{mn} {this.GreaterThanOperator} {othmn}");
                    break;
                case SqlOperator.GreaterThanOrEqual:
                    this._builder.Append($"{mn} {this.GreaterThanOrEqualOperator} {othmn}");
                    break;
                case SqlOperator.LessThan:
                    this._builder.Append($"{mn} {this.LessThanOperator} {othmn}");
                    break;
                case SqlOperator.LessThanOrEqual:
                    this._builder.Append($"{mn} {this.LessThanOrEqualOperator} {othmn}");
                    break;
                default:
                    throw new NotSupportedException();
            }

            return this;
        }
    }
}