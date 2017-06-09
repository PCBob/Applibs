
namespace Applibs.Store.SqlServers
{
    public class SqlServerDialectSettings : StorageDialectSettings
    {
        public SqlServerDialectSettings()
        {
        }

        public override string Name => "Microsoft SQL Server";

        public override string ParameterPrefix => "@";

        public override string LeadingEscape { get; } = "[";

        public override string TailingEscape { get; } = "]";
    }
}