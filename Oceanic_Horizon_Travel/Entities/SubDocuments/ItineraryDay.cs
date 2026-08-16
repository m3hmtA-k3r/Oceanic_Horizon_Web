namespace Oceanic_Horizon_Travel.Entities.SubDocuments
{ 
    // Tur detay sayfasında accordion olarak gün gün gösterilir.
    public class ItineraryDay
    {
        public int DayNumber { get; set; }  

        public LocalizedText Title { get; set; }  
        public LocalizedText Description { get; set; }
        public LocalizedText City { get; set; }   

        public LocalizedText? Transportation { get; set; }
        public LocalizedText? Accommodation { get; set; }
        public LocalizedText? Meals { get; set; }     
    }
}
