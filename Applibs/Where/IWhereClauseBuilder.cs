
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Applibs.Where
{
    public interface IWhereClauseBuilder<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        IWhereClause Object { get; }

        IWhereClauseBuilder<TKey, TEntity> And();

        IWhereClauseBuilder<TKey, TEntity> Or();

        IWhereClauseBuilder<TKey, TEntity> StartEscape();

        IWhereClauseBuilder<TKey, TEntity> AndStartEscape();

        IWhereClauseBuilder<TKey, TEntity> OrStartEscape();

        IWhereClauseBuilder<TKey, TEntity> EndEscape();

        IWhereClauseBuilder<TKey, TEntity> Like(Expression<Func<TEntity, object>> member, string value);

        IWhereClauseBuilder<TKey, TEntity> AndLike(Expression<Func<TEntity, object>> member, string value);

        IWhereClauseBuilder<TKey, TEntity> OrLike(Expression<Func<TEntity, object>> member, string value);

        IWhereClauseBuilder<TKey, TEntity> NotLike(Expression<Func<TEntity, object>> member, string value);

        IWhereClauseBuilder<TKey, TEntity> AndNotLike(Expression<Func<TEntity, object>> member, string value);

        IWhereClauseBuilder<TKey, TEntity> OrNotLike(Expression<Func<TEntity, object>> member, string value);

        IWhereClauseBuilder<TKey, TEntity> Between(Expression<Func<TEntity, object>> member, object value, object other);

        IWhereClauseBuilder<TKey, TEntity> AndBetween(Expression<Func<TEntity, object>> member, object value, object other);

        IWhereClauseBuilder<TKey, TEntity> OrBetween(Expression<Func<TEntity, object>> member, object value, object other);

        IWhereClauseBuilder<TKey, TEntity> NotBetween(Expression<Func<TEntity, object>> member, object value, object other);

        IWhereClauseBuilder<TKey, TEntity> AndNotBetween(Expression<Func<TEntity, object>> member, object value, object other);

        IWhereClauseBuilder<TKey, TEntity> OrNotBetween(Expression<Func<TEntity, object>> member, object value, object other);

        IWhereClauseBuilder<TKey, TEntity> In<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values);

        IWhereClauseBuilder<TKey, TEntity> AndIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values);

        IWhereClauseBuilder<TKey, TEntity> OrIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values);

        IWhereClauseBuilder<TKey, TEntity> NotIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values);

        IWhereClauseBuilder<TKey, TEntity> AndNotIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values);

        IWhereClauseBuilder<TKey, TEntity> OrNotIn<TValue>(Expression<Func<TEntity, object>> member, IEnumerable<TValue> values);

        IWhereClauseBuilder<TKey, TEntity> IsNull(Expression<Func<TEntity, object>> member);

        IWhereClauseBuilder<TKey, TEntity> AndIsNull(Expression<Func<TEntity, object>> member);

        IWhereClauseBuilder<TKey, TEntity> OrIsNull(Expression<Func<TEntity, object>> member);

        IWhereClauseBuilder<TKey, TEntity> IsNotNull(Expression<Func<TEntity, object>> member);

        IWhereClauseBuilder<TKey, TEntity> AndIsNotNull(Expression<Func<TEntity, object>> member);

        IWhereClauseBuilder<TKey, TEntity> OrIsNotNull(Expression<Func<TEntity, object>> member);

        IWhereClauseBuilder<TKey, TEntity> Equal(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> Equal<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> AndEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> AndEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> OrEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> OrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> NotEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> NotEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> AndNotEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> AndNotEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> OrNotEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> OrNotEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> GreaterThan(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> GreaterThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> AndGreaterThan(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> AndGreaterThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> OrGreaterThan(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> OrGreaterThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> GreaterThanOrEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> GreaterThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> AndGreaterThanOrEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> AndGreaterThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> OrGreaterThanOrEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> OrGreaterThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> LessThan(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> LessThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> AndLessThan(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> AndLessThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> OrLessThan(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> OrLessThan<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> LessThanOrEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> LessThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> AndLessThanOrEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> AndLessThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> OrLessThanOrEqual(Expression<Func<TEntity, object>> member, object value);

        IWhereClauseBuilder<TKey, TEntity> OrLessThanOrEqual<TFKey, TOther>(Expression<Func<TEntity, object>> member, Expression<Func<TOther, object>> other)
            where TFKey : IEquatable<TFKey>
            where TOther : class, IEntity<TFKey>;

        IWhereClauseBuilder<TKey, TEntity> Clear();

        WhereClauseResult Build();
    }
}