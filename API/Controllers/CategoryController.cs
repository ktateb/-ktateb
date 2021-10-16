using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Common;
using AutoMapper;
using DAL.Entities.Categories;
using Microsoft.AspNetCore.Mvc;
using Model.Category.Input;
using Model.Category.Output;
using Services;

namespace API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ICategoryServices _categoryServices;

        public CategoryController(IMapper mapper, ICategoryServices categoryServices)
        {
            _mapper = mapper;
            _categoryServices = categoryServices;
        }
        [HttpGet("{id}/SubCategories")]
        public async Task<List<CategoryOutput>> subcategories(int id) =>
            _mapper.Map<List<Category>, List<CategoryOutput>>(await _categoryServices.GetSubCategoriesAsync(id));

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryOutput>> GetCategory(int id)
        {
            var Category = _mapper.Map<Category, CategoryOutput>(await _categoryServices.GetCategoryByIdAsync(id));
            if (Category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(Category);
        }

        [HttpGet("TopLevelCategories")]
        public async Task<List<CategoryOutput>> GetTopLevelCategories() =>
            _mapper.Map<List<Category>, List<CategoryOutput>>(await _categoryServices.GetTopLevelCategoriesAsync());

        [HttpGet("{id}/Parent")]
        public async Task<ActionResult<CategoryOutput>> Parent(int id)
        {

            if (await _categoryServices.GetCategoryByIdAsync(id) == null)
            {
                return NotFound("category not found");
            }
            var parent = _mapper.Map<Category, CategoryOutput>(await _categoryServices.GetParentOrDefaultfCategoryAsync(id));
            if (parent == null)
            {
                return BadRequest("this category hase no parent");
            }
            return Ok(parent);
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(CategoryCreateInput category)
        {
            if (category.Parentid != null)
            {
                if (await _categoryServices.GetCategoryByIdAsync(category.Parentid ?? 0) == null)
                    return BadRequest("parent category not found");
            }
            if (await _categoryServices.CreateNewCategoryAsync(_mapper.Map<CategoryCreateInput, Category>(category)))
            {
                return Ok("Done");
            }
            return BadRequest("category not added");
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update(CategoryUpdateInput category)
        {
            if (category.parentId != null)
            {
                if (await _categoryServices.GetCategoryByIdAsync(category.parentId ?? 0) == null)
                    return BadRequest("parent category not found");
            } 
            if (await _categoryServices.UpdateCategoryAsync(_mapper.Map<CategoryUpdateInput, Category>(category)))
            {
                return Ok("Done");
            }
            return NotFound("category not Found");
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int id)
        {

            if (await _categoryServices.GetCategoryByIdAsync(id) == null)
                return NotFound("category not found");

            if (await _categoryServices.HaseCoursesAsync(id))
            {
                return BadRequest("this category hase Courses");
            }
            if (await _categoryServices.HaseSubCategoriesAsync(id))
            {
                return BadRequest("this category hase SubCategories");
            }
            if (await _categoryServices.DeletCategoryAsync(id))
            {
                return Ok("Done");
            }
            return BadRequest("category not deleted");
        }

    }
}