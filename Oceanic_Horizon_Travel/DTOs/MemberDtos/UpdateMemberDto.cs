namespace Oceanic_Horizon_Travel.DTOs.MemberDtos
{
    public class UpdateMemberDto
    {
        public string? Id { get; set; }

        // Admin bunları değiştirebilir
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
