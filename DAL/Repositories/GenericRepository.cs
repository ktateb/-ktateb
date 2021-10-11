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
        public GenericRepository(StoreContext DbContext)
        {
            _dbContext = DbContext;
        }

        /// <summary>
        /// This method return always query to use it for including or filtering or ordering..
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <returns> Query of type you passed</returns>
        public IQueryable<T> GetQuery() =>
            _dbContext.Set<T>().AsQueryable<T>();

        /// <summary>
        /// This method take id to search in database..
        /// </summary>
        /// <param name="id">id</param>
        /// <typeparam name="T">Entity</typeparam>
        /// <returns>Entity you passed</returns>
        public async Task<T> FindAsync(int id) =>
            await _dbContext.Set<T>().FindAsync(id);

        /// <summary>
        /// This method return always list of entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <returns>List of Entity</returns>
        public async Task<List<T>> GetListAsync() =>
            await _dbContext.Set<T>().ToListAsync();

        /// <summary>
        /// This method for add Entity in database and return true if adding is success and false if is not.
        /// </summary>
        /// <param name="input">Entity</param>
        /// <returns>true or false</returns>
        public async Task<bool> CreateAsync(T input)
        {
            var dbRecord = await _dbContext.Set<T>().FindAsync(input.Id);
            if (dbRecord != null)
                return false;
            await _dbContext.AddAsync<T>(input);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// This method for update entity in database return true if adding is success and false if is not.
        /// </summary>
        /// <param name="input">Entity</param>
        /// <returns>true or false</returns>
        public async Task<bool> UpdateAsync(T input)
        {
            var dbRecord = await _dbContext.Set<T>().FindAsync(input.Id);
            if (dbRecord == null)
                return false;
            _dbContext.Update(dbRecord);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// This method for delete Entity from database by id and return true if adding is success and false if is not.
        /// </summary>
        /// <param name="id">id</param>
        /// <typeparam name="T">Entity</typeparam>
        /// <returns>true or false</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var dbRecord = await _dbContext.Set<T>().FindAsync(id);
            if (dbRecord == null)
                return false;
            _dbContext.Remove(dbRecord);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> FindAsync(int id);
        public Task<bool> CreateAsync(T input);
        public Task<bool> UpdateAsync(T input);
        public Task<bool> DeleteAsync(int id);
        public Task<List<T>> GetListAsync();
        public IQueryable<T> GetQuery();
    }
}