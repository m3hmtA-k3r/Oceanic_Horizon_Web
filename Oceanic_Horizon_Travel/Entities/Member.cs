using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Member: BaseEntity // Sistemdeki tüm kullanıcıları — müşteri, ev sahibi, personel, admin. Ayrım Roles[] ile yapılacak
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string? Iban { get; set; }   // kazancının yatırılacağı hesap — normal üyelerde boş kalır
        public bool IsActive { get; set; } = true; // üye aktif mi değil mi kontrolü için

        public List<string> Roles { get; set; }   // "Member", "Host", "Personnel", "Admin"


    }
}
