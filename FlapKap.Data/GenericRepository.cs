using FlapKap.Data.UnitOfWork;
using FlapKap.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace FlapKap.Data
{
    public abstract class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        private DbSet<T> _entities;

        private string _errorMessage = string.Empty;

        private bool _isDisposed;
        private ApplicationDBContext Context { get; set; }

        public GenericRepository(IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            _isDisposed = false;
            Context = unitOfWork.Context;


        }
        public virtual IQueryable<T> Table
        {
            get { return Entities; }
        }
        protected virtual DbSet<T> Entities
        {
            get { return _entities ?? (_entities = Context.Set<T>()); }
        }
        public virtual IQueryable<T> GetAll()
        {
            return Entities.AsQueryable<T>();
        }
        public virtual IQueryable<T> FindAll()
        {
            return Context.Set<T>();
        }

        public virtual T Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                //}
                Entities.Add(entity);
                if (Context == null || _isDisposed)
                    Context = new ApplicationDBContext();
                return entity;

            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<T> BulkInsert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }

                Context.ChangeTracker.AutoDetectChangesEnabled = false;
                Context.Set<T>().AddRange(entities);
                Context.SaveChanges();
                foreach (var entity in entities)
                {
                    Context.Entry(entity).State = EntityState.Detached;
                }
                return entities;
            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }
                throw;
            }
        }

        public virtual void BulkUpdateWithRelated(List<T> entityList)
        {
            try
            {


                if (entityList == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new ApplicationDBContext();
                foreach (var entityToUpdate in entityList)
                {
                    Entities.Update(entityToUpdate);

                }

                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }

                throw;
            }
        }

        public virtual void UpdateWithRelated(T entity)
        {
            try
            {


                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new ApplicationDBContext();
                Entities.Update(entity);


                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }

                throw;
            }
        }


        public virtual void BulkUpdate(List<T> entityList)
        {
            try
            {
                if (entityList == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new ApplicationDBContext();
                foreach (var entityToUpdate in entityList)
                {
                    SetEntryModified(entityToUpdate);

                }

                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }

                throw;
            }
        }

        //public virtual T Modify(T entity)
        //{
        //    this.Context.Set<T>().Update(entity);
        //    var result= this.Context.SaveChanges();
        //    if (result > 0)
        //        return entity;
        //    else
        //        return null;
        //}

        public virtual T Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new ApplicationDBContext();

                //SetEntryModified(entity);
                SetEntryModified(entity);

                return entity;

                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }

                throw;
            }
        }
        public virtual void DeleteIfExist(T entity)
        {
            if (entity == null) return;
            Delete(entity);
        }
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new ApplicationDBContext();

                /*if (entity is BaseEntity)
                {
                    (entity as BaseEntity).DeletionDate = Globals.ToLocal(DateTime.UtcNow, generalSetting);
                    (entity as BaseEntity).IsDeleted = true;
                }*/

                Entities.Remove(entity);
            }

            catch (ConstraintException e)
            {
                throw e;
            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }

                throw e;
            }
        }
        public void BulkDeleteIfExist(List<T> entityList)
        {
            if (entityList == null || entityList.Count <= 0) return;
            BulkDelete(entityList);
        }
        public void BulkDelete(List<T> entityList)
        {
            try
            {
                if (entityList.Count <= 0)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new ApplicationDBContext();
                Entities.RemoveRange(entityList);


                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be called with Unit of work
            }

            catch (ConstraintException e)
            {
                throw e;
            }
            catch (DbUpdateException e)
            {
                SqlException s = e.InnerException.InnerException as SqlException;
                if (s != null && s.Number == 2627)
                {
                    _errorMessage += string.Format("PartNumberAlreadyExists", s.Number);
                }
                else
                {
                    _errorMessage += string.Format("AnErrorOccuredPleaseContactYourSystemAdministrator");
                }

                throw e;
            }
        }
        public virtual void SetEntryModified(T entity)
        {
            /*if (entity is IAuditEntity)
            {
                (entity as IAuditEntity).ModifiedDate = Globals.ToLocal(DateTime.UtcNow, generalSetting);
            }*/
            Context.Entry(entity).State = EntityState.Modified;

        }

        //public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        //{
        //    return this.Context.Set<T>().Where(expression).AsNoTracking();
        //}

        public virtual T GetByID(int? id)
        {
            return Entities.Find(id);
        }
        public virtual T GetEntityByCondition(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities;

            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault();

        }

        public virtual async Task<T> GetEntityByConditionAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities;

            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();

        }


        public virtual T GetEntityByConditionWithTracking(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault();
        }


        public virtual async Task<T> GetEntityByConditionWithTrackingAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetEntityByConditiontAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities;

            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();

        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return Entities.AsNoTracking().AsQueryable().Where(expression);
        }

        public IQueryable<T> GetByConditionWithTracking(Expression<Func<T, bool>> expression)
        {
            return Entities.AsQueryable<T>().Where(expression);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities;
            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {

                //return orderBy(query).ToList();
                return orderBy(query);
            }
            else
            {
                //return query.ToList(); 
                return query;
            }
        }

        public virtual IQueryable<T> GetWithTracking(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {

                //return orderBy(query).ToList();
                return orderBy(query);
            }
            else
            {
                //return query.ToList(); 
                return query;
            }
        }

        public virtual IQueryable<T> GetWithInclude(Expression<Func<T, bool>> filter, Expression<Func<T, dynamic>>[] navigationPropertyPath, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = Entities.AsQueryable();
            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }
            if (navigationPropertyPath != null)
            {
                foreach (var includeProperty in navigationPropertyPath)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {

                //return orderBy(query).ToList();
                return orderBy(query);
            }
            else
            {
                //return query.ToList(); 
                return query;
            }
        }

        public virtual IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {

            return Context.Set<T>().FromSqlRaw(query, parameters).AsEnumerable();
        }

        public virtual int ExecStoredProcedure(string query, params object[] parameters)
        {

            return Context.Database.ExecuteSqlRaw(query, parameters);
        }



        public T ExecWithStoreProcedure_SingleEntity(string query, params object[] parameters)
        {
            //return Context.Query<T>().FromSqlRaw(query, parameters).FirstOrDefault();
            var xxx = Context.Set<T>().FromSqlRaw(query, parameters);
            return xxx.AsEnumerable().FirstOrDefault();

        }

        public virtual bool Exists(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (query.Any())
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = Entities.Find(id);
            if (entityToDelete != null)
                Delete(entityToDelete);
        }
        public virtual void _UpdateByPropList(T entity, params string[] properties)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                Context.Set<T>().Attach(entity);

            foreach (var property in properties)
                Context.Entry(entity)
                    .Property(property)
                    .IsModified = true;

        }



        public virtual void BulkUpdateByPropList(List<T> list, params string[] properties)
        {
            foreach (var entity in list)
            {
                Context.Set<T>().Attach(entity);

                foreach (var property in properties)
                    Context.Entry(entity)
                        .Property(property)
                        .IsModified = true;
            }

        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task<bool> DetachLocal(Expression<Func<T, bool>> match)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            try
            {
                var local = Context.Set<T>().Local.AsQueryable().FirstOrDefault(match);

                if (local != null)
                {
                    Context.Entry(local).State = EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public async Task<bool> ModifiedLocal(Expression<Func<T, bool>> match)

        {
            try
            {
                var local = Context.Set<T>().Local.AsQueryable().FirstOrDefault(match);

                if (local != null)
                {
                    Context.Entry(local).State = EntityState.Modified;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public virtual void UpdateT(T entity)
        {


            var entry = Context.Entry(entity);


            Context.Set<T>().Attach(entity);
            PropertyInfo[] properties = typeof(T).GetProperties();

            //context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            foreach (PropertyInfo prop in properties)
            {
                var notMapped = prop.GetCustomAttributes(typeof(NotMappedAttribute), false);
                if (prop.Name != nameof(BaseEntity.Id) && prop.Name != nameof(BaseEntity.AddUserId) && prop.Name != nameof(BaseEntity.AddedDate) &&
                    !(prop.PropertyType.IsClass && prop.PropertyType != typeof(string)) && !prop.PropertyType.IsArray && !prop.PropertyType.IsGenericType && !prop.PropertyType.IsInterface
                     && notMapped.Length == 0)
                {
                    Context.Entry(entity)
                         .Property(prop.Name)
                         .IsModified = true;
                }

            }
            SetEntryModified(entity);
            //foreach (var property in entity.)
            //    context.Entry<T>(entity)
            //        .Property(property)
            //        .IsModified = true;

        }


    }
}