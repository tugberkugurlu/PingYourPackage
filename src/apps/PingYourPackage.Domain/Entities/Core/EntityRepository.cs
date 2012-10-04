using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class EntityRepository<T> : IEntityRepository<T>
        where T : class, IEntity, new() {

        readonly DbContext _entitiesContext;

        public EntityRepository(DbContext entitiesContext) {

            if (entitiesContext == null) {

                throw new ArgumentNullException("entitiesContext");
            }

            _entitiesContext = entitiesContext;
        }

        public virtual IQueryable<T> GetAll() {

            return _entitiesContext.Set<T>();
        }

        public virtual IQueryable<T> All {

            get {
                return GetAll();
            }
        }

        public virtual IQueryable<T> AllIncluding(
            params Expression<Func<T, object>>[] includeProperties) {

            IQueryable<T> query = _entitiesContext.Set<T>();
            foreach (var includeProperty in includeProperties) {

                query = query.Include(includeProperty);
            }

            return query;
        }

        public T GetSingle(Guid key) {

            return GetAll().FirstOrDefault(x => x.Key == key);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) {

            return _entitiesContext.Set<T>().Where(predicate);
        }

        public virtual PaginatedList<T> Paginate(int pageIndex, int pageSize) {

            return Paginate(null, pageIndex, pageSize);
        }

        public virtual PaginatedList<T> Paginate(
            Expression<Func<T, bool>> predicate, int pageIndex, int pageSize) {

            IQueryable<T> query = (predicate == null) ?
                _entitiesContext.Set<T>().AsQueryable() :
                _entitiesContext.Set<T>().Where(predicate).AsQueryable();

            return new PaginatedList<T>(query, pageIndex, pageSize);
        }

        public virtual void Add(T entity) {

            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            if (dbEntityEntry.State != EntityState.Detached) {

                dbEntityEntry.State = EntityState.Added;
            }
            else {

                _entitiesContext.Set<T>().Add(entity);
            }
        }

        public virtual void Edit(T entity) {

            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            if (dbEntityEntry.State == EntityState.Deleted) {

                _entitiesContext.Set<T>().Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity) {

            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            if (dbEntityEntry.State != EntityState.Detached) {

                dbEntityEntry.State = EntityState.Deleted;
            }
            else {

                DbSet dbSet = _entitiesContext.Set<T>();
                dbSet.Attach(entity);
                dbSet.Remove(entity);
            }
        }

        public virtual void Save() {

            _entitiesContext.SaveChanges();
        }
    }
}