using AutoMonitoring.DAL.Infrastructure.Database;
using AutoMonitoring.DAL.Repositories.Interfaces;
using AutoMonitoring.Domain.Entities.Implementations;
using AutoMonitoring.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoMonitoring.DAL.Repositories.Implementations;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(e => !e.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Delete(T entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }
}