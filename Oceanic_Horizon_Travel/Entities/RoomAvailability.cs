using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class RoomAvailability: BaseEntity
    {
        public string RoomId { get; set; } // Hangi oda tipi
        public DateTime Date { get; set; } // Hangi gun gunlerde
        public int AvailableRooms { get; set; } // O gun kac oda bos 
        public decimal Price { get; set; } // ilgili gune ozel ucret

    }
}
