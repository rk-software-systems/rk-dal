using System;
using System.Collections.Generic;
using System.Linq;

namespace RKSoftware.DAL.Contract
{
    /// <summary>
    /// This storage can be used to preform only READ operations
    /// </summary>
    public interface IReadonlyStorage : IDisposable
    {
        /// <summary>
        /// Get Set of stored Entries. This method is not thread safe as it is using delayed queriable execution
        /// </summary>
        /// <typeparam name="T">Entry type</typeparam>
        /// <returns>Queryable collection of entries</returns>
        IQueryable<T> Set<T>() where T : class;
    }
}
