using FluentValidation;
using Oceanic_Horizon_Travel.DTOs.SiteSettingsDtos;

namespace Oceanic_Horizon_Travel.Validations.SiteSettingsValidations
{
    public class UpdateSiteSettingsValidator : AbstractValidator<UpdateSiteSettingsDto>
    {
        public UpdateSiteSettingsValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Firma adı boş bırakılamaz.")
                .MaximumLength(80).WithMessage("Firma adı en fazla 80 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(30).WithMessage("Telefon numarası en fazla 30 karakter olmalıdır.");

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("Adres en fazla 250 karakter olmalıdır.");

            RuleFor(x => x.About.Tr)
                .NotEmpty().WithMessage("Hakkımızda boş bırakılamaz.")
                .MaximumLength(2000).WithMessage("Hakkımızda en fazla 2000 karakter olmalıdır.");

            RuleFor(x => x.About.En)
                .NotEmpty().WithMessage("About us cannot be empty.")
                .MaximumLength(2000).WithMessage("About us must not exceed 2000 characters.");

            RuleFor(x => x.About.Pt)
                .MaximumLength(2000).WithMessage("Sobre nós deve ter no máximo 2000 caracteres.");

            RuleFor(x => x.Mission.Tr)
                .MaximumLength(1000).WithMessage("Misyon en fazla 1000 karakter olmalıdır.");

            RuleFor(x => x.Mission.En)
                .MaximumLength(1000).WithMessage("Mission must not exceed 1000 characters.");

            RuleFor(x => x.Mission.Pt)
                .MaximumLength(1000).WithMessage("A missão deve ter no máximo 1000 caracteres.");

            RuleFor(x => x.Vision.Tr)
                .MaximumLength(1000).WithMessage("Vizyon en fazla 1000 karakter olmalıdır.");

            RuleFor(x => x.Vision.En)
                .MaximumLength(1000).WithMessage("Vision must not exceed 1000 characters.");

            RuleFor(x => x.Vision.Pt)
                .MaximumLength(1000).WithMessage("A visão deve ter no máximo 1000 caracteres.");

            RuleFor(x => x.Facebook)
                .MaximumLength(200).WithMessage("Facebook adresi en fazla 200 karakter olmalıdır.");

            RuleFor(x => x.Instagram)
                .MaximumLength(200).WithMessage("Instagram adresi en fazla 200 karakter olmalıdır.");

            RuleFor(x => x.Youtube)
                .MaximumLength(200).WithMessage("Youtube adresi en fazla 200 karakter olmalıdır.");

            RuleFor(x => x.LinkedIn)
                .MaximumLength(200).WithMessage("LinkedIn adresi en fazla 200 karakter olmalıdır.");
        }
    }
}