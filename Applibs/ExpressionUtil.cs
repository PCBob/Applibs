
using System;
using System.Linq.Expressions;

namespace Applibs
{
    public static class ExpressionUtil
    {
        public static string GetMemberName<T>(this Expression<Func<T, object>> member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            Expression @base = member;
            while (true)
            {
                switch (@base.NodeType)
                {
                    case ExpressionType.Parameter:
                        return ((ParameterExpression)@base).Type.Name;
                    case ExpressionType.Convert:
                        @base = ((UnaryExpression)@base).Operand;
                        break;
                    case ExpressionType.MemberAccess:
                        return ((MemberExpression)@base).Member.Name;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}