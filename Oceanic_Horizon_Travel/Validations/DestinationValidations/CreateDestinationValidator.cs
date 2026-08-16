using FluentValidation;
using Oceanic_Horizon_Travel.DTOs.DestinationDtos;

namespace Oceanic_Horizon_Travel.Validations.DestinationValidations
{
    public class CreateDestinationValidator : AbstractValidator<CreateDestinationDto>
    {
        public CreateDestinationValidator()
        {

            RuleFor(x => x.City.Tr)
                .NotEmpty().WithMessage("Şehir boş bırakılamaz.")
                .MaximumLength(60).WithMessage("Şehir en fazla 60 karakter olmalıdır.");

            RuleFor(x => x.City.En)
                .NotEmpty().WithMessage("City cannot be empty.")
                .MaximumLength(60).WithMessage("City must not exceed 60 characters.");

            RuleFor(x => x.City.Pt)
                .MaximumLength(60).WithMessage("A cidade deve ter no máximo 60 caracteres.");

            RuleFor(x => x.Country.Tr)
                .NotEmpty().WithMessage("Ülke boş bırakılamaz.")
                .MaximumLength(60).WithMessage("Ülke en fazla 60 karakter olmalıdır.");

            RuleFor(x => x.Country.En)
                .NotEmpty().WithMessage("Country cannot be empty.")
                .MaximumLength(60).WithMessage("Country must not exceed 60 characters.");

            RuleFor(x => x.Country.Pt)
                .MaximumLength(60).WithMessage("O país deve ter no máximo 60 caracteres.");

            RuleFor(x => x.SeoUrl)
                .NotEmpty().WithMessage("Seo Url boş bırakılamaz.")
                .MaximumLength(80).WithMessage("Seo Url en fazla 80 karakter olmalıdır.")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Seo Url sadece küçük harf, rakam ve tire içerebilir. Örnek: kapadokya-goreme");

            RuleFor(x => x.ShortDescription.Tr)
                .NotEmpty().WithMessage("Kısa açıklama boş bırakılamaz.")
                .MaximumLength(160).WithMessage("Kısa açıklama en fazla 160 karakter olmalıdır.");

            RuleFor(x => x.ShortDescription.En)
                .NotEmpty().WithMessage("Short description cannot be empty.")
                .MaximumLength(160).WithMessage("Short description must not exceed 160 characters.");

            RuleFor(x => x.ShortDescription.Pt)
                .MaximumLength(160).WithMessage("A descrição curta deve ter no máximo 160 caracteres.");

            RuleFor(x => x.Description.Tr)
                .NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
                .MaximumLength(4000).WithMessage("Açıklama en fazla 4000 karakter olmalıdır.");

            RuleFor(x => x.Description.En)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(4000).WithMessage("Description must not exceed 4000 characters.");

            RuleFor(x => x.Description.Pt)
                .MaximumLength(4000).WithMessage("A descrição deve ter no máximo 4000 caracteres.");

            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("Kapak görseli seçmelisiniz.");
        }
    }
}
