using FluentValidation;
using Oceanic_Horizon_Travel.DTOs.MemberDtos;

namespace Oceanic_Horizon_Travel.Validations.MemberValidations
{
    public class RegisterMemberValidator: AbstractValidator<RegisterMemberDto> // Kayıt formundan gelen verinin kurallara uyup uymadığını kontrol ediyoruz. 
    {
        public RegisterMemberValidator()
        {// Kurallar burada durur; controller'ı doğrulama kodlarıyla kalabalıklastırmamak için buraya koyduk 
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad Boş Bırakılamaz")
                .MinimumLength(2).WithMessage("Ad En Az 2 Karakter Olmalıdır")
                .MaximumLength(50).WithMessage("Ad En Fazla 50 Karakter Olmalıdır");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad Boş Bırakılamaz")
                .MinimumLength(2).WithMessage("Soyad En Az 2 Karakter Olmalıdır")
                .MaximumLength(50).WithMessage("Soyad En Fazla 50 Karakter Olmalıdır");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta Boş Bırakılamaz")
                .EmailAddress().WithMessage("Geçerli Bir E-posta Adresi Giriniz");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon Numarası Boş Bırakılamaz");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre Boş Bırakılamaz")
                .MinimumLength(6).WithMessage("Şifre En Az 6 Karakter Olmalıdır");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Şifreler Birbiriyle Uyuşmuyor");
        }
    }
}
