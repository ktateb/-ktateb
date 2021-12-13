using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Model.Vedio
{
    public class VedioInput
    {
        public int SectionId { get; set; }
        public String VedioTitle { get; set; }
        public String ShortDescription { get; set; }  
    }
    public class VedioInputValidator : AbstractValidator<VedioInput>
    {
        public VedioInputValidator()
        {
            RuleFor(x => x.VedioTitle)
                .NotEmpty().WithMessage("Please enter Title"); 
            RuleFor(x => x.ShortDescription)
               .NotEmpty().WithMessage("Please enter ShortDescription");
        }
    }
}