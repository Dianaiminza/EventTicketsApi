using EventsTicket.Infrastructure.Context;
using Infrastructure.shared.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventsTicket.Infrastructure.Repository.Abstractions
{
    public interface IRepositoryUnit
    {
        IQueryable<TEntity> Entity<TEntity>()
      where TEntity : class, IEntity;

        void Create<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        Task<IDbContextTransaction> BeginTransactionAsync();

        DatabaseContext DbContext();

        Task SaveAsync(
            string errorMessage = "Failed to perform save operation",
            CancellationToken cancellationToken = default);
    }
}
