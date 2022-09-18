#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Billiard.DataLayer.Context;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Billiard.Entities;

#endregion

namespace Billiard.Services
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        #region Private Fields

        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWork _unitOfWork;

        #endregion Private Fields

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = unitOfWork.Set<TEntity>();
        }

        public bool Delete(object id)
        {
            var entity = _dbSet.Find(id);

            if (entity == null)
            {
                return false;
            }
            _unitOfWork.Entry(entity).State = EntityState.Deleted;
            int returnValue = _unitOfWork.SaveChanges();
            if (returnValue > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }
            _unitOfWork.Entry(entity).State = EntityState.Deleted;

            int returnValue = _unitOfWork.SaveChanges();
            if (returnValue > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(params object[] keyValues)
        {
            var entity = Find(keyValues);

            if (entity == null)
            {
                return false;
            }

            _unitOfWork.Entry(entity).State = EntityState.Deleted;
            int returnValue = await _unitOfWork.SaveChangesAsync();
            if (returnValue > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = Find(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            _unitOfWork.Entry(entity).State = EntityState.Deleted;
            int returnValue = await _unitOfWork.SaveChangesAsync();
            if (returnValue > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public async Task<bool> Insert(TEntity entity)
        {
            _unitOfWork.Entry(entity).State = EntityState.Added;
            int returnValue = await _unitOfWork.SaveChangesAsync();
            if (returnValue > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Insert(item);
            }
        }

        public void InsertOrUpdateGraph(TEntity entity)
        {
            _dbSet.AddRange(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                _unitOfWork.Entry(item).State = EntityState.Added;
            }
            _unitOfWork.SaveChanges();

        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters).AsQueryable();
        }

        public async Task<bool> Update(TEntity entity)
        {
            _unitOfWork.Entry(entity).State = EntityState.Modified;

            int returnValue = await _unitOfWork.SaveChangesAsync();
            if (returnValue > 0)
            {
                return true;
            }
            else
            {
                return false;
            };

        }


        internal IQueryable<TEntity> Select(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    List<Expression<Func<TEntity, object>>> includes = null,
    int? page = null,
    int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    List<Expression<Func<TEntity, object>>> includes = null,
    int? page = null,
    int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }
    }
}