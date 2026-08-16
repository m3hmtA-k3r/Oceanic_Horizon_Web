using FluentValidation;
using Oceanic_Horizon_Travel.DTOs.TourDtos;

namespace Oceanic_Horizon_Travel.Validations.TourValidations
{
    public class CreateTourValidator : AbstractValidator<CreateTourDto>
    {
        public CreateTourValidator()
        {

            RuleFor(x => x.DestinationId)
                .NotEmpty().WithMessage("Destinasyon seçmelisiniz.");

            RuleFor(x => x.Title.Tr)
                .NotEmpty().WithMessage("Tur adı boş bırakılamaz.")
                .MinimumLength(3).WithMessage("Tur adı en az 3 karakter olmalıdır.")
                .MaximumLength(120).WithMessage("Tur adı en fazla 120 karakter olmalıdır.");

            RuleFor(x => x.Title.En)
                .NotEmpty().WithMessage("Tour title cannot be empty.")
                .MinimumLength(3).WithMessage("Tour title must be at least 3 characters.")
                .MaximumLength(120).WithMessage("Tour title must not exceed 120 characters.");

            RuleFor(x => x.Title.Pt)
                .MaximumLength(120).WithMessage("O título do tour deve ter no máximo 120 caracteres.");

            RuleFor(x => x.SeoUrl)
                .NotEmpty().WithMessage("Seo Url boş bırakılamaz.")
                .MaximumLength(80).WithMessage("Seo Url en fazla 80 karakter olmalıdır.")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Seo Url sadece küçük harf, rakam ve tire içerebilir. Örnek: santorini-ruyasi");

            RuleFor(x => x.Description.Tr)
                .NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
                .MinimumLength(20).WithMessage("Açıklama en az 20 karakter olmalıdır.")
                .MaximumLength(4000).WithMessage("Açıklama en fazla 4000 karakter olmalıdır.");

            RuleFor(x => x.Description.En)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MinimumLength(20).WithMessage("Description must be at least 20 characters.")
                .MaximumLength(4000).WithMessage("Description must not exceed 4000 characters.");

            RuleFor(x => x.Description.Pt)
                .MaximumLength(4000).WithMessage("A descrição deve ter no máximo 4000 caracteres.");

            RuleFor(x => x.Night)
                .GreaterThan(0).WithMessage("Gece sayısı 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(60).WithMessage("Gece sayısı en fazla 60 olabilir.");

            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.")
                .LessThan(1000000).WithMessage("Fiyat çok yüksek görünüyor, kontrol edin.");

            RuleFor(x => x.CurrencyType)
                .NotEmpty().WithMessage("Para birimi seçmelisiniz.")
                .Must(x => x == "TRY" || x == "EUR" || x == "USD")
                .WithMessage("Para birimi TRY, EUR veya USD olmalıdır.");

            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("Kapak görseli seçmelisiniz.");
        }
    }
}
