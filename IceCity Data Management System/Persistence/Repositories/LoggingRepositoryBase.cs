using Microsoft.Extensions.Logging;

namespace IceCity_Data_Management_System.Persistence.Repositories
{
    public abstract class LoggingRepositoryBase<TEntity>(ApplicationDbContext context, ILogger logger)
        where TEntity : class
    {
        protected readonly ApplicationDbContext _context = context;
        protected readonly ILogger _logger = logger;

        protected void LogInfo(string message)
        {
            _logger.LogInformation("[{Timestamp}] {Message}", DateTime.Now, message);
        }

        protected void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, "[{Timestamp}] {Message}", DateTime.Now, message);
        }
    }
}
