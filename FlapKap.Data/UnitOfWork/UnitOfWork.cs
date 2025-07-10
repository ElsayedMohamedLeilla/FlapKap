using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FlapKap.Data.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext, new()
    {
        private readonly ApplicationDBContext _context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
        private Dictionary<string, object> _repositories;

        private IDbContextTransaction _objTran;
        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ApplicationDBContext Context
        {
            get { return _context; }
        }

        public void CreateTransaction()
        {
            _objTran = _context.Database.BeginTransaction();
        }

        public async Task CreateTransactionAsync()
        {
            _objTran = await _context.Database.BeginTransactionAsync();
        }
        public void Commit()
        {
            _objTran.Commit();
        }
        public async Task CommitAsync()
        {
            await _objTran.CommitAsync();
        }
        public void Rollback()
        {
            try
            {
                if (_objTran == null) return;
                _objTran.Rollback();
                _objTran.Dispose();
            }
            catch (Exception)
            {
                // do nothing
            }
        }
        public async Task RollbackAsync()
        {
            if (_objTran == null) return;
            await _objTran.RollbackAsync();
            await _objTran.DisposeAsync();
        }
        public void DetachEntity<T>(T Entity) where T : class
        {
            var changedEntriesCopy = _context.Entry(Entity);

            changedEntriesCopy.State = EntityState.Detached;
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                SqlException s = e?.InnerException?.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("Part number '{0}' already exists.", s.Number);
                }
                if (e != null)
                {
                    _errorMessage += string.Format("You can't delete this item");
                }
                else
                {
                    _errorMessage += string.Format("An error occured - please contact your system administrator.");
                }
                throw;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                SqlException s = e?.InnerException?.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("Part number '{0}' already exists.", s.Number);
                }
                if (e != null)
                {
                    _errorMessage += string.Format("You cant delete this item");
                }
                else
                {
                    _errorMessage += string.Format("An error occured - please contact your system administrator.");
                }
                throw;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }
        public GenericRepository<T> GenericRepository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<T>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<T>)_repositories[type];
        }
    }
}