
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Dapper;

using Applibs.Mapping;
using Applibs.Sorting;
using Applibs.Where;

namespace Applibs.Store
{
    public abstract class DefaultEntityStore<TKey, TEntity> : IEntityStore<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IStoreMapping _storeMapping = null;
        private readonly StorageDialectSettings _dialectSettings = null;
        private readonly IClassMap _classMap = null;

        protected DefaultEntityStore(IStoreMapping storeMapping, StorageDialectSettings dialectSettings)
        {
            _storeMapping = storeMapping ?? throw new ArgumentNullException(nameof(storeMapping));
            _dialectSettings = dialectSettings ?? throw new ArgumentNullException(nameof(dialectSettings));
            _classMap = ClassMapCached.Fetch<TKey, TEntity>();
        }

        protected IStoreMapping StoreMapping => this._storeMapping;

        protected StorageDialectSettings DialectSettings => this._dialectSettings;

        protected string Newline => Environment.NewLine;

        protected abstract Expression<Func<TEntity, object>> DefaultSortField { get; }

        protected abstract Orderby DefaultOrderby { get; }

        protected IClassMap ClassMap => this._classMap;

        public virtual int Insert(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> incrementFields = null, IDbTransaction tr = null)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = this.ConstructInsertSql(entity, incrementFields);

            try
            {
                return conn.Execute(result.sql, result.dynParms, tr);
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

        public virtual async Task<int> InsertAsync(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> incrementFields = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken))
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = this.ConstructInsertSql(entity, incrementFields);

            try
            {
                var cmd = this.BuildDapperCmd(result.sql, result.dynParms, tr, cancellationToken: cctoken);

                return await conn.ExecuteAsync(cmd);
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

        public virtual int Delete(IDbConnection conn, TKey id, IDbTransaction tr = null) => this.Delete(conn, new WhereClause(this._storeMapping, this._dialectSettings).Invoke<TKey, TEntity>().Equal(_ => _.Id, id).Build(), tr);

        public virtual int Delete(IDbConnection conn, WhereClauseResult whereClause, IDbTransaction tr = null)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var result = this.ConstructDeleteSql(whereClause);

            try
            {
                return conn.Execute(result.sql, result.dynParms, tr);
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

        public virtual async Task<int> DeleteAsync(IDbConnection conn, TKey id, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken)) => await this.DeleteAsync(conn, new WhereClause(this._storeMapping, this._dialectSettings).Invoke<TKey, TEntity>().Equal(_ => _.Id, id).Build(), tr, cctoken);

        public virtual async Task<int> DeleteAsync(IDbConnection conn, WhereClauseResult whereClause, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken))
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var result = this.ConstructDeleteSql(whereClause);

            try
            {
                var cmd = this.BuildDapperCmd(result.sql, result.dynParms, tr, cancellationToken: cctoken);

                return await conn.ExecuteAsync(cmd);
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

        public virtual int Update(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> updateFields = null, WhereClauseResult whereClause = null, IDbTransaction tr = null)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = this.ConstructUpdateSql(entity, updateFields, whereClause);

            try
            {
                return conn.Execute(result.sql, result.dynParms, tr);
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

        public virtual async Task<int> UpdateAsync(IDbConnection conn, TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> updateFields = null, WhereClauseResult whereClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken))
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = this.ConstructUpdateSql(entity, updateFields, whereClause);

            try
            {
                var cmd = this.BuildDapperCmd(result.sql, result.dynParms, tr, cancellationToken: cctoken);

                return await conn.ExecuteAsync(cmd);
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

        public virtual int Count(IDbConnection conn, Expression<Func<TEntity, object>> member = null, WhereClauseResult whereClause = null, IDbTransaction tr = null)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);
            var columnName = member == null ? "*" : $"{escapeTableName}.{this._storeMapping.GetEscapeColumnName(_classMap.Properties.Get(member.GetMemberName()).ColumnName, this._dialectSettings)}";
            var builder = new StringBuilder();
            var dynParms = new DynamicParameters();

            builder.Append($"SELECT COUNT({columnName}) FROM {escapeTableName}{this.Newline}");
            if (whereClause != null)
            {
                builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}WHERE ({whereClause.WhereClause}){this.Newline}");

                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }
            builder.Append(";");

            var sql = builder.ToString();
            builder.Clear();

            try
            {
                return conn.QuerySingle<int>(sql, dynParms, tr);
            }
            catch (Exception ex)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                foreach (var item in dynParms.ParameterNames)
                {
                    parameters.Insert(item, dynParms.Get<object>(item));
                }

                throw new ExecuteSqlErrorException(sql, parameters, ex.Message, ex);
            }
        }

        public virtual async Task<int> CountAsync(IDbConnection conn, Expression<Func<TEntity, object>> member = null, WhereClauseResult whereClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken))
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);
            var columnName = member == null ? "*" : $"{escapeTableName}.{this._storeMapping.GetEscapeColumnName(_classMap.Properties.Get(member.GetMemberName()).ColumnName, this._dialectSettings)}";
            var builder = new StringBuilder();
            var dynParms = new DynamicParameters();

            builder.Append($"SELECT COUNT({columnName}) FROM {escapeTableName}{this.Newline}");
            if (whereClause != null)
            {
                builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}WHERE ({whereClause.WhereClause}){this.Newline}");

                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }
            builder.Append(";");

            var sql = builder.ToString();
            builder.Clear();

            try
            {
                var cmd = this.BuildDapperCmd(sql, dynParms, tr, cancellationToken: cctoken);
                return await conn.QuerySingleAsync(cmd);
            }
            catch (Exception ex)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                foreach (var item in dynParms.ParameterNames)
                {
                    parameters.Insert(item, dynParms.Get<object>(item));
                }

                throw new ExecuteSqlErrorException(sql, parameters, ex.Message, ex);
            }
        }

        public virtual bool Exists(IDbConnection conn, WhereClauseResult whereClause = null, IDbTransaction tr = null)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            StringBuilder builder = new StringBuilder();
            DynamicParameters dynParms = new DynamicParameters();
            string escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);

            builder.Append("SELECT ISNULL(");
            builder.Append("(");
            builder.Append($"SELECT TOP(1) 1 FROM {escapeTableName}");
            if (whereClause != null)
            {
                builder.Append($" WHERE ({whereClause.WhereClause})");
                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }
            builder.Append(")");
            builder.Append(",0");
            builder.Append(")");
            builder.Append(";");

            string sql = builder.ToString();
            builder.Clear();

            try
            {
                return conn.QuerySingle<bool>(sql, dynParms, tr);
            }
            catch (Exception ex)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                foreach (var item in dynParms.ParameterNames)
                {
                    parameters.Insert(item, dynParms.Get<object>(item));
                }

                throw new ExecuteSqlErrorException(sql, parameters, ex.Message, ex);
            }
        }

        public virtual async Task<bool> ExistsAsync(IDbConnection conn, WhereClauseResult whereClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken))
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            StringBuilder builder = new StringBuilder();
            DynamicParameters dynParms = new DynamicParameters();
            string escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);

            builder.Append("SELECT ISNULL(");
            builder.Append("(");
            builder.Append($"SELECT TOP(1) 1 FROM {escapeTableName}");
            if (whereClause != null)
            {
                builder.Append($" WHERE ({whereClause.WhereClause})");
                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }
            builder.Append(")");
            builder.Append(",0");
            builder.Append(")");
            builder.Append(";");

            string sql = builder.ToString();
            builder.Clear();

            try
            {
                var cmd = this.BuildDapperCmd(sql, dynParms, tr, cancellationToken: cctoken);
                return await conn.QuerySingleAsync(cmd);
            }
            catch (Exception ex)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                foreach (var item in dynParms.ParameterNames)
                {
                    parameters.Insert(item, dynParms.Get<object>(item));
                }

                throw new ExecuteSqlErrorException(sql, parameters, ex.Message, ex);
            }
        }

        public virtual TEntity Single(IDbConnection conn, TKey id, IDbTransaction tr = null) => this
            .Select(conn,
                new WhereClause(this._storeMapping, this._dialectSettings).Invoke<TKey, TEntity>().Equal(_ => _.Id, id)
                    .Build(), null, tr).SingleOrDefault();

        public virtual async Task<TEntity> SingleAsync(IDbConnection conn, TKey id, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken)) => (await this
            .SelectAsync(conn,
                new WhereClause(this._storeMapping, this._dialectSettings).Invoke<TKey, TEntity>().Equal(_ => _.Id, id)
                    .Build(), null, tr, cctoken)).SingleOrDefault();

        public virtual IEnumerable<TEntity> Select(IDbConnection conn, WhereClauseResult whereClause = null,
            string sqlOrderbyClause = null, IDbTransaction tr = null) => this.Select(conn, null, whereClause,
            sqlOrderbyClause, tr);

        public virtual async Task<IEnumerable<TEntity>> SelectAsync(IDbConnection conn, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken)) => await this.SelectAsync(conn, null, whereClause,
            sqlOrderbyClause, tr, cctoken);

        public virtual IEnumerable<TEntity> Select(IDbConnection conn, IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null)
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var result = this.ConstructSelectSql(selectFields, whereClause, sqlOrderbyClause);

            try
            {
                return conn.Query<TEntity>(result.sql, result.dynParms, tr);
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

        public virtual async Task<IEnumerable<TEntity>> SelectAsync(IDbConnection conn, IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null, CancellationToken cctoken = default(CancellationToken))
        {
            if (conn == null)
            {
                throw new ArgumentNullException(nameof(conn));
            }

            var result = this.ConstructSelectSql(selectFields, whereClause, sqlOrderbyClause);

            try
            {
                var cmd = this.BuildDapperCmd(result.sql, result.dynParms, tr, cancellationToken: cctoken);

                return await conn.QueryAsync<TEntity>(cmd);
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

        public abstract IPageResult<TKey, TEntity> SelectPage(IDbConnection conn, int pageNumber, int pageSize,
            WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null);

        public abstract IPageResult<TKey, TEntity> SelectPage(IDbConnection conn, int pageNumber, int pageSize,
            IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null,
            string sqlOrderbyClause = null, IDbTransaction tr = null);

        public abstract Task<IPageResult<TKey, TEntity>> SelectPageAsync(IDbConnection conn, int pageNumber, int pageSize,
            WhereClauseResult whereClause = null, string sqlOrderbyClause = null, IDbTransaction tr = null,
            CancellationToken cctoken = default(CancellationToken));

        public abstract Task<IPageResult<TKey, TEntity>> SelectPageAsync(IDbConnection conn, int pageNumber, int pageSize,
            IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause = null,
            string sqlOrderbyClause = null, IDbTransaction tr = null,
            CancellationToken cctoken = default(CancellationToken));

        protected virtual Dapper.CommandDefinition BuildDapperCmd(string commandText, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?), CommandFlags flags = CommandFlags.Buffered, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new CommandDefinition(commandText, parameters, transaction, commandTimeout, commandType, flags, cancellationToken);
        }

        protected virtual string BuildOrderbyClause()
        {
            string mn = $"{this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings)}.{this._storeMapping.GetEscapeColumnName(_classMap.Properties.Get(DefaultSortField.GetMemberName()).ColumnName, this._dialectSettings)}";
            var order = DefaultOrderby == Orderby.Ascending
                ? this._dialectSettings.AscendigStatement
                : this._dialectSettings.DescendingStatement;

            return $"{mn} {order}";
        }

        private (string sql, DynamicParameters dynParms) ConstructInsertSql(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> incrementFields)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DynamicParameters dynParms = new DynamicParameters();
            StringBuilder builder = new StringBuilder();
            string escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);

            var propertyMaps = _classMap.Properties.PropertyMaps.Where(p => !p.Ignored && !p.IsReadOnly).ToList();

            var incrementFieldList = incrementFields?.ToList() ?? new List<Expression<Func<TEntity, object>>>();
            if (incrementFieldList.Any())
            {
                propertyMaps = propertyMaps.Where(p => !incrementFieldList.Any(exp => string.Equals(p.Name, exp.GetMemberName(), StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            builder.Append($"INSERT INTO {escapeTableName}{this.Newline}");
            string charString = AppUtility.GetCharString(" ", "INSERT INTO".Length);
            builder.Append($"{charString}(");

            var index = 1;
            var total = propertyMaps.Count;
            foreach (var item in propertyMaps)
            {
                string mn = $"{escapeTableName}.{this._storeMapping.GetEscapeColumnName(item.ColumnName, this._dialectSettings)}";
                if (index == 1)
                {
                    builder.Append($"{mn}{this.Newline}");
                }
                else if (index == total)
                {
                    builder.Append($"{charString},{mn}");
                }
                else
                {
                    builder.Append($"{charString},{mn}{this.Newline}");
                }

                index++;
            }
            builder.Append($"){this.Newline}");
            builder.Append($"{AppUtility.GetCharString(" ", "INSERT".Length)}VALUES{this.Newline}");
            builder.Append($"{charString}(");

            index = 1;
            foreach (var item in propertyMaps)
            {
                string pn = $"{this._dialectSettings.ParameterPrefix}{item.Name}";
                object pv = item.Property.GetValue(entity, null) ?? DBNull.Value;
                dynParms.Add(pn, pv);

                if (index == 1)
                {
                    builder.Append($"{pn}{this.Newline}");
                }
                else if (index == total)
                {
                    builder.Append($"{charString},{pn}");
                }
                else
                {
                    builder.Append($"{charString},{pn}{this.Newline}");
                }

                index++;
            }
            builder.Append($"){this.Newline}");
            builder.Append(";");

            string sql = builder.ToString();
            builder.Clear();

            return (sql, dynParms);
        }

        private (string sql, DynamicParameters dynParms) ConstructDeleteSql(WhereClauseResult whereClause)
        {
            DynamicParameters dynParms = new DynamicParameters();
            StringBuilder builder = new StringBuilder();
            string escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);

            builder.Append($"DELETE FROM {escapeTableName}{this.Newline}");
            if (whereClause != null)
            {
                builder.Append($"{AppUtility.GetCharString(" ", "DELETE".Length)}WHERE ({whereClause.WhereClause}){this.Newline}");
                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }
            builder.Append(";");

            string sql = builder.ToString();
            builder.Clear();

            return (sql, dynParms);
        }

        private (string sql, DynamicParameters dynParms) ConstructUpdateSql(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> updateFields, WhereClauseResult whereClause)
        {
            DynamicParameters dynParms = new DynamicParameters();
            StringBuilder builder = new StringBuilder();
            string escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);

            var propertyMaps = _classMap.Properties.PropertyMaps.Where(p => !p.Ignored && p.CanModified).ToList();

            var updateFieldList = updateFields?.ToList() ?? new List<Expression<Func<TEntity, object>>>();
            if (updateFieldList.Any())
            {
                propertyMaps = propertyMaps
                    .Where(p => updateFieldList.Any(exp => string.Equals(p.Name, exp.GetMemberName(),
                        StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            builder.Append($"UPDATE {escapeTableName}{this.Newline}");
            builder.Append($"{AppUtility.GetCharString(" ", "UPD".Length)}SET");

            var index = 1;
            var charString = AppUtility.GetCharString(" ", "UPDATE".Length);
            foreach (var item in propertyMaps)
            {
                string mn = $"{escapeTableName}.{this._storeMapping.GetEscapeColumnName(item.ColumnName, this._dialectSettings)}";
                string pn = $"{this._dialectSettings.ParameterPrefix}{item.Name}";
                object pv = item.Property.GetValue(entity, null) ?? DBNull.Value;
                dynParms.Add(pn, pv);

                builder.Append(index == 1 ? $" {mn} = {pn}{this.Newline}" : $"{charString},{mn} = {pn}{this.Newline}");

                index++;
            }
            if (whereClause != null)
            {
                builder.Append($"{AppUtility.GetCharString(" ", "U".Length)}WHERE ({whereClause.WhereClause}){this.Newline}");

                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }
            builder.Append(";");

            string sql = builder.ToString();
            builder.Clear();

            return (sql, dynParms);
        }

        private (string sql, DynamicParameters dynParms) ConstructSelectSql(IEnumerable<Expression<Func<TEntity, object>>> selectFields, WhereClauseResult whereClause, string sqlOrderbyClause)
        {
            DynamicParameters dynParms = new DynamicParameters();
            StringBuilder builder = new StringBuilder();
            string escapeTableName = this._storeMapping.GetEscapeTableName(_classMap.TableName, this._dialectSettings);

            var propertyMaps = _classMap.Properties.PropertyMaps.Where(p => !p.Ignored).ToList();

            var selectFieldList = selectFields?.ToList() ?? new List<Expression<Func<TEntity, object>>>();
            if (selectFieldList.Any())
            {
                propertyMaps = propertyMaps.Where(p => selectFieldList.Any(exp => string.Equals(p.Name,
                    exp.GetMemberName(), StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            builder.Append("SELECT ");

            int index = 1;
            string charString = AppUtility.GetCharString(" ", "SELECT".Length);
            foreach (var item in propertyMaps)
            {
                string mn = $"{escapeTableName}.{this._storeMapping.GetEscapeColumnName(item.ColumnName, this._dialectSettings)}";

                builder.Append(index == 1 ? $"{mn}{this.Newline}" : $"{charString},{mn}{this.Newline}");

                index++;
            }
            builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}FROM {escapeTableName}{this.Newline}");

            if (whereClause != null)
            {
                builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}WHERE ({whereClause.WhereClause}){this.Newline}");

                foreach (var item in whereClause.Parameter)
                {
                    dynParms.Add(item.Key, item.Value);
                }
                whereClause.Parameter.Clear();
            }
            sqlOrderbyClause = string.IsNullOrEmpty(sqlOrderbyClause) ? this.BuildOrderbyClause() : sqlOrderbyClause;

            builder.Append($"{AppUtility.GetCharString(" ", "SE".Length)}ORDER BY {sqlOrderbyClause}{this.Newline}");

            builder.Append(";");
            string sql = builder.ToString();
            builder.Clear();

            return (sql, dynParms);
        }
    }
}