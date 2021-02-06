using Microsoft.EntityFrameworkCore;
using RKSoftware.DAL.Contract;
using System;
using System.Linq;

namespace RKSoftware.DAL.EntityFramework
{
    public class EntityFrameworkReadonlyStorage : IReadonlyStorage
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkReadonlyStorage"/> class.
        /// </summary>
        public EntityFrameworkReadonlyStorage(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _dbContext.Set<T>().AsNoTracking();
        }
    }
}
