namespace RKSoftware.DAL.Core;

/// <summary>
/// This service abstracts storage that supports Transactions.
/// </summary>
public interface ITransactionalStorage : IStorage
{
    /// <summary>
    /// After this operation, all subsequent operations won't be persisted in storage before commit.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// After this operation, all subsequent operations won't be persisted in storage before commit.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Persist all operations accumulated since <see cref="BeginTransactionAsync()"/> to storage and end the transaction.
    /// Does nothing if no transaction is active.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Persist all operations accumulated since <see cref="BeginTransactionAsync()"/> to storage and end the transaction.
    /// Does nothing if no transaction is active.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Discard all uncommitted changes and end the active transaction.
    /// </summary>
    Task ResetTransactionAsync();

    /// <summary>
    /// Discard all uncommitted changes and end the active transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to observe.</param>
    Task ResetTransactionAsync(CancellationToken cancellationToken);
}
