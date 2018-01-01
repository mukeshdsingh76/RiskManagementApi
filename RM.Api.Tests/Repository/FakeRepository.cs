using RM.Data.Models;
using RM.Data.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RM.Api.Tests.Repository
{
  public class FakeRepository<T> : IRepository<T>
   where T : class
  {
    public object _object { get; set; }
    Dictionary<Type, object> dataDictionary = null;

    public FakeRepository(object data)
    {
      _object = data;
      dataDictionary = new Dictionary<Type, object>();
      dataDictionary.Add(typeof(T), _object);
    }

    private T nameof(List<ProjectStatus> list)
    {
      throw new NotImplementedException();
    }

    public Task<T> AddAsyn(T t)
    {
      throw new NotImplementedException();
    }

    public Task<int> CountAsync()
    {
      throw new NotImplementedException();
    }

    public Task<int> DeleteAsyn(T entity)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    public Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
    {
      throw new NotImplementedException();
    }

    public Task<T> FindAsync(Expression<Func<T, bool>> match)
    {
      throw new NotImplementedException();
    }

    public Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
    {
      throw new NotImplementedException();
    }

    public Task<ICollection<T>> GetAllAsynAsync(bool eager = false)
    {
      Type type = typeof(T);
      var data = dataDictionary[type];
      IEnumerable<T> list = (IEnumerable<T>)data;
      return Task.Run(() => (ICollection<T>)list);
    }

    public Task<T> GetAsync(int id)
    {
      throw new NotImplementedException();
    }

    public Task<int> SaveAsync()
    {
      throw new NotImplementedException();
    }

    public Task<T> UpdateAsyn(T t, object key)
    {
      throw new NotImplementedException();
    }
  }
}
