using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Test.Integration.Fakes {

    public class FakeDbSet<T> : IDbSet<T> where T : class {

        ObservableCollection<T> _collection;
        IQueryable _query;

        public FakeDbSet() {

            _collection = new ObservableCollection<T>();
            _query = _collection.AsQueryable();
        }

        public T Add(T entity) {

            _collection.Add(entity);
            return entity;
        }

        public T Attach(T entity) {

            _collection.Add(entity);
            return entity;
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T {

            return Activator.CreateInstance<TDerivedEntity>();
        }

        public T Create() {

            return Activator.CreateInstance<T>();
        }

        public virtual T Find(params object[] keyValues) {

            throw new NotImplementedException();
        }

        public ObservableCollection<T> Local {

            get { return _collection; }
        }

        public T Remove(T entity) {

            _collection.Remove(entity);
            return entity;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {

            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {

            return _collection.GetEnumerator();
        }

        Type IQueryable.ElementType {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider {
            get { return _query.Provider; }
        }
    }
}
