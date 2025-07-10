using System.Linq.Expressions;

namespace FlapKap.Data
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> GetAll();
        T GetByID(int? id);
        Task<T> GetByIdAsync(object id);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> GetByConditionWithTracking(Expression<Func<T, bool>> expression);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string IncludeProperties = "");
        IQueryable<T> GetWithTracking(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string IncludeProperties = "");
        T GetEntityByCondition(Expression<Func<T, bool>> filter = null, string IncludeProperties = "");
        Task<T> GetEntityByConditionAsync(Expression<Func<T, bool>> filter = null, string IncludeProperties = "");
        T GetEntityByConditionWithTracking(Expression<Func<T, bool>> filter = null, string IncludeProperties = "");
        Task<T> GetEntityByConditionWithTrackingAsync(Expression<Func<T, bool>> filter = null, string IncludeProperties = "");
        T Insert(T entity);
        void BulkUpdateWithRelated(List<T> entityList);
        void BulkUpdate(List<T> entityList);
        IEnumerable<T> BulkInsert(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity);
        void BulkDelete(List<T> entityList);
        void BulkDeleteIfExist(List<T> entityList);
        void Delete(object id);
        bool Exists(Expression<Func<T, bool>> filter = null);
        void _UpdateByPropList(T entity, params string[] properties);
        IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters);
        T ExecWithStoreProcedure_SingleEntity(string query, params object[] parameters);
        void BulkUpdateByPropList(List<T> list, params string[] properties);
        Task<bool> DetachLocal(Expression<Func<T, bool>> match);
        Task<bool> ModifiedLocal(Expression<Func<T, bool>> match);
        void UpdateT(T entity);
        void DeleteIfExist(T entity);
        IQueryable<T> GetWithInclude(Expression<Func<T, bool>> filter, Expression<Func<T, dynamic>>[] navigationPropertyPath, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        void UpdateWithRelated(T entity);
    }
}