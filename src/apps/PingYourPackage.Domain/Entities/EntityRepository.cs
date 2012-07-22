using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class EntityRepository<T> : IEntityRepository<T>
        where T : class, IEntity, new() {

        readonly IEntitiesContext _entities;

        public EntityRepository(IEntitiesContext entities) {

            _entities = entities;
        }

        public virtual IQueryable<T> GetAll() {

            return _entities.Set<T>();
        }

        public virtual IQueryable<T> All {

            get {
                return GetAll();
            }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties) {

            IQueryable<T> query = _entities.Set<T>();
            foreach (var includeProperty in includeProperties) {

                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual T Find(params object[] keyValues) {

            return _entities.Set<T>().Find(keyValues);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) {

            return _entities.Set<T>().Where(predicate);
        }

        public virtual PaginatedList<T> Paginate(int pageIndex, int pageSize) {

            return Paginate(null, pageIndex, pageSize);
        }

        public virtual PaginatedList<T> Paginate(Expression<Func<T, bool>> predicate, int pageIndex, int pageSize) {

            IQueryable<T> query = (predicate == null) ?
                _entities.Set<T>().AsQueryable() :
                _entities.Set<T>().Where(predicate).AsQueryable();

            return new PaginatedList<T>(query, pageIndex, pageSize);
        }

        public virtual void Add(T entity) {

            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity) {

            _entities.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity) {

            _entities.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual void Save() {

            _entities.SaveChanges();
        }
    }
}