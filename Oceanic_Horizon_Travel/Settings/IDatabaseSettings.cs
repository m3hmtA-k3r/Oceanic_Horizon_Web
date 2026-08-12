namespace Oceanic_Horizon_Travel.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MemberCollectionName { get; set; }


    }
}
