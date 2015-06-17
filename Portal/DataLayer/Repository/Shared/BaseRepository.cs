using System;
using DataLayer.Entitties;

namespace DataLayer.Repository.Shared
{
    public abstract class BaseRepository : IDisposable
    {
        public PuEntities DbConn;

        protected BaseRepository()
        {
            DbConn = new PuEntities();
        }

        protected BaseRepository(PuEntities dbConn)
        {
            DbConn = dbConn;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposing)
                    return;

                DbConn?.Dispose();
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
            finally
            {
                DbConn = null;
            }
        }

    }
}
