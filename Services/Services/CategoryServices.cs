using System;
using DAL.Entities.Categories;
using DAL.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using DAL.Entities.Courses;
using Common.Services;
using Model.Category.Output;
using AutoMapper;
using Model.Category.Input;
using Model.Helper;

namespace Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IGenericRepository<Course> _coursesrepository;
        private readonly IGenericRepository<Category> _categoryrepository;
        private readonly IMapper _mapper;
        public CategoryServices(IMapper mapper, IGenericRepository<Category> categoryrepository, IGenericRepository<Course> coursesrepository)
        {
            _categoryrepository = categoryrepository;
            _coursesrepository = coursesrepository;
            _mapper = mapper;
        }
        public async Task<ResultService<List<CategoryOutput>>> GetSubCategoriesAsync(int id)
        {
            ResultService<List<CategoryOutput>> result = new();
            if (!(await _categoryrepository.IsExist(id)))
            {
                result.ErrorField = nameof(id);
                result.Messege = "category Not Found";
                result.Code = ResultStatusCode.NotFound;
                return result;
            }
            result.Result = _mapper.Map<List<Category>, List<CategoryOutput>>
            (
                await _categoryrepository
                .GetQuery().Where(
                    s => s.BaseCategoryID == id
                ).ToListAsync()
            );
            if (result.Result != null && result.Result.Count != 0)
            {
                return result;
            }
            result.Code = ResultStatusCode.NotFound;
            result.Messege = "SubCategories NotFound";
            return result;
        }

        public async Task<ResultService<CategoryOutput>> GetCategoryByIdAsync(int id)
        {
            var result = new ResultService<CategoryOutput>();
            var Category = await _categoryrepository.FindAsync(id);
            if (Category is null)
            {
                result.ErrorField = nameof(id);
                result.Code = ResultStatusCode.NotFound;
                result.Messege = "Category Not Found";
                return result;
            }
            result.Code = ResultStatusCode.Ok;
            result.Result = _mapper.Map<Category, CategoryOutput>(Category);
            return result;
        }
        public async Task<ResultService<List<CategoryOutput>>> GetTopLevelCategoriesAsync()
        {
            ResultService<List<CategoryOutput>> result = new();
            result.Result = _mapper.Map<List<Category>, List<CategoryOutput>>(await _categoryrepository.GetQuery().Where(c => c.BaseCategoryID == null).ToListAsync());
            if (result.Result == null || result.Result.Count == 0)
            {
                result.Code = ResultStatusCode.NotFound;
                result.Messege = "No Top Level Categories yet";
            }
            return result;
        }

        public async Task<ResultService<CategoryOutput>> GetParentAsync(int childid)
        {
            var child = await _categoryrepository.GetQuery().Where(c => c.Id == childid).Include(c => c.BaseCategory).FirstOrDefaultAsync();
            ResultService<CategoryOutput> result = new();
            if (child is null)
            {
                result.ErrorField = nameof(childid);
                result.Messege = "Category NotFound";
                result.Code = ResultStatusCode.NotFound;
                return result;
            }
            else if (child.BaseCategory is null)
            {
                result.Code = ResultStatusCode.BadRequest;
                result.Messege = "this Category has no Parent";
                return result;
            }
            result.Result = _mapper.Map<Category, CategoryOutput>(child.BaseCategory);
            return result;
        }

        public async Task<ResultService<CategoryOutput>> CreateNewCategoryAsync(CategoryCreateInput category)
        {
            ResultService<CategoryOutput> result = new();
            try
            {
                if (category.Parentid != null && !(await _categoryrepository.IsExist(category.Parentid ?? 0)))
                {
                    result.Messege = "Parent Not Found";
                    result.ErrorField = nameof(category.Parentid);
                    result.Code = ResultStatusCode.BadRequest;
                    return result;
                }
                var CategoryToCreate = _mapper.Map<CategoryCreateInput, Category>(category);
                if (await _categoryrepository.CreateAsync(CategoryToCreate))
                {
                    result.Result = _mapper.Map<Category, CategoryOutput>(CategoryToCreate);
                    return result;
                }
                result.Messege = "Category Not Added";
                result.Code = ResultStatusCode.BadRequest;
                return result;
            }
            catch
            {
                return ResultService<CategoryOutput>.GetErrorResult();
            }
        }

        public async Task<ResultService<CategoryOutput>> UpdateCategoryAsync(CategoryUpdateInput category)
        {
            ResultService<CategoryOutput> result = new();
            try
            {
                if (!(await _categoryrepository.IsExist(category.Id)))
                {
                    result.ErrorField = nameof(category.Id);
                    result.Messege = "category Not Found";
                    result.Code = ResultStatusCode.NotFound;
                    return result;
                }
                if (category.ParentId != null && !(await _categoryrepository.IsExist(category.ParentId ?? 0)))
                {
                    result.ErrorField = nameof(category.ParentId);
                    result.Messege = "Parent Not Found";
                    result.Code = ResultStatusCode.BadRequest;
                    return result;
                }
                var CategoryToUpdate = _mapper.Map<CategoryUpdateInput, Category>(category);
                if (await _categoryrepository.UpdateAsync(CategoryToUpdate))
                {
                    result.Result = _mapper.Map<Category, CategoryOutput>(CategoryToUpdate);
                    return result;
                }
                result.Messege = "Category Not Added";
                result.Code = ResultStatusCode.BadRequest;
                return result;
            }
            catch
            {
                return ResultService<CategoryOutput>.GetErrorResult();
            }
        }
        public async Task<ResultService<bool>> DeletCategoryAsync(int id)
        {
            ResultService<bool> result = new();
            try
            {
                result.Result = false;
                if (!await _categoryrepository.IsExist(id))
                {
                    result.ErrorField = nameof(id);
                    result.Messege = "category not found";
                    result.Code = ResultStatusCode.NotFound;
                    return result;
                }
                if (await HaseCoursesAsync(id))
                {
                    result.Messege = "this category Has Courses";
                    result.Code = ResultStatusCode.BadRequest;
                    return result;
                }
                if (await HaseSubCategoriesAsync(id))
                {
                    result.Messege = "this category Has SubCategories";
                    result.Code = ResultStatusCode.BadRequest;
                    return result;
                }
                if (await _categoryrepository.DeleteAsync(id))
                {
                    result.Result = true;
                    result.Code = ResultStatusCode.Ok;
                    return result;
                }
                result.Code = ResultStatusCode.BadRequest;
                result.Messege = "category not Deleted";
                return result;
            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }
        }
        public async Task<bool> HaseCoursesAsync(int id) =>
            await _coursesrepository.GetQuery().Where(x => x.CategoryId == id).AnyAsync();

        public async Task<bool> HaseSubCategoriesAsync(int id) =>
            await _categoryrepository.GetQuery().Where(x => x.BaseCategoryID == id).AnyAsync();
        public async Task<PagedList<Course>> GetCourses(int CatId, CategoryCoursesParams Params)
        {
            var Query = _coursesrepository.GetQuery().Include(c=>c.PriceHistory)
            .Where(c => c.CategoryId == CatId&& c.PriceHistory.Where(s => s.StartedApplyDate <= DateTime.Now).OrderByDescending(s => s.StartedApplyDate).Select(s => s.Price).FirstOrDefault() >= Params.LowerPrice && c.PriceHistory.Where(s => s.StartedApplyDate <= DateTime.Now).OrderByDescending(s => s.StartedApplyDate).Select(s => s.Price).FirstOrDefault() <= Params.HigherPrice);
            if (Params.Orderby == CategoryCoursesParams.Ordaring.Rating)
            {
                Query = Query.Include(r=>r.Ratings).OrderByDescending(c => c.Ratings.Average(s => (int)s.RatingStar));
            }
            if (Params.Orderby == CategoryCoursesParams.Ordaring.popular)
            {
                Query = Query.Include(r=>r.FavoriteByList).OrderByDescending(x => x.FavoriteByList.Count);
            }
            if (Params.Orderby == CategoryCoursesParams.Ordaring.Price)
            {
                Query = Query.OrderBy(x => x.PriceHistory.Where(s => s.StartedApplyDate <= DateTime.Now).OrderByDescending(s => s.StartedApplyDate).Select(s => s.Price).FirstOrDefault());
            }
            if (Params.Orderby == CategoryCoursesParams.Ordaring.Student)
            {
                Query = Query.Include(r=>r.Students).OrderByDescending(x => x.Students.Count);
            }
            return await PagedList<Course>.CreatePagingListAsync(Query, Params.PageNumber, Params.PageSize);
        }
    }
    public interface ICategoryServices
    {
        public Task<ResultService<CategoryOutput>> GetCategoryByIdAsync(int id);
        public Task<ResultService<CategoryOutput>> CreateNewCategoryAsync(CategoryCreateInput category);
        public Task<ResultService<bool>> DeletCategoryAsync(int id);
        public Task<ResultService<CategoryOutput>> UpdateCategoryAsync(CategoryUpdateInput category);
        public Task<ResultService<List<CategoryOutput>>> GetSubCategoriesAsync(int id);
        public Task<ResultService<List<CategoryOutput>>> GetTopLevelCategoriesAsync();
        public Task<ResultService<CategoryOutput>> GetParentAsync(int childid);
        public Task<PagedList<Course>> GetCourses(int CatId, CategoryCoursesParams Params);
    }
}
