using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Payment: BaseEntity
    {
        public string BookingId { get; set; } // ödeme hangi Rezarvasyondan Geldi. 
        public decimal Amount { get; set; } // ödeenne turar
        public string PaymentMethod {  get; set; } // Ödeme methodu 
        public string TransactionNumber { get; set; } // İşlem numarası Makbuz işin
        public string Status { get; set; } // Ödeme oldu yada ollmadı gibi 

    }
}
