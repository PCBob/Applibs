
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Dapper;

using Applibs.Where;

namespace Applibs.Store.SqlServers
{
    public abstract class SqlServerEntityStore<TKey, TEntity> : DefaultEntityStore<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        protected SqlServerEntityStore(IStoreMapping storeMapping, StorageDialectSettings dialectSettings)
            : base(storeMapping, dialectSettings)
        {
        }

        public override IPageResult<TKey, TEntity> SelectPage(IDbConnection conn, int pageNumber, int pageSize, WhereClauseResult whereClause = null,
            string sqlOrderbyClause = null, IDbTransaction tr = null) => this.SelectPage(conn, pageNumber, pageSize, null, whereClause, sqlOrderbyClause, tr);

        public override IPageResult<TKey, TEntity> SelectPage(IDbConnection conn, int pageNumber, int pageSize, IEnumerable<Expression<Func<TEntity, object>>> selectFields,
            WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var result = this.ConstructMsSqlSelectPage(pageNumber, pageSize, selectFields, whereClause, sqlOrderbyClause);

            try
            {
                SqlMapper.GridReader reader = conn.QueryMultiple(result.sql, result.dynParms, tr);

                int totalNumberOfRecords = reader.ReadSingleOrDefault<int>();
                IEnumerable<TEntity> entities = reader.Read<TEntity>();

                return new PageResult<TKey, TEntity>(pageNumber, pageSize, totalNumberOfRecords).AddItems(entities);
            }
            catch (Exception ex)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                foreach (var item in result.dynParms.ParameterNames)
                {
                    parameters.Insert(item, result.dynParms.Get<object>(item));
                }

                throw new ExecuteSqlErrorException(result.sql, parameters, ex.Message, ex);
            }
        }

        public override async Task<IPageResult<TKey, TEntity>> SelectPageAsync(IDbConnection conn, int pageNumber, int pageSize, WhereClauseResult whereClause = null,
            string sqlOrderbyClause = null, IDbTransaction tr = null, CancellationToken cctoken = new CancellationToken()) => await this.SelectPageAsync(conn, pageNumber, pageSize, null, whereClause, sqlOrderbyClause, tr, cctoken);

        public override async Task<IPageResult<TKey, TEntity>> SelectPageAsync(IDbConnection conn, int pageNumber, int pageSize, IEnumerable<Expression<Func<TEntity, object>>> selectFields,
            WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null,
            CancellationToken cctoken = new CancellationToken())
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var result = this.ConstructMsSqlSelectPage(pageNumber, pageSize, selectFields, whereClause, sqlOrderbyClause);

            try
            {
                var cmd = base.BuildDapperCmd(result.sql, result.dynParms, tr, cancellationToken: cctoken);

                SqlMapper.GridReader reader = await conn.QueryMultipleAsync(cmd);

                int totalNumberOfRecords = await reader.ReadSingleOrDefaultAsync<int>();
                IEnumerable<TEntity> entities = await reader.ReadAsync<TEntity>();

                return new PageResult<TKey, TEntity>(pageNumber, pageSize, totalNumberOfRecords).AddItems(entities);
            }
            catch (Exception ex)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                foreach (var item in result.dynParms.ParameterNames)
                {
                    parameters.Insert(item, result.dynParms.Get<object>(item));
                }

                throw new ExecuteSqlErrorException(result.sql, parameters, ex.Message, ex);
            }
        }

        private (string sql, DynamicParameters dynParms) ConstructMsSqlSelectPage(int pageNumber, int pageSize, IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause, string sqlOrderbyClause)
        {
            StringBuilder builder = new StringBuilder();
            DynamicParameters dynParms = new DynamicParameters();

            var escapeTableName = base.StoreMapping.GetEscapeTableName(base.ClassMap.TableName, base.DialectSettings);

            builder.Append($"SELECT COUNT(*) AS _TotalNumberOfRecords FROM {escapeTableName}{base.Newline}");
            if (whereClause != null)
            {
                builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}WHERE ({whereClause.WhereClause}){base.Newline}");
            }
            builder.Append($";{base.Newline}");
            //
            var propertyMaps = base.ClassMap.Properties.PropertyMaps.Where(p => !p.Ignored).ToList();

            IEnumerable<PropertyInfo> propertyInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite && p.PropertyType.IsSimpleType());

            var selectFieldList = selectFields?.ToList() ?? new List<Expression<Func<TEntity, object>>>();
            if (selectFieldList.Any())
            {
                propertyMaps = propertyMaps.Where(p => selectFieldList.Any(exp => string.Equals(p.Name,
                    exp.GetMemberName(), StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            builder.Append($"SELECT TOP({pageSize}) ");
            var propertyinfoList = propertyInfos.ToList();

            var charString = AppUtility.GetCharString(" ", "SELECT".Length);
            int index = 1;
            foreach (var item in propertyMaps)
            {
                string pgn = $"PageResult.{base.StoreMapping.GetEscapeColumnName(item.ColumnName, base.DialectSettings)}";

                builder.Append(index == 1 ? $"{pgn}{base.Newline}" : $"{charString},{pgn}{base.Newline}");

                index++;
            }
            builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}FROM (");
            sqlOrderbyClause = string.IsNullOrEmpty(sqlOrderbyClause) ? base.BuildOrderbyClause() : sqlOrderbyClause;
            builder.Append($"SELECT ROW_NUMBER() OVER(ORDER BY {sqlOrderbyClause}) AS _RowNumber{base.Newline}");
            foreach (var item in propertyMaps)
            {
                string mn = $"{escapeTableName}.{base.StoreMapping.GetEscapeColumnName(item.ColumnName, base.DialectSettings)}";

                builder.Append($"{charString},{mn}{base.Newline}");
            }
            builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}FROM {escapeTableName}{base.Newline}");

            if (whereClause != null)
            {
                builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}WHERE ({whereClause.WhereClause}){base.Newline}");

                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }

            builder.Append($"{charString}) ");
            builder.Append($"AS PageResult{base.Newline}");
            builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}WHERE PageResult._RowNumber > {(pageNumber - 1) * pageSize}{base.Newline}");
            builder.Append(";");

            string sql = builder.ToString();
            builder.Clear();

            return (sql, dynParms);
        }
    }
}