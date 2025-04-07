namespace RKSoftware.DAL.Core;

/// <summary>
/// This storage can be used to preform only READ operations
/// </summary>
public interface IReadonlyStorage : IDisposable
{
    /// <summary>
    /// Get Set of stored Entries. This method is not thread safe as it is using delayed queryable execution
    /// </summary>
    /// <typeparam name="T">Entry type</typeparam>
    /// <returns>Queryable collection of entries</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
    IQueryable<T> Set<T>() where T : class;
#pragma warning restore CA1716 // Identifiers should not match keywords
}
