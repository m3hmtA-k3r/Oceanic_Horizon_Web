namespace Oceanic_Horizon_Travel.Entities.SubDocuments
{
    public class ImageItem
    {
        public string Url { get; set; }
        public int Order { get; set; } // Galeride Kaçıncı sırada görünecek
        public bool IsCover { get; set; } // Kapagın görselimi ? 
    }
}
