namespace Oceanic_Horizon_Travel.Entities.SubDocuments
{
    public class Guest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; } // TC Kimlik no veya Pasaport No
        public DateTime BirthDate { get; set; } // Dogum tarihi
    }
}
