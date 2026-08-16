using FluentValidation;
using Oceanic_Horizon_Travel.DTOs.BannerDtos;

namespace Oceanic_Horizon_Travel.Validations.BannerValidations
{
    public class UpdateBannerValidator : AbstractValidator<UpdateBannerDto>
    {
        public UpdateBannerValidator()
        {
            RuleFor(x => x.Title.Tr)
                 .NotEmpty().WithMessage("Başlık  boş bırakılamaz.")
                 .MaximumLength(120).WithMessage("Başlık en fazla 120 karakter olmalıdır.");

            RuleFor(x => x.Title.En)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(120).WithMessage("Title  must not exceed 120 characters.");

            RuleFor(x => x.Title.Pt)
                .MaximumLength(120).WithMessage("O título deve ter no máximo 120 caracteres.");


            RuleFor(x => x.Description.Tr)
                .NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
                .MaximumLength(300).WithMessage("Açıklama en fazla 300 karakter olmalıdır.");

            RuleFor(x => x.Description.En)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(300).WithMessage("Description must not exceed 300 characters.");

            RuleFor(x => x.Description.Pt)
                .MaximumLength(300).WithMessage("A descrição deve ter no máximo 300 caracteres.");


        }
    }
}
