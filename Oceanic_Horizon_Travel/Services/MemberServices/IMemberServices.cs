
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.Entities;


namespace Oceanic_Horizon_Travel.Services.MemberServices
{
    public interface IMemberServices
    {
        //Frontend: üyelik akışı
        Task<bool> IsEmailExistAsync(string email);
        Task RegisterAsync(RegisterMemberDto registerMemberDto);
        Task<Member> LoginAsync(LoginMemberDto loginMemberDto);

        //AdminPaneli Üyelik
        Task<List<ResultMemberDto>> GetAllAsync();
        Task<ResultMemberDto> GetByIdAsync(string id);
        Task UpdateRolesAndStatusAsync(UpdateMemberDto updateMemberDto);

        Task<List<ResultMemberDto>> GetByIdsAsync(List<string> ids); // Toplu çekme metotları 

    }
}
