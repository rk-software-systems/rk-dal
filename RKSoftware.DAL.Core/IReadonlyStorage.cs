using System.Linq;

namespace RKSoftware.DAL.Contract
{
    /// <summary>
    /// This storage can be used to preform only READ operations
    /// </summary>
    public interface IReadonlyStorage
    {
        /// <summary>
        /// Get Set of stored Entries
        /// </summary>
        /// <typeparam name="T">Entry type</typeparam>
        /// <returns>Queryable collection of entries</returns>
        IQueryable<T> Set<T>() where T: class;
    }
}
