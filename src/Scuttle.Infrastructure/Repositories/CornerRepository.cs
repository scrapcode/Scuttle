
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Scuttle.Domain.Entities;
using Scuttle.Domain.Interfaces;
using Scuttle.Infrastructure.Persistence;

namespace Scuttle.Infrastructure.Repositories;

public class CornerRepository : ICornerRepository
{
    private readonly ApplicationDbContext _context;

    public CornerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add a Corner
    public async Task<Corner> AddAsync(Corner corner, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(corner, cancellationToken);
        await _context.SaveChangesAsync();

        return corner;
    }

    // Delete a Corner
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var corner = await _context.Corners.FindAsync(new object[] { id }, cancellationToken);
        if (corner != null)
        {
            _context.Corners.Remove(corner);
            await _context.SaveChangesAsync();
        }
    }

    // Check if exists by Name
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Corners.AnyAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    // Check if exists by UrlSlug
    public async Task<bool> ExistsByUrlSlug(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Corners.AnyAsync(c => c.UrlSlug.ToLower() == slug.ToLower(), cancellationToken);
    }

    // Get All Corners
    public async Task<IEnumerable<Corner>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Corners
            .Include(c => c.Creator)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    // Get Corner by Id
    public async Task<Corner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Corners
            .Include(c => c.Creator)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    // Get Corner by UrlSlug
    public async Task<Corner?> GetByUrlSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Corners
            .Include(c => c.Creator)
            .FirstOrDefaultAsync(c => c.UrlSlug == slug, cancellationToken);
    }
    
    // Update a Corner
    public async Task UpdateAsync(Corner corner, CancellationToken cancellationToken = default)
    {
        _context.Corners.Update(corner);
        await _context.SaveChangesAsync();
    }
}
