using FluentValidation;
using Oceanic_Horizon_Travel.DTOs.CategoryDtos;

namespace Oceanic_Horizon_Travel.Validations.CategoryValidations
{
    public class UpdateCategoryValidator: AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name.Tr)
              .NotEmpty().WithMessage("Kategori adı boş bırakılamaz.")
              .MaximumLength(60).WithMessage("Kategori adı en fazla 60 karakter olmalıdır.");

            RuleFor(x => x.Name.En)
                .NotEmpty().WithMessage("Category name cannot be empty.")
                .MaximumLength(60).WithMessage("Category name must not exceed 60 characters.");

            RuleFor(x => x.Name.Pt)
                .MaximumLength(60).WithMessage("O nome da categoria deve ter no máximo 60 caracteres.");


            RuleFor(x => x.SeoUrl)
                .NotEmpty().WithMessage("Seo Url boş bırakılamaz.")
                .MaximumLength(60).WithMessage("Seo Url en fazla 60 karakter olmalıdır.")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Seo Url sadece küçük harf, rakam ve tire içerebilir. Örnek: kultur-turu");
        }
    }
}
