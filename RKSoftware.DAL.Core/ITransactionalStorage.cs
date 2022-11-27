using System.Threading.Tasks;

namespace RKSoftware.DAL.Contract
{
    /// <summary>
    /// This service abstracts storage that supports Transactions.
    /// </summary>
    public interface ITransactionalStorage : IStorage
    {
        /// <summary>
        /// After this operation, all subsequent operations won't be persisted in storage before commit.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Persist all accumulated operations to storage.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Reset all uncommited changes in context.
        /// </summary>
        Task ResetTransactionAsync();
    }
}
