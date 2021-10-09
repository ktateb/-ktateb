using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DataContext;
using DAL.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;
        private readonly DbSet<T> entities;

        public GenericRepository(StoreContext DbContext)
        {
            _dbContext = DbContext;
        }

        public IQueryable<T> GetQuery() =>
            _dbContext.Set<T>().AsQueryable<T>();

        public async Task<T> FindAsync(int id) =>
            await _dbContext.Set<T>().FindAsync(id);

        public async Task<List<T>> GetListAsync() =>
            await _dbContext.Set<T>().ToListAsync();
    }
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> FindAsync(int id);
        public Task<List<T>> GetListAsync();
        public IQueryable<T> GetQuery();
    }
}