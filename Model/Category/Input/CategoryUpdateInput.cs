using System.ComponentModel.DataAnnotations;
using FluentValidation;
namespace Model.Category.Input
{
    public class CategoryUpdateInput
    {
        public int id { get; set; }
        public int? parentId { get; set; }
        [Required]
        public string name{get;set;}
    }
    public class CategoryUpdateInputValidator : AbstractValidator<CategoryUpdateInput>
    {
        public CategoryUpdateInputValidator()
        {
            RuleFor(x =>x.id)
                .NotEmpty().WithMessage("Please enter Category Id");         
            RuleFor(x =>x.name)
                .NotEmpty().WithMessage("Please enter Category Name");  
        }
    }
}