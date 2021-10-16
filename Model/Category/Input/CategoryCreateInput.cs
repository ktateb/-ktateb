using System.ComponentModel.DataAnnotations;
using FluentValidation;
namespace Model.Category.Input
{
    public class CategoryCreateInput
    { 
        public int? Parentid { get; set; }
        [Required]
        public string name { get; set; } 
    }
    public class CategoryCreateInputValidator : AbstractValidator<CategoryCreateInput>
    {
        public CategoryCreateInputValidator()
        {
            RuleFor(x =>x.name)
                .NotEmpty().WithMessage("Please enter Category Name");   
                     
        }
    }
}