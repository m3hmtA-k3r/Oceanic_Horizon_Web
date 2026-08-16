namespace Oceanic_Horizon_Travel.Entities.SubDocuments
{
    // Turun bir kalkış tarihi Tour belgesinin icinde liste olarak durur
    // yapılan Tourlar yaz boyunca 10 kez kalkabilir aynız amanda her kalkışın kendi kontenjanı ve fiyatı olur 
    public class TourDate
    {
        public string Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Quota { get; set; }   
        public int AvailableSeats { get; set; }

        public decimal Price { get; set; } 

        public string Status { get; set; }// "Açık",-,"Doldu",-,"İptal"
    }
}
