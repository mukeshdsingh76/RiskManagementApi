using Microsoft.EntityFrameworkCore;
using RM.Data;
using RM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RM.Services.Repository
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly RMContext _context;

    public Repository(RMContext context)
    {
      _context = context;
    }

    public IQueryable<T> GetAll(bool eager = false)
    {
      var query = _context.Set<T>().AsQueryable();
      if (eager)
      {
        foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
          query = query.Include(property.Name);
      }
      return query;
    }

    public virtual async Task<ICollection<T>> GetAllAsyn(bool eager = false)
    {
      var query = _context.Set<T>().AsQueryable();
      if (eager)
      {
        foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
          query = query.Include(property.Name);
      }
      return await query.ToListAsync();
    }

    public virtual T Get(int id)
    {
      return _context.Set<T>().Find(id);
    }

    public virtual Task<T> GetAsync(int id)
    {
      return _context.Set<T>().FindAsync(id);
    }

    public virtual T Add(T t)
    {
      _context.Set<T>().Add(t);
      _context.SaveChanges();
      return t;
    }

    public virtual async Task<T> AddAsyn(T t)
    {
      _context.Set<T>().Add(t);
      await _context.SaveChangesAsync().ConfigureAwait(false);
      return t;
    }

    public virtual T Find(Expression<Func<T, bool>> match)
    {
      return _context.Set<T>().SingleOrDefault(match);
    }

    public virtual Task<T> FindAsync(Expression<Func<T, bool>> match)
    {
      return _context.Set<T>().SingleOrDefaultAsync(match);
    }

    public ICollection<T> FindAll(Expression<Func<T, bool>> match)
    {
      return _context.Set<T>().Where(match).ToList();
    }

    public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
    {
      return await _context.Set<T>().Where(match).ToListAsync().ConfigureAwait(false);
    }

    public virtual void Delete(T entity)
    {
      _context.Set<T>().Remove(entity);
      _context.SaveChanges();
    }

    public virtual Task<int> DeleteAsyn(T entity)
    {
      _context.Set<T>().Remove(entity);
      return _context.SaveChangesAsync();
    }

    public virtual T Update(T t, object key)
    {
      if (t == null)
        return null;
      T exist = _context.Set<T>().Find(key);
      if (exist != null)
      {
        _context.Entry(exist).CurrentValues.SetValues(t);
        _context.SaveChanges();
      }
      return exist;
    }

    public virtual async Task<T> UpdateAsyn(T t, object key)
    {
      if (t == null)
        return null;
      T exist = await _context.Set<T>().FindAsync(key).ConfigureAwait(false);
      if (exist != null)
      {
        _context.Entry(exist).CurrentValues.SetValues(t);
        await _context.SaveChangesAsync().ConfigureAwait(false);
      }
      return exist;
    }

    public int Count()
    {
      return _context.Set<T>().Count();
    }

    public Task<int> CountAsync()
    {
      return _context.Set<T>().CountAsync();
    }

    public virtual void Save()
    {
      _context.SaveChanges();
    }

    public virtual Task<int> SaveAsync()
    {
      return _context.SaveChangesAsync();
    }

    public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
    {
      return _context.Set<T>().Where(predicate);
    }

    public virtual async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
    {
      return await _context.Set<T>().Where(predicate).ToListAsync().ConfigureAwait(false);
    }

    public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
    {
      IQueryable<T> queryable = GetAll();
      foreach (Expression<Func<T, object>> includeProperty in includeProperties)
      {
        queryable = queryable.Include<T, object>(includeProperty);
      }

      return queryable;
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
        this.disposed = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}