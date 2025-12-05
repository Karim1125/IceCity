using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IceCity_Data_Management_System.Persistence.Repositories
{
    public class GenericRepository<TEntity>(
        ApplicationDbContext context,
        ILogger<GenericRepository<TEntity>> logger)
        : LoggingRepositoryBase<TEntity>(context, logger), IRepository<TEntity>
        where TEntity : class
    {
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FindAsync(id);
                LogInfo($"Get {typeof(TEntity).Name} by Id={id} → {(entity != null ? "FOUND" : "NOT FOUND")}");
                return entity;
            }
            catch (Exception ex)
            {
                LogError($"Get {typeof(TEntity).Name} by Id={id} FAILED", ex);
                throw;
            }
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                LogInfo($"GetAll {typeof(TEntity).Name}");
                return await _context.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                LogError($"GetAll {typeof(TEntity).Name} FAILED", ex);
                throw;
            }
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            LogInfo($"GetAllQueryable {typeof(TEntity).Name}");
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                LogInfo($"Find {typeof(TEntity).Name} (predicate applied)");
                return await _context.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                LogError($"Find {typeof(TEntity).Name} FAILED", ex);
                throw;
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                LogInfo($"Add {typeof(TEntity).Name} succeeded");
            }
            catch (Exception ex)
            {
                LogError($"Add {typeof(TEntity).Name} FAILED", ex);
                throw;
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                LogInfo($"Update {typeof(TEntity).Name} succeeded");
            }
            catch (Exception ex)
            {
                LogError($"Update {typeof(TEntity).Name} FAILED", ex);
                throw;
            }
        }

        public void Remove(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                LogInfo($"Remove {typeof(TEntity).Name} succeeded");
            }
            catch (Exception ex)
            {
                LogError($"Remove {typeof(TEntity).Name} FAILED", ex);
                throw;
            }
        }
    }
}
