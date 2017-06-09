
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Applibs.Store
{
    public abstract class DefaultStorage : IStorage
    {
        private readonly string _connectionString = null;
        private readonly Guid _id;

        protected DefaultStorage(string connectionString)
        {
            this._connectionString = connectionString;
            this._id = Guid.NewGuid();
        }

        protected string ConnectionString => this._connectionString;

        protected Guid Id => this._id;

        public virtual void Execute(Action<IDbConnection> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                callback(conn);
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

        public virtual void Execute(Action<IDbConnection, IDbTransaction> callback, IsolationLevel iso = IsolationLevel.ReadCommitted)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            IDbTransaction tr = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                tr = conn.BeginTransaction(iso);
                callback(conn, tr);
                tr.Commit();
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

        public virtual TResult Execute<TResult>(Func<IDbConnection, TResult> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                TResult result = callback(conn);

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

        public TResult Execute<TResult>(Func<IDbConnection, IDbTransaction, TResult> callback, IsolationLevel iso = IsolationLevel.ReadCommitted)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            IDbTransaction tr = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                tr = conn.BeginTransaction(iso);
                TResult result = callback(conn, tr);
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

        public virtual async Task ExecuteAsync(Func<IDbConnection, CancellationToken, Task> callback, CancellationToken cctoken = default(CancellationToken))
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
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

        public virtual async Task ExecuteAsync(Func<IDbConnection, IDbTransaction, CancellationToken, Task> callback, IsolationLevel iso = IsolationLevel.ReadCommitted, CancellationToken cctoken = default(CancellationToken))
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            IDbTransaction tr = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                tr = conn.BeginTransaction(iso);
                await callback(conn, tr, cctoken);
                tr.Commit();
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

        public virtual async Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, CancellationToken, Task<TResult>> callback, CancellationToken cctoken = default(CancellationToken))
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
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

        public virtual async Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, IDbTransaction, CancellationToken, Task<TResult>> callback, IsolationLevel iso = IsolationLevel.ReadCommitted, CancellationToken cctoken = default(CancellationToken))
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            IDbConnection conn = null;
            IDbTransaction tr = null;
            try
            {
                conn = this.CreateDbConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
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

        protected abstract IDbConnection CreateDbConnection();
    }
}