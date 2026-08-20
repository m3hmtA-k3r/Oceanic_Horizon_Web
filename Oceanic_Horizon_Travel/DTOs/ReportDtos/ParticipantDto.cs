namespace Oceanic_Horizon_Travel.DTOs.ReportDtos
{
    
    public class ParticipantDto
    {// Rapordaki tek satır — bir katılımcı
        public string FullName { get; set; } = "";
        public string IdentityNumber { get; set; } = "";
        public DateTime BirthDate { get; set; }

        public string BookingNumber { get; set; } = "";
        public string BookedBy { get; set; } = ""; // rezervasyonu yapan üye
        public string Phone { get; set; } = "";
    }
}
