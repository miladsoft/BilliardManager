using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Billiard.Entities;

namespace Billiard.Services
{
    public interface IRepository<TEntity> where TEntity : class, IObjectState
    {

        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        Task<bool> Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void InsertOrUpdateGraph(TEntity entity);
        void InsertGraphRange(IEnumerable<TEntity> entities);
        Task<bool> Update(TEntity entity);
        bool Delete(object id);
        bool Delete(TEntity entity);
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        IQueryable<TEntity> Queryable();
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
    }
}