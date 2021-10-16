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
            await _coursesrepository.GetQuery().Where(x => x.CategoryId == id).CountAsync() > 0;

        public async Task<bool> HaseSubCategoriesAsync(int id) =>
            await _categoryrepository.GetQuery().Where(x => x.BaseCategoryID == id).CountAsync() > 0;

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
    }
}