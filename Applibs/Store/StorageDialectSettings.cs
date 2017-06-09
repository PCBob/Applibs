
namespace Applibs.Store
{
    public abstract class StorageDialectSettings
    {
        protected StorageDialectSettings()
        {
        }

        public abstract string Name { get; }

        public abstract string ParameterPrefix { get; }

        public virtual string LeadingEscape => string.Empty;

        public virtual string TailingEscape => string.Empty;

        public virtual string AndStatement => "AND";

        public virtual string OrStatement => "OR";

        public virtual string AscendigStatement => "ASC";

        public virtual string DescendingStatement => "DESC";

        public virtual string NotStatement => "NOT";

        public virtual string LeftJoinStatement => "LEFT JOIN";

        public virtual string RightJoinStatement => "RIGHT JOIN";

        public virtual string InnerJoinStatement => "INNER JOIN";

        public virtual string FullJoinStatement => "FUll JOIN";

        public virtual string EqualOperator => "=";

        public virtual string NotEqualOperator => "<>";

        public virtual string IsOperator => "IS";

        public virtual string IsNotOperator => "IS NOT";

        public virtual string GreaterThanOperator => ">";

        public virtual string GreaterThanOrEqualOperator => ">=";

        public virtual string LessThanOperator => "<";

        public virtual string LessThanOrEqualOperator => "<=";
    }
}