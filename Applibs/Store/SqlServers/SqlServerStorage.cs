
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Applibs.Store.SqlServers
{
    public class SqlServerStorage: DefaultStorage
    {
        public SqlServerStorage(string connectionString)
            : base(connectionString)
        {
        }

        public override async Task ExecuteAsync(Func<IDbConnection, CancellationToken, Task> callback, CancellationToken cctoken = new CancellationToken())
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            SqlConnection conn = null;
            try
            {
                conn = (SqlConnection)this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync(cctoken);
                }
                await callback(conn, cctoken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public override async Task ExecuteAsync(Func<IDbConnection, IDbTransaction, CancellationToken, Task> callback, IsolationLevel iso = IsolationLevel.ReadCommitted,
            CancellationToken cctoken = new CancellationToken())
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            SqlConnection conn = null;
            SqlTransaction tr = null;
            try
            {
                conn = (SqlConnection)this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync(cctoken);
                }
                tr = conn.BeginTransaction(iso);
                await callback(conn, tr, cctoken);
                tr.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    tr.Rollback();
                }
                catch (Exception cmex)
                {
                    throw cmex;
                }

                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                tr?.Dispose();
            }
        }

        public override async Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, CancellationToken, Task<TResult>> callback, CancellationToken cctoken = new CancellationToken())
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            SqlConnection conn = null;
            try
            {
                conn = (SqlConnection)this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync(cctoken);
                }
                TResult result = await callback(conn, cctoken);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public override async Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, IDbTransaction, CancellationToken, Task<TResult>> callback, IsolationLevel iso = IsolationLevel.ReadCommitted,
            CancellationToken cctoken = new CancellationToken())
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            SqlConnection conn = null;
            SqlTransaction tr = null;
            try
            {
                conn = (SqlConnection)this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync(cctoken);
                }
                tr = conn.BeginTransaction(iso);
                TResult result = await callback(conn, tr, cctoken);
                tr.Commit();

                return result;
            }
            catch (Exception ex)
            {
                try
                {
                    tr?.Rollback();
                }
                catch (Exception cmex)
                {
                    throw cmex;
                }

                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                tr?.Dispose();
            }
        }

        protected override IDbConnection CreateDbConnection() => new SqlConnection(base.ConnectionString);
    }
}