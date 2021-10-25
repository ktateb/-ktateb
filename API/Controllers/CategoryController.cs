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
using Microsoft.AspNetCore.Authorization;
using Common.Services;
namespace API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpGet("{id}/SubCategories")]
        public async Task<ActionResult<ResultService<List<CategoryOutput>>>> SubSategories(int id) =>
            GetResult<List<CategoryOutput>>(await _categoryServices.GetSubCategoriesAsync(id));

        [HttpGet("{id}")]
        public async Task<ActionResult<ResultService<CategoryOutput>>> GetCategory(int id) =>
            GetResult<CategoryOutput>(await _categoryServices.GetCategoryByIdAsync(id));

        [HttpGet("TopLevelCategories")]
        public async Task<ActionResult<ResultService<List<CategoryOutput>>>> GetTopLevelCategories() =>
           GetResult<List<CategoryOutput>>(await _categoryServices.GetTopLevelCategoriesAsync());


        [HttpGet("{id}/Parent")]
        public async Task<ActionResult<ResultService<CategoryOutput>>> Parent(int id) =>
           GetResult<CategoryOutput>(await _categoryServices.GetParentAsync(id));

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("Create")]
        public async Task<ActionResult<ResultService<CategoryOutput>>> Create(CategoryCreateInput category) =>
             GetResult<CategoryOutput>(await _categoryServices.CreateNewCategoryAsync(category));

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost("Update")]
        public async Task<ActionResult<ResultService<CategoryOutput>>> Update(CategoryUpdateInput category) =>
         GetResult<CategoryOutput>(await _categoryServices.UpdateCategoryAsync(category));


        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("Delete")]
        public async Task<ActionResult<ResultService<bool>>> Delete(int id) =>
            GetResult<bool>(await _categoryServices.DeletCategoryAsync(id));

    }
}