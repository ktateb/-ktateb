using FluentValidation;
namespace Model.Category.Input
{
    public class CategoryCreateInput
    { 
        public int? Parentid { get; set; }
        public string Name { get; set; } 
    }
    public class CategoryCreateInputValidator : AbstractValidator<CategoryCreateInput>
    {
        public CategoryCreateInputValidator()
        {
            RuleFor(x =>x.Name)
                .NotEmpty().WithMessage("Please enter Category Name");   
                     
        }
    }
}