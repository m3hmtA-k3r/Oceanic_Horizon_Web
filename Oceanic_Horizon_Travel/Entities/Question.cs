using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    // Tur hakkında kullanıcı sorusu. Admin cevaplayıp yayınlayınca tur detaylarında görünür.
    public class Question : BaseEntity
    {
        public string TourId { get; set; }
        public string MemberId { get; set; }

        public string Text { get; set; }                  // çevrilmez — kullanıcı hangi dilde yazdıysa
        public string Answer { get; set; }
        public DateTime? AnsweredDate { get; set; }
        public string AnsweredByAdminId { get; set; }    // audit — hangi yönetici cevapladı

        public bool IsAnswered { get; set; }
        public bool IsApproved { get; set; }
    }
}
