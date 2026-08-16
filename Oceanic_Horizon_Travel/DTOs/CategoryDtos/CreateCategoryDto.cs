using Oceanic_Horizon_Travel.Entities.SubDocuments;

namespace Oceanic_Horizon_Travel.DTOs.CategoryDtos
{
    public class CreateCategoryDto
    {
        public LocalizedText Name { get; set; } = new();
        public string SeoUrl { get; set; }        // tüm dillerde aynı
        public string? Icon { get; set; }         // Material Symbols ikon adı
        public bool IsActive { get; set; }
    }
}
