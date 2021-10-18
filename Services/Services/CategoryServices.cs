using DAL.Entities.Categories;
using DAL.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using DAL.Entities.Courses;
namespace Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IGenericRepository<Course> _coursesrepository;
        private readonly IGenericRepository<Category> _categoryrepository;
        public CategoryServices(IGenericRepository<Category> categoryrepository, IGenericRepository<Course> coursesrepository)
        {
            _categoryrepository = categoryrepository;
            _coursesrepository = coursesrepository;
        }
        public async Task<Category> GetCategoryByIdAsync(int id) =>
            await _categoryrepository.FindAsync(id);

        public async Task<bool> CreateNewCategoryAsync(Category category) =>
            await _categoryrepository.CreateAsync(category);

        public async Task<bool> DeletCategoryAsync(int id) =>
            await _categoryrepository.DeleteAsync(id);

        public async Task<bool> UpdateCategoryAsync(Category category) =>
            await _categoryrepository.UpdateAsync(category);

        public async Task<Category> GetParentOrDefaultfCategoryAsync(int childid) =>
            await _categoryrepository.GetQuery().Where(c => c.Id == childid).Select(c => c.BaseCategory).FirstOrDefaultAsync();
        public async Task<List<Category>> GetSubCategoriesAsync(int id) =>
            await _categoryrepository.GetQuery().Where(s => s.BaseCategoryID == id).ToListAsync();
        public async Task<List<Category>> GetTopLevelCategoriesAsync() =>
            await _categoryrepository.GetQuery().Where(c => c.BaseCategoryID == null).ToListAsync();

        public async Task<bool> HaseCoursesAsync(int id) =>
            await _coursesrepository.GetQuery().Where(x => x.CategoryId == id).AnyAsync();

        public async Task<bool> HaseSubCategoriesAsync(int id) =>
            await _categoryrepository.GetQuery().Where(x => x.BaseCategoryID == id).AnyAsync();

       /* public async Task<List<Course>> getCourseOrdered(int CategoryId, int Limit, int Orderby  , int Moreafter=0 )
        {
            IQueryable<Course> q = _coursesrepository.GetQuery().Where(c => c.CategoryId == CategoryId);
            if (Orderby == 1)
                q = q.OrderBy(c => c.CreatedDate);
            else if (Orderby == 2)
            {
                q = q.OrderBy(c => c.Ratings.Average(x=>((int)x.RatingStar)));
            }
            return await q.SkipWhile(c => c.Id != Moreafter || Moreafter == 0).Skip(1).Take(Limit).ToListAsync();

        }*/
    }

    public interface ICategoryServices
    {
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task<bool> CreateNewCategoryAsync(Category category);

        public Task<bool> DeletCategoryAsync(int id);

        public Task<bool> UpdateCategoryAsync(Category category);

        public Task<List<Category>> GetSubCategoriesAsync(int id);

        public Task<List<Category>> GetTopLevelCategoriesAsync();

        public Task<Category> GetParentOrDefaultfCategoryAsync(int childid);

        public Task<bool> HaseSubCategoriesAsync(int id);

        public Task<bool> HaseCoursesAsync(int id);

    //  public Task<List<Course>> getCourseOrdered(int CategoryId, int Orderby, int Limit, int Moreafter);
    }
}