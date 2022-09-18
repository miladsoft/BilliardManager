using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Billiard.Entities;

namespace Billiard.Services
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields
        private readonly IRepository<TEntity> _repository;
        #endregion Private Fields

        #region Constructor
        public Service(IRepository<TEntity> repository) { _repository = repository; }
        #endregion Constructor

        public virtual TEntity Find(params object[] keyValues) { return _repository.Find(keyValues); }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters) { return _repository.SelectQuery(query, parameters).AsQueryable(); }

        public virtual async Task<bool> InsertAsync(TEntity entity) { return await _repository.Insert(entity); }

        public virtual void InsertRange(IEnumerable<TEntity> entities) { _repository.InsertRange(entities); }

        public virtual void InsertOrUpdateGraph(TEntity entity) { _repository.InsertOrUpdateGraph(entity); }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities) { _repository.InsertGraphRange(entities); }

        public virtual async Task<bool> UpdateAsync(TEntity entity) { return await _repository.Update(entity); }

        public virtual bool Delete(object id) { return _repository.Delete(id); }

        public virtual bool Delete(TEntity entity) { return _repository.Delete(entity); }

        public IQueryFluent<TEntity> Query() { return _repository.Query(); }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject) { return _repository.Query(queryObject); }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query) { return _repository.Query(query); }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues) { return await _repository.FindAsync(keyValues); }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues) { return await _repository.FindAsync(cancellationToken, keyValues); }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues) { return await DeleteAsync(CancellationToken.None, keyValues); }

        //IF 04/08/2014 - Before: return await DeleteAsync(cancellationToken, keyValues);
        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues) { return await _repository.DeleteAsync(cancellationToken, keyValues); }

        public IQueryable<TEntity> Queryable() { return _repository.Queryable(); }


    }
}