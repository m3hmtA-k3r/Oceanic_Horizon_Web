namespace Oceanic_Horizon_Travel.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; } 
        public string MemberCollectionName { get; set; }
        public string BannerCollectionName { get; set; }
        public string DestinationCollectionName { get; set; }
        public string TourCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string SiteSettingsCollectionName { get; set; }

    }
}
