using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Review: BaseEntity // Müşterinin gittiği tur veya kaldığı otel hakkında yazdığı görüş ve verdiği puanlar gibi düşün
    {
        public string MemberId { get; set; } // yorumu yazan uye
        public string Type { get; set; } // Nereye yorum yapildi
        public string EntityId { get; set; } // Yorumu yapilan otelin veya turun Kimligi
        public int Rating { get; set; } // uyenin verdigi puan degerlendirmesi
        public string Comment { get; set; } // Yorum icerigi
        public bool IsApproved { get; set; } // Musteri onayini Admin onayladi veya onaylamadi

    }
}
