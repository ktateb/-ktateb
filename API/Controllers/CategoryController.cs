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
using API.Helpers;
using DAL.Entities.Courses;
using Model.Course.Outputs;

namespace API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;
        public CategoryController(IMapper mapper,ICategoryServices categoryServices)
        {
            _mapper = mapper;
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
        
        /// <summary>
        /// filter the courses of category and Disply them  
        /// </summary>  
        /// <param name="Id">Category Id</param>
        /// <param name="Params">for orderBy (popular = 1, Rating= 2, Price= 3,Students= 4)</param>
        [HttpPost("{Id}/Courses")]
        public async Task<List<CourseOutput>> Get(int Id, CategoryCoursesParams Params)
        {
            var Courses = await _categoryServices.GetCourses(Id, Params);
            Response.AddPagination(Courses.CurrentPage, Courses.ItemsPerPage, Courses.TotalItems, Courses.TotalPages);
            return _mapper.Map<List<Course>, List<CourseOutput>>(Courses);
        }

    }
}