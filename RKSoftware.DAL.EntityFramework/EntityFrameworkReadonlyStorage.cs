using Microsoft.EntityFrameworkCore;
using RKSoftware.DAL.Contract;
using System;
using System.Linq;

namespace RKSoftware.DAL.EntityFramework
{
    public class EntityFrameworkReadonlyStorage : IReadonlyStorage
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkReadonlyStorage"/> class.
        /// </summary>
        public EntityFrameworkReadonlyStorage(DbContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _dbContext = context;
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _dbContext.Set<T>().AsNoTracking();
        }
    }
}
