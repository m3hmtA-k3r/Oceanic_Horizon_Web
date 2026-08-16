namespace Oceanic_Horizon_Travel.Entities.SubDocuments
{
    public class BookingItem
    {
        public string Type { get; set; } 

        public string Title { get; set; }

        public string? TourDateId { get; set; } 


        public string? RoomId { get; set; } 

        public DateTime? CheckIn { get; set; }  

        public DateTime? CheckOut { get; set; } 

        public int Quantity { get; set; }  

        public decimal UnitPrice { get; set; } 

        public decimal Subtotal { get; set; } 
    }
}
