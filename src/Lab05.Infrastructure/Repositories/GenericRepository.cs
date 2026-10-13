using System.Linq.Expressions;
using Lab05.Application.Interfaces;
using Lab05.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace Lab05.Infrastructure.Repositories;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();
    public async Task<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => _dbSet.AnyAsync(predicate);
    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
    public void Update(TEntity entity)
    {
        var key = _context.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!.Properties.Single();
        var id = _context.Entry(entity).Property(key.Name).CurrentValue;
        var tracked = _dbSet.Local.FirstOrDefault(item =>
            Equals(_context.Entry(item).Property(key.Name).CurrentValue, id));
        if (tracked != null)
            _context.Entry(tracked).CurrentValues.SetValues(entity);
        else
            _dbSet.Update(entity);
    }
    public void Delete(TEntity entity) => _dbSet.Remove(entity);
}
