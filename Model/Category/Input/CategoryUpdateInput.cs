using FluentValidation;
namespace Model.Category.Input
{
    public class CategoryUpdateInput
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name{get;set;}
    }
    public class CategoryUpdateInputValidator : AbstractValidator<CategoryUpdateInput>
    {
        public CategoryUpdateInputValidator()
        {
            RuleFor(x =>x.Id)
                .NotEmpty().WithMessage("Please enter Category Id");         
            RuleFor(x =>x.Name)
                .NotEmpty().WithMessage("Please enter Category Name");  
        }
    }
}