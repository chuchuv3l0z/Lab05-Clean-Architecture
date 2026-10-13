using Lab05.Application.Interfaces;
using Lab05.Infrastructure.Persistence;
namespace Lab05.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();
    public UnitOfWork(ApplicationDbContext context) => _context = context;
    public IGenericRepository<T> Repository<T>() where T : class
    {
        if (!_repositories.TryGetValue(typeof(T), out var repository))
        {
            repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
        }
        return (IGenericRepository<T>)repository;
    }
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
