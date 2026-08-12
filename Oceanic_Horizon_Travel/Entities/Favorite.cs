using Oceanic_Horizon_Travel.Entities.Common;

namespace Oceanic_Horizon_Travel.Entities
{
    public class Favorite: BaseEntity
    {
        public string MemberId { get; set; } // Favoriye ekleyen üye
        public string Type { get; set; } // Favori ne olacak Örn: "Destination", "Tour", "Estate" gibi.
        public string EntityId { get; set; } // Favori edilen entity'nin ID'si

    }
}

