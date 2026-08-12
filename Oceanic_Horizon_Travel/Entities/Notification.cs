using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Notification: BaseEntity //Üyeye gösterilecek bildirim — "rezervasyonunuz onaylandı" gibi olacak
    {
        public string MemberId { get; set; } // Bildirim Kime Ait olacak
        public string Title { get; set; } // Bildirim başlıgı
        public string Description { get; set; } 
        public bool IsRead { get; set; } // üye bunu gördü
    }
}
