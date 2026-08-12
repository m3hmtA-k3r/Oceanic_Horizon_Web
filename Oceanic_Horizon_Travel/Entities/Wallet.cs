using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    // Wallet = ev sahibinin cüzdanı. Rezervasyonlardan gelen kazanç burada birikir,
    // host buradan çekim talebi açar. Her host için tek kayıt.
    public class Wallet : BaseEntity
    {
        public string MemberId { get; set; }  // Cüzdan kime ait           

        public decimal AvailableBalance { get; set; } // Çekilebilir bakiye — konaklama tamamlandı, para host'un   

        public decimal PendingBalance { get; set; }// Bekleyen bakiye — rezervasyon var ama misafir daha gitmedi

        public decimal TotalEarned { get; set; }// Bugüne kadarki toplam kazanç

        public decimal TotalWithdrawn { get; set; }// Bugüne kadar çekilen toplam
    }
}
