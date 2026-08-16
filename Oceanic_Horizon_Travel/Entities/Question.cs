using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    // Tur hakkında kullanıcı soru sorar sonra Admin cevaplayınca tur detay sayfasında görünürcek
    public class Question : BaseEntity
    {
        public string TourId { get; set; }
        public string MemberId { get; set; }

        public string Text { get; set; } // soru metni — Customer Artık Allah ne verir yazarsa o dilde kalsın
        public string? Answer { get; set; }  
        public DateTime? AnsweredDate { get; set; }  

        public bool IsAnswered { get; set; }
        public bool IsApproved { get; set; }
    }
}
