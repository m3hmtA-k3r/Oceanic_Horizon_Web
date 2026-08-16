namespace Oceanic_Horizon_Travel.Entities.SubDocuments
{
    // Çok dilli Dil
    public class LocalizedText
    {
        public string? Tr { get; set; }
        public string? En { get; set; }
        public string? Pt { get; set; }

        // Aktif dile göre değeri döner. Çeviri boşsa Türkçeye düşer,
        public string Get(string culture) => culture switch
        {
            "en" => string.IsNullOrWhiteSpace(En) ? Tr ?? "" : En,
            "pt" => string.IsNullOrWhiteSpace(Pt) ? Tr ?? "" : Pt,
            _ => Tr ?? ""
        };
    }
}
