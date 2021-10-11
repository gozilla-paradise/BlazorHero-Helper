using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class Validation
    {
        public const string TemplateCode = @"using $_ADD_EDIT_CQRS_NAMESPACE_$;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;

namespace $_NAMESPACE_$
{
    public class AddEdit$_ENTITY_$CommandValidator : AbstractValidator<AddEdit$_ENTITY_$Command>
    {
        public AddEdit$_ENTITY_$CommandValidator(IStringLocalizer<AddEdit$_ENTITY_$CommandValidator> localizer)
        {
            //TODO: Insert Data Validation Here
            /*RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer[""Name is required!""]);
            RuleFor(request => request.Barcode)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer[""Barcode is required!""]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer[""Description is required!""]);
            RuleFor(request => request.BrandId)
                .GreaterThan(0).WithMessage(x => localizer[""Brand is required!""]);
            RuleFor(request => request.Rate)
                .GreaterThan(0).WithMessage(x => localizer[""Rate must be greater than 0""]);*/
        }
    }
}";
    }
}
