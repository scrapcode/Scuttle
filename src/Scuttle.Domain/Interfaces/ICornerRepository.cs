using System;
using Scuttle.Domain.Entities;


namespace Scuttle.Domain.Interfaces;

public interface ICornerRepository
{
    Task<Corner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Corner?> GetByUrlSlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Corner>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUrlSlug(string slug, CancellationToken cancellationToken = default);
    Task<Corner> AddAsync(Corner corner, CancellationToken cancellationToken = default);
    Task UpdateAsync(Corner corner, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
