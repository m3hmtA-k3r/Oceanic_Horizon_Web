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

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori seçmelisiniz.");

            
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
                .WithMessage("Seo Url sadece küçük harf, rakam ve tire içerebilir. Örnek: roma-toskana-kesfi");

           
            RuleFor(x => x.ShortDescription.Tr)
                .NotEmpty().WithMessage("Kısa açıklama boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Kısa açıklama en fazla 200 karakter olmalıdır.");

            RuleFor(x => x.ShortDescription.En)
                .NotEmpty().WithMessage("Short description cannot be empty.")
                .MaximumLength(200).WithMessage("Short description must not exceed 200 characters.");

            RuleFor(x => x.ShortDescription.Pt)
                .MaximumLength(200).WithMessage("A descrição curta deve ter no máximo 200 caracteres.");

            
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

            
            RuleFor(x => x.Route.Tr)
                .MaximumLength(250).WithMessage("Rota en fazla 250 karakter olmalıdır.");

            RuleFor(x => x.Route.En)
                .MaximumLength(250).WithMessage("Route must not exceed 250 characters.");

            RuleFor(x => x.Route.Pt)
                .MaximumLength(250).WithMessage("A rota deve ter no máximo 250 caracteres.");

           
            RuleFor(x => x.Day)
                .GreaterThan(0).WithMessage("Gün sayısı 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(60).WithMessage("Gün sayısı en fazla 60 olabilir.");

            RuleFor(x => x.Night)
                .GreaterThanOrEqualTo(0).WithMessage("Gece sayısı negatif olamaz.")
                .LessThanOrEqualTo(60).WithMessage("Gece sayısı en fazla 60 olabilir.");

            
            RuleFor(x => x.MaxCapacity)
                .GreaterThan(0).WithMessage("Maksimum kontenjan 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(500).WithMessage("Maksimum kontenjan en fazla 500 olabilir.");

            RuleFor(x => x.MinParticipant)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum katılımcı negatif olamaz.")
                .LessThanOrEqualTo(x => x.MaxCapacity)
                .WithMessage("Minimum katılımcı, maksimum kontenjandan büyük olamaz.");

           
            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.")
                .LessThan(1000000).WithMessage("Fiyat çok yüksek görünüyor, kontrol edin.");

            RuleFor(x => x.CurrencyType)
                .NotEmpty().WithMessage("Para birimi seçmelisiniz.")
                .Must(x => x == "TRY" || x == "EUR" || x == "USD")
                .WithMessage("Para birimi TRY, EUR veya USD olmalıdır.");


            RuleFor(x => x.TourType)
                .MaximumLength(40).WithMessage("Tur tipi en fazla 40 karakter olmalıdır.");

            RuleFor(x => x.GuideLanguage)
                .MaximumLength(60).WithMessage("Rehber dili en fazla 60 karakter olmalıdır.");

            RuleFor(x => x.StartCity.Tr)
                .MaximumLength(60).WithMessage("Başlangıç şehri en fazla 60 karakter olmalıdır.");

            RuleFor(x => x.StartCity.En)
                .MaximumLength(60).WithMessage("Departure city must not exceed 60 characters.");

            RuleFor(x => x.StartCity.Pt)
                .MaximumLength(60).WithMessage("A cidade de partida deve ter no máximo 60 caracteres.");

            RuleFor(x => x.Transportation.Tr)
                .MaximumLength(120).WithMessage("Ulaşım bilgisi en fazla 120 karakter olmalıdır.");

            RuleFor(x => x.Transportation.En)
                .MaximumLength(120).WithMessage("Transportation info must not exceed 120 characters.");

            RuleFor(x => x.Transportation.Pt)
                .MaximumLength(120).WithMessage("As informações de transporte devem ter no máximo 120 caracteres.");

            RuleFor(x => x.Accommodation.Tr)
                .MaximumLength(120).WithMessage("Konaklama bilgisi en fazla 120 karakter olmalıdır.");

            RuleFor(x => x.Accommodation.En)
                .MaximumLength(120).WithMessage("Accommodation info must not exceed 120 characters.");

            RuleFor(x => x.Accommodation.Pt)
                .MaximumLength(120).WithMessage("As informações de acomodação devem ter no máximo 120 caracteres.");

            RuleFor(x => x.VisaInfo.Tr)
                .MaximumLength(250).WithMessage("Vize bilgisi en fazla 250 karakter olmalıdır.");

            RuleFor(x => x.VisaInfo.En)
                .MaximumLength(250).WithMessage("Visa info must not exceed 250 characters.");

            RuleFor(x => x.VisaInfo.Pt)
                .MaximumLength(250).WithMessage("As informações de visto devem ter no máximo 250 caracteres.");

            
            RuleForEach(x => x.TourDates).ChildRules(date =>
            {
                date.RuleFor(d => d.StartDate)
                    .NotEmpty().WithMessage("Başlangıç tarihi boş bırakılamaz.");

                date.RuleFor(d => d.EndDate)
                    .GreaterThan(d => d.StartDate)
                    .WithMessage("Bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");

                date.RuleFor(d => d.Quota)
                    .GreaterThan(0).WithMessage("Kontenjan 0'dan büyük olmalıdır.");

                date.RuleFor(d => d.Price)
                    .GreaterThan(0).WithMessage("Tarih fiyatı 0'dan büyük olmalıdır.");
            });

           
            RuleForEach(x => x.Itinerary).ChildRules(day =>
            {
                day.RuleFor(d => d.DayNumber)
                    .GreaterThan(0).WithMessage("Gün numarası 0'dan büyük olmalıdır.");

                // Title
                day.RuleFor(d => d.Title.Tr)
                    .NotEmpty().WithMessage("Program gününün başlığı boş bırakılamaz.")
                    .MaximumLength(120).WithMessage("Program gününün başlığı en fazla 120 karakter olmalıdır.");

                day.RuleFor(d => d.Title.En)
                    .NotEmpty().WithMessage("Itinerary day title cannot be empty.")
                    .MaximumLength(120).WithMessage("Itinerary day title must not exceed 120 characters.");

                day.RuleFor(d => d.Title.Pt)
                    .MaximumLength(120).WithMessage("O título do dia do itinerário deve ter no máximo 120 caracteres.");

                
                day.RuleFor(d => d.Description.Tr)
                    .NotEmpty().WithMessage("Program gününün açıklaması boş bırakılamaz.")
                    .MaximumLength(1000).WithMessage("Program gününün açıklaması en fazla 1000 karakter olmalıdır.");

                day.RuleFor(d => d.Description.En)
                    .NotEmpty().WithMessage("Itinerary day description cannot be empty.")
                    .MaximumLength(1000).WithMessage("Itinerary day description must not exceed 1000 characters.");

                day.RuleFor(d => d.Description.Pt)
                    .MaximumLength(1000).WithMessage("A descrição do dia do itinerário deve ter no máximo 1000 caracteres.");
            });

           
            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("Kapak görseli seçmelisiniz.");
        }
    }
}