using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Model.Vedio
{
    public class VedioUpdateInput
    {
        public int Id { get; set; } 
        public String VedioTitle { get; set; }
        public String ShortDescription { get; set; }
    }
    public class VedioUpdateInputValidator : AbstractValidator<VedioUpdateInput>
    {
        public VedioUpdateInputValidator()
        {
            RuleFor(x => x.VedioTitle)
                .NotEmpty().WithMessage("Please enter Title"); 
            RuleFor(x => x.ShortDescription)
               .NotEmpty().WithMessage("Please enter ShortDescription");
        }
    }
}