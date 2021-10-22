﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Product.Application.Contracts.Repositories
{
  public interface IRepository<T> where T : class
  {
    Task<T> GetByIdAsync(int id);
    Task<T> GetByLongIdAsync(long id);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
      bool disableTracking = true);

    Task<T> AddAsync(T entity);
    Task<IReadOnlyList<T>> AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> DeleteAsync(Expression<Func<T, bool>> criteria);
  }
}
